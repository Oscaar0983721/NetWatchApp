using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetWatchApp.Classes.Models
{
    public class User
    {
        public User()
        {
            Ratings = new List<Rating>();
            ViewingHistories = new List<ViewingHistory>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string IdentificationNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        // Navigation properties
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ViewingHistory> ViewingHistories { get; set; }
    }
}

