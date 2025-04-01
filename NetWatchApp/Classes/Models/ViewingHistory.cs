using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetWatchApp.Classes.Models
{
    public class ViewingHistory
    {
        [Key]
        public int Id { get; set; }

        public DateTime ViewDate { get; set; }

        public int WatchedMinutes { get; set; }

        public bool Completed { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public int? EpisodeId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }

        [ForeignKey("EpisodeId")]
        public virtual Episode Episode { get; set; }
    }
}