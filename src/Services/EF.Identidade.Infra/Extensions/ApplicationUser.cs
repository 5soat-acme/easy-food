using Microsoft.AspNetCore.Identity;

namespace EF.Identidade.Infra.Extensions;

public class ApplicationUser : IdentityUser
{
    public string Cpf { get; set; }
}