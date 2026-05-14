using System.ComponentModel.DataAnnotations;

namespace PhotoSpotOman.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "civil is required.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
           ErrorMessage = "Must contain an uppercase and lowercase letters && a special character")]
        public string? Password { get; set; }

    }
}
