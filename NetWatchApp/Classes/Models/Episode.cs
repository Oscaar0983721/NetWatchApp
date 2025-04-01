using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetWatchApp.Classes.Models
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int SeasonNumber { get; set; }

        public int EpisodeNumber { get; set; }

        public int DurationMinutes { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Foreign key
        public int ContentId { get; set; }

        // Navigation property
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}