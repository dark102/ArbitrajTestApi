﻿// <auto-generated />
using System;
using ArbitrajTestApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArbitrajTestApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250401070441_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ArbitrajTestApi.Models.ArbitrageData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BiQuarterFuturePrice")
                        .HasColumnType("numeric");

                    b.Property<string>("BiQuarterFutureSymbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("PercentageDifference")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PriceDifference")
                        .HasColumnType("numeric");

                    b.Property<decimal>("QuarterFuturePrice")
                        .HasColumnType("numeric");

                    b.Property<string>("QuarterFutureSymbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Timestamp")
                        .IsDescending()
                        .HasDatabaseName("IX_ArbitrageData_Timestamp");

                    b.ToTable("ArbitrageData");
                });

            modelBuilder.Entity("ArbitrajTestApi.Models.FuturesPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Timestamp")
                        .IsDescending()
                        .HasDatabaseName("IX_FuturesPrice_Timestamp");

                    b.ToTable("FuturesPrices");
                });

            modelBuilder.Entity("ArbitrajTestApi.Models.Log", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("id"));

                    b.Property<string>("exception")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("level")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("log_event")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("logevent")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("message_template")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("messagetemplate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("properties")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.HasIndex("level")
                        .HasDatabaseName("IX_Logs_Level");

                    b.HasIndex("timestamp")
                        .IsDescending()
                        .HasDatabaseName("IX_Logs_Timestamp");

                    b.ToTable("logs");
                });

            modelBuilder.Entity("ArbitrajTestApi.Models.TrackedPairs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("BiQuarterFutureSymbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastDateOfEntry")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("QuarterFutureSymbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isNew")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("QuarterFutureSymbol", "BiQuarterFutureSymbol")
                        .IsUnique();

                    b.ToTable("TrackedPairs");
                });
#pragma warning restore 612, 618
        }
    }
}
