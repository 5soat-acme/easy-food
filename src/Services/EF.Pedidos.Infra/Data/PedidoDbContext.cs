using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using EF.Infra.Commons.Mediator;
using EF.Pedidos.Domain.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EF.Pedidos.Infra.Data;

public sealed class PedidoDbContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;

    public PedidoDbContext(DbContextOptions<PedidoDbContext> options, IMediatorHandler mediator) : base(options)
    {
        _mediator = mediator;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Pedido>? Pedidos { get; set; }

    public async Task<bool> Commit()
    {
        await _mediator.PublishEvents(this);
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