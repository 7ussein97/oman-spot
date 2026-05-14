using System.ComponentModel.DataAnnotations;

namespace PhotoSpotOman.Models
{
    public class User
    {
      

            public int Id { get; set; }
            [Required(ErrorMessage = "civil is required.")]
            public string? Name { get; set; }
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string? Email { get; set; }
            [Required(ErrorMessage = "Password is required.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
               ErrorMessage = "Must contain an uppercase and lowercase letters && a special character")]
            public string? Password { get; set; }
            public string? Role { get; set; }
            public bool? IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public DateTime? UpdatedAt { get; set; }
        public ICollection<Spot>? Spots { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }

    }
    }


