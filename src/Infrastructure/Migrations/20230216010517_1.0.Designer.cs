﻿// <auto-generated />
using System;
using PoEGamblingHelper.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PoEGamblingHelper.Infrastructure.Database;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230216010517_1.0")]
    partial class _10
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entity.Currency", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<decimal>("ChaosEquivalent")
                        .HasColumnType("numeric");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("Domain.Entity.Gem.GemData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GemData");
                });

            modelBuilder.Entity("Domain.Entity.Gem.GemTradeData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("ChaosValue")
                        .HasColumnType("numeric");

                    b.Property<bool>("Corrupted")
                        .HasColumnType("boolean");

                    b.Property<string>("DetailsId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("DivineValue")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ExaltedValue")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("GemDataId")
                        .HasColumnType("uuid");

                    b.Property<int>("GemLevel")
                        .HasColumnType("integer");

                    b.Property<int>("GemQuality")
                        .HasColumnType("integer");

                    b.Property<int>("ListingCount")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GemDataId");

                    b.ToTable("GemTradeData");
                });

            modelBuilder.Entity("Domain.Entity.League", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("League");
                });

            modelBuilder.Entity("Domain.Entity.TempleCost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ChaosValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("TempleCost");
                });

            modelBuilder.Entity("Domain.Entity.Gem.GemTradeData", b =>
                {
                    b.HasOne("Domain.Entity.Gem.GemData", null)
                        .WithMany("Gems")
                        .HasForeignKey("GemDataId");
                });

            modelBuilder.Entity("Domain.Entity.Gem.GemData", b =>
                {
                    b.Navigation("Gems");
                });
#pragma warning restore 612, 618
        }
    }
}
