using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EF.Clientes.Application.Commands;
using EF.Domain.Commons.Communication;
using EF.Domain.Commons.Mediator;
using EF.Identidade.Application.DTOs.Requests;
using EF.Identidade.Application.DTOs.Responses;
using EF.Identidade.Application.Services.Interfaces;
using EF.Identidade.Infra.Extensions;
using EF.Infra.Commons.Extensions;
using EF.WebApi.Commons.Identity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EF.Identidade.Application.Services;

public class AcessoAppService : IAcessoAppService
{
    private readonly IdentitySettings _identitySettings;
    private readonly IMediatorHandler _mediator;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AcessoAppService(IMediatorHandler mediator, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, IOptions<IdentitySettings> settings)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _userManager = userManager;
        _identitySettings = settings.Value;
    }

    public async Task<OperationResult<RespostaTokenAcesso>> CriarUsuario(
        NovoUsuario novoUsuario)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = novoUsuario.Email,
            Email = novoUsuario.Email,
            EmailConfirmed = true,
            Cpf = novoUsuario.Cpf
        };

        var identityResult = await _userManager.CreateAsync(applicationUser, novoUsuario.Senha);

        if (identityResult.Succeeded)
        {
            var result = await RegistrarCliente(novoUsuario);
            if (!result.IsValid) return OperationResult<RespostaTokenAcesso>.Failure(result);

            return OperationResult<RespostaTokenAcesso>.Success(await GerarToken(novoUsuario.Email));
        }

        var errors = new List<string>();
        foreach (var error in identityResult.Errors)
            errors.Add(error.Description);

        return OperationResult<RespostaTokenAcesso>.Failure(errors);
    }

    public RespostaTokenAcesso GerarTokenAcessoNaoIdentificado(string? cpf = null)
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

    public async Task<OperationResult<RespostaTokenAcesso>> Autenticar(UsuarioLogin usuario)
    {
        var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Senha,
            false, true);

        if (result.Succeeded) return OperationResult<RespostaTokenAcesso>.Success(await GerarToken(usuario.Email));

        return OperationResult<RespostaTokenAcesso>.Failure("Usuário ou senha incorretos");
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

    private async Task<RespostaTokenAcesso> GerarToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

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
        claims.Add(new Claim("session_id", Guid.NewGuid().ToString()));
        claims.Add(new Claim("user_type", "authenticated"));
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
}