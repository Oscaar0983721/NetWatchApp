using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetWatchApp.Classes.Models
{
    public class ViewingHistory
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ContentId { get; set; }

        public DateTime WatchDate { get; set; }

        public string WatchedEpisodes { get; set; } // Comma-separated list of episode numbers

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}

