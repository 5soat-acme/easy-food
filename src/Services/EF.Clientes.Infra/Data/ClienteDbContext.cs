using EF.Clientes.Domain.Models;
using EF.Core.Commons.Messages;
using EF.Core.Commons.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF.Clientes.Infra.Data;

public sealed class ClienteDbContext : DbContext, IUnitOfWork
{
    public ClienteDbContext(DbContextOptions<ClienteDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Cliente> Clientes { get; set; }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClienteDbContext).Assembly);
        modelBuilder.Ignore<Event>();

        base.OnModelCreating(modelBuilder);
    }
}