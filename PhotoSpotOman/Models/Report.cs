using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSpotOman.Models
{
    public class Report
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int ReportedBy { get; set; }
        public User? User { get; set; }

        public int? SpotId { get; set; }
        public Spot? Spot { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
