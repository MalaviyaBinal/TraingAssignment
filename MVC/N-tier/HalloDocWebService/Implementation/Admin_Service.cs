using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebRepo.Interface;
using HalloDocWebService.Authentication;
using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Data;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;

namespace HalloDocWebServices.Implementation
{
    public class Admin_Service : IAdmin_Service
    {
        private readonly IAdmin_Repository _repository;
        public Admin_Service(IAdmin_Repository repository)
        {
            _repository = repository;
        }
        public AssignCaseModel AssignCaseModel(int regid)
        {
            AssignCaseModel model = new();
            if (regid == 0)
            {
                var phy = _repository.getPhysicianList();
                model.physicians = phy;
            }
            else
            {
                var phy = _repository.getPhysicianListByregion(regid);
                model.physicians = phy;
            }
            var reg = _repository.getRegions();
            model.regions = reg;
            return model;
        }
        public void blockConfirm(int id, string notes)
        {
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = notes,
                Status = 10,
                Createddate = DateTime.Now,
            };
            _repository.addRequestStatusLogTable(statuslog);
            Request request = _repository.getRequestByID(id);

            Blockrequest blockrequest = new Blockrequest
            {
                Phonenumber = request.Phonenumber,
                Email = request.Email,
                Requestid = request.Requestid,
                Reason = notes,
                Createddate = DateTime.Now
            };
            _repository.addBlockRequestTable(blockrequest);
            request.Status = 10;
            _repository.updateRequest(request);
        }
        public void cancelConfirm(int id, int reasonid, string notes)
        {
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = notes,
                Status = 3,
                Createddate = DateTime.Now,
            };
            _repository.addRequestStatusLogTable(statuslog);
            Request request = _repository.getRequestByID(id);
            Casetag reason = _repository.getCasetag(reasonid);
            request.Status = 5;
            request.Casetag = reason.Name;
            _repository.updateRequest(request);
        }
        public void clearConfirm(int id)
        {
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = "case cleared",
                Status = 10,
                Createddate = DateTime.Now,
            };
            _repository.addRequestStatusLogTable(statuslog);
            Request request = _repository.getRequestByID(id);
            request.Status = 10;
            _repository.updateRequest(request);
        }
        public void deleteAllFile(int id, string[] filenames)
        {
            List<Requestwisefile> files = new();
            foreach (var filename in filenames)
            {
                files.Add(_repository.getRequestWiseFileByName(filename, id));
            }
            foreach (var file in files)
            {
                file.Isdeleted = new BitArray(1, true);
                _repository.updateRequestWiseFile(file);
            }
        }
        public void deleteFile(int id)
        {
            var file = _repository.getRequestWiseFile(id);
            file.Isdeleted = new BitArray(1, true);
            _repository.updateRequestWiseFile(file);
        }
        public MemoryStream downloadFile(string[] filenames)
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
        public Encounterformmodel EncounterAdmin(int id)
        {
            var patientData = _repository.getRequestClientById(id);
            Encounterformmodel model = new();
            var info = _repository.getEncounterTable(id);
            model.patientData = patientData;
            model.confirmationDetail = _repository.getRequestByID(id);
            model.Requestid = info.Requestid;
            model.Abd = info.Abd;
            model.Skin = info.Skin;
            model.Hr = info.Hr;
            model.O2 = info.O2;
            model.Rr = info.Rr;
            model.Cv = info.Cv;
            model.BpS = info.BpS;
            model.BpD = info.BpD;
            model.Temp = info.Temp;
            model.Allergies = info.Allergies;
            model.Chest = info.Chest;
            model.Date = info.Date;
            model.Diagnosis = info.Diagnosis;
            model.Extr = info.Extr;
            model.Heent = info.Heent;
            model.FollowUp = info.FollowUp;
            model.HistoryIllness = info.HistoryIllness;
            model.MedicalHistory = info.MedicalHistory;
            model.Medications = info.Medications;
            model.Procedures = info.Procedures;
            model.MedicationDispensed = info.MedicationDispensed;
            model.Neuro = info.Neuro;
            model.Pain = info.Pain;
            model.Other = info.Other;
            model.TreatmentPlan = info.TreatmentPlan;
            return model;
        }
        public AdminProfile getAdminProfileData(string? email)
        {
            var admin = _repository.getAdminTableDataByEmail(email);
            var adminRegion = _repository.getAdminRegionByAdminId(admin.Adminid);
            AdminProfile profile = new();
            profile.admin = admin;
            profile.adminuser = _repository.getAspnetuserByEmail(email);
            profile.regions = _repository.getRegions();
            profile.adminregion = adminRegion;
            profile.isAdminProfile = true;
            return profile;
        }
        public AdminProfile getAdminData(int id)
        {
            var admin = _repository.getAdminByAdminID(id);
            var adminRegion = _repository.getAdminRegionByAdminId(admin.Adminid);
            AdminProfile profile = new();
            profile.admin = admin;
            profile.adminuser = _repository.getAspnetuserByID(admin.Aspnetuserid);
            profile.regions = _repository.getRegions();
            profile.adminregion = adminRegion;
            profile.isAdminProfile = false;
            return profile;
        }
        public byte[] getBytesForFile(int id)
        {
            var file = _repository.getRequestWiseFile(id);
            var filepath = "D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\UploadedFiles\\" + Path.GetFileName(file.Filename);
            var bytes = System.IO.File.ReadAllBytes(filepath);
            return bytes;
        }
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check)
        {
            List<AdminDashboardTableModel> tabledata;
            if (check == 0)
            {
                return _repository.getDashboardTablesWithoutcheck(id);
            }
            else
            {
                return _repository.getDashboardTables(id, check);
            }
        }
        public SendOrderModel getOrderModel(int id, int profId, int businessId)
        {
            SendOrderModel model = new();
            if (businessId == 0)
            {
                model.business = _repository.getHealthProfessionalList();
                Healthprofessional healthprofessional = new();
                healthprofessional.Businesscontact = string.Empty;
                healthprofessional.Email = string.Empty;
                healthprofessional.Faxnumber = string.Empty;
                model.businessDetail = healthprofessional;
            }
            else
            {
                model.business = _repository.getHealthProfessional(profId);
                model.businessDetail = _repository.getHealthProfessionalDetail(businessId);
            }
            model.professions = _repository.getHealthProfessionalTypeList();
            model.req_id = id;
            return model;
        }
        public AdminViewUpload getPatientDocument(int? id)
        {
            AdminViewUpload model = new();
            model.FileList = _repository.getPatientDocument(id);
            model.patientData = _repository.getRequestClientById(id);
            model.confirmationDetail = _repository.getRequestByID(id);
            return model;
        }
        public Dictionary<int, string> GetExtension(int id)
        {
            var fileList = _repository.getPatientDocument(id);
            Dictionary<int, string> requestIdCounts = new Dictionary<int, string>();
            foreach (var file in fileList)
            {
                var extension = Path.GetExtension(file.Filename);
                requestIdCounts.Add(file.Requestwisefileid, extension);
            }
            return requestIdCounts;
        }
        public Requestclient getRequestClientByID(int id)
        {
            return _repository.getRequestClientById(id);
        }
        public Requestwisefile getRequestWiseFileByID(int id)
        {
            return _repository.getRequestWiseFile(id);
        }
        public void patientRequestByAdmin(RequestForMe info, string email)
        {
            var user = _repository.getUserByEmail(info.email);
            var createdby = _repository.getUserByEmail(email);
            if (user == null)
            {
                Aspnetuser aspuser = new Aspnetuser
                {
                    Usarname = info.first_name,
                    Email = info.email,
                    Phonenumber = info.phonenumber,
                    Createddate = DateTime.Now,
                    Modifieddate = DateTime.Now
                };
                _repository.addAspnetuserTable(aspuser);
                User users = new User
                {
                    Firstname = info.first_name,
                    Lastname = info.last_name,
                    Email = info.email,
                    Mobile = info.phonenumber,
                    Street = info.street,
                    City = info.city,
                    State = info.state,
                    Aspnetuserid = aspuser.Id,
                    Createdby = createdby.Firstname,
                    Createddate = DateTime.Now,
                };
                user = users;
                _repository.addUserTable(users);
            }
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
            _repository.adRequestClientTable(reqclient);
            Requestnote note = new Requestnote
            {
                Requestid = req.Requestid,
                Createdby = createdby.Firstname,
                Createddate = DateTime.Now,
                Adminnotes = info.admin_notes
            };
            _repository.addRequestNotesTAble(note);
        }
        public void requestAssign(int phyId, string notes, int id, string email)
        {
            var admin = _repository.getAspnetuserByEmail(email);
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = notes,
                Status = 2,
                Createddate = DateTime.Now,
                Physicianid = phyId,
                Adminid = admin.Id
            };
            _repository.addRequestStatusLogTable(statuslog);
            Request request = _repository.getRequestByID(id);
            request.Status = 2;
            _repository.updateRequest(request);
        }
        public void requestTransfer(int phyId, string notes, int id, string email)
        {
            Request request = _repository.getRequestByID(id);
            var admin = _repository.getAspnetuserByEmail(email);
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = notes,
                Status = 2,
                Createddate = DateTime.Now,
                Physicianid = request.Physicianid,
                Transtophysicianid = phyId,
                Adminid = admin.Id
            };
            _repository.addRequestStatusLogTable(statuslog);
            request.Physicianid = phyId;
            _repository.updateRequest(request);
        }
        public void saveNotes(Notes n, int id)
        {
            var user = _repository.getRequestByID(id);
            var reqnotes = _repository.getREquestNotes(id);
            if (reqnotes != null && n.AdminNote != null)
            {
                reqnotes.Adminnotes = n.AdminNote;
                reqnotes.Physiciannotes = reqnotes.Physiciannotes;
                reqnotes.Createdby = user.Firstname;
                reqnotes.Modifiedby = user.Firstname;
                reqnotes.Modifieddate = DateTime.Now;
                _repository.updateRequestNote(reqnotes);
            }
            else if (reqnotes == null)
            {
                Requestnote addreq = new Requestnote
                {
                    Requestid = id,
                    Adminnotes = n.AdminNote,
                    Createdby = user.Email,
                    Createddate = DateTime.Now,
                };
                _repository.addRequestNotesTAble(addreq);
            }
        }
        public void SendAgreementCancleConfirm(int id, string info, string? email)
        {
            Request req = _repository.getRequestByID(id);
            User admin = _repository.getUserByEmail(email);
            req.Status = 7;
            _repository.updateRequest(req);
            Requeststatuslog log = new Requeststatuslog
            {
                Requestid = id,
                Status = 7,
                Physicianid = req.Physicianid,
                Adminid = admin.Userid,
                Notes = info,
                Createddate = DateTime.Now,
            };
            _repository.addRequestStatusLogTable(log);
        }
        public void SendAgreementConfirm(int id)
        {
            Request req = _repository.getRequestByID(id);
            req.Status = 4;
            _repository.updateRequest(req);
            TokenRegister token = _repository.getTokenRegisterById(id);
            token.IsDeleted = new BitArray(1, true);
            token.IsVerified = new BitArray(1, true);
            _repository.updateTokenRegisterTable(token);

        }
        public void sendAgreementMail(int id, string email)
        {
            Requestclient client = _repository.getRequestClientById(id);
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var token = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            TokenRegister tokenRegister = new TokenRegister();
            tokenRegister.Email = client.Email;
            tokenRegister.Requestid = id;
            tokenRegister.TokenValue = token;
            _repository.addTokenRegister(tokenRegister);

            var receiver =client.Email;
            var subject = "Review Agreement";
            var message = "Hello "+ client.Firstname +" "+client.Lastname+",Review Your Agreement:https://localhost:44380/Admin/SendAgreement?token=" + token;

            SendEmail(receiver, message, subject);
            var admin = _repository.getAdminTableDataByEmail(email);
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = "Make Your Appointment",
                Emailid = client.Email,
                Roleid = 1,
                Adminid = admin.Adminid,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Action = 1
            };
            _repository.addEmailLogTable(emaillog);

            SendSMS(client.Phonenumber, message);
            Smslog smslog = new Smslog
            {
                Smstemplate = message,
                Mobilenumber = client.Phonenumber,
                Createdate = DateTime.Now,
                Adminid = admin.Adminid,
                Sentdate = DateTime.Now,
                Senttries = 1
            };
            _repository.addSmsLogTable(smslog);
        }
        public Requestclient sendAgreementService(string token)
        {
            var data = _repository.getRequestClientByToken(token);
            return _repository.getRequestClientById(data.Requestid);
        }
        public void SendEmail(int id, string[] filenames,string email)
        {
            List<Requestwisefile> files = new();
            foreach (var filename in filenames)
            {
                files.Add(_repository.getRequesWiseFileList(id, filename));
            }
            Request request = _repository.getRequestByID(id);
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Documents of Request ";
            var message = "Hello "+request.Firstname +" "+request.Lastname+",Find the Files uploaded for your request in below:";
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);
            foreach (var file in files)
            {
                var filePath = "D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\UploadedFiles\\" + file.Filename;
                if (File.Exists(filePath))
                {
                    byte[] fileContent;
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        fileContent = new byte[fileStream.Length];
                        fileStream.Read(fileContent, 0, (int)fileStream.Length);
                    }
                    var attachment = new Attachment(new MemoryStream(fileContent), file.Filename);
                    mailMessage.Attachments.Add(attachment);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            client.SendMailAsync(mailMessage);
            var admin = _repository.getAdminTableDataByEmail(email);
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = "Make Your Appointment",
                Emailid = request.Email,
                Roleid = 1,
                Adminid = admin.Adminid,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Action = 1
            };
            _repository.addEmailLogTable(emaillog);
        }
        public void sendOrder(SendOrderModel info)
        {
            Orderdetail orderdetail = new Orderdetail
            {
                Vendorid = info.BusinessSelect,
                Requestid = info.req_id,
                Faxnumber = info.FaxNumber,
                Email = info.email,
                Businesscontact = info.businesscontact,
                Prescription = info.prescription,
                Noofrefill = info.noOfRetail,
                Createddate = DateTime.Now
            };
            _repository.addOrderDetailTable(orderdetail);
        }
        public AdminDashboard setAdminDashboardCount()
        {
            var viewmodel = new AdminDashboard
            {
                NewCount = _repository.getcount(1),
                PendingCount = _repository.getcount(2),
                ActiveCount = _repository.getcount(3),
                ConcludeCount = _repository.getcount(4),
                TocloseCount = _repository.getcount(5),
                UnpaidCount = _repository.getcount(6),
            };
            return viewmodel;
        }
        public void uploadFileAdmin(IFormFile fileToUpload, int id, string email)
        {
            var admin = _repository.getAspnetuserByEmail(email);
            string FileNameOnServer = "D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\UploadedFiles\\";
            FileNameOnServer += fileToUpload.FileName;
            using var stream = System.IO.File.Create(FileNameOnServer);
            fileToUpload.CopyTo(stream);
            var userobj = _repository.getRequestByID(id);
            Requestwisefile reqclient = new Requestwisefile
            {
                Requestid = id,
                Filename = fileToUpload.FileName,
                Createddate = DateTime.Now,
                Adminid = admin.Id
            };
            _repository.addRequestWiseFile(reqclient);
        }
        public ViewCaseModel ViewCaseModel(int id)
        {
            var data = _repository.getRequestClientById(id);
            var region = _repository.getRegionByRegionId(data.Regionid);
            ViewCaseModel model = new();
            model.FName = data.Firstname;
            model.LName = data.Lastname;
            model.DOB = DateTime.Now;
            model.Notes = data.Notes;
            model.Phonenumber = data.Phonenumber;
            model.Email = data.Email;
            model.RegionName = region.Name;
            model.requestId = id;
            model.Address = data.Street + " " + data.City + " " + data.State + " " + data.Zipcode;
            return model;
        }
        public Notes ViewNotes(int id)
        {
            var reqnote = _repository.getREquestNotes(id);
            var notelog = _repository.getRequestStatusLog(id);
            List<string>? strings = new List<string>();
            Notes notes = new();
            if (reqnote != null)
            {
                if (reqnote.Adminnotes == null)
                    notes.AdminNote = "---";
                else
                    notes.AdminNote = reqnote.Adminnotes;
                if (reqnote.Physiciannotes == null)
                    notes.PhyNotes = "---";
                else
                    notes.PhyNotes = reqnote.Physiciannotes;
            }
            else
            {
                notes.AdminNote = notes.PhyNotes = "---";
            }
            if (notelog.Count != 0)
            {
                foreach (var item in notelog)
                {
                    if (item.Transtoadmin != null)
                    {
                        var phy = _repository.getPhysicianById(item.Physicianid);
                        var str = phy.Firstname + " has transfered case to admin On " + item.Createddate.ToString() + " : " + item.Notes;
                        strings.Add(str);
                    }
                    else if (item.Transtophysicianid != null)
                    {
                        var phy1 = _repository.getPhysicianById(item.Physicianid);
                        var phy2 = _repository.getPhysicianById(item.Transtophysicianid);
                        var str = "admin transfered case from " + phy1.Firstname + "to " + phy2.Firstname + " on " + item.Createddate.ToString() + ": " + item.Notes;
                        strings.Add(str);
                    }
                    else if (item.Physicianid != null && item.Adminid != null)
                    {
                        var phy = _repository.getPhysicianById(item.Physicianid);
                        var str = "admin transfered case To " + phy.Firstname + " on " + item.Createddate.ToString() + ": " + item.Notes;
                        strings.Add(str);
                    }
                }
                notes.transfreNotes = strings;
            }
            else
            {
                strings.Add("---");
                notes.transfreNotes = strings;
            }
            notes.req_id = id;
            return notes;
        }
        public void saveEncounterForm(Encounterformmodel info)
        {
            var model = _repository.getEncounterTable(info.Requestid);
            model.Requestid = info.Requestid;
            model.Abd = info.Abd;
            model.Skin = info.Skin;
            model.Hr = info.Hr;
            model.O2 = info.O2;
            model.Rr = info.Rr;
            model.Cv = info.Cv;
            model.BpS = info.BpS;
            model.BpD = info.BpD;
            model.Temp = info.Temp;
            model.Allergies = info.Allergies;
            model.Chest = info.Chest;
            model.Date = info.Date;
            model.Diagnosis = info.Diagnosis;
            model.Extr = info.Extr;
            model.Heent = info.Heent;
            model.FollowUp = info.FollowUp;
            model.HistoryIllness = info.HistoryIllness;
            model.MedicalHistory = info.MedicalHistory;
            model.Medications = info.Medications;
            model.Procedures = info.Procedures;
            model.MedicationDispensed = info.MedicationDispensed;
            model.TreatmentPlan = info.TreatmentPlan;
            model.Neuro = info.Neuro;
            model.Pain = info.Pain;
            model.Other = info.Other;
            _repository.updateEncounterForm(model);
        }
        public byte[] GetBytesForExport(int state)
        {
            List<AdminDashboardTableModel> data = _repository.getDashboardTablesWithoutcheck(state);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Requestor";
                worksheet.Cells[1, 3].Value = "Physician";
                worksheet.Cells[1, 4].Value = "Date of Service";
                worksheet.Cells[1, 5].Value = "Requested Date";
                worksheet.Cells[1, 6].Value = "Phone Number";
                worksheet.Cells[1, 7].Value = "Email";
                worksheet.Cells[1, 8].Value = "Address";
                worksheet.Cells[1, 9].Value = "Request Type";
                worksheet.Cells[1, 10].Value = "Requestor Phone Number";
                worksheet.Cells[1, 11].Value = "Status";
                worksheet.Cells[1, 12].Value = "Region Name";
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.Name;
                    worksheet.Cells[row, 2].Value = item.Requestor;
                    worksheet.Cells[row, 3].Value = item.physician;
                    worksheet.Cells[row, 4].Value = ConvertToFormattedDate(item.Dateofservice);
                    worksheet.Cells[row, 5].Value = ConvertToFormattedDate(item.Requesteddate);
                    worksheet.Cells[row, 6].Value = item.Phonenumber;
                    worksheet.Cells[row, 7].Value = item.Email;
                    worksheet.Cells[row, 8].Value = item.Address;
                    worksheet.Cells[row, 9].Value = item.RequestTypeName;
                    worksheet.Cells[row, 10].Value = item.RequestorPhonenumber;
                    worksheet.Cells[row, 11].Value = item.Status;
                    worksheet.Cells[row, 12].Value = item.RegionID;
                    row++;
                }
                return package.GetAsByteArray();
            }
        }
        public static string ConvertToFormattedDate(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("MMM dd, yyyy");
            }
            return string.Empty;
        }
        public byte[] GetBytesForExportAll()
        {
            List<AdminDashboardTableModel> data = _repository.GetExportAllData();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Requestor";
                worksheet.Cells[1, 3].Value = "Physician";
                worksheet.Cells[1, 4].Value = "Date of Service";
                worksheet.Cells[1, 5].Value = "Requested Date";
                worksheet.Cells[1, 6].Value = "Phone Number";
                worksheet.Cells[1, 7].Value = "Email";
                worksheet.Cells[1, 8].Value = "Address";
                worksheet.Cells[1, 9].Value = "Request Type";
                worksheet.Cells[1, 10].Value = "Requestor Phone Number";
                worksheet.Cells[1, 11].Value = "Status";
                worksheet.Cells[1, 12].Value = "Region Name";
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.Name;
                    worksheet.Cells[row, 2].Value = item.Requestor;
                    worksheet.Cells[row, 3].Value = item.physician;
                    worksheet.Cells[row, 4].Value = ConvertToFormattedDate(item.Dateofservice);
                    worksheet.Cells[row, 5].Value = ConvertToFormattedDate(item.Requesteddate);
                    worksheet.Cells[row, 6].Value = item.Phonenumber;
                    worksheet.Cells[row, 7].Value = item.Email;
                    worksheet.Cells[row, 8].Value = item.Address;
                    worksheet.Cells[row, 9].Value = item.RequestTypeId;
                    worksheet.Cells[row, 10].Value = item.RequestorPhonenumber;
                    worksheet.Cells[row, 11].Value = item.Status;
                    worksheet.Cells[row, 12].Value = item.RegionID;
                    row++;
                }
                return package.GetAsByteArray();
            }
        }
        public List<Physician> getPhysicianList()
        {
            return _repository.getPhysicianList();
        }
        public List<Region> getRegionList()
        {
            return _repository.getRegions();
        }
        public AdminViewUpload closeCase(int id)
        {
            var patientData = _repository.getRequestClientById(id);
            AdminViewUpload model = new();
            model.patientData = patientData;
            model.confirmationDetail = _repository.getRequestByID(id);
            model.FileList = _repository.getPatientDocument(id);
            return model;
        }
        public void closeCaseSaveData(int id, AdminViewUpload n)
        {
            var request = _repository.getRequestByID(id);
            var reqclient = _repository.getRequestClientById(id);
            if (n.email != null)
            {
                request.Email = n.email;
                request.Phonenumber = n.phone;
                reqclient.Email = n.email;
                reqclient.Phonenumber = n.phone;
                _repository.updateRewuestClient(reqclient);
            }
            request.Status = 9;
            _repository.updateRequest(request);
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Status = 9,
                Createddate = DateTime.Now,
            };
            _repository.addRequestStatusLogTable(statuslog);
        }
        public void updateadminaddress(AdminProfile info)
        {
            var model = _repository.getAdminByAdminId(info.admin);
            model.Address1 = info.admin.Address1;
            model.Address2 = info.admin.Address2;
            model.City = info.admin.City;
            model.Zip = info.admin.Zip;
            model.Altphone = info.admin.Altphone;
            model.Modifieddate = DateTime.Now;
            _repository.updateAdmin(model);
        }
        public void updateadminform(AdminProfile info)
        {

            var model = _repository.getAdminByAdminId(info.admin);
            var regions1 = _repository.getAdminRegionByAdminId(model.Adminid);
            List<int> adminreg = new();
            foreach (Adminregion region in regions1)
            {
                adminreg.Add(region.Regionid);
            }
            List<int> addd = info.SelectedReg.Except(adminreg).ToList();
            List<int> del = adminreg.Except(info.SelectedReg).ToList();
            foreach (int reg in addd)
            {
                Adminregion ar = new() { Adminid = info.admin.Adminid, Regionid = reg };
                _repository.addAdminReg(ar);
            }
            foreach (int reg in del)
            {
                Adminregion ar = new() { Adminid = info.admin.Adminid, Regionid = reg };
                _repository.RemoveAdminReg(ar);
            }
            model.Firstname = info.admin.Firstname;
            model.Lastname = info.admin.Lastname;
            model.Email = info.admin.Email;
            model.Mobile = info.admin.Mobile;
            model.Modifieddate = DateTime.Now;
            _repository.updateAdmin(model);
        }
        public void sendLinkAdminDashboard(AdminDashBoardPagination info,string email)
        {
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Make Your Appointment";
            var message = "Hello "+info.Fname +" " + info.Lname+",You are invited to visit :https://localhost:44380/";
            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);
            //mailclient.SendMailAsync(mailMessage);
            var admin = _repository.getAdminTableDataByEmail(email);
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = "Make Your Appointment",
                Emailid = info.Email,
                Roleid = 1,
                Adminid = admin.Aspnetuserid,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Action = 1
            };
            _repository.addEmailLogTable(emaillog);

            //sms 
            SendSMS(info.PhoneNumber, message);
            Smslog smslog = new Smslog
            {
                Smstemplate = message,
                Mobilenumber = info.PhoneNumber,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Senttries = 1
            };
            _repository.addSmsLogTable(smslog);
            
        }
        public AdminProviderModel getProviderDataForAdmin(int id)
        {
            AdminProviderModel model = new AdminProviderModel();
            model.regions = _repository.getRegions();
            if (id == 0)
            {
                model.physicians = _repository.getPhysicianList();
            }
            else model.physicians = _repository.getPhysicianListByregion(id);
            return model;
        }
        public void addProviderByAdmin(AdminProviderModel phy)
        {
            Physician model = new();
            model.Firstname = "Dr" + phy.Firstname;
            model.Lastname = phy.Lastname;
            model.Email = phy.Email;
            model.Mobile = phy.Mobile;
            model.Medicallicense = phy.Medicallicense;
            model.Npinumber = phy.Npinumber;
            model.Syncemailaddress = phy.Syncemailaddress;
            model.Address1 = phy.Address1;
            model.Address2 = phy.Address2;
            model.City = phy.City;
            model.Zip = phy.Zip;
            model.Adminnotes = phy.Adminnotes;
            model.Altphone = phy.Altphone;
            model.Businessname = phy.Businessname;
            model.Businesswebsite = phy.Businesswebsite;
            model.Createdby = "Admin";
            model.Createddate = DateTime.Now;
            model.Isagreementdoc = phy.AgreementDoc != null ? new BitArray(1, true) : new BitArray(1, false);
            model.Isbackgrounddoc = phy.backgroundDoc != null ? new BitArray(1, true) : new BitArray(1, false);
            model.Istrainingdoc = phy.TrainingDoc != null ? new BitArray(1, true) : new BitArray(1, false);
            model.Isnondisclosuredoc = phy.NonDisclosure != null ? new BitArray(1, true) : new BitArray(1, false);
            model.Photo = phy.Photo.FileName;
            _repository.addPhysician(model);
            if (phy.TrainingDoc != null)
            {
                string filename = "HIPAA" + Path.GetExtension(phy.TrainingDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + model.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                phy.TrainingDoc?.CopyTo(stream);
            }
            if (phy.AgreementDoc != null)
            {
                string filename = "AgreementDoc" + Path.GetExtension(phy.AgreementDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + model.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                phy.AgreementDoc?.CopyTo(stream);
            }
            if (phy.backgroundDoc != null)
            {
                string filename = "BackgroundDoc" + Path.GetExtension(phy.backgroundDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + model.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                phy.backgroundDoc?.CopyTo(stream);
            }
            if (phy.NonDisclosure != null)
            {
                string filename = "NonDisclosureDoc" + Path.GetExtension(phy.NonDisclosure?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\PhysicianDoc\\" + model.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                phy.NonDisclosure?.CopyTo(stream);
            }
            if (phy.Photo != null)
            {
                string filename = "Profile" + Path.GetExtension(phy.Photo?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + model.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                phy.Photo?.CopyTo(stream);
            }


        }
        public Physician getPhysicianByID(int id)
        {
            return _repository.getPhysicianById(id);
        }
        public void SendSMS(string phonenumber, string message)
        {
            //string accountSid = "ACf3e07eb694877aff1ffd392934bdb764";
            //string authToken = "fe03fccadb7d42d562d8f9879bb50ece";
            //string twilioPhoneNumber = "+12676139096";

            //TwilioClient.Init(accountSid, authToken);

            //try
            //{
            //    var smsMessage = MessageResource.Create(
            //        body: message,
            //        from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
            //        to: new Twilio.Types.PhoneNumber(phonenumber)
            //    );

            //    Console.WriteLine("SMS sent successfully. SID: " + smsMessage.Sid);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("An error occurred while sending the SMS: " + ex.Message);
            //}
            Console.WriteLine(message);

        }
        public void SendEmail(string email, string msg, string sub)
        {
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = email;
            var subject = sub;
            var message = msg;
            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);
            mailclient.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
        }
        public void ContactProviderSendMessage(string email, string phone, string note, int selected)
        {
            switch (selected)
            {
                case 1:
                    SendSMS(phone, note);
                    break;
                case 2:
                    SendEmail(email, note, "Alert!!!");
                    break;
                case 3:
                    SendEmail(email, note, "Alert!!!");
                    SendSMS(phone, note);
                    break;

            }


        }
        public AdminProviderModel getProviderByAdmin(int id, string u)
        {
            Physician phy = _repository.getPhysicianById(id);
            Aspnetuser asp = _repository.getAspnetuserByID(phy.Aspnetuserid);
            AdminProviderModel model = new AdminProviderModel();
            model.regions = _repository.getRegions();
            model.phyregions = _repository.getPhysicianRegionByPhy(id);
            model.physician = phy;
            model.aspnetuser = asp;
            model.Firstname = "Dr" + phy.Firstname;
            model.Lastname = phy.Lastname;
            model.Email = phy.Email;
            model.Mobile = phy.Mobile;
            model.Medicallicense = phy.Medicallicense;
            model.Npinumber = phy.Npinumber;
            model.Syncemailaddress = phy.Syncemailaddress;
            model.Address1 = phy.Address1;
            model.Address2 = phy.Address2;
            model.City = phy.City;
            model.Zip = phy.Zip;
            model.Adminnotes = phy.Adminnotes;
            model.Altphone = phy.Altphone;
            model.Businessname = phy.Businessname;
            model.Businesswebsite = phy.Businesswebsite;
            model.Isagrementdoc = phy.Isagreementdoc;
            model.IsbackgroundDoc = phy.Isbackgrounddoc;
            model.IsLisenceDoc = phy.Islicensedoc;
            model.IsNonDisclosure = phy.Isnondisclosuredoc;
            if (u == null)
                model.isProviderEdit = true;
            else model.isProviderEdit = false;
            model.IsTrainingDoc = phy.Istrainingdoc;
            model.isPhoto = phy.Photo != null ? true : false;
            model.isSignature = phy.Signature != null ? true : false;
            model.PhotoName = phy.Photo != null ? phy.Photo : null;
            model.SignatureName = phy.Signature != null ? phy.Signature : null;

            return model;
        }
        public void savePhysicianPassword(AdminProviderModel info)
        {
            var aspnetuser = _repository.getAspnetuserByID(info.aspnetuser.Id);
            if (info.Passwordhash != null)
            {
                aspnetuser.Passwordhash = info.Passwordhash;
            }


            aspnetuser.Modifieddate = DateTime.Now;
            _repository.updateAspnetUser(aspnetuser);
        }
        public void savePhysicianInfo(AdminProviderModel model)
        {

            _repository.removeAllPhysicianRegion(model.physician.Physicianid);
            _repository.addAllPhysicianRegion(model.SelectedReg, model.physician.Physicianid);
            var physician = _repository.getPhysicianById(model.physician.Physicianid);
            physician.Firstname = model.Firstname;
            physician.Lastname = model.Lastname;
            physician.Email = model.Email;
            physician.Mobile = model.Mobile;
            physician.Medicallicense = model.Medicallicense;
            physician.Npinumber = model.Npinumber;
            physician.Syncemailaddress = model.Syncemailaddress;
            physician.Modifiedby = model.Email;
            physician.Modifieddate = DateTime.Now;
            _repository.updatePhysician(physician);
        }
        public void savePhysicianBillingInfo(AdminProviderModel model)
        {
            var physician = _repository.getPhysicianById(model.physician.Physicianid);
            physician.Address1 = model.Address1;
            physician.Address2 = model.Address2;
            physician.City = model.City;
            //physician.Regionid = Int32.Parse(model.City);
            physician.Zip = model.Zip;
            physician.Altphone = model.Altphone;
            physician.Modifieddate = DateTime.Now;
            _repository.updatePhysician(physician);
        }
        public RoleModel GetMenuData(int check)
        {
            RoleModel model = new();
            model.SelectedRole = check;
            if (check == 0)
            {
                model.menu = _repository.getmenudataof();
            }
            else
            {
                model.menu = _repository.getMenuListWithCheck(check);
            }
            return model;
        }
        public void generateRole(string roleName, string[] selectedRoles, int check, string email)
        {
            var roles = selectedRoles[0].Split(',');
            Role role = new Role();
            role.Name = roleName;
            role.Createddate = DateTime.Now;
            role.Accounttype = (short)check;
            role.Createdby = email;
            role.Isdeleted = new BitArray(1, false);
            _repository.saveRole(role);
            foreach (string item in roles)
            {
                Rolemenu rolemenu = new Rolemenu();
                rolemenu.Roleid = role.Roleid;
                rolemenu.Menuid = Int32.Parse(item);
                _repository.saveRoleMenu(rolemenu);
            }
        }
        public List<Role> getRoleList()
        {
            return _repository.getRoleList();
        }
        public void updateroleof(RoleModel roleModel)
        {
            _repository.removeAllRoleMenu(roleModel.RoleId);
            foreach (var item in roleModel.RoleIds)
            {
                Rolemenu rolemenu = new Rolemenu();
                rolemenu.Roleid = roleModel.RoleId;
                rolemenu.Menuid = item;
                _repository.saveRoleMenu(rolemenu);
            }
        }
        public RoleModel EditRole(int id)
        {
            Role role = _repository.getRoleByID(id);
            RoleModel model = new RoleModel();
            model.rolemenus = _repository.getSelectedRoleMenuByRoleID(id);
            model.menu = _repository.getmenudataof();
            model.RoleName = role.Name;
            model.RoleId = role.Roleid;
            model.SelectedRole = role.Accounttype;
            return model;
        }
        public void CreateAdminAccount(AdminProfile model, string email)
        {
            Aspnetuser aspnetuser = new Aspnetuser();
            aspnetuser.Passwordhash = model.adminuser.Passwordhash;
            aspnetuser.Usarname = model.admin.Lastname + model.admin.Firstname.ToCharArray().First();
            aspnetuser.Createddate = DateTime.Now;
            aspnetuser.Modifieddate = DateTime.Now;
            aspnetuser.Email = model.admin.Email;
            aspnetuser.Phonenumber = model.admin.Mobile;
            _repository.addAspnetuserTable(aspnetuser);
            Admin admin = new Admin();
            admin.Aspnetuserid = aspnetuser.Id;
            admin.Address1 = model.admin.Address1;
            admin.Address2 = model.admin.Address2;
            admin.Createddate = DateTime.Now;
            admin.Altphone = model.admin.Altphone;
            admin.Email = model.admin.Email;
            admin.City = model.admin.City;
            admin.Zip = model.admin.Zip;
            admin.Isdeleted = new BitArray(1, false);
            admin.Firstname = model.admin.Firstname;
            admin.Lastname = model.admin.Lastname;
            admin.Mobile = model.admin.Mobile;
            admin.Regionid = model.regionid;
            admin.Roleid = model.roleid;
            admin.Createdby = email;
            _repository.addAdminTable(admin);
            foreach (var item in model.SelectedReg)
            {
                Adminregion adminregion = new Adminregion();
                adminregion.Adminid = admin.Adminid;
                adminregion.Regionid = item;
                _repository.addAdminReg(adminregion);
            }
        }
        public AdminProfile getAdminRoleData()
        {
            AdminProfile model = new AdminProfile();
            model.roles = _repository.getRolesOfAdmin();
            model.regions = _repository.getRegions();
            return model;
        }

        public UserAccess getUserAccessData(int roleid)
        {
            UserAccess userAccess = new UserAccess();
            switch (roleid)
            {
                case 3:
                    userAccess.Aspnetuser = _repository.getAspnetUserList(roleid);
                    userAccess.physicsian = _repository.getPhysicianList();
                    userAccess.admins = null;
                    break;
                case 1:
                    userAccess.Aspnetuser = _repository.getAspnetUserList(roleid);
                    userAccess.admins = _repository.getAdminList();
                    userAccess.physicsian = null;
                    break;
                default:
                    userAccess.Aspnetuser = _repository.getAspnetUserList(roleid);
                    userAccess.admins = _repository.getAdminList();
                    userAccess.physicsian = _repository.getPhysicianList();
                    break;

            }

            return userAccess;
        }

        public void savePhysicianProfile(AdminProviderModel info)
        {
            var phy = _repository.getPhysicianById(info.physician.Physicianid);
            phy.Businessname = info.Businessname;
            phy.Businesswebsite = info.Businesswebsite;
            if (info.Photo != null)
                phy.Photo = "Profile" + Path.GetExtension(info.Photo?.FileName);
            if (info.Signature != null)
                phy.Signature = "Signature" + Path.GetExtension(info.Signature?.FileName);
            phy.Adminnotes = info.Adminnotes;
            phy.Modifieddate = DateTime.Now;
            _repository.updatePhysician(phy);
            if (phy.Photo != null)
            {
                string filename = "Profile" + Path.GetExtension(info.Photo?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + info.physician.Physicianid + "\\" + filename);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                info.Photo?.CopyTo(stream)
;
            }
            if (phy.Signature != null)
            {
                string filename = "Signature" + Path.GetExtension(info.Signature?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + info.physician.Physicianid + "\\" + filename);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using FileStream stream = new(path, FileMode.Create);
                info.Signature?.CopyTo(stream)
;
            }
        }

        public void savePhysicianDocuments(AdminProviderModel info)
        {
            var valtrue = new BitArray(1, true);
            var valfalse = new BitArray(1, false);
            var phy = _repository.getPhysicianById(info.physician.Physicianid);
            phy.Isagreementdoc = info.AgreementDoc != null ? valtrue : valfalse;
            phy.Isbackgrounddoc = info.backgroundDoc != null ? valtrue : valfalse;
            phy.Istrainingdoc = info.TrainingDoc != null ? valtrue : valfalse;
            phy.Isnondisclosuredoc = info.NonDisclosure != null ? valtrue : valfalse;
            phy.Islicensedoc = info.LisenceDoc != null ? valtrue : valfalse;
            _repository.updatePhysician(phy);


            if (info.TrainingDoc != null/* && info.IsTrainingDoc[0] != false */)
            {
                string filename = "HIPAA" + Path.GetExtension(info.TrainingDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + info.physician.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using FileStream stream = new(path, FileMode.Create);
                info.TrainingDoc?.CopyTo(stream);
            }
            if (info.AgreementDoc != null)
            {
                string filename = "AgreementDoc" + Path.GetExtension(info.AgreementDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + info.physician.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using FileStream stream = new(path, FileMode.Create);
                info.AgreementDoc?.CopyTo(stream);
            }
            if (info.backgroundDoc != null)
            {
                string filename = "BackgroundDoc" + Path.GetExtension(info.backgroundDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + info.physician.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using FileStream stream = new(path, FileMode.Create);
                info.backgroundDoc?.CopyTo(stream);
            }
            if (info.NonDisclosure != null)
            {
                string filename = "NonDisclosureDoc" + Path.GetExtension(info.NonDisclosure?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + info.physician.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using FileStream stream = new(path, FileMode.Create);
                info.NonDisclosure?.CopyTo(stream);
            }
            if (info.LisenceDoc != null)
            {
                string filename = "LisenceDoc" + Path.GetExtension(info.LisenceDoc?.FileName);
                string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + info.physician.Physicianid + "\\" + filename);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using FileStream stream = new(path, FileMode.Create);
                info.LisenceDoc?.CopyTo(stream);
            }


        }

        public List<Physicianlocation> getPhysicianLocation()
        {
            return _repository.getPhysicianLocationList();
        }

        public AdminPartnersModel getVenderDetail(int profession, int pagenumber, string searchstr)
        {

            AdminPartnersModel model = new();
            model.vendorList =_repository.getVenderDetail(profession, searchstr);
            model.professions = _repository.getHealthProfessionalTypeList();
            model.SelectedRegion = profession;
            var count = model.vendorList.Count();
            if (count > 0)
            {
                model.vendorList = model.vendorList.Skip((pagenumber - 1) * 3).Take(3).ToList();

                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }
            return model;

        }



        public DeleteAccountDetails openDeleteModal(int id, string acntType)
        {
            DeleteAccountDetails model = new();
            switch (acntType)
            {
                case "Vender":
                    model.venderID = id;
                    model.accountTypeName = acntType;
                    break;
                case "AccessRole":
                    model.roleId = id;
                    model.accountTypeName = acntType;
                    break;
                case "Physician":
                    model.PhyId = id;
                    model.accountTypeName = acntType;
                    break;
                case "Unblock":
                    model.blockId = id;
                    model.accountTypeName = acntType;
                    break;
                case "Request":
                    model.reqId = id;
                    model.accountTypeName = acntType;
                    break;

            }
            return model;
        }

        public void deleteVender(int id)
        {
            var vender = _repository.getHealthProfessionalDetail(id);
            vender.Isdeleted = new BitArray(1, true);
            _repository.updateHealthProfessionalTable(vender);
        }

        public void deleteAccessRole(int id)
        {
            var role = _repository.getAccessroleById(id);
            role.Isdeleted = new BitArray(1, true);
            _repository.updateRoleTable(role);
        }

        public void deletePhysicianAccount(int id)
        {
            var phy = _repository.getPhysicianById(id);
            phy.Isdeleted = new BitArray(1, true);
            _repository.updatePhysician(phy);
        }

        public SendOrderModel GetEditBusinessData(int id)
        {
            SendOrderModel model = new SendOrderModel();
            model.businessDetail = _repository.getHealthProfessionalDetail(id);
            model.professions = _repository.getHealthProfessionalTypeList();
            return model;
        }
        public void UpdateBusinessData(SendOrderModel model)
        {
            Healthprofessional business = _repository.getHealthProfessionalDetail(model.businessDetail.Vendorid);
            business.Vendorname = model.businessDetail.Vendorname;
            business.Profession = model.businessDetail.Profession;
            business.Faxnumber = model.businessDetail.Faxnumber;
            business.Address = model.businessDetail.Address;
            business.City = model.businessDetail.City;
            business.State = model.businessDetail.State;
            business.Zip = model.businessDetail.Zip;
            business.Phonenumber = model.businessDetail.Phonenumber;
            business.Email = model.businessDetail.Email;
            business.Businesscontact = model.businessDetail.Businesscontact;
            business.Modifieddate = DateTime.Now;
            _repository.updateHealthProfessionalTable(business);
        }

        public List<Requesttype> GetRequestTypes()
        {
            return _repository.getRequestTypeList();
        }

        public AdminRecordsModel getSearchRecordData(AdminRecordsModel model)
        {

            model.Data = _repository.getRequestClientList();
            model.ReqNotes = _repository.getREquestNotesList();
            model.ReqType = _repository.getRequestTypeList();
            model.phy = _repository.getPhysicianList();
            return model;
        }
        public void SendSms(string receiverPhoneNumber = "+9183200566504", string message = "this is testing")
        {
            string accountSid = "ACf3e07eb694877aff1ffd392934bdb764";
            string authToken = "fe03fccadb7d42d562d8f9879bb50ece";
            string twilioPhoneNumber = "+12676139096";

            //TwilioClient.Init(accountSid, authToken);

            //try
            //{
            //    var smsMessage = MessageResource.Create(
            //        body: message,
            //        from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
            //        to: new Twilio.Types.PhoneNumber(receiverPhoneNumber)
            //    );

            //    Console.WriteLine("SMS sent successfully. SID: " + smsMessage.Sid);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("An error occurred while sending the SMS: " + ex.Message);
            //}
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Emailid = "abc@gmail.com",
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Subjectname = "this is subject",
                Isemailsent = new BitArray(1, true),
                Senttries = 1
            };
            _repository.addEmailLogTable(emaillog);
            Smslog smslog = new Smslog
            {
                Smstemplate = message,
                Mobilenumber = receiverPhoneNumber,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Senttries = 1
            };
            _repository.addSmsLogTable(smslog);
        }

        public Email_SMS_LogModel GetEmailLogs(int roleid, string name, string email, string createdDate, string sentDate, int pagenumber)
        {
            List<Emaillog> emaillogtable = _repository.GetAllEmailLogs(roleid, name, email, createdDate, sentDate);
            List<Email_SMS_LogModel> emailLogs = new List<Email_SMS_LogModel>();
            foreach (Emaillog log in emaillogtable)
            {
                //Aspnetuser aspnetuser = _repository.GetAspNetUserByEmail(log.Emailid);
                //Role role = _repository.GetRoleById((int)log.Roleid);
                int startIndex = log.Emailtemplate.IndexOf("Hello ") + "Hello ".Length;
                int endIndex = log.Emailtemplate.IndexOf(",", startIndex);
                string storedName = log.Emailtemplate.Substring(startIndex, endIndex - startIndex);
                Email_SMS_LogModel emaillog = new Email_SMS_LogModel
                {
                    LogID = log.Emaillogid,
                    Receipient = storedName,
                    Actions = log.Action,
                    RoleName = log.Roleid.ToString(),
                    Email = log.Emailid,
                    createdDate = log.Createdate.ToString("MMM dd, yyyy"),
                    sentDate = log.Createdate.ToString("MMM dd, yyyy"),
                    sent = log.Isemailsent[0].ToString(),
                    sentTries = log.Senttries.ToString(),
                    ConfirmationNum = log.Confirmationnumber,
                };
                emailLogs.Add(emaillog);
            }
            Email_SMS_LogModel model = new Email_SMS_LogModel();
            var count = emailLogs.Count();
            if (count > 0)
            {
                emailLogs = emailLogs.Skip((pagenumber - 1) * 3).Take(3).ToList();

                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }
            model.Logs = emailLogs;
            return model;
        }

        public Email_SMS_LogModel GetSmsLogs(int roleid, string name, string mobile, string createdDate, string sentDate, int pagenumber)
        {
            List<Smslog> smslogtable = _repository.GetAllSmsLogs(roleid, name, mobile, createdDate, sentDate);
            List<Email_SMS_LogModel> smsLogs = new List<Email_SMS_LogModel>();
            foreach (Smslog log in smslogtable)
            {
                //Aspnetuser aspnetuser = _repository.GetAspNetUserByEmail(log.Emailid);
                //Role role = _repository.GetRoleById((int)log.Roleid);
                int startIndex = log.Smstemplate.IndexOf("Hello ") + "Hello ".Length;
                int endIndex = log.Smstemplate.IndexOf(",", startIndex);
                string storedName = log.Smstemplate.Substring(startIndex, endIndex - startIndex);
                Email_SMS_LogModel smslog = new Email_SMS_LogModel
                {
                    LogID = log.Smslogid,
                    Receipient = storedName,
                    Actions = log.Action,
                    RoleName = log.Roleid.ToString(),
                    Mobile = log.Mobilenumber,
                    createdDate = log.Createdate.ToString("MMM dd, yyyy hh:mm tt"),
                    sentDate = log.Createdate.ToString("MMM dd, yyyy"),
                    sent = log.Issmssent[0].ToString(),
                    sentTries = log.Senttries.ToString(),
                    ConfirmationNum = log.Confirmationnumber != null ? log.Confirmationnumber : "--",
                };
                smsLogs.Add(smslog);
            }
            Email_SMS_LogModel model = new Email_SMS_LogModel();
            var count = smsLogs.Count();
            if (count > 0)
            {
                smsLogs = smsLogs.Skip((pagenumber - 1) * 3).Take(3).ToList();

                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }
            model.Logs = smsLogs;
            return model;
        }

        public SchedulingViewModel openShiftModel(int regionid)
        {
            SchedulingViewModel model = new();
            if (regionid == 0)
            {
                var physician = _repository.getPhysicianList();
                model.physics = physician;
            }
            else
            {

                var physician = _repository.getPhysicianListByregion(regionid);
                model.physics = physician;
            }
            var region = _repository.getRegions();
            model.regions = region;
            model.SelectedRegion = regionid;
            return model;
        }

        public void CreateShift(SchedulingViewModel model)
        {
            var weekdays = "";
            foreach (var i in model.daylist)
            {
                weekdays += i;
            }
            var shift = new Shift
            {
                Physicianid = model.PhysicianId,
                Startdate = model.ShiftDate,
                Isrepeat = new BitArray(1, model.IsRepeat),
                Weekdays = weekdays,
                Createdby = "Admin",
                Createddate = DateTime.Now,
                Repeatupto = model.RepeatCount
            };
            _repository.AddShiftTable(shift);


            List<Shiftdetail> shiftdetails = new();

            Shiftdetail shiftdetail = new()
            {
                Shiftid = shift.Shiftid,
                Shiftdate = model.ShiftDate,
                Regionid = model.RegionId,
                Starttime = model.StartTime,
                Endtime = model.EndTime,
                Status = 0,
                Isdeleted = new BitArray(1, false),
            };
            shiftdetails.Add(shiftdetail);

            if (model.IsRepeat)
            {

                List<DateOnly> days = new();
                days.Add(model.ShiftDate);
                for (var i = 0; i < model.RepeatCount; i++)
                {
                    for (int j = 0; j < model.daylist.Count; j++)
                    {
                        int temp;
                        switch (model.daylist[j])
                        {
                            case 0:
                                temp = (int)DayOfWeek.Sunday - (int)DateTime.Parse(days.Last().ToString()).DayOfWeek;
                                break;
                            case 1:
                                temp = (int)DayOfWeek.Monday - (int)DateTime.Parse(days.Last().ToString()).DayOfWeek;
                                break;
                            case 2:
                                temp = (int)DayOfWeek.Tuesday - (int)DateTime.Parse(days.Last().ToString()).DayOfWeek;
                                break;
                            case 3:
                                temp = (int)DayOfWeek.Wednesday - (int)DateTime.Parse(days.Last().ToString()).DayOfWeek;
                                break;
                            case 4:
                                temp = (int)DayOfWeek.Thursday - (int)DateTime.Parse(days.Last().ToString()).DayOfWeek;
                                break;
                            case 5:
                                temp = (int)DayOfWeek.Friday - (int)DateTime.Parse(days.Last().ToString()).DayOfWeek;
                                break;
                            default:
                                temp = (int)DayOfWeek.Saturday - (int)DateTime.Parse(days.Last().ToString()).DayOfWeek;
                                break;
                        }
                        if (temp <= 0)
                        {
                            temp += 7;
                        }
                        days.Add(days.Last().AddDays(temp));
                    }
                }
                foreach (var day in days)
                {
                    Shiftdetail shiftdetail1 = new()
                    {
                        Shiftid = shift.Shiftid,
                        Shiftdate = day,
                        Regionid = model.RegionId,
                        Starttime = model.StartTime,
                        Endtime = model.EndTime,
                        Status = 1,
                        Isdeleted = new BitArray(1, false),
                    };
                    if (days[0] == shiftdetail1.Shiftdate)
                    {
                        continue;
                    }
                    shiftdetails.Add(shiftdetail1);
                }
            }

            _repository.AddShiftDetails(shiftdetails);
        }

        public ShiftDetailsModel getSchedulingData(int reg)
        {
            ShiftDetailsModel model = new();
            model.physicians = _repository.getPhysicianList();
            model.regions = _repository.getRegions();
            model.shiftDetails = _repository.getshiftDetail(reg);
            model.SelectedRegion = reg;
            return model;
        }

        public AdminRecordsModel getBlockHistoryData(string searchstr, DateTime date, string email, string mobile, int pagenumber)
        {
            AdminRecordsModel model = new();
            model.blockRequests = _repository.getBlockData(searchstr, date, email, mobile);
            var count = model.blockRequests.Count();
            if (count > 0)
            {
                model.blockRequests = model.blockRequests.Skip((pagenumber - 1) * 3).Take(3).ToList();

                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }

            return model;
        }

        public void unblockRequest(int id, string email)
        {
            Blockrequest req = _repository.getBlockRequestById(id);
            req.Isactive = new BitArray(1, true);
            req.Modifieddate = DateTime.Now;
            _repository.updateBlockRequest(req);
            Request r = _repository.getRequestByID(req.Requestid);
            r.Status = 1;
            r.Modifieddate = DateTime.Now;
            _repository.updateRequest(r);
            Admin admin = _repository.getAdminTableDataByEmail(email);
            Requeststatuslog log = new Requeststatuslog
            {
                Status = 1,
                Requestid = req.Requestid,
                Adminid = admin.Adminid,
                Createddate = DateTime.Now,
            };
            _repository.addRequestStatusLogTable(log);
        }

        public void deleteRequest(int id)
        {
            Request r = _repository.getRequestByID(id);
            r.Isdeleted = new BitArray(1, true);
            r.Modifieddate = DateTime.Now;
            _repository.updateRequest(r);

        }

        public PatientHistoryTable PatientHistoryTable(string? fname, string? lname, string? email, string? phone, int pagenumber)
        {
            IQueryable<PatientHistoryTable> tabledata = _repository.GetPatientHistoryTable(fname, lname, email, phone);
            PatientHistoryTable model = new PatientHistoryTable();

            var count = tabledata.Count();
            if (count > 0)
            {
                tabledata = tabledata.Skip((pagenumber - 1) * 3).Take(3);

                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }
            model.patientHistoryData = tabledata.ToList();
            return model;
        }

        public PatientRecordModel PatientRecord(int id, int pagenumber)
        {
            List<Request> requests = _repository.GetAllRequestsByAid(id);
            List<PatientRecordModel> records = new List<PatientRecordModel>();
            foreach (Request request in requests)
            {
                Physician physician = new Physician();
                if (request.Physicianid != null)
                {
                    physician = _repository.getPhysicianById((int)request.Physicianid);
                }
                PatientRecordModel record = new PatientRecordModel
                {
                    rid = request.Requestid,
                    Name = request.Firstname + " " + request.Lastname,
                    createdDate = request.Createddate.ToString("MMM dd, yyyy"),
                    conNo = request.Confirmationnumber ?? "-",
                    phyName = physician.Firstname == null && physician.Lastname == null ? "-" : "Dr. " + physician.Firstname + " " + physician.Lastname,
                    concludeDate = request.Status == 6 && request.Modifieddate != null ? request.Modifieddate.Value.ToString("MMM dd, yyyy") : "-",
                    status = _repository.GetStatus(request.Status) ?? "-",
                    docNo = _repository.GetNumberOfDocsByRid(request.Requestid),
                };
                records.Add(record);
            }
            PatientRecordModel model = new PatientRecordModel();
            var count = records.Count();
            if (count > 0)
            {
                records = records.Skip((pagenumber - 1) * 3).Take(3).ToList();

                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }
            model.records = records;
            return model;
        }

        public ShiftDetailsModel getViewShiftData(int id, int regid)
        {
            ShiftDetailsModel model = new ShiftDetailsModel();
            Shiftdetail sd = _repository.getShiftDetailByShiftDetailId(id);

            Shift s = _repository.getShiftByID(sd.Shiftid);
            //if (regid != 0)
            //{
            //    model.RegionId = regid;
            //    model.physicians = _repository.getPhysicianListByregion(regid);
            //}
            //else
            //{
            //    model.RegionId = (int)sd.Regionid;
            //    model.physicians = _repository.getPhysicianList();
            //}
            DateOnly date = DateOnly.Parse(sd.Shiftdate.ToString("yyyy-MM-dd"));
            model.regions = _repository.getRegions();
            model.Shiftdate = date;
            model.Physicianid = s.Physicianid;
            model.shiftData = s;
            model.ShiftDetailData = sd;
            model.RegionId = (int)sd.Regionid;
            model.physicians = _repository.getPhysicianList();


            return model;
        }

        public void UpdateShiftDetailData(ShiftDetailsModel model, string email)
        {

            Admin admin = _repository.getAdminTableDataByEmail(email);
            Shiftdetail sd = _repository.getShiftDetailByShiftDetailId(model.ShiftDetailData.Shiftdetailid);
            sd.Modifiedby = admin.Aspnetuserid.ToString();
            sd.Starttime = model.ShiftDetailData.Starttime;
            sd.Endtime = model.ShiftDetailData.Endtime;
            sd.Shiftdate = model.Shiftdate;
            sd.Modifieddate = DateTime.Now;
            _repository.UpdateShiftDetailTable(sd);
        }
        public void DeleteShiftDetails(int id)
        {
            Shiftdetail sd = _repository.getShiftDetailByShiftDetailId(id);
            sd.Isdeleted = new BitArray(1, true);
            sd.Modifieddate = DateTime.Now;
            _repository.UpdateShiftDetailTable(sd);
        }

        public void UpdateShiftDetailsStatus(int id)
        {
            Shiftdetail sd = _repository.getShiftDetailByShiftDetailId(id);
            if (sd.Status == 0)
            {
                sd.Status = 1;
            }
            else
            {
                sd.Status = 0;
            }
            _repository.UpdateShiftDetailTable(sd);
        }

        public ShiftDetailsModel getReviewShiftData(int reg, bool isCurrentMonth, int pagenumber)
        {
            ShiftDetailsModel model = new ShiftDetailsModel();
            model.regions = _repository.getRegions();
            model.RegionId = reg;
            model.shiftdetail = _repository.getShiftDetailByRegion(reg);
            if (isCurrentMonth) {
                model.shiftdetail = model.shiftdetail.Where(e => e.Shiftdate.Month == DateTime.Now.Month).ToList();
            }
            var count = model.shiftdetail.Count();
            model.TotalRecord = count;

            if (count > 0)
            {
                model.shiftdetail = model.shiftdetail.Skip((pagenumber - 1) * 3).Take(3).ToList();
                model.FromRec = (pagenumber-1) * 3;
                model.ToRec = model.FromRec + 3;
                if(model.ToRec > model.TotalRecord) {
                    model.ToRec = model.TotalRecord;
                }
                model.FromRec += 1;
                model.TotalPages = (int)Math.Ceiling((double)count / 3);
                model.CurrentPage = pagenumber;
                model.PreviousPage = pagenumber > 1;
                model.NextPage = pagenumber < model.TotalPages;
            }
            return model;
        }

        public void DeletShift(string[] selectedShifts)
        {
            var shifts = selectedShifts[0].Split(',');
            foreach (var shift in shifts)
            {
                Shiftdetail shiftdetail = _repository.getShiftDetailByShiftDetailId(int.Parse(shift));
                shiftdetail.Isdeleted = new BitArray(1, true);
                _repository.UpdateShiftDetailTable(shiftdetail);
            }
        }

        public void ApproveShift(string[] selectedShifts)
        {
            var shifts = selectedShifts[0].Split(',');
            foreach (var shift in shifts)
            {
                Shiftdetail shiftdetail = _repository.getShiftDetailByShiftDetailId(int.Parse(shift));
                shiftdetail.Status = 1;
                _repository.UpdateShiftDetailTable(shiftdetail);
            }
        }

        public ShiftDetailsModel getProviderOnCall(int reg)
        {
            return new ShiftDetailsModel
            {
                regions = _repository.getRegions(),
                physicians = _repository.getPhysicianOnCallList(reg)
            };
        }

        public void SaveNotificationStatus(string[] phyList)
        {
            var phy = phyList[0].Split(',');
            int[] ints = new int[phy.Length];
            int t = 0;
            foreach (var i in phy)
            {
                ints[t++] = int.Parse(i);
            }
            List<Physiciannotification> selected = _repository.getSelectedPhyNotification(ints);
            List<Physiciannotification> notSelected = _repository.getNotSelectedPhyNotification(ints);
            foreach (var i in selected)
            {
                Physiciannotification notification = _repository.getPhyNotificationByPhyID(i.Pysicianid);
                notification.Isnotificationstopped = new BitArray(1, true);
                _repository.updatePhyNotificationTable(notification);
            }
            foreach (var i in notSelected)
            {
                Physiciannotification notification = _repository.getPhyNotificationByPhyID(i.Pysicianid);
                notification.Isnotificationstopped = new BitArray(1, false);
                _repository.updatePhyNotificationTable(notification);
            }
        }

        public void DTYSupportRequest(string notes,string email)
        {
            var data = _repository.GetUnscheduledPhysicanID();
            var phy = _repository.getUnscheduledPhysicianList(data);
            foreach (var i in phy)
            {
                
                var receiver = i.Email;
                var subject = "DTY Support Request";
                var message = "We are short on coverage and needs additional support On Call to respond to Requests.::"+notes;
                SendEmail(receiver, message, subject);

                var admin = _repository.getAdminTableDataByEmail(email);
                Emaillog emaillog = new Emaillog
                {
                    Emailtemplate = message,
                    Subjectname = "Make Your Appointment",
                    Emailid = i.Email,
                    Roleid = 1,
                    Adminid = admin.Adminid,
                    Createdate = DateTime.Now,
                    Sentdate = DateTime.Now,
                    Isemailsent = new BitArray(1, true),
                    Action = 1
                };
                _repository.addEmailLogTable(emaillog);
            }
        }


        public void AddVendor(SendOrderModel model)
        {
            var business = new Healthprofessional
            {
                Vendorname = model.businessDetail.Vendorname,
                Profession = model.SelectedProfession,
                Faxnumber = model.businessDetail.Faxnumber,
                Address = model.businessDetail.Address,
                City = model.businessDetail.City,
                State = model.businessDetail.State,
                Zip = model.businessDetail.Zip,
                //Regionid = model.businessDetail.Regionid,
                Createddate = DateTime.Now,
                Phonenumber = model.businessDetail.Phonenumber,
                Email = model.businessDetail.Email,
                Businesscontact = model.businessDetail.Businesscontact,

            };
            _repository.addHealthProfessionTable(business);
        }

        public SendOrderModel getVenderData()
        {
            SendOrderModel model = new();
            model.professions = _repository.getVenderDetail(0,null);
            
            return model;
        }

        public int getRequestTypeByRequestID(int id)
        {
            var req = _repository.getRequestByID(id);
            return req.Requesttypeid;
        }
    }
}

