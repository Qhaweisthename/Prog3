using System;
using System.ComponentModel.DataAnnotations;

namespace   TaskOneDraft.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public decimal PaymentAmount { get; set; }

        // Foreign key reference to Claim
        public int ClaimID { get; set; }
        public Claim Claim { get; set; }
    }
}
