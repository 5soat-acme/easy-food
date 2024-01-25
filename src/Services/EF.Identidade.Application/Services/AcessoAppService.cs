using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EF.Clientes.Application.Commands;
using EF.Domain.Commons.Communication;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Utils;
using EF.Domain.Commons.ValueObjects;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using EF.Identidade.Application.Services.Interfaces;
using EF.Identidade.Infra.Extensions;
using EF.Infra.Commons.Extensions;
using EF.WebApi.Commons.Identity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EF.Identidade.Application.Services;

public class AcessoAppService : IAcessoAppService
{
    private readonly IdentitySettings _identitySettings;
    private readonly IMediatorHandler _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public AcessoAppService(IMediatorHandler mediator, UserManager<ApplicationUser> userManager,
        IOptions<IdentitySettings> settings)
    {
        _mediator = mediator;
        _userManager = userManager;
        _identitySettings = settings.Value;
    }

    public async Task<OperationResult<RespostaTokenAcesso>> CriarUsuario(
        NovoUsuario novoUsuario)
    {
        var cpf = novoUsuario.Cpf.SomenteNumeros(novoUsuario.Cpf);
        var newApplicationUser = new ApplicationUser
        {
            UserName = novoUsuario.Email,
            Email = novoUsuario.Email,
            EmailConfirmed = true,
            Cpf = cpf
        };

        var identityResult = await _userManager.CreateAsync(newApplicationUser);

        if (identityResult.Succeeded)
        {
            var result = await RegistrarCliente(novoUsuario);
            if (!result.IsValid) return OperationResult<RespostaTokenAcesso>.Failure(result);

            var applicationUser = await _userManager.FindByEmailAsync(novoUsuario.Email);
            return OperationResult<RespostaTokenAcesso>.Success(await GerarTokenUsuarioIdentificado(applicationUser));
        }

        var errors = new List<string>();
        foreach (var error in identityResult.Errors)
            errors.Add(error.Description);

        return OperationResult<RespostaTokenAcesso>.Failure(errors);
    }

    public async Task<OperationResult<RespostaTokenAcesso>> Identificar(UsuarioAcesso? usuario)
    {
        if (usuario?.Email is not null)
        {
            var applicationUser = await _userManager.FindByEmailAsync(usuario.Email);
            return await Identificar(applicationUser);
        }

        if (usuario?.Cpf is not null) return await IdentificarPorCpf(usuario.Cpf);

        return AcessarComUsuarioNaoRegistrado();
    }

    public RespostaTokenAcesso GerarTokenUsuarioNaoIdentificado(string? cpf = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Nbf,
                DateTime.UtcNow.ToUnixEpochDate().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTime.UtcNow.ToUnixEpochDate().ToString()),
            new("user_type", "anonymous"),
            new("session_id", Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(cpf)) claims.Add(new Claim("user_cpf", cpf));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var encodedToken = CodificarToken(identityClaims);
        return ObterRespostaToken(encodedToken, claims);
    }

    private async Task<OperationResult<RespostaTokenAcesso>> Identificar(ApplicationUser? applicationUser)
    {
        if (applicationUser is not null)
            return OperationResult<RespostaTokenAcesso>.Success(await GerarTokenUsuarioIdentificado(applicationUser));

        return OperationResult<RespostaTokenAcesso>.Failure("Usuário inválido");
    }

    private async Task<OperationResult<RespostaTokenAcesso>> IdentificarPorCpf(string cpf)
    {
        if(!Cpf.Validar(cpf)) return OperationResult<RespostaTokenAcesso>.Failure("CPF inválido");
        cpf = cpf.SomenteNumeros(cpf);
        
        var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);
        if (applicationUser is not null)
            return await Identificar(applicationUser);

        return OperationResult<RespostaTokenAcesso>.Failure("Usuário inválido");
    }

    private OperationResult<RespostaTokenAcesso> AcessarComUsuarioNaoRegistrado()
    {
        var result = GerarTokenUsuarioNaoIdentificado();
        return OperationResult<RespostaTokenAcesso>.Success(result);
    }

    private async Task<RespostaTokenAcesso> GerarTokenUsuarioIdentificado(ApplicationUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var identityClaims = await ObterClaimsUsuario(claims, user);
        var encodedToken = CodificarToken(identityClaims);

        return ObterRespostaToken(encodedToken, claims, user);
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims,
        ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf,
            DateTime.UtcNow.ToUnixEpochDate().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat,
            DateTime.UtcNow.ToUnixEpochDate().ToString(),
            ClaimValueTypes.Integer64));
        claims.Add(new Claim("user_type", "registred"));
        claims.Add(new Claim("session_id", Guid.NewGuid().ToString()));
        claims.Add(new Claim("user_cpf", user.Cpf));

        foreach (var userRole in userRoles) claims.Add(new Claim("role", userRole));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
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
        IEnumerable<Claim> claims, ApplicationUser? user = null)
    {
        return new RespostaTokenAcesso
        {
            Token = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_identitySettings.ExpirationHours).TotalSeconds,
            User = new UsuarioToken
            {
                Id = user?.Id,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private async Task<ValidationResult> RegistrarCliente(NovoUsuario novoUsuario)
    {
        var applicationUser = await _userManager.FindByEmailAsync(novoUsuario.Email);

        try
        {
            var result = await _mediator.Send(new CriarClienteCommand
            {
                Email = novoUsuario.Email,
                Cpf = novoUsuario.Cpf,
                PrimeiroNome = novoUsuario.Nome,
                Sobrenome = novoUsuario.Sobrenome
            });

            if (!result.IsValid()) await _userManager.DeleteAsync(applicationUser);

            return result.ValidationResult;
        }
        catch (Exception)
        {
            await _userManager.DeleteAsync(applicationUser);
            throw;
        }
    }
}