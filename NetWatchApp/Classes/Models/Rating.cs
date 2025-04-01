using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetWatchApp.Classes.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        public int Score { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime RatingDate { get; set; }

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