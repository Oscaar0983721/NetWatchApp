using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetWatchApp.Classes.Models
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }

        public int ContentId { get; set; }

        public int EpisodeNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int Duration { get; set; } // In minutes

        // Navigation property
        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}

