﻿using EF.Domain.Commons.Messages;
using EF.Domain.Commons.Repository;
using EF.Estoques.Domain.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EF.Estoques.Infra;

public sealed class EstoqueDbContext : DbContext, IUnitOfWork
{
    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<MovimentacaoEstoque> MovimentacaoEstoques { get; set; }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EstoqueDbContext).Assembly);
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        base.OnModelCreating(modelBuilder);
    }
}