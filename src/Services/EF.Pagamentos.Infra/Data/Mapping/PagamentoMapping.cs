﻿using EF.Pagamentos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Pagamentos.Infra.Data.Mapping;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.PedidoId)
            .IsRequired();

        builder.Property(c => c.Valor)
            .IsRequired();
    }
}