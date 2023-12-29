using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using EF.Pagamentos.Domain.Models;
using FluentValidation.Results;

namespace EF.Pagamentos.Infra;

public sealed class PagamentoDbContext : DbContext, IUnitOfWork
{
    public PagamentoDbContext(DbContextOptions<PagamentoDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<FormaPagamento> FormasPagamento { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoDbContext).Assembly);
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        base.OnModelCreating(modelBuilder);
    }
}