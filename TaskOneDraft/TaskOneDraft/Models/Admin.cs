namespace TaskOneDraft.Models
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        // Each admin processes multiple claims
        public ICollection<Claim> Claims { get; set; }
    }
}
