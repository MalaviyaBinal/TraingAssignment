
using HalloDocWebEntity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebRepo.Interface
{
    public interface IPatient_Repository
    {
        public void addAspnetuserTable(Aspnetuser aspuser);
        public void addBussinessTable(Business business);
        public void addConciergeTable(Concierge con);
        void addEmailLogTable(Emaillog emaillog);
        public void addRequestBusinessTable(Requestbusiness reqbusiness);
        public void addRequestClientTable(Requestclient reqclient);
        public void addRequestConciergeTable(Requestconcierge reqCon);
        public void addRequestFileTable(Requestwisefile reqclient);
        public void addRequestTable(Request req);
        void addTokenRegister(TokenRegister tokenRegister);
        public void addUserTable(User user);
        public Aspnetuser getAspnetUser(int aspnetuserid);
        public bool getAspnetUserAny(string email);
        public Aspnetuser getAspnetUserByEmail(string usarname);
        public Request getFirstRequestTable(int id);
        public List<Requestwisefile> getPatientDocument(int? id);
        Requestclient getRequestClientByEmail(string? email);
        public Requestwisefile getRequestWiseFile(int id);
        public List<Requestwisefile> getRequestWiseFileTolist(int id);
        public int getReqWiseFile(int requestid);
        TokenRegister getTokenRegisterByToken(string token);
        public User getUserByEmail(string? email);
        public User ProfileUser(string? username);
        public List<Request> RequestRepo(int? id);
        public void saveDbChanges();
        public void updateAspnetuserTable(Aspnetuser asp);
        void updateRequestTable(Request req);
        public void updateUserTable(User user);
        public bool ValidateUser(string usarname, string passwordhash);

    }
}
