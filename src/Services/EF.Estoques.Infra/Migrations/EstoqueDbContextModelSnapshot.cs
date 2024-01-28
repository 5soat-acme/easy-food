﻿// <auto-generated />
using System;
using EF.Estoques.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EF.Estoques.Infra.Migrations
{
    [DbContext(typeof(EstoqueDbContext))]
    partial class EstoqueDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EF.Estoques.Domain.Models.Estoque", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantidade")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId")
                        .IsUnique();

                    b.ToTable("Estoques", (string)null);
                });

            modelBuilder.Entity("EF.Estoques.Domain.Models.MovimentacaoEstoque", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DataLancamento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EstoqueId")
                        .HasColumnType("uuid");

                    b.Property<int>("OrigemMovimentacao")
                        .HasColumnType("integer");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantidade")
                        .HasColumnType("integer");

                    b.Property<int>("TipoMovimentacao")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EstoqueId");

                    b.ToTable("MovimentacoesEstoque", (string)null);
                });

            modelBuilder.Entity("EF.Estoques.Domain.Models.MovimentacaoEstoque", b =>
                {
                    b.HasOne("EF.Estoques.Domain.Models.Estoque", "Estoque")
                        .WithMany("Movimentacoes")
                        .HasForeignKey("EstoqueId")
                        .IsRequired();

                    b.Navigation("Estoque");
                });

            modelBuilder.Entity("EF.Estoques.Domain.Models.Estoque", b =>
                {
                    b.Navigation("Movimentacoes");
                });
#pragma warning restore 612, 618
        }
    }
}