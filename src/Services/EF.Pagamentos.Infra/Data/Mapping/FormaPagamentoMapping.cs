using EF.Pagamentos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Pagamentos.Infra.Data.Mapping;

public class FormaPagamentoMapping : IEntityTypeConfiguration<FormaPagamento>
{
    public void Configure(EntityTypeBuilder<FormaPagamento> builder)
    {
        builder.ToTable("FormasPagamento");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.TipoFormaPagamento)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.TipoFormaPagamento)
            .IsUnique();

        builder.HasData(SeedFormaPagamento());
    }

    private IList<FormaPagamento> SeedFormaPagamento()
    {
        var listaFormas = new List<FormaPagamento>();

        listaFormas.AddRange([
            new FormaPagamento(TipoFormaPagamento.CartaoCredito, "Cartão de Crédito"),
            new FormaPagamento(TipoFormaPagamento.Pix, "Pix"),
            new FormaPagamento(TipoFormaPagamento.MercadoPago, "Mercado Pago")
        ]);

        return listaFormas;
    }
}
