using EF.Core.Commons.ValueObjects;
using EF.Identidade.Domain.Models;
using EF.Identidade.Domain.Services;
using EF.Identidade.Infra.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EF.Identidade.Infra.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsuarioService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<string>> CriarUsuario(Usuario usuario)
    {
        var cpf = usuario.Cpf.Numero;
        var newApplicationUser = new ApplicationUser
        {
            UserName = usuario.Email.Endereco,
            Email = usuario.Email.Endereco,
            EmailConfirmed = true,
            Cpf = cpf
        };

        var identityResult = await _userManager.CreateAsync(newApplicationUser);

        var errors = new List<string>();
        foreach (var error in identityResult.Errors)
            errors.Add(error.Description);

        return errors;
    }

    public async Task ExcluirUsuario(Usuario usuario)
    {
        await _userManager.DeleteAsync(new ApplicationUser { Id = usuario.Id.ToString() });
    }

    public async Task<Usuario?> ObterUsuarioPorEmail(string email)
    {
        var applicationUser = await _userManager.FindByEmailAsync(email);
        if (applicationUser is null) return null;
        
        return new Usuario(new Cpf(applicationUser.Cpf), new Email(applicationUser.Email!));;
    }

    public async Task<Usuario?> ObterUsuarioPorCpf(string cpf)
    {
        var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);
        
        return new Usuario(Guid.Parse(applicationUser.Id), new Cpf(applicationUser.Cpf),
            new Email(applicationUser.Email!));
    }

// todo:
    // private async Task<List<UsuarioClaim>> ObterUsuarioClaims(Guid id)
    // {
    //     var identityClaims = await _userManager.GetClaimsAsync(new ApplicationUser { Id = id.ToString() });
    //     var identityRoles = await _userManager.GetRolesAsync(new ApplicationUser { Id = id.ToString() });
    //
    //     foreach (var userRole in identityRoles) identityClaims.Add(new Claim("role", userRole));
    //
    //     return identityClaims.Select(identityClaim => new UsuarioClaim(identityClaim.Type, identityClaim.Value))
    //         .ToList();
    // }
}