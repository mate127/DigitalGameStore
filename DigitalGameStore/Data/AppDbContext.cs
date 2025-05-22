using DigitalGameStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DigitalGameStore.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<Licence> Licences { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Catalogue> Catalogues { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<User>().ToTable("Users");

            // Game - Genre (many-to-one)
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Genre)
                .WithMany(x => x.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Game - Publisher (Admin) (many-to-one)
            modelBuilder.Entity<Game>()
                .HasOne(p => p.Publisher)
                .WithMany(g => g.CreatedGames)
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Game - Licence (one-to-many)
            modelBuilder.Entity<Game>()
                .HasMany(l => l.Licences)
                .WithOne(g => g.Game)
                .HasForeignKey(l => l.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            // Game - Reviews (one-to-many)
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Reviews)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review - User (many-to-one)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Licence composite key
            modelBuilder.Entity<Licence>()
                .HasKey(l => new { l.UserId, l.GameId });

            // Licence - User (many-to-one)
            modelBuilder.Entity<Licence>()
                .HasOne(l => l.User)
                .WithMany(u => u.Licences)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Game - Catalogue (many-to-many)
            modelBuilder.Entity<Game>()
            .HasMany(s => s.Catalogues)
            .WithMany(c => c.Games)
            .UsingEntity<Dictionary<string, object>>(
                "GameCatalogues", // name of existing table
                j => j.HasOne<Catalogue>().WithMany().HasForeignKey("CatalogueId"),
                j => j.HasOne<Game>().WithMany().HasForeignKey("GameId"));
        }
    }
}
