namespace ContractMonthlyClaimSystem.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public string SupportingDocumentPath { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

}
