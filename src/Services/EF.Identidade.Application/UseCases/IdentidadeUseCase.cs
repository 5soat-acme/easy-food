using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EF.Clientes.Application.DTOs;
using EF.Clientes.Application.UseCases.Interfaces;
using EF.Core.Commons.Communication;
using EF.Core.Commons.Utils;
using EF.Core.Commons.ValueObjects;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using EF.Identidade.Application.UseCases.Interfaces;
using EF.Identidade.Domain.Models;
using EF.Identidade.Domain.Services;
using EF.Infra.Commons.Extensions;
using EF.WebApi.Commons.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Claim = System.Security.Claims.Claim;
using UsuarioClaim = EF.Identidade.Application.DTOs.Responses.UsuarioClaim;

namespace EF.Identidade.Application.UseCases;

public class IdentidadeUseCase : IIdentidadeUseCase
{
    private readonly ICriarClienteUseCase _criarClienteUseCase;
    private readonly IdentitySettings _identitySettings;
    private readonly IUsuarioService _usuarioService;

    public IdentidadeUseCase(ICriarClienteUseCase criarClienteUseCase, IOptions<IdentitySettings> settings,
        IUsuarioService usuarioService)
    {
        _criarClienteUseCase = criarClienteUseCase;
        _usuarioService = usuarioService;
        _identitySettings = settings.Value;
    }

    public async Task<OperationResult<RespostaTokenAcesso>> CriarUsuario(
        NovoUsuario novoUsuario)
    {
        var usuario = new Usuario(new Cpf(novoUsuario.Cpf), new Email(novoUsuario.Email));

        var errors = await _usuarioService.CriarUsuario(usuario);

        if (errors.Count == 0)
        {
            var result = await RegistrarCliente(novoUsuario);
            if (!result.IsValid) return OperationResult<RespostaTokenAcesso>.Failure(result.GetErrorMessages());

            var usuarioCriado = await _usuarioService.ObterUsuarioPorEmail(novoUsuario.Email);
            return OperationResult<RespostaTokenAcesso>.Success(GerarTokenUsuarioIdentificado(usuarioCriado));
        }

        return OperationResult<RespostaTokenAcesso>.Failure(errors);
    }

    public async Task<OperationResult<RespostaTokenAcesso>> AcessarSistema(UsuarioAcesso? usuarioAcesso)
    {
        if (usuarioAcesso?.Email is not null)
        {
            var usuario = await _usuarioService.ObterUsuarioPorEmail(usuarioAcesso.Email);
            return Identificar(usuario);
        }

        if (usuarioAcesso?.Cpf is not null) return await IdentificarPorCpf(usuarioAcesso.Cpf);

        return AcessarComUsuarioNaoRegistrado();
    }

    private OperationResult<RespostaTokenAcesso> Identificar(Usuario? usuario)
    {
        if (usuario is not null)
            return OperationResult<RespostaTokenAcesso>.Success(GerarTokenUsuarioIdentificado(usuario));

        return OperationResult<RespostaTokenAcesso>.Failure("Usuário inválido");
    }

    private async Task<OperationResult<RespostaTokenAcesso>> IdentificarPorCpf(string cpf)
    {
        if (!Cpf.Validar(cpf)) return OperationResult<RespostaTokenAcesso>.Failure("CPF inválido");
        cpf = cpf.SomenteNumeros(cpf);

        var usuario = await _usuarioService.ObterUsuarioPorCpf(cpf);
        if (usuario is not null)
            return Identificar(usuario);

        return OperationResult<RespostaTokenAcesso>.Failure("Usuário inválido");
    }

    private OperationResult<RespostaTokenAcesso> AcessarComUsuarioNaoRegistrado()
    {
        var result = GerarTokenUsuarioNaoIdentificado();
        return OperationResult<RespostaTokenAcesso>.Success(result);
    }

    private ClaimsIdentity ObterTokenClaims()
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Nbf,
                DateTime.UtcNow.ToUnixEpochDate().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTime.UtcNow.ToUnixEpochDate().ToString()),
        };

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private RespostaTokenAcesso GerarTokenUsuarioNaoIdentificado(string? cpf = null)
    {
        var tokenClaims = ObterTokenClaims();

        tokenClaims.AddClaim(new Claim("user_type", "anonymous"));
        tokenClaims.AddClaim(new Claim("session_id", Guid.NewGuid().ToString()));

        if (!string.IsNullOrEmpty(cpf)) tokenClaims.AddClaim(new Claim("user_cpf", cpf));

        var encodedToken = CodificarToken(tokenClaims);
        return ObterRespostaToken(encodedToken, tokenClaims.Claims);
    }

    private RespostaTokenAcesso GerarTokenUsuarioIdentificado(Usuario usuario)
    {
        var tokenClaims = ObterTokenClaims();
        
        tokenClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()));
        tokenClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, usuario.Email.Endereco));
        tokenClaims.AddClaim(new Claim("user_type", "registred"));
        tokenClaims.AddClaim(new Claim("session_id", Guid.NewGuid().ToString()));
        tokenClaims.AddClaim(new Claim("user_cpf", usuario.Cpf.Numero));
        
        var encodedToken = CodificarToken(tokenClaims);
        return ObterRespostaToken(encodedToken, tokenClaims.Claims, usuario);
    }

    private string CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_identitySettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _identitySettings.Issuer,
            Audience = _identitySettings.ValidIn,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_identitySettings.ExpirationHours),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private RespostaTokenAcesso ObterRespostaToken(string encodedToken,
        IEnumerable<Claim> claims, Usuario? user = null)
    {
        return new RespostaTokenAcesso
        {
            Token = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_identitySettings.ExpirationHours).TotalSeconds,
            User = new UsuarioToken
            {
                Id = user?.Id.ToString(),
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private async Task<OperationResult> RegistrarCliente(NovoUsuario novoUsuario)
    {
        var usuario = await _usuarioService.ObterUsuarioPorEmail(novoUsuario.Email);

        try
        {
            var result = await _criarClienteUseCase.Handle(new CriarClienteDto
            {
                Email = novoUsuario.Email,
                Cpf = novoUsuario.Cpf,
                PrimeiroNome = novoUsuario.Nome,
                Sobrenome = novoUsuario.Sobrenome
            });

            if (!result.IsValid) await _usuarioService.ExcluirUsuario(usuario);

            return result;
        }
        catch (Exception)
        {
            await _usuarioService.ExcluirUsuario(usuario);
            throw;
        }
    }
}