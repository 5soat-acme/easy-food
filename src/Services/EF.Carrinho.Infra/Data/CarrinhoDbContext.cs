using EF.Carrinho.Domain.Models;
using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using EF.Infra.Commons.Mediator;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EF.Carrinho.Infra.Data;

public sealed class CarrinhoDbContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;

    public CarrinhoDbContext(DbContextOptions<CarrinhoDbContext> options, IMediatorHandler mediator) : base(options)
    {
        _mediator = mediator;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<CarrinhoCliente>? Carrinhos { get; set; }
    public DbSet<Item>? Itens { get; set; }

    public async Task<bool> Commit()
    {
        await _mediator.PublishEvents(this);
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarrinhoDbContext).Assembly);
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Cascade;

        base.OnModelCreating(modelBuilder);
    }
}