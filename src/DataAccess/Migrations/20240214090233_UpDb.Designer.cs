﻿// <auto-generated />
using System;
using DataAccessLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(ProjectDbContext))]
    [Migration("20240214090233_UpDb")]
    partial class UpDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.Entities.Licenses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ActivationDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("date");

                    b.Property<int>("LicenseStatus")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Licenses", (string)null);
                });

            modelBuilder.Entity("Models.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("Models.Entities.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Models.Entities.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("LicensesId")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int>("TicketStatus")
                        .HasColumnType("int");

                    b.Property<int>("TicketType")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LicensesId");

                    b.HasIndex("UserId");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("LicensesId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LicensesId");

                    b.HasIndex("RolesId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Models.Entities.UserRefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRefreshToken");
                });

            modelBuilder.Entity("Models.Entities.Licenses", b =>
                {
                    b.HasOne("Models.Entities.Product", "Product")
                        .WithMany("Licenses")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_Licenses_Product_Id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Models.Entities.Ticket", b =>
                {
                    b.HasOne("Models.Entities.Licenses", "Licenses")
                        .WithMany("Ticket")
                        .HasForeignKey("LicensesId")
                        .HasConstraintName("FK_Ticket_Licenses_Id");

                    b.HasOne("Models.Entities.User", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Ticket_UserId");

                    b.Navigation("Licenses");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Entities.User", b =>
                {
                    b.HasOne("Models.Entities.Licenses", "Licenses")
                        .WithMany("Users")
                        .HasForeignKey("LicensesId")
                        .HasConstraintName("FK_Users_Licenses_Id");

                    b.HasOne("Models.Entities.Roles", "Roles")
                        .WithMany("Users")
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Users_Role_Id");

                    b.Navigation("Licenses");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Models.Entities.UserRefreshToken", b =>
                {
                    b.HasOne("Models.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_RefreshTokens_User_Id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Entities.Licenses", b =>
                {
                    b.Navigation("Ticket");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Models.Entities.Product", b =>
                {
                    b.Navigation("Licenses");
                });

            modelBuilder.Entity("Models.Entities.Roles", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Models.Entities.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
