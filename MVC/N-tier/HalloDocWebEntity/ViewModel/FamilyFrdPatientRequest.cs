using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class FamilyFrdPatientRequest
    {

        public string first_name { get; set; } = null!;

        public string? last_name { get; set; }

        public string? email { get; set; }
        public string? phone { get; set; }

        public string? relation_with { get; set; }

        public string? symptoms { get; set; }

        public string? p_first_name { get; set; }

        public string? p_last_name { get; set; }

        public string? password { get; set; }

        [Compare("password", ErrorMessage = "must match password")]
        public string? con_password { get; set; }
        public DateTime? p_dob { get; set; }


        public string? p_email { get; set; }
        public string? p_phone { get; set; }



        public string? p_street { get; set; }

        public string? p_city { get; set; }

        public string? p_state { get; set; }

        public string? p_zip_code { get; set; }

        public IFormFile fileToUpload { get; set; }

        public DateTime Createddate { get; set; } = DateTime.Now;
    }
}
