using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class Email_SMS_LogModel
    {
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
    }
}
