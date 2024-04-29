using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class PatientRequest
    {
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "First name Required....")]
        public string first_name { get; set; } = null!;
        [Required(ErrorMessage = "Last name Required....")]
        public string? last_name { get; set; }
        [Required(ErrorMessage = "Required....")]
        public DateOnly? dob { get; set; }
        [Required(ErrorMessage = "Email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string email_user { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string? phone { get; set; }
         [Required(ErrorMessage = "Required.")]
        public string? street { get; set; }
         [Required(ErrorMessage = "Required.")]
        public string? city { get; set; }
        [Required(ErrorMessage = "Required.")]
        public string? state { get; set; }
        
        [Required(ErrorMessage = "Please provide Zip")]
        [RegularExpression(@"[0-9]{6}", ErrorMessage = "Invalid Zip")]
        public string? zip_code { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[@@$!%*?&])(?=.*\d).{8,}$", ErrorMessage = "invalid password formate")]
        public string? password { get; set; }
        [Compare(nameof(password),ErrorMessage ="Pssword and confirm password must match..")]
        public string? con_password { get; set; }

        public DateTime Createddate { get; set; } = DateTime.Now;

        public IFormFile? fileToUpload { get; set; }
    }
}
