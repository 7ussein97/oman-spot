using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSpotOman.Models
{
    public class SpotImage
    {
        public int Id { get; set; }

        [ForeignKey("Spot")]
        public int SpotId { get; set; }
        public Spot? Spot { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        [ForeignKey("User")]
        public int UploadedBy { get; set; }
        public User? User { get; set; }
    }
}
