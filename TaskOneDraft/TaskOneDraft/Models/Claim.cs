using System;
using System.ComponentModel.DataAnnotations;
using TaskOneDraft.Models;

namespace TaskOneDraft.Models
{
    public class Claim
    {
        public int ClaimID { get; set; }

        [Required]
        public int ContractID { get; set; }
        public Contract Contract { get; set; }

        [Required]
        [Range(1, 744)] // Maximum hours in a month (31 days * 24 hours)
        public int HoursWorked { get; set; }

        public decimal ClaimAmount { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string ApprovalStatus { get; set; }

        // Foreign key to Lecturer
        public int LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        // Admin processing the claim
        public int? AdminID { get; set; }
        public Admin Admin { get; set; }

        // Each claim can lead to one payment
        public Payment Payment { get; set; }
    }
}
