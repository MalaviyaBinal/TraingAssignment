

using HalloDocWebEntity.Data;
using HalloDocWebRepo.Interface;
using Microsoft.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HalloDocWebRepo.Implementation
{
    public class Patient_Repository : IPatient_Repository
    {
        private readonly ApplicationContext _context;

        public Patient_Repository(ApplicationContext context)
        {
            _context = context;
        }

        public void addAspnetuserTable(Aspnetuser aspuser)
        {
            _context.Aspnetusers.Add(aspuser);
            _context.SaveChanges();
        }

        public void addBussinessTable(Business business)
        {
            _context.Businesses.Add(business);
            _context.SaveChanges();

        }

        public void addConciergeTable(Concierge con)
        {
            _context.Concierges.Add(con);
            _context.SaveChanges();
        }

        public void addRequestBusinessTable(Requestbusiness reqbusiness)
        {
            _context.Requestbusinesses.Add(reqbusiness);
            _context.SaveChanges();
        }

        public void addRequestClientTable(Requestclient reqclient)
        {
            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();
        }

        public void addRequestConciergeTable(Requestconcierge reqCon)
        {
            _context.Requestconcierges.Add(reqCon);
            _context.SaveChanges();
        }

        public void addRequestTable(Request req)
        {
            _context.Requests.Add(req);
            _context.SaveChanges();
        }

        public void addTokenRegister(TokenRegister tokenRegister)
        {
            _context.TokenRegisters.Add(tokenRegister);
            _context.SaveChanges();
        }

        public void addUserTable(User user)
        {
            _context.Users.Add(user);

            _context.SaveChanges();
        }

        public Aspnetuser getAspnetUser(int aspnetuserid)
        {
            return _context.Aspnetusers.FirstOrDefault(u => u.Id == aspnetuserid);
        }

        public bool getAspnetUserAny(string email)
        {
            return _context.Aspnetusers.Any(u => u.Email == email);
        }

        public Aspnetuser getAspnetUserByEmail(string usarname)
        {
            return _context.Aspnetusers.FirstOrDefault(m => m.Email == usarname);
        }

        public Requestclient getRequestClientByEmail(string? email)
        {
            return _context.Requestclients.FirstOrDefault(m => m.Email == email);
        }

        public Requestwisefile getRequestWiseFile(int id)
        {
            return _context.Requestwisefiles.Find(id);

        }

        public List<Requestwisefile> getRequestWiseFileTolist(int id)
        {
            return _context.Requestwisefiles.Where(u => u.Requestid == id).ToList();
        }

        public TokenRegister getTokenRegisterByToken(string token)
        {
            return _context.TokenRegisters.FirstOrDefault(m => m.TokenValue == token);
        }

        public User getUserByEmail(string? email)
        {
            return _context.Users.FirstOrDefault(m => m.Email == email);
        }

        public void updateAspnetuserTable(Aspnetuser asp)
        {
            _context.Aspnetusers.Update(asp);
        }

        public void updateRequestTable(Request req)
        {
            _context.Requests.Update(req);
            _context.SaveChanges();
        }

        public bool ValidateUser(string usarname, string passwordhash)
        {
            Aspnetuser user = _context.Aspnetusers.FirstOrDefault(u => u.Email == usarname && u.Passwordhash == passwordhash);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void IPatient_Repository.addRequestFileTable(Requestwisefile reqclient)
        {
            _context.Requestwisefiles.Add(reqclient);
        }

        Request IPatient_Repository.getFirstRequestTable(int id)
        {
            return _context.Requests.FirstOrDefault(m => m.Requestid == id);
        }

        List<Requestwisefile> IPatient_Repository.getPatientDocument(int? id)
        {
            return _context.Requestwisefiles.Where(m => m.Requestid == id && m.Isdeleted == null).ToList();

        }

        int IPatient_Repository.getReqWiseFile(int requestid)
        {
            return _context.Requestwisefiles.Count(r => r.Requestid == requestid && r.Isdeleted == null);
        }

        User IPatient_Repository.ProfileUser(string? username)
        {
            return _context.Users.FirstOrDefault(m => m.Email == username);
        }

        List<Request> IPatient_Repository.RequestRepo(int? id)
        {
            return _context.Requests.Where(m => m.Userid == id && m.Status != 10).ToList();
        }

        void IPatient_Repository.saveDbChanges()
        {
            _context.SaveChanges();
        }

        void IPatient_Repository.updateUserTable(User user)
        {
            _context.Users.Update(user);
        }
    }
}
