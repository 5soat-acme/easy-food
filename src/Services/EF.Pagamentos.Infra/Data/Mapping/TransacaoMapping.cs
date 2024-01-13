using EF.Pagamentos.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Pagamentos.Infra.Data.Mapping;

public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("Transacoes");

        builder.HasKey(c => c.Id);
        
        builder.HasOne(c => c.Pagamento)
            .WithMany(c => c.Transacoes);
    }
}