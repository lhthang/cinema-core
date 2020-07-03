using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models.Base
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<ScreenType> ScreenTypes { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomScreenType> RoomScreenTypes { get; set; }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieScreenType> MovieScreenTypes { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<MovieActor> MovieActors { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<Cluster> Clusters { get; set; }
        public virtual DbSet<ClusterUser> ClusterUsers { get; set; }

        public virtual DbSet<Showtime> Showtime { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
            .Property(e => e.Wallpapers)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Movie>()
            .Property(e => e.Languages)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Movie>()
            .Property(e => e.Directors)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

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

            //Movie-ScreenType
            modelBuilder.Entity<MovieScreenType>()
               .HasKey(ms => new { ms.MovieId, ms.ScreenTypeId });

            modelBuilder.Entity<MovieScreenType>()
                .HasOne(m => m.Movie)
                .WithMany(ms => ms.MovieScreenTypes)
                .HasForeignKey(b => b.MovieId);

            modelBuilder.Entity<MovieScreenType>()
                .HasOne(s => s.ScreenType)
                .WithMany(ms => ms.MovieScreenTypes)
                .HasForeignKey(s => s.ScreenTypeId);

            //Movie-Actor
            modelBuilder.Entity<MovieActor>()
               .HasKey(ms => new { ms.MovieId, ms.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(m => m.Movie)
                .WithMany(ms => ms.MovieActors)
                .HasForeignKey(b => b.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(s => s.Actor)
                .WithMany(ms => ms.MovieActors)
                .HasForeignKey(s => s.ActorId);

            //Movie-Genre
            modelBuilder.Entity<MovieGenre>()
               .HasKey(ms => new { ms.MovieId, ms.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(m => m.Movie)
                .WithMany(ms => ms.MovieGenres)
                .HasForeignKey(b => b.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(s => s.Genre)
                .WithMany(ms => ms.MovieGenres)
                .HasForeignKey(s => s.GenreId);

            //Cluster-User (0..1 to 0..1)
            modelBuilder.Entity<ClusterUser>()
                .HasKey(cu => new { cu.ClusterId, cu.UserId });
            modelBuilder.Entity<ClusterUser>()
                .HasIndex(cu => cu.ClusterId).IsUnique();
            modelBuilder.Entity<ClusterUser>()
                .HasIndex(cu => cu.UserId).IsUnique();

            //Cluster-Room (1 to n)
            modelBuilder.Entity<Cluster>()
                .HasMany(c => c.Rooms)
                .WithOne(r => r.Cluster)
                .HasForeignKey(r => r.ClusterId);
                
            //Movie-Rate
            modelBuilder.Entity<Movie>()
                .HasOne(s => s.Rate)
                .WithMany(m => m.Movies)
                .OnDelete(DeleteBehavior.SetNull);

            //Showtime
            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Showtimes)
                .IsRequired();
            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Showtimes)
                .IsRequired();
            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.ScreenType)
                .WithMany(st => st.Showtimes)
                .IsRequired();
            modelBuilder.Entity<Showtime>()
                .HasOne(s => s.ScreenType)
                .WithMany(st => st.Showtimes)
                .IsRequired();
            modelBuilder.Entity<Showtime>()
                .HasMany(s => s.Tickets)
                .WithOne(t => t.Showtime);

            //Ticket
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Showtime)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.ShowtimeId);
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Promotion)
                .WithMany(p => p.Tickets);

            //Promotion
            modelBuilder.Entity<Promotion>()
                .HasMany(p => p.Tickets)
                .WithOne(t => t.Promotion);
        }
    }
}
