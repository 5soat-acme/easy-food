using EF.Carrinho.Domain.Models;
using EF.Domain.Commons.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Carrinho.Infra.Data.Mapping;

public class CarrinhoClienteMapping : IEntityTypeConfiguration<CarrinhoCliente>
{
    public void Configure(EntityTypeBuilder<CarrinhoCliente> builder)
    {
        builder.ToTable("Carrinho");

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.ClienteId)
            .HasName("IDX_Cliente");
        
        builder.OwnsOne(c => c.ClienteCpf, tf =>
        {
            tf.Property(c => c.Numero)
                .IsRequired()
                .HasMaxLength(Cpf.CpfMaxLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.CpfMaxLength})");
        });

        builder.HasMany(c => c.Itens)
            .WithOne(c => c.Carrinho)
            .HasForeignKey(c => c.CarrinhoId);
    }
}