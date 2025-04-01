using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetWatchApp.Classes.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string IdentificationNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime RegistrationDate { get; set; }

        // Navigation properties
        public virtual ICollection<ViewingHistory> ViewingHistories { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}