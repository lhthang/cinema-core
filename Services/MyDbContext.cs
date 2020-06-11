﻿using cinema_core.Models;
using cinema_core.Models.Rate;
using cinema_core.Models.Room;
using cinema_core.Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Services
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options) {
            Database.Migrate();
        }

        public virtual DbSet<ScreenType> ScreenTypes { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomScreenType> RoomScreenTypes { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
               .HasKey(us => new { us.RoleId, us.UserId });

            modelBuilder.Entity<UserRole>()
                .HasOne(b => b.User)
                .WithMany(bc => bc.UserRoles)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(b => b.Role)
                .WithMany(bc => bc.UsersRole)
                .HasForeignKey(b => b.RoleId);

            modelBuilder.Entity<RoomScreenType>()
               .HasKey(rs => new { rs.RoomId, rs.ScreenTypeId });

            modelBuilder.Entity<RoomScreenType>()
                .HasOne(b => b.Room)
                .WithMany(bc => bc.RoomScreenTypes)
                .HasForeignKey(b => b.RoomId);

            modelBuilder.Entity<RoomScreenType>()
                .HasOne(b => b.ScreenType)
                .WithMany(bc => bc.RoomScreenTypes)
                .HasForeignKey(b => b.ScreenTypeId);
        }
    }
}
