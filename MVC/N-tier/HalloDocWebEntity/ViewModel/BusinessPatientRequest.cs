using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class BusinessPatientRequest
    {
        [Required(ErrorMessage = "First name Required....")]
        public string first_name { get; set; } = null!;
        [Required(ErrorMessage = "Last name Required....")]
        public string? last_name { get; set; }
        [Required(ErrorMessage = "Required....")]
        public string isExist { get; set; }
        //public string? password { get; set; }

        //[Compare("password", ErrorMessage = "must match password")]
        //public string? con_password { get; set; }

        [Required(ErrorMessage = "Email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        //[RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]

        public string? phone { get; set; }
        [Required(ErrorMessage = "Provide Business Name....")]
        public string business_name { get; set; } = string.Empty;

        public string? case_no { get; set; }

        public string? symptoms { get; set; }
        [Required(ErrorMessage = "Patient name Required....")]
        public string? p_first_name { get; set; }
        [Required(ErrorMessage = "Patient name Required....")]
        public string? p_last_name { get; set; }
        [Required(ErrorMessage = "Required....")]
        public DateTime? dob { get; set; }
        [Required(ErrorMessage = "Email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string? p_email { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]

        public string? p_phone { get; set; }

        [Required(ErrorMessage = "Please provider Street")]

        public string? p_street { get; set; }
        [Required(ErrorMessage = "Please provider City")]
        public string? p_city { get; set; }
        [Required(ErrorMessage = "Please provider State")]
        public string? p_state { get; set; }
        [Required(ErrorMessage = "Please provider Zip")]
        public string? p_zip_code { get; set; }

        public DateTime Createddate { get; set; } = DateTime.Now;
        public IFormFile? fileToUpload { get; set; }
    }
}
