﻿// <auto-generated />
using System;
using CryptoWallet.Migraciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CryptoWallet.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CryptoWallet.Models.CreditCard", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("id"), 1L, 1);

                    b.Property<string>("cardHolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("cardNumber")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("idUser")
                        .HasColumnType("int");

                    b.Property<int?>("securityCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("idUser");

                    b.ToTable("creditCards");
                });

            modelBuilder.Entity("CryptoWallet.Models.CryptoWallet", b =>
                {
                    b.Property<int>("CryptoWalletID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CryptoWalletID"), 1L, 1);

                    b.Property<int>("WalletID")
                        .HasColumnType("int");

                    b.Property<int>("cryptoType")
                        .HasColumnType("int");

                    b.HasKey("CryptoWalletID");

                    b.HasIndex("WalletID");

                    b.ToTable("cryptoWallets");
                });

            modelBuilder.Entity("CryptoWallet.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"), 1L, 1);

                    b.Property<int>("CryptoWalletID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("cryptoType")
                        .HasColumnType("int");

                    b.Property<float>("monto")
                        .HasColumnType("real");

                    b.Property<int>("transactionNumber")
                        .HasColumnType("int");

                    b.Property<int>("transactionTypes")
                        .HasColumnType("int");

                    b.HasKey("TransactionID");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("CryptoWallet.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("CryptoWallet.Models.Wallet", b =>
                {
                    b.Property<int>("WalletID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WalletID"), 1L, 1);

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("WalletID");

                    b.ToTable("wallets");
                });

            modelBuilder.Entity("CryptoWallet.Models.CreditCard", b =>
                {
                    b.HasOne("CryptoWallet.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("idUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("CryptoWallet.Models.CryptoWallet", b =>
                {
                    b.HasOne("CryptoWallet.Models.Wallet", null)
                        .WithMany("cryptoWallets")
                        .HasForeignKey("WalletID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CryptoWallet.Models.Wallet", b =>
                {
                    b.Navigation("cryptoWallets");
                });
#pragma warning restore 612, 618
        }
    }
}
