using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EF.Cupons.Domain.Models;

namespace EF.Cupons.Infra.Data.Mapping
{
    public class CupomProdutoMapping : IEntityTypeConfiguration<CupomProduto>
    {
        public void Configure(EntityTypeBuilder<CupomProduto> builder)
        {
            builder.ToTable("CupomProdutos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ProdutoId)
                .IsRequired();

            builder.HasOne(c => c.Cupom)
                .WithMany(c => c.CupomProdutos);
        }
    }
}