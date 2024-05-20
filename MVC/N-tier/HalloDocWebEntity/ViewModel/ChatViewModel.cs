
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class ChatViewModel
    {

        public int Receiver { get; set; }
        public int Sender { get; set; }
        public int CurrentUserId { get; set; }
        public string SenderType { get; set; }
        public string ReceiverType { get; set; }
        public string? ReceiverName { get; set; }
        public string SenderName { get; set; }
    }
}
