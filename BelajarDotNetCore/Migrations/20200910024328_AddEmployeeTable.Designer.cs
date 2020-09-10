﻿// <auto-generated />
using System;
using BelajarDotNetCoreAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BelajarDotNetCoreAPI.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20200910024328_AddEmployeeTable")]
    partial class AddEmployeeTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateDate");

                    b.Property<DateTimeOffset>("DeleteDate");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.HasKey("Id");

                    b.ToTable("tb_m_department");
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.Division", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateDate");

                    b.Property<DateTimeOffset>("DeleteDate");

                    b.Property<int>("DepartmentId");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.Employee", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<DateTimeOffset>("JoinDate");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("tb_m_employee");
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("tb_m_role");
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("tb_m_user");
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.UserRole", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("tb_tr_userRole");
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.Division", b =>
                {
                    b.HasOne("BelajarDotNetCoreAPI.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.User", b =>
                {
                    b.HasOne("BelajarDotNetCoreAPI.Models.Employee", "Employee")
                        .WithOne("User")
                        .HasForeignKey("BelajarDotNetCoreAPI.Models.User", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BelajarDotNetCoreAPI.Models.UserRole", b =>
                {
                    b.HasOne("BelajarDotNetCoreAPI.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BelajarDotNetCoreAPI.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}