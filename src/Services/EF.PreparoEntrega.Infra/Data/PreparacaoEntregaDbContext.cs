using EF.Domain.Commons.Mediator;
using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using EF.Infra.Commons.Mediator;
using EF.PreparoEntrega.Domain.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EF.PreparoEntrega.Infra.Data;

public sealed class PreparacaoEntregaDbContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;

    public PreparacaoEntregaDbContext(DbContextOptions<PreparacaoEntregaDbContext> options, IMediatorHandler mediator) : base(options)
    {
        _mediator = mediator;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Pedido>? Pedidos { get; set; }
    public DbSet<Item>? Itens { get; set; }

    public async Task<bool> Commit()
    {
        AtualizarDatas();
        await _mediator.PublishEvents(this);
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PreparacaoEntregaDbContext).Assembly);
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        base.OnModelCreating(modelBuilder);
    }

    private void AtualizarDatas()
    {
        var entries = ChangeTracker.Entries()
            .Where(entry => entry.Entity.GetType().GetProperty("DataCriacao") != null
                            || entry.Entity.GetType().GetProperty("DataUltimaAtualizacao") != null);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                DefinirPropriedadeSeExistir(entry, "DataCriacao", DateTime.Now);
                DefinirPropriedadeSeExistir(entry, "DataUltimaAtualizacao", DateTime.Now, false);
            }

            if (entry.State == EntityState.Modified)
            {
                DefinirPropriedadeSeExistir(entry, "DataCriacao", entry.Property("DataCriacao").CurrentValue, false);
                DefinirPropriedadeSeExistir(entry, "DataUltimaAtualizacao", DateTime.Now);
            }
        }
    }

    private void DefinirPropriedadeSeExistir(EntityEntry entry, string nomePropriedade, object valor,
        bool modificar = true)
    {
        var propriedade = entry.Property(nomePropriedade);
        if (propriedade != null)
        {
            if (modificar)
                propriedade.CurrentValue = valor;
            else
                propriedade.IsModified = false;
        }
    }
}