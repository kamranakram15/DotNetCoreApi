using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Domain.Entities
{
    public class LoginHistory
    {
        [Key]
        public int LoginId { get; set; } // Primary key
        public int UserId { get; set; } // Foreign key referencing User
        public DateTime LoginTime { get; set; } = DateTime.UtcNow; // Time of login
        public string? IPAddress { get; set; } // IP address used during login
        public string? Device { get; set; } // Device information
        public bool IsFirstLogin { get; set; } // Flag to indicate first login

        // Navigation property
        public virtual User User { get; set; } // Navigation property to User
    }
}
