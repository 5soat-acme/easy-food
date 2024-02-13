using EF.Clientes.Domain.Models;
using EF.Core.Commons.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Clientes.Infra.Data.Mapping;

public class ClienteMapping : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.Nome, tf =>
        {
            tf.Property(c => c.PrimeiroNome)
                .IsRequired()
                .HasColumnName("PrimeiroNome")
                .HasColumnType("varchar(200)");

            tf.Property(c => c.Sobrenome)
                .IsRequired()
                .HasColumnName("Sobrenome")
                .HasColumnType("varchar(200)");
        });

        builder.OwnsOne(c => c.Cpf, tf =>
        {
            tf.Property(c => c.Numero)
                .IsRequired()
                .HasMaxLength(Cpf.CpfMaxLength)
                .HasColumnName("Cpf")
                .HasColumnType($"varchar({Cpf.CpfMaxLength})");
        });

        builder.OwnsOne(c => c.Email, tf =>
        {
            tf.Property(c => c.Endereco)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType($"varchar({Email.EnderecoMaxLength})");
        });
    }
}