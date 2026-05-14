using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSpotOman.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Spot")]
        public int SpotId { get; set; }
        public Spot? Spot { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       
    }
}
