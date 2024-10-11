using System;
using System.Collections.Generic;
using TaskOneDraft.Models;

namespace TaskOneDraft.Models
{
    public class Contract
    {
        public int ContractID { get; set; }
        public string ContractName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RatePerHour { get; set; }

        // Foreign key reference to Lecturer
        public int LecturerID { get; set; }
        public Lecturer Lecturer { get; set; }

        // Each contract can have multiple claims
        public ICollection<Claim> Claims { get; set; }
    }
}
