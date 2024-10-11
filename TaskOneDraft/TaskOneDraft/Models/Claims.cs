using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskOneDraft.Models
{
    public class Claims
    {
        //cols yoou want in the claims table 
        public int Id { get; set; }
        [Required]
        public string ?LecturerID { get; set; }
        [Required]
        public string ?FirstName { get; set; }
        [Required]
        public string ?LastName { get; set; }
        [Required]
        public DateTime ClaimsPeriodStart { get; set; }
        [Required]
        public DateTime ClaimsPeriodEnd { get; set; }
        [Required]
        public double HoursWorked { get; set; }
        [Required]
        public double RatePerHour { get; set; }
        [Required]
        public double TotalAmount { get; set; }
        public string ?DescriptionOfWork { get; set; }
        //Mark rhis property as notmapped so EF
        [NotMapped]
        public List<IFormFile> SupportingDocuments { get; set; }
        //status tag/ property 
        public string? Status { get; set; } = "Approved";

    }
}
