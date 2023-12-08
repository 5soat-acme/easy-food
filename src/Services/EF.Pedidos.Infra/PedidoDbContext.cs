using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using EF.Pedidos.Domain.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EF.Pedidos.Infra;

public sealed class PedidoDbContext : DbContext, IUnitOfWork
{
    public PedidoDbContext(DbContextOptions<PedidoDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Pedido> Pedidos { get; set; }

    public async Task<bool> Commit()
    {
        // TODO: Aqui você pode percorrer os eventos de dominio e processa-los
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidoDbContext).Assembly);
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        base.OnModelCreating(modelBuilder);
    }
}