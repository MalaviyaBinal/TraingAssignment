using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HalloDoc.Web.Hubs
{
    public class ChatHub : Hub
    {

        private readonly ApplicationContext _context;
        public ChatHub(ApplicationContext context)
        {
            _context = context;
        }
        public void SendMessage(string Sender, string SenderType, string Receiver, string ReceiverType)
        {
            int senderId = 0, receiverId = 0;
            switch (SenderType)
            {
                case "Admin":
                    senderId = _context.Admins.FirstOrDefault(e => e.Adminid == int.Parse(Sender)).Aspnetuserid;
                    break;
                case "Patient":
                    int? temp = _context.Requests.FirstOrDefault(e => e.Requestid == int.Parse(Sender)).Userid;
                    senderId = (int)_context.Users.FirstOrDefault(e => e.Userid == (int)temp).Aspnetuserid;
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
                    int? temp = _context.Requests.FirstOrDefault(e => e.Requestid == int.Parse(Receiver)).Userid;
                    receiverId = (int)_context.Users.FirstOrDefault(e => e.Userid == (int)temp).Aspnetuserid;
                    break;
                case "Provider":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    break;
            }
            List<Chat> data = _context.Chats.Where(e => (e.SenderId == senderId || e.ReceiverId == senderId) && (e.SenderId == receiverId || e.ReceiverId == receiverId)).ToList();
            //var data =_context.Chats.Select(e=> e.Message).ToList();
            Clients.All.SendAsync("ReceiveMessage", data);
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public void SaveData(string Sender, string SenderType, string Receiver, string ReceiverType, string message)
        {
            int senderId = 0, receiverId = 0;
            switch (SenderType)
            {
                case "Admin":
                    senderId = _context.Admins.FirstOrDefault(e => e.Adminid == int.Parse(Sender)).Aspnetuserid;
                    break;
                case "Patient":
                    int? temp = _context.Requests.FirstOrDefault(e => e.Requestid == int.Parse(Sender)).Userid;
                    senderId = (int)_context.Users.FirstOrDefault(e => e.Userid == (int)temp).Aspnetuserid;
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
                    int? temp = _context.Requests.FirstOrDefault(e => e.Requestid == int.Parse(Receiver)).Userid;
                    receiverId = (int)_context.Users.FirstOrDefault(e => e.Userid == (int)temp).Aspnetuserid;
                    break;
                case "Provider":
                    receiverId = (int)_context.Physicians.FirstOrDefault(e => e.Physicianid == int.Parse(Receiver)).Aspnetuserid;
                    break;
            }

            Chat chat = new Chat
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                SentDate = DateTime.Now,
                SentTime = TimeOnly.FromDateTime(DateTime.Now)
            };
            _context.Chats.Add(chat);
            _context.SaveChanges();
        }

    }
}
