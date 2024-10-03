using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; } // Primary key
        public string? Username { get; set; } // Unique username (email)
        public string? PasswordHash { get; set; } // Hashed password
        public string? FirstName { get; set; } // User's first name
        public string? LastName { get; set; } // User's last name
        public string? Device { get; set; } // Device info
        public string? IPAddress { get; set; } // User's IP address
        public decimal Balance { get; set; } // User's account balance
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Account creation date

        // Navigation properties can be added later if needed
        public virtual ICollection<LoginHistory> LoginHistories { get; set; }
        public virtual ICollection<TransactionHistory> TransactionHistories { get; set; }
    }
}
