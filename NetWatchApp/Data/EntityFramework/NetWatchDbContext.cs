using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;

namespace NetWatchApp.Data.EntityFramework
{
    public class NetWatchDbContext : DbContext
    {
        public NetWatchDbContext()
        {
        }

        public NetWatchDbContext(DbContextOptions<NetWatchDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Episode> Episodes { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<ViewingHistory> ViewingHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=NetWatch.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
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

            modelBuilder.Entity<User>()
                .HasMany(u => u.Ratings)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ViewingHistories)
                .WithOne(vh => vh.User)
                .HasForeignKey(vh => vh.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

