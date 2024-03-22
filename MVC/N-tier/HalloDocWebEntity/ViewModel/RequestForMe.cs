using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class RequestForMe
    {
        
        public string? symptoms { get; set; }
        public string? first_name { get; set; }

        public string? last_name { get; set; }

        public DateOnly? dob { get; set; }

        public string? email { get; set; }
        public string? phonenumber { get; set; }
        public string? street { get; set; }

        public string? city { get; set; }

        public string? state { get; set; }
      
        public string? zipcode { get; set; }
        public string? admin_notes { get; set; }

        
        public string? room { get; set; }
        public string? relation { get; set; }

        public IFormFile fileToUpload { get; set; }

    }
}
