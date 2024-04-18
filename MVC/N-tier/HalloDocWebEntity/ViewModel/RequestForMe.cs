using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class RequestForMe
    {
       
        public string? symptoms { get; set; }
        [Required(ErrorMessage = "Required....")]
        public string? first_name { get; set; }
        [Required(ErrorMessage = "Required....")]
        public string? last_name { get; set; }
        [Required(ErrorMessage = "Required....")]
        public DateOnly? dob { get; set; }
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]

        public string? phonenumber { get; set; }
        [Required(ErrorMessage = "Required....")]
        public string? street { get; set; }
        [Required(ErrorMessage = "Required....")]
        public string? city { get; set; }
        [Required(ErrorMessage = "Required....")]
        public string? state { get; set; }
        [Required(ErrorMessage = "Required....")]
       
        public string? zipcode { get; set; }

        public string? admin_notes { get; set; }

        
        public string? room { get; set; }
        public string? relation { get; set; }

        public IFormFile fileToUpload { get; set; }

    }
}
