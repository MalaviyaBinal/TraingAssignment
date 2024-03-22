using HalloDocWebEntity.Data;
using HalloDocWebRepo.Implementation;
using HalloDocWebRepo.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebService.Utils
{
    public class SessionUtils
    {
        private readonly IAdmin_Repository _repo ;
        public SessionUtils(IAdmin_Repository repo)
        {
            _repo = repo; 
        }

        public static void SetLoggedUsers(ISession session, Aspnetuser u)
        {
            session.SetString("userName", u.Usarname);
            session.SetString("userEmail", u.Email);
            session.SetString("Isheader", "unset");

        }

        public static Aspnetuser GetLoggedUsers(ISession session)
        {
            var obj = new Admin_Repository();
            Aspnetuser user = obj.getAspnetuserByEmail(session.GetString("userName"));
            return user;
        }
    }
}
