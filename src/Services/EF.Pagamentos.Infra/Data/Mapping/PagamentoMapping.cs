using EF.Pagamentos.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EF.Pagamentos.Infra.Data.Mapping;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.PedidoId)
            .IsRequired();

        builder.Property(c => c.DataLancamento)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(c => c.Valor)
            .IsRequired();

        builder.HasOne(c => c.FormaPagamento)
            .WithMany()
            .HasForeignKey(f => f.FormaPagamentoId);
    }
}
