using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSpotOman.Models
{
    public class Like
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Spot")]
        public int SpotId { get; set; }
        public Spot? Spot { get; set; }
    }
}
