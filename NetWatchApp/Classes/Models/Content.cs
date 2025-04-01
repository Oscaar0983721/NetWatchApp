using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetWatchApp.Classes.Models
{
    public enum ContentType
    {
        Movie,
        Series
    }

    public class Content
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public ContentType Type { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        public int ReleaseYear { get; set; }

        public int DurationMinutes { get; set; }

        [StringLength(50)]
        public string Platform { get; set; }

        [StringLength(200)]
        public string ImagePath { get; set; }

        // Navigation properties
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<ViewingHistory> ViewingHistories { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}