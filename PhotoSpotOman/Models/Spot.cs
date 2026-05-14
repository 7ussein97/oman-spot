using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhotoSpotOman.Models
{
    public class Spot
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? LocationName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Region { get; set; }
        public string Status { get; set; } = "pending"; // approved, pending, rejected

        [ForeignKey("User")]
        public int AddedBy { get; set; }
        public User? User { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<SpotImage>? Images { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
    }
}
