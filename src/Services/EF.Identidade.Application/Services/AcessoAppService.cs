using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EF.Clientes.Application.Commands;
using EF.Domain.Commons.Mediator;
using EF.Identidade.Application.DTOs;
using EF.WebApi.Commons.Identity;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace EF.Identidade.Application.Services;

public class AcessoAppService : IAcessoAppService
{
    private readonly IdentitySettings _identitySettings;
    private readonly IMediatorHandler _mediator;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AcessoAppService(IMediatorHandler mediator, SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager, IOptions<IdentitySettings> settings)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _userManager = userManager;
        _identitySettings = settings.Value;
    }

    public async Task<(ValidationResult ValidationResult, RespostaTokenAcesso? Token)> CriarUsuario(
        NovoUsuario novoUsuario)
    {
        var identityUser = new IdentityUser
        {
            UserName = novoUsuario.Email,
            Email = novoUsuario.Email,
            EmailConfirmed = true
        };

        var identityResult = await _userManager.CreateAsync(identityUser, novoUsuario.Senha);

        var result = new ValidationResult();
        RespostaTokenAcesso? jwt = null;

        if (identityResult.Succeeded)
        {
            result = await RegistrarCliente(novoUsuario);
            jwt = await GerarToken(novoUsuario.Email);
        }

        return (result, jwt);
    }

    public RespostaTokenAcesso GerarTokenAcessoNaoIdentificado(string? cpf = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("user_type", "anonymous")
        };

        if (!string.IsNullOrEmpty(cpf)) claims.Add(new Claim("user_cpf", cpf));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var encodedToken = CodificarToken(identityClaims);
        return ObterRespostaToken(encodedToken, claims);
    }

    public async Task<(ValidationResult ValidationResult, RespostaTokenAcesso? Token)> Autenticar(UsuarioLogin usuario)
    {
        var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Senha,
            false, true);

        RespostaTokenAcesso? jwt = null;
        ValidationResult validationResult = new();

        if (!result.Succeeded)
        {
            validationResult.Errors.Add(new ValidationFailure("Usuario", "Usuário ou senha incorretos"));
            return (validationResult, jwt);
        }

        jwt = await GerarToken(usuario.Email);
        return (validationResult, jwt);
    }

    private async Task<ValidationResult> RegistrarCliente(NovoUsuario novoUsuario)
    {
        var identityUser = await _userManager.FindByEmailAsync(novoUsuario.Email);

        try
        {
            var result = await _mediator.Send(new CriarClienteCommand
            {
                Email = novoUsuario.Email,
                Cpf = novoUsuario.Cpf,
                PrimeiroNome = novoUsuario.Nome,
                Sobrenome = novoUsuario.Sobrenome
            });

            if (!result.IsValid) await _userManager.DeleteAsync(identityUser);

            return result;
        }
        catch (Exception)
        {
            await _userManager.DeleteAsync(identityUser);
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
        IdentityUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf,
            ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat,
            ToUnixEpochDate(DateTime.UtcNow).ToString(),
            ClaimValueTypes.Integer64));
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
        IEnumerable<Claim> claims, IdentityUser? user = null)
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


    private static long ToUnixEpochDate(DateTime date)
    {
        return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);
    }
}