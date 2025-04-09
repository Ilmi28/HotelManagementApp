﻿// <auto-generated />
using System;
using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelManagementApp.Infrastructure.Database.Migrations;

[DbContext(typeof(HotelManagementAppDbContext))]
[Migration("20250405171950_Migration4")]
partial class Migration4
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "8.0.14");

        modelBuilder.Entity("HotelManagementApp.Core.Models.BlacklistedUser", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("BlacklistedDate")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("BlackListedUsers");
            });

        modelBuilder.Entity("HotelManagementApp.Core.Models.ConfirmEmailToken", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ConfirmEmailTokenHash")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<DateTime>("ExpirationDate")
                    .HasColumnType("TEXT");

                b.Property<bool>("IsRevoked")
                    .HasColumnType("INTEGER");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("ConfirmEmailTokens");
            });

        modelBuilder.Entity("HotelManagementApp.Core.Models.Operation", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Operations");

                b.HasData(
                    new
                    {
                        Id = 1,
                        Name = "REGISTER"
                    },
                    new
                    {
                        Id = 2,
                        Name = "LOGIN"
                    },
                    new
                    {
                        Id = 3,
                        Name = "CREATE"
                    },
                    new
                    {
                        Id = 4,
                        Name = "UPDATE"
                    },
                    new
                    {
                        Id = 5,
                        Name = "DELETE"
                    },
                    new
                    {
                        Id = 6,
                        Name = "PASSWORD CHANGE"
                    });
            });

        modelBuilder.Entity("HotelManagementApp.Core.Models.RefreshToken", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("ExpirationDate")
                    .HasColumnType("TEXT");

                b.Property<bool>("IsRevoked")
                    .HasColumnType("INTEGER");

                b.Property<string>("RefreshTokenHash")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("RefreshTokens");
            });

        modelBuilder.Entity("HotelManagementApp.Core.Models.ResetPasswordToken", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("ExpirationDate")
                    .HasColumnType("TEXT");

                b.Property<bool>("IsRevoked")
                    .HasColumnType("INTEGER");

                b.Property<string>("ResetPasswordTokenHash")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("ResetPasswordTokens");
            });

        modelBuilder.Entity("HotelManagementApp.Core.Models.UserLog", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("Date")
                    .HasColumnType("TEXT");

                b.Property<int>("Operation")
                    .HasColumnType("INTEGER");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("UserLogs");
            });

        modelBuilder.Entity("HotelManagementApp.Core.Models.VIPUser", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<DateTime>("VIPDate")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("VIPUsers");
            });

        modelBuilder.Entity("HotelManagementApp.Infrastructure.Database.Identity.User", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("TEXT");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("INTEGER");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("TEXT");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("INTEGER");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("TEXT");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<string>("PasswordHash")
                    .HasColumnType("TEXT");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("TEXT");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("TEXT");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("INTEGER");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex");

                b.ToTable("AspNetUsers", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("TEXT");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex");

                b.ToTable("AspNetRoles", (string)null);

                b.HasData(
                    new
                    {
                        Id = "1",
                        Name = "Client",
                        NormalizedName = "CLIENT"
                    },
                    new
                    {
                        Id = "2",
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new
                    {
                        Id = "3",
                        Name = "Manager",
                        NormalizedName = "MANAGER"
                    },
                    new
                    {
                        Id = "4",
                        Name = "Worker",
                        NormalizedName = "WORKER"
                    });
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("TEXT");

                b.Property<string>("ClaimValue")
                    .HasColumnType("TEXT");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("TEXT");

                b.Property<string>("ClaimValue")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("TEXT");

                b.Property<string>("ProviderKey")
                    .HasColumnType("TEXT");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.Property<string>("RoleId")
                    .HasColumnType("TEXT");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("TEXT");

                b.Property<string>("LoginProvider")
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .HasColumnType("TEXT");

                b.Property<string>("Value")
                    .HasColumnType("TEXT");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("HotelManagementApp.Infrastructure.Database.Identity.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("HotelManagementApp.Infrastructure.Database.Identity.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HotelManagementApp.Infrastructure.Database.Identity.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("HotelManagementApp.Infrastructure.Database.Identity.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
#pragma warning restore 612, 618
    }
}
