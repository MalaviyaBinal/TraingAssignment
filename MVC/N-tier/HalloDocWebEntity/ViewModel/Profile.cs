using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;
namespace HalloDocWebEntity.ViewModel
{
    public class Profile
    {
        public List<Request>? Request { get; set; }
        public User? User { get; set; }

        [Required(ErrorMessage = "First name Required....")]
        [MaxLength(50,ErrorMessage ="Maximum 50 character allowed..")]
        public string? first_name { get; set; }
        [Required(ErrorMessage = "Last name Required....")]
        public string? last_name { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [EmailAddress( ErrorMessage = "Enter a valid email...   ")]
        public string email { get; set; }
        [Required(ErrorMessage = "Street Required....")]
        public string? street { get; set; }
        [Required(ErrorMessage = "City Required....")]
        public string? city { get; set; }
        [Required(ErrorMessage = "State Required....")]
        public string? state { get; set; }
        [Required(ErrorMessage = "Zip code Required....")]
        [RegularExpression(@"[0-9]{6}", ErrorMessage = "Invalid Zip")]
        public string? zipcode { get; set; }
        [Required(ErrorMessage = "Enter Date of Birth....")]
        public DateOnly? dob { get; set; } 
    }
}
