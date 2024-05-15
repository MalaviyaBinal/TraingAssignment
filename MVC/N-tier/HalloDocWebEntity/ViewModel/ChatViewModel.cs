
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class ChatViewModel
    {

        public int PhysicianId { get; set; }
        public int AdminId { get; set; }
        public int CurrentUserId { get; set; }
        public string SenderType { get; set; }
        public string ReceiverType { get; set; }
    }
}
