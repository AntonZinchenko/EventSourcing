﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Transfer.Storage.Migrations
{
    [DbContext(typeof(TransferDbContext))]
    partial class TransferDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Transfer.Orchestrators.Transfer.TransferState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnName("TransferId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SourceAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Sum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("TargetAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CorrelationId");

                    b.ToTable("TransferStates");
                });
#pragma warning restore 612, 618
        }
    }
}
