
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class ChatViewModel
    {

        public int Receiver { get; set; }
        public int Receiver1 { get; set; }
        public string Receiver1Name { get; set; }
        public int? Receiver2 { get; set; }
        public string? Receiver2Name { get; set; }
        public int Sender { get; set; }
        public int CurrentUserId { get; set; }
        public string SenderType { get; set; }
        public string ReceiverType { get; set; }
        public string? ReceiverName { get; set; }
        public string SenderName { get; set; }
    }
}
