using System;
using System.Collections.Generic;

namespace TaskOneDraft.Models
{
    public class Lecturer
    {
        public int LecturerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // A lecturer can have multiple contracts
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
