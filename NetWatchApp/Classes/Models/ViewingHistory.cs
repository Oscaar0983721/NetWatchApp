using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetWatchApp.Classes.Models
{
    public class ViewingHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime WatchDate { get; set; }

        // For series, store comma-separated episode numbers
        public string WatchedEpisodes { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public int ContentId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}

