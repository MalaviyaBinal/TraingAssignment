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
        public string? phone { get; set; }
         [Required(ErrorMessage = "Required.")]
        public string? street { get; set; }
         [Required(ErrorMessage = "Required.")]
        public string? city { get; set; }
        [Required(ErrorMessage = "Required.")]
        public string? state { get; set; }
        [Required(ErrorMessage = "Required.")]
        public string? zip_code { get; set; }

        public DateTime Createddate { get; set; } = DateTime.Now;

        public IFormFile? fileToUpload { get; set; }
    }
}
