using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Twilio.TwiML.Messaging;

namespace HalloDoc.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _context;
        public ChatHub(ApplicationContext context)
        {
            _context = context;
        }
        public void SendMessage(string Sender, string SenderType, string Receiver, string ReceiverType, string Receiver2)
        {
            int senderId = 0, receiverId = 0, receiver2Id = 0;
            List<Chat> data = new();
            switch (SenderType)
            {
                case "Admin":
                    senderId = _context.Admins.FirstOrDefault(e => e.Adminid == int.Parse(Sender)).Aspnetuserid;
                    break;
                case "Patient":
                    senderId = int.Parse(Sender);
                    break;
                case "Provider":
                    senderId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Sender)).Aspnetuserid;
                    break;

            }
            switch (ReceiverType)
            {
                case "Admin":
                    receiverId = _context.Admins.FirstOrDefault(e => e.Adminid == int.Parse(Receiver)).Aspnetuserid;
                    break;
                case "Patient":
                    receiverId = int.Parse(Receiver);
                    break;
                case "Provider":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    break;
                case "AdminGroup":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    receiver2Id = int.Parse(Receiver2);
                    break;
                case "ProviderGroup":
                    receiverId = int.Parse(Receiver);
                    receiver2Id = int.Parse(Receiver2);
                    break; 
                case "PatientGroup":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    receiver2Id = int.Parse(Receiver2);
                    break;
            }
            if (ReceiverType == "PatientGroup" ||ReceiverType == "ProviderGroup" ||ReceiverType == "AdminGroup" )
            {
                data = _context.Chats.Where(e =>
                (e.SenderId == senderId || e.ReceiverId == senderId || e.Receiver2Id == senderId)
                &&
                (e.SenderId == receiver2Id || e.ReceiverId == receiver2Id || e.Receiver2Id == receiver2Id)
                &&
                (e.SenderId == receiverId || e.ReceiverId == receiverId || e.Receiver2Id == receiverId) && e.IsGroup == true).OrderBy(e =>e .SentDate).ToList();
            }
            else
            {
                data = _context.Chats.Where(e => (e.SenderId == senderId || e.ReceiverId == senderId) && (e.SenderId == receiverId || e.ReceiverId == receiverId) && e.IsGroup == false).ToList();
            }
            string[] str = { "10","20","30"};
            // to All clients
            Clients.All.SendAsync("ReceiveMessage", data);
            //to single client
            Clients.Clients(str.ToList()).SendAsync("ReceiveMessage", data);

        }
        public void SaveData(string Sender, string SenderType, string Receiver, string ReceiverType, string message, string Receiver2)
        {
            int senderId = 0, receiverId = 0, receiver2Id = 0;
            switch (SenderType)
            {
                case "Admin":
                    senderId = _context.Admins.FirstOrDefault(e => e.Adminid == int.Parse(Sender)).Aspnetuserid;
                    break;
                case "Patient":
                    senderId = int.Parse(Sender);
                    break;
                case "Provider":
                    senderId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Sender)).Aspnetuserid;
                    break;
            }
            switch (ReceiverType)
            {
                case "Admin":
                    receiverId = _context.Admins.FirstOrDefault(e => e.Adminid == int.Parse(Receiver)).Aspnetuserid;
                    break;
                case "Patient":
                    receiverId = int.Parse(Receiver);
                    break;
                case "Provider":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    break;
                case "AdminGroup":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    receiver2Id = int.Parse(Receiver2);
                    break;
                case "ProviderGroup":
                    receiverId = int.Parse(Receiver);
                    receiver2Id = int.Parse(Receiver2);
                    break;
                case "PatientGroup":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    receiver2Id = _context.Admins.FirstOrDefault(e => e.Adminid == int.Parse(Receiver)).Aspnetuserid;
                    break;
            }
            Chat chat = new Chat
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                SentDate = DateTime.Now,
                SentTime = TimeOnly.FromDateTime(DateTime.Now),
                IsGroup = false
            };
            if (ReceiverType == "PatientGroup" || ReceiverType == "ProviderGroup" || ReceiverType == "AdminGroup")
            {
                chat.Receiver2Id = receiver2Id;
                chat.IsGroup = true;
            }
            _context.Chats.Add(chat);
            _context.SaveChanges();
        }
    }
}
