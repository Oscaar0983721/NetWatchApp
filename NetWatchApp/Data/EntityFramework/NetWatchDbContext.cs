using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using System.IO;

namespace NetWatchApp.Data.EntityFramework
{
    public class NetWatchDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<ViewingHistory> ViewingHistories { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "NetWatchApp.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.IdentificationNumber)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure Content entity
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Episodes)
                .WithOne(e => e.Content)
                .HasForeignKey(e => e.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ViewingHistory entity
            modelBuilder.Entity<ViewingHistory>()
                .HasOne(vh => vh.User)
                .WithMany(u => u.ViewingHistories)
                .HasForeignKey(vh => vh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ViewingHistory>()
                .HasOne(vh => vh.Content)
                .WithMany(c => c.ViewingHistories)
                .HasForeignKey(vh => vh.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ViewingHistory>()
                .HasOne(vh => vh.Episode)
                .WithMany()
                .HasForeignKey(vh => vh.EpisodeId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Configure Rating entity
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Content)
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.ContentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}