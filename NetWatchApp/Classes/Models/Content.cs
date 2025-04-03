using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NetWatchApp.Classes.Models
{
    public class Content
    {
        public Content()
        {
            Episodes = new List<Episode>();
            Ratings = new List<Rating>();
            ViewingHistories = new List<ViewingHistory>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int ReleaseYear { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } // "Movie" or "Series"

        [Required]
        [StringLength(50)]
        public string Platform { get; set; }

        public int Duration { get; set; } // In minutes (for movies)

        public string ImagePath { get; set; }

        // Navigation properties
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ViewingHistory> ViewingHistories { get; set; }

        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Ratings == null || !Ratings.Any())
                    return 0;

                return Math.Round(Ratings.Average(r => r.Score), 1);
            }
        }
    }
}

