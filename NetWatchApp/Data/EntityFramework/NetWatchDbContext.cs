using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using System;
using System.IO;

namespace NetWatchApp.Data.EntityFramework
{
    public class NetWatchDbContext : DbContext
    {
        private static string _dbPath;

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
            if (!optionsBuilder.IsConfigured)
            {
                // Verificar si ya tenemos una ruta de base de datos definida
                if (string.IsNullOrEmpty(_dbPath))
                {
                    // Obtener el directorio base de la aplicación
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                    // Definir la ruta del archivo de base de datos
                    string dbFileName = "NetWatchApp.db";

                    // Si existe un archivo con nombre alternativo, usarlo
                    string altDbPath = Path.Combine(baseDir, "NetWatchApp_new.db");
                    if (File.Exists(altDbPath))
                    {
                        _dbPath = altDbPath;
                    }
                    else
                    {
                        _dbPath = Path.Combine(baseDir, dbFileName);
                    }
                }

                // Configurar SQLite con tiempo de espera aumentado
                optionsBuilder.UseSqlite($"Data Source={_dbPath}",
                    options => options.CommandTimeout(120)); // Aumentar tiempo de espera a 2 minutos

                // Habilitar registro detallado en modo debug
#if DEBUG
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.LogTo(Console.WriteLine);
#endif
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar entidad User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.IdentificationNumber).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.IsAdmin).IsRequired();
                entity.Property(u => u.RegistrationDate).IsRequired();

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.IdentificationNumber).IsUnique();
            });

            // Configurar entidad Content
            modelBuilder.Entity<Content>(entity =>
            {
                entity.ToTable("Contents");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).IsRequired();
                entity.Property(c => c.ReleaseYear).IsRequired();
                entity.Property(c => c.Genre).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Type).IsRequired().HasMaxLength(20);
                entity.Property(c => c.Platform).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Duration).IsRequired();

                entity.HasMany(c => c.Episodes)
                    .WithOne(e => e.Content)
                    .HasForeignKey(e => e.ContentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Ratings)
                    .WithOne(r => r.Content)
                    .HasForeignKey(r => r.ContentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.ViewingHistories)
                    .WithOne(vh => vh.Content)
                    .HasForeignKey(vh => vh.ContentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar entidad Episode
            modelBuilder.Entity<Episode>(entity =>
            {
                entity.ToTable("Episodes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EpisodeNumber).IsRequired();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Duration).IsRequired();
            });

            // Configurar entidad Rating
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Ratings");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Score).IsRequired();
                entity.Property(r => r.RatingDate).IsRequired();

                entity.HasIndex(r => new { r.UserId, r.ContentId }).IsUnique();

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Ratings)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar entidad ViewingHistory
            modelBuilder.Entity<ViewingHistory>(entity =>
            {
                entity.ToTable("ViewingHistories");
                entity.HasKey(vh => vh.Id);
                entity.Property(vh => vh.WatchDate).IsRequired();

                entity.HasOne(vh => vh.User)
                    .WithMany(u => u.ViewingHistories)
                    .HasForeignKey(vh => vh.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

