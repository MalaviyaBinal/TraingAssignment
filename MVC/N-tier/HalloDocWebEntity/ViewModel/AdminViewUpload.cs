
using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Http;

namespace HalloDocWebEntity.ViewModel
{
    public class AdminViewUpload
    {
        public List<Requestwisefile> FileList { get; set; }
        public Requestclient patientData { get; set; }
        public Request confirmationDetail { get; set; }


        public IFormFile fileToUpload { get; set; }
        public DateOnly? DOB { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }

    }
}
