﻿// <auto-generated />
using System;
using EF.Pedidos.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EF.Pedidos.Infra.Data.Migrations
{
    [DbContext(typeof(PedidoDbContext))]
    partial class PedidoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EF.Pedidos.Domain.Models.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Desconto")
                        .HasColumnType("numeric");

                    b.Property<string>("NomeProduto")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantidade")
                        .HasColumnType("integer");

                    b.Property<decimal>("ValorUnitario")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.ToTable("ItensPedido", (string)null);
                });

            modelBuilder.Entity("EF.Pedidos.Domain.Models.Pedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClienteId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CorrelacaoId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CorrelacaoId")
                        .HasDatabaseName("IDX_CorrelacaoId");

                    b.ToTable("Pedidos", (string)null);
                });

            modelBuilder.Entity("EF.Pedidos.Domain.Models.Item", b =>
                {
                    b.HasOne("EF.Pedidos.Domain.Models.Pedido", "Pedido")
                        .WithMany("Itens")
                        .HasForeignKey("PedidoId")
                        .IsRequired();

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("EF.Pedidos.Domain.Models.Pedido", b =>
                {
                    b.Navigation("Itens");
                });
#pragma warning restore 612, 618
        }
    }
}
