using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebServices.Interfaces
{
    public interface IPatient_Service
    {
        public void editProfile(PatientRequest model, string username);
        public List<Requestwisefile> getPatientDocument(int? id);
        public int getReqWiseFile(int requestid);
        public Dictionary<int, int> GetCount(string email);
        public Profile ReturnRequest(string? username);
        public void uploadFile(IFormFile fileToUpload, int id);
        public bool ValidateUser(string usarname, string passwordhash);
        public User getUserByEmail(string? email);
        public Aspnetuser getAspnetuserByEmail(string usarname);
        public void saveDataRequestForMe(RequestForMe info, string email);
        public void saveDataForSomeone(RequestForMe info, string? v);
        public void createPatientByBusiness(BusinessPatientRequest info);
        public void createPatientByConcierge(ConciergePatientRequest info);
        public void createPatientByFamilyFrd(FamilyFrdPatientRequest info);
        public void createPatient(PatientRequest info);
        public Requestwisefile getRequestWiseFile(int id);
        public byte[] getBytesForFileDownload(int id);
        public MemoryStream downloadAlll(string[] filenames);
        public void sendMail(ForgotPwdModel info);
        public Boolean getAspnetUserAny(string email);
        void sendMailForCreateAccount(string email);
        loginModel createAccountService(string token);
        void createAccountSaveData(loginModel info);
    }
}
