﻿// <auto-generated />
using System;
using EF.Cupons.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EF.Cupons.Infra.Migrations
{
    [DbContext(typeof(CupomDbContext))]
    [Migration("20231216113755_BaseInicialCupons")]
    partial class BaseInicialCupons
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EF.Cupons.Domain.Models.Cupom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CodigoCupom")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("PorcentagemDesconto")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CodigoCupom");

                    b.ToTable("Cupons", (string)null);
                });

            modelBuilder.Entity("EF.Cupons.Domain.Models.CupomProduto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CupomId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CupomId");

                    b.ToTable("CupomProdutos", (string)null);
                });

            modelBuilder.Entity("EF.Cupons.Domain.Models.CupomProduto", b =>
                {
                    b.HasOne("EF.Cupons.Domain.Models.Cupom", "Cupom")
                        .WithMany("CupomProdutos")
                        .HasForeignKey("CupomId")
                        .IsRequired();

                    b.Navigation("Cupom");
                });

            modelBuilder.Entity("EF.Cupons.Domain.Models.Cupom", b =>
                {
                    b.Navigation("CupomProdutos");
                });
#pragma warning restore 612, 618
        }
    }
}
