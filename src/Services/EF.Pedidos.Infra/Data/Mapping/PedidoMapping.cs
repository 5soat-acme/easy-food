using EF.Domain.Commons.ValueObjects;
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
        
        builder.OwnsOne(c => c.Cpf, tf =>
        {
            tf.Property(c => c.Numero)
                .IsRequired()
                .HasMaxLength(Cpf.CpfMaxLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.CpfMaxLength})");
        });

        builder.HasMany(c => c.Itens)
            .WithOne(c => c.Pedido)
            .HasForeignKey(c => c.PedidoId);
    }
}