using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class PatientRequest
    {
        public string? symptoms { get; set; }
        public string first_name { get; set; } = null!;

        public string? last_name { get; set; }

        public DateOnly? dob { get; set; }

        public string email_user { get; set; } = "abx";
        public string? phone { get; set; }

        public string? street { get; set; }

        public string? city { get; set; }

        public string? state { get; set; }

        public string? password { get; set; }

        [Compare("password", ErrorMessage = "must match password")]
        public string? con_password { get; set; }

        public string? zip_code { get; set; }

        public DateTime Createddate { get; set; } = DateTime.Now;

        public IFormFile? fileToUpload { get; set; }
    }
}
