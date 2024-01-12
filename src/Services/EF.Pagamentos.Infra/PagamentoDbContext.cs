using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using EF.Infra.Commons.Data;
using EF.Pagamentos.Domain.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EF.Pagamentos.Infra;

public sealed class PagamentoDbContext : DbContext, IUnitOfWork
{
    public PagamentoDbContext(DbContextOptions<PagamentoDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Pagamento> Pagamentos { get; set; }

    public async Task<bool> Commit()
    {
        DbContextExtension.SetDates(ChangeTracker.Entries());
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