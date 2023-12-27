using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using EF.Cupons.Domain.Models;
using FluentValidation.Results;

namespace EF.Cupons.Infra
{
    public sealed class CupomDbContext : DbContext, IUnitOfWork
    {
        public CupomDbContext(DbContextOptions<CupomDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Cupom> Cupons{ get; set; }
        public DbSet<CupomProduto> CupomProdutos { get; set; }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CupomDbContext).Assembly);
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            base.OnModelCreating(modelBuilder);
        }
    }
}