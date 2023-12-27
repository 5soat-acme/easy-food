using EF.Pedidos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Pedidos.Infra.Data.Mapping;

public class PedidoMapping : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.CorrelacaoId)
            .HasName("IDX_CorrelacaoId");

        builder.HasMany(c => c.Itens)
            .WithOne(c => c.Pedido)
            .HasForeignKey(c => c.PedidoId);
    }
}