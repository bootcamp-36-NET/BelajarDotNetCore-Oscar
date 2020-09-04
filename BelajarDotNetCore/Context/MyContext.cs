﻿using BelajarDotNetCoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BelajarDotNetCoreAPI.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BelajarDotNetCoreAPI.Context
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}