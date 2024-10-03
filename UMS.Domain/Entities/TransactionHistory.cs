using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Domain.Entities
{
    public class TransactionHistory
    {
        [Key]
        public int TransactionId { get; set; } // Primary key
        public int UserId { get; set; } // Foreign key referencing User
        public decimal TransactionAmount { get; set; } // Amount added or deducted
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow; // Date and time of the transaction
        public string? TransactionType { get; set; } // Type of transaction (Credit or Debit)
        public string? Remarks { get; set; } // Additional information

        // Navigation property
        public virtual User User { get; set; } // Navigation property to User
    }
}
