using EF.Identidade.Infra.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EF.Identidade.Infra.Data;

public class IdentidadeDbContext : IdentityDbContext<ApplicationUser>
{
    public IdentidadeDbContext(DbContextOptions<IdentidadeDbContext> options) : base(options)
    {
    }
}