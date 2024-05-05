using HalloDocWebRepo.Interface;
using HalloDocWebServices.Interfaces;
using HalloDocWebEntity.ViewModel;
using Microsoft.AspNetCore.Http;
using HalloDocWebEntity.Data;
using System.Collections;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
namespace HalloDocWebServices.Implementation
{
    public class Patient_Service : IPatient_Service
    {
        private readonly IPatient_Repository _repository;
        public Patient_Service(IPatient_Repository repository)
        {
            _repository = repository;
        }
        void IPatient_Service.editProfile(Profile model, string username)
        {
            var user = _repository.ProfileUser(username);
            var asp = _repository.getAspnetUser(user.Aspnetuserid);
            user.Firstname = model.first_name;
            user.Lastname = model.last_name;
            user.Mobile = model.phone;
            user.Street = model.street;
            user.City = model.city;
            user.State = model.state;
            user.Zip = model.zipcode;
            user.Intdate = model.dob.Value.Day;
            user.Intyear = model.dob.Value.Year;
            user.Strmonth = System.String.Format("{0:MMM}", model.dob);
            _repository.updateUserTable(user);
            asp.Usarname = model.first_name;
            asp.Phonenumber = model.phone;
            _repository.updateAspnetuserTable(asp);
        }
        List<Requestwisefile> IPatient_Service.getPatientDocument(int? id)
        {
            return _repository.getPatientDocument(id);
        }
        int IPatient_Service.getReqWiseFile(int requestid)
        {
            return _repository.getReqWiseFile(requestid);
        }
        Dictionary<int, int> IPatient_Service.GetCount(string email)
        {
            var user = _repository.getUserByEmail(email);
            var users = _repository.RequestRepo(user.Userid);
            Dictionary<int, int> requestIdCounts = new Dictionary<int, int>();
            foreach (var request in users)
            {
                int count = _repository.getReqWiseFile(request.Requestid);
                requestIdCounts.Add(request.Requestid, count);
            }
            return requestIdCounts;
        }
        Profile IPatient_Service.ReturnRequest(string? username)
        {
            var user = _repository.getUserByEmail(username);
            List<Request> userData = _repository.RequestRepo(user.Userid);
            var profileUserData = _repository.ProfileUser(username);
            Profile profile = new Profile
            {
                Request = userData,
                dob = DateOnly.Parse(DateTime.Parse(profileUserData.Intyear + profileUserData.Strmonth + profileUserData.Intdate).ToString("yyyy-MM-dd")),
                first_name = user.Firstname,
                last_name = user.Lastname,
                phone = user.Mobile,
                email = user.Email,
                state = user.State,
                street = user.Street,
                zipcode = user.Zip,
                city = user.City,
            };
            
            return (profile);
        }
        void IPatient_Service.uploadFile(IFormFile fileToUpload, int id)
        {
            var FileNameOnServer = "D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\UploadedFiles\\";
            FileNameOnServer += fileToUpload.FileName;
            using var stream = System.IO.File.Create(FileNameOnServer);
            fileToUpload.CopyTo(stream);
            var userobj = _repository.getFirstRequestTable(id);
            //var userobj = _context.Requests.FirstOrDefaultAsync(m => m.Requestid == id);
            Requestwisefile reqclient = new Requestwisefile
            {
                Requestid = id,
                Filename = fileToUpload.FileName,
                Createddate = DateTime.Now,
            };
            _repository.addRequestFileTable(reqclient);
        }
        public RequestForMe getUserByEmail(string? email)
        {
            var user = _repository.getUserByEmail(email);
            RequestForMe model = new RequestForMe
            {
                first_name = user.Firstname,
                last_name = user.Lastname,
                dob = DateOnly.Parse(DateTime.Parse(user.Intyear + user.Strmonth + user.Intdate).ToString("yyyy-MM-dd")),
            email = user.Email,
                phonenumber = user.Mobile
            };
            return model;
        }
        Aspnetuser IPatient_Service.getAspnetuserByEmail(string usarname)
        {
            return _repository.getAspnetUserByEmail(usarname);
        }
        void IPatient_Service.saveDataForSomeone(RequestForMe info, string email)
        {
            var userid = _repository.getUserByEmail(email);
            Request req = new Request
            {
                Requesttypeid = 2,
                Userid = userid.Userid,
                Firstname = userid.Firstname,
                Lastname = userid.Lastname,
                Phonenumber = userid.Mobile,
                Email = userid.Email,
                Status = 1,
                Relationname = info.relation,
                Createddate = DateTime.Now,
                Isurgentemailsent = new BitArray(1, false)
            };
            _repository.addRequestTable(req);
            Requestclient reqclient = new Requestclient
            {
                Requestid = req.Requestid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phonenumber,
                Email = info.email,
                Location = info.street + "," + info.city + "," + info.state + " ," + info.zipcode,
                Regionid = 1
            };
            _repository.addRequestClientTable(reqclient);
            var file = info.fileToUpload;
            var uniqueFileName = Path.GetFileName(file.FileName);
            var uploads = @"D:\Projects\HelloDOC\MVC\N-tier\HalloDoc.Web\wwwroot\UploadedFiles\";
            var filePath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));
            var addrequestfile = new Requestwisefile
            {
                Createddate = DateTime.Now,
                Filename = uniqueFileName,
                Requestid = req.Requestid
            };
            _repository.addRequestFileTable(addrequestfile);
        }
        public void saveDataRequestForMe(RequestForMe info, string? email)
        {
            var user = _repository.getUserByEmail(email);
            Request req = new Request
            {
                Requesttypeid = 2,
                Userid = user.Userid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phonenumber,
                Email = info.email,
                Status = 1,
                Createddate = DateTime.Now,
                Isurgentemailsent = new BitArray(1, false)
            };
            _repository.addRequestTable(req);
            Requestclient reqclient = new Requestclient
            {
                Requestid = req.Requestid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phonenumber,
                Email = info.email,
                Location = info.street + "," + info.city + "," + info.state + " ," + info.zipcode,
                Regionid = 1
            };
            _repository.addRequestClientTable(reqclient);
            var file = info.fileToUpload;
            var uniqueFileName = Path.GetFileName(file.FileName);
            var uploads = @"D:\Projects\HelloDOC\MVC\N-tier\HalloDoc.Web\wwwroot\UploadedFiles\";
            var filePath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));
            var addrequestfile = new Requestwisefile
            {
                Createddate = DateTime.Now,
                Filename = uniqueFileName,
                Requestid = req.Requestid
            };
            _repository.addRequestFileTable(addrequestfile);
        }
        void IPatient_Service.createPatientByBusiness(BusinessPatientRequest info)
        {
            var user = _repository.getUserByEmail(info.p_email);
            var reqid = 0;
            if (user == null)
            {
                Request req1 = new Request
                {
                    Requesttypeid = 2,
                    Firstname = info.first_name,
                    Lastname = info.last_name,
                    Phonenumber = info.phone,
                    Email = info.email,
                    Status = 1,
                    Createddate = info.Createddate,
                    Isurgentemailsent = new BitArray(1, false)
                };
                _repository.addRequestTable(req1);
                reqid = req1.Requestid;
            }
            else
            {
                Request req = new Request
                {
                    Requesttypeid = 2,
                    Userid = user.Userid,
                    Firstname = info.first_name,
                    Lastname = info.last_name,
                    Phonenumber = info.phone,
                    Email = info.email,
                    Status = 1,
                    Createddate = info.Createddate,
                    Isurgentemailsent = new BitArray(1, false)
                };
                _repository.addRequestTable(req);
                reqid = req.Requestid;
            }
            Requestclient reqclient = new Requestclient
            {
                Requestid = reqid,
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Phonenumber = info.phone,
                Location = info.p_street + "," + info.p_city + "," + info.p_state + " ," + info.p_zip_code,
                Street = info.p_street, City = info.p_city,
                State = info.p_state,
                Zipcode = info.p_zip_code,
                Email = info.p_email,
                Regionid = 1,
                Intdate = info.dob.Value.Day,
                Intyear = info.dob.Value.Year,
                Strmonth = ((DateTime)info.dob).ToString("MMM")
            };
            _repository.addRequestClientTable(reqclient);
            Business business = new Business
            {
                Name = info.business_name,
                Createdby = info.first_name,
                Createddate = DateTime.Now
            };
            _repository.addBussinessTable(business);
            Requestbusiness reqbusiness = new Requestbusiness
            {
                Requestid = reqid,
                Businessid = business.Businessid
            };
            _repository.addRequestBusinessTable(reqbusiness);
        }
        public void createPatientByConcierge(ConciergePatientRequest info)
        {
            var user = _repository.getUserByEmail(info.p_email);
            var reqid = 0;
            if (user == null)
            {
                Request req1 = new Request
                {
                    Requesttypeid = 2,
                    Firstname = info.first_name,
                    Lastname = info.last_name,
                    Phonenumber = info.phone,
                    Email = info.email,
                    Status = 1,
                    Createddate = info.Createddate,
                    Isurgentemailsent = new BitArray(1, false)
                };
                _repository.addRequestTable(req1);
                reqid = req1.Requestid;
            }
            else
            {
                Request req = new Request
                {
                    Requesttypeid = 2,
                    Userid = user.Userid,
                    Firstname = info.first_name,
                    Lastname = info.last_name,
                    Phonenumber = info.phone,
                    Email = info.email,
                    Status = 1,
                    Createddate = info.Createddate,
                    Isurgentemailsent = new BitArray(1, false)
                };
                _repository.addRequestTable(req);
                reqid = req.Requestid;
            }
            Requestclient reqclient = new Requestclient
            {
                Requestid = reqid,
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Phonenumber = info.p_phone,
                Location = info.h_street + "," + info.h_city + "," + info.h_state + " ," + info.h_zip_code,
                Street = info.h_street,
                City = info.h_city,
                State = info.h_state,
                Zipcode = info.h_zip_code,
              
                Email = info.p_email,
                Regionid = 1,
                Intdate = info.p_dob.Value.Day,
                Intyear = info.p_dob.Value.Year,
                Strmonth = ((DateTime)info.p_dob).ToString("MMM")
            };
            _repository.addRequestClientTable(reqclient);
            Concierge con = new Concierge
            {
                Conciergename = info.hotel_name,
                Street = info.h_street,
                City = info.h_city,
                Zipcode = info.h_zip_code,
                State = info.h_state,
                Regionid = 1,
            };
            _repository.addConciergeTable(con);
            Requestconcierge reqCon = new Requestconcierge
            {
                Requestid = reqid,
                Conciergeid = con.Conciergeid,
            };
            _repository.addRequestConciergeTable(reqCon);
        }
        public void createPatientByFamilyFrd(FamilyFrdPatientRequest info)
        {
            var user = _repository.getUserByEmail(info.p_email);
            var reqid = 0;
            if (user == null)
            {
                Request req1 = new Request
                {
                    Requesttypeid = 2,
                    Firstname = info.first_name,
                    Lastname = info.last_name,
                    Phonenumber = info.phone,
                    Email = info.email,
                    Status = 1,
                    Createddate = info.Createddate,
                    Relationname = info.relation_with,
                    Isurgentemailsent = new BitArray(1, false)
                };
                _repository.addRequestTable(req1);
                reqid = req1.Requestid;
            }
            else
            {
                Request req = new Request
                {
                    Requesttypeid = 2,
                    Userid = user.Userid,
                    Firstname = info.first_name,
                    Lastname = info.last_name,
                    Phonenumber = info.phone,
                    Email = info.email,
                    Status = 1,
                    Createddate = info.Createddate,
                    Relationname = info.relation_with,
                    Isurgentemailsent = new BitArray(1, false)
                };
                _repository.addRequestTable(req);
            }
            if(info.fileToUpload != null)
            {
                var file = info.fileToUpload;
                var uniqueFileName = Path.GetFileName(file.FileName);
                var uploads = @"D:\Projects\HelloDOC\MVC\N-tier\HalloDoc.Web\wwwroot\UploadedFiles\";
                var filePath = Path.Combine(uploads, uniqueFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                var addrequestfile = new Requestwisefile
                {
                    Createddate = DateTime.Now,
                    Filename = uniqueFileName,
                    Requestid = reqid
                };
                _repository.addRequestFileTable(addrequestfile);
            }
           
            Requestclient reqclient = new Requestclient
            {
                Requestid = reqid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phone,
                Street = info.p_street,
                City = info.p_city,
                State = info.p_state,
                Zipcode = info.p_zip_code,
                Email = info.email,
                Regionid = 1,
                Notes = info.symptoms,
                Intdate = info.p_dob.Value.Day,
                Intyear = info.p_dob.Value.Year,
                Strmonth = ((DateTime)info.p_dob).ToString("MMM"),
            };
            _repository.addRequestClientTable(reqclient);
        }
        public void createPatient(PatientRequest info)
        {
            Aspnetuser aspuser = new Aspnetuser
            {
                Usarname = info.first_name,
                Passwordhash = Crypto.HashPassword(info.password) ,
                Email = info.email_user,
                Phonenumber = info.phone,
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now
            };
            _repository.addAspnetuserTable(aspuser);
            User user = new User
            {
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email_user,
                Mobile = info.phone,
                Street = info.street,
                City = info.city,
                State = info.state,
                Aspnetuserid = aspuser.Id,
                Createdby = info.first_name,
                Createddate = info.Createddate,
            };
            Request req1 = new Request
            {
                Requesttypeid = 2,
                Userid = user.Userid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phone,
                Email = info.email_user,
                Status = 1,
                Createddate = info.Createddate,
                Isurgentemailsent = new BitArray(1, false)
            };
            _repository.addRequestTable(req1);
            Requestclient reqclient = new Requestclient
            {
                Requestid = req1.Requestid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phone,
                Street = info.street,
                City = info.city,
                State = info.state,
                Zipcode = info.zip_code,
                Email = info.email_user,
                Regionid = 1,
                Intdate = info.dob.Value.Day,
                Intyear = info.dob.Value.Year,
                Strmonth = ((DateOnly)info.dob).ToString("MMM"),
                Notes = info.symptoms
            };
            _repository.addRequestClientTable(reqclient);
            var file = info.fileToUpload;
            var uniqueFileName = Path.GetFileName(file.FileName);
            var uploads = @"D:\Projects\HelloDOC\MVC\N-tier\HalloDoc.Web\wwwroot\UploadedFiles\";
            var filePath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));
            var addrequestfile = new Requestwisefile
            {
                Createddate = DateTime.Now,
                Filename = uniqueFileName,
                Requestid = req1.Requestid
            };
            _repository.addRequestFileTable(addrequestfile);
        }
        public Requestwisefile getRequestWiseFile(int id)
        {
            return _repository.getRequestWiseFile(id);
        }
        public byte[] getBytesForFileDownload(int id)
        {
            var file = _repository.getRequestWiseFile(id);
            var filepath = "D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\UploadedFiles\\" + Path.GetFileName(file.Filename);
            var bytes = System.IO.File.ReadAllBytes(filepath);
            return bytes;
        }
        public MemoryStream downloadAlll(string[] filenames)
        {
            string repositoryPath = @"D:\Projects\HelloDOC\MVC\N-tier\HalloDoc.Web\wwwroot\UploadedFiles\";
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (string filename in filenames)
                    {
                        string filePath = Path.Combine(repositoryPath, filename);
                        if (System.IO.File.Exists(filePath))
                        {
                            zipArchive.CreateEntryFromFile(filePath, filename);
                        }
                    }
                }
                zipStream.Seek(0, SeekOrigin.Begin);
                return zipStream;
            }
        }
        public void sendMail(ForgotPwdModel info)
        {
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = info.Email;
            var subject = "Reset Password";
            var message = "Reset Your Password: https://localhost:7050/Home";
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
        }
        public bool getAspnetUserAny(string email)
        {
            return _repository.getAspnetUserAny(email);
        }
        public void sendMailForCreateAccount(string email)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var token = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Create Your Account";
            var message = "Hello "+ email+ ",Click Here to Create Account:https://localhost:44380/Home/CreateAccount?token=" + token;
            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);
            mailclient.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = subject,
                Emailid = email,
                Roleid = 2,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Action = 1
            };
            _repository.addEmailLogTable(emaillog);
            TokenRegister tokenRegister = new TokenRegister();
            tokenRegister.Email = email;
            tokenRegister.TokenValue = token;
            tokenRegister.CreatedDate = DateTime.Now;
            _repository.addTokenRegister(tokenRegister);
        }
        public loginModel createAccountService(string token)
        {
            loginModel model = new loginModel();
            TokenRegister tokenRegister = _repository.getTokenRegisterByToken(token);
            if (tokenRegister != null)
            {
                Requestclient client = _repository.getRequestClientByEmail(tokenRegister.Email);
                if (client != null)
                {
                    model.Usarname = tokenRegister.Email;
                }
                else
                {
                    model.Usarname = null;
                }
            }
            return model;
        }
        public void createAccountSaveData(loginModel info)
        {
            if (info != null)
            {
                Requestclient client = _repository.getRequestClientByEmail(info.Usarname);
                if (client != null)
                {
                    Aspnetuser aspnetuser = new Aspnetuser
                    {
                        Usarname = info.Usarname,
                        Passwordhash = Crypto.HashPassword(info.Passwordhash),
                        Phonenumber = client.Phonenumber,
                        Createddate = DateTime.Now,
                        Email = info.Usarname,
                        Role = 2
                    };
                    _repository.addAspnetuserTable(aspnetuser);
                    User user = new User
                    {
                        Aspnetuserid = aspnetuser.Id,
                        Firstname = client.Firstname,
                        Lastname = client.Lastname,
                        Email = info.Usarname,
                        Mobile = client.Phonenumber,
                        Street = client.Street,
                        State = client.State,
                        Zip = client.Zipcode,
                        City = client.City,
                        Createddate = DateTime.Now,
                        Intdate = client.Intdate,
                        Intyear = client.Intyear,
                        Strmonth = client.Strmonth
                    };
                    _repository.addUserTable(user);
                    Request req = _repository.getFirstRequestTable(client.Requestid);
                    req.Userid = user.Userid;
                    _repository.updateRequestTable(req);
                }
            }
        }
        public bool checkUserExists(Aspnetuser user)
        {
            return _repository.checkUserExists(user.Email);
        }
        public void SendResetPwdPage(Aspnetuser user)
        {
            Random random = new Random();
            user = _repository.getAspnetUserByEmail(user.Email);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var token = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = "binalmalaviya2002@gmail.com";  //user.Email;
            var subject = "Reset Password";
            var message = "Hello" + user.Usarname + " ,Click Here to reset your password:https://localhost:44380/Home/ResetPassword?token=" + token + "\nLink will be active for 20 minutes only!!!";
            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);
            mailclient.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = subject,
                Emailid = user.Email,
                Roleid = 2,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Action = 1
            };
            _repository.addEmailLogTable(emaillog);
            TokenRegister tokenRegister = new TokenRegister();
            tokenRegister.Email = user.Email;
            tokenRegister.TokenValue = token;
            tokenRegister.Requestid = user.Id;
            tokenRegister.CreatedDate = DateTime.Now;
            _repository.addTokenRegister(tokenRegister);
        }
        public bool checkTokenExists(string token)
        {
            return _repository.checkTokenExists(token);
        }
        public ResetPWDModel getAspnetUserByToken(string token)
        {
            TokenRegister tokenreg = _repository.getTokenRegisterByToken(token);
            Aspnetuser user = _repository.getAspnetUser((int)tokenreg.Requestid);
            ResetPWDModel model = new ResetPWDModel
            {
                Usarname = user.Email,
            };
            return model;
        }
        public void changePassword(ResetPWDModel user)
        {
            Aspnetuser netuser = _repository.getAspnetUserByEmail(user.Usarname);
            if (netuser != null)
            {
                netuser.Passwordhash = Crypto.HashPassword(user.Passwordhash) ;
            }
            _repository.updateAspnetuserTable(netuser);
        }
    }
}
