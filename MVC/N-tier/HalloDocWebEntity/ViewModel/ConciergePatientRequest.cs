using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class ConciergePatientRequest
    {
        public string first_name { get; set; } = null!;

        public string? password { get; set; }

        [Compare("password", ErrorMessage = "must match password")]
        public string? con_password { get; set; }

        public string? last_name { get; set; }

        public string? email { get; set; }
        public string? phone { get; set; }

        public string hotel_name { get; set; } = string.Empty;

        public string? h_street { get; set; }

        public string? h_city { get; set; }

        public string? h_state { get; set; }

        public string? h_zip_code { get; set; }


        public string? symptoms { get; set; }

        public string? p_first_name { get; set; }

        public string? p_last_name { get; set; }

        public DateTime? p_dob { get; set; }

        public string? p_email { get; set; }
        public string? p_phone { get; set; }

        public DateTime Createddate { get; set; } = DateTime.Now;
        public IFormFile fileToUploade { get; set; }
    }
}
