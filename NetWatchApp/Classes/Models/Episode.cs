using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetWatchApp.Classes.Models
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public int Duration { get; set; } // In minutes

        // Foreign key
        public int ContentId { get; set; }

        // Navigation property
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}

