using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class Email_SMS_LogModel
    {
        public List<Email_SMS_LogModel> Logs { get; set; }
        public string Receipient { get; set; }
        public int? Actions { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string createdDate { get; set; }
        public string sentDate { get; set; }
        public string sent { get; set; }
        public string sentTries { get; set; }
        public string ConfirmationNum { get; set; }
        public int LogID { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
