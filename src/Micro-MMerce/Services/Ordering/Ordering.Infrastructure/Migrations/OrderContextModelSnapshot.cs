﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ordering.Infrastructure.Persistence;

#nullable disable

namespace Ordering.Infrastructure.Migrations
{
    [DbContext(typeof(OrderContext))]
    partial class OrderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Ordering.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AddressLine")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CardName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Expiration")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ZipCode")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
