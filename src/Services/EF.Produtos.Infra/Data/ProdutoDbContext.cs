using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using EF.Produtos.Domain.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EF.Produtos.Infra.Data;

public sealed class ProdutoDbContext : DbContext, IUnitOfWork
{
    public ProdutoDbContext(DbContextOptions<ProdutoDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Produto> Produtos { get; set; }

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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProdutoDbContext).Assembly);
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        base.OnModelCreating(modelBuilder);
    }
}