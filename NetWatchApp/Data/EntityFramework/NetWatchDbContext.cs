using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using System;
using System.IO;

namespace NetWatchApp.Data.EntityFramework
{
    public class NetWatchDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ViewingHistory> ViewingHistories { get; set; }

        public NetWatchDbContext() : base()
        {
        }

        public NetWatchDbContext(DbContextOptions<NetWatchDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Get the application's base directory
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Define the database file path
            string dbPath = Path.Combine(baseDir, "NetWatchApp.db");

            // Configure SQLite with increased command timeout
            optionsBuilder.UseSqlite($"Data Source={dbPath}",
                options => options.CommandTimeout(120)); // Increase timeout to 2 minutes

            // Enable detailed logging in debug mode
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine);
#endif

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.IdentificationNumber)
                .IsUnique();

            // Configure Content entity
            modelBuilder.Entity<Content>()
                .HasMany(c => c.Episodes)
                .WithOne(e => e.Content)
                .HasForeignKey(e => e.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Content>()
                .HasMany(c => c.Ratings)
                .WithOne(r => r.Content)
                .HasForeignKey(r => r.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Content>()
                .HasMany(c => c.ViewingHistories)
                .WithOne(vh => vh.Content)
                .HasForeignKey(vh => vh.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure User-Rating relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Ratings)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure User-ViewingHistory relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.ViewingHistories)
                .WithOne(vh => vh.User)
                .HasForeignKey(vh => vh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Rating entity
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.ContentId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

