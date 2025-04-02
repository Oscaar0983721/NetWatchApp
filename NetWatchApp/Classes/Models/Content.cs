using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public int ReleaseYear { get; set; }

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } // Movie or Series

        [Required]
        [MaxLength(50)]
        public string Platform { get; set; }

        public int Duration { get; set; } // In minutes (for movies)

        // Property for image
        [MaxLength(255)]
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
                if (Ratings == null || Ratings.Count == 0)
                    return 0;

                double sum = 0;
                foreach (var rating in Ratings)
                {
                    if (rating != null)
                        sum += rating.Score;
                }
                return Math.Round(sum / Ratings.Count, 1);
            }
        }
    }
}

