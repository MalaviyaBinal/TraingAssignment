using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Fax;
using Twilio.TwiML.Voice;

namespace HalloDocWebServices.Implementation
{
    public class Provider_Service : IProvider_Service
    {
        private readonly IProvider_Repository _repository;
        public Provider_Service(IProvider_Repository repository)
        {
            _repository = repository;
        }

        public List<Region> getRegionList()
        {
            return _repository.getRegions();
        }

        public AdminDashboard setProviderDashboardCount(string email)
        {
            Physician physician = _repository.GetPhyByEmail(email);
            var viewmodel = new AdminDashboard
            {
                NewCount = _repository.getcount(1, physician.Physicianid),
                PendingCount = _repository.getcount(2, physician.Physicianid),
                ActiveCount = _repository.getcount(3, physician.Physicianid),
                ConcludeCount = _repository.getcount(4, physician.Physicianid),
                TocloseCount = _repository.getcount(5, physician.Physicianid),
                UnpaidCount = _repository.getcount(6, physician.Physicianid),
            };
            return viewmodel;
        }
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check, string email)
        {
            Physician physician = _repository.GetPhyByEmail(email);
            List<AdminDashboardTableModel> tabledata;
            if (check == 0)
            {
                return _repository.getDashboardTablesWithoutcheck(id, physician.Physicianid);
            }
            else
            {
                return _repository.getDashboardTables(id, check, physician.Physicianid);
            }
        }
        public List<Physician> getPhysicianList()
        {
            return _repository.getPhysicianList();
        }
        public void patientRequestByProvider(RequestForMe info, string email)
        {
            User user = _repository.getUserByEmail(info.email);
            var createdby = _repository.GetPhyByEmail(email);

            Request req = new Request
            {
                Requesttypeid = 2,

                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phonenumber,
                Email = info.email,
                Status = 1,
                Createddate = DateTime.Now,

                Isurgentemailsent = new BitArray(1, false)
            };
            if (user != null)
            {
                req.Userid = user.Userid;
            }
            _repository.addRequestTable(req);
            Requestclient reqclient = new Requestclient
            {
                Intyear = info.dob.Value.Year,
                Intdate = info.dob.Value.Day,
                Strmonth = info.dob.Value.Month.ToString(),
                Requestid = req.Requestid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Phonenumber = info.phonenumber,
                Email = info.email,
                Location = info.street + "," + info.city + "," + info.state + " ," + info.zipcode,
                Street = info.street,
                City = info.city,
                State = info.state,
                Zipcode = info.zipcode,
                Regionid = 1
            };
            _repository.adRequestClientTable(reqclient);
            Requestnote note = new Requestnote
            {
                Requestid = req.Requestid,
                Createdby = createdby.Aspnetuserid.ToString(),
                Createddate = DateTime.Now,
                Physiciannotes = info.admin_notes
            };
            _repository.addRequestNotesTAble(note);
        }
        public void SendSMS(string phonenumber, string message)
        {
            string accountSid = "ACf3e07eb694877aff1ffd392934bdb764";
            string authToken = "fe03fccadb7d42d562d8f9879bb50ece";
            string twilioPhoneNumber = "+12676139096";

            TwilioClient.Init(accountSid, authToken);

            try
            {
                var smsMessage = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(phonenumber)
                );

                Console.WriteLine("SMS sent successfully. SID: " + smsMessage.Sid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while sending the SMS: " + ex.Message);
            }
            Console.WriteLine(message);

        }
        public void sendLinkProviderDashboard(AdminDashBoardPagination info, string email)
        {
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Make Your Appointment";
            var message = "Hello " + info.Fname + " " + info.Lname + ",You are invited to visit :https://localhost:44380/";
            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);
            mailclient.SendMailAsync(mailMessage);
            var phy = _repository.GetPhyByEmail(email);
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = "Make Your Appointment",
                Emailid = info.Email,
                Roleid = 1,
                Physicianid = phy.Aspnetuserid,
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
                Senttries = 1,
                Roleid = 3,
                Action = 2,
                Issmssent = new BitArray(1, true),
            };
            _repository.addSmsLogTable(smslog);

        }

        public void AcceptConsultRequest(int id, string email)
        {
            Physician phy = _repository.GetPhyByEmail(email);
            Request data = _repository.getRequestByReqID(id);
            data.Accepteddate = DateTime.Now;
            data.Status = 2;
            _repository.updateRequestTable(data);
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = phy.Firstname + " " + phy.Lastname + "has Accepted The request",
                Status = 1,
                Createddate = DateTime.Now,
                Physicianid = phy.Physicianid,

            };
            _repository.addRequestStatusLogTable(statuslog);
        }
        public ViewCaseModel ViewCaseModel(int id)
        {
            Requestclient data = _repository.getRequestClientById(id);
            Request request = _repository.getRequestByReqID(data.Requestid);
            Region region = _repository.getRegionByRegionId(data.Regionid);
            ViewCaseModel model = new();
            model.FName = data.Firstname;
            model.LName = data.Lastname;
            model.DOB = DateTime.Parse(DateTime.Parse(data.Intdate + data.Strmonth + data.Intyear).ToString("yyyy-MM-dd"));
            model.Notes = data.Notes;
            model.Phonenumber = data.Phonenumber;
            model.Email = data.Email;
            model.RegionName = region.Name;
            model.requestId = id;
            model.Address = data.Street + " " + data.City + " " + data.State + " " + data.Zipcode;
            model.status = request.Status;
            return model;
        }
        public Notes ViewNotes(int id)
        {
            Requestnote reqnote = _repository.getREquestNotes(id);
            List<Requeststatuslog> notelog = _repository.getRequestStatusLog(id);
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
                        Physician phy = _repository.getPhysicianById(item.Physicianid);
                        var str = phy.Firstname + " has transfered case to admin On " + item.Createddate.ToString() + " : " + item.Notes;
                        strings.Add(str);
                    }
                    else if (item.Transtophysicianid != null)
                    {
                        Physician phy1 = _repository.getPhysicianById(item.Physicianid);
                        Physician phy2 = _repository.getPhysicianById(item.Transtophysicianid);
                        var str = "admin transfered case from " + phy1.Firstname + "to " + phy2.Firstname + " on " + item.Createddate.ToString() + ": " + item.Notes;
                        strings.Add(str);
                    }
                    else if (item.Physicianid != null && item.Adminid != null)
                    {
                        Physician phy = _repository.getPhysicianById(item.Physicianid);
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
        public void saveNotes(Notes n, int id)
        {
            var user = _repository.getRequestByReqID(id);
            var reqnotes = _repository.getREquestNotes(id);
            if (reqnotes != null && n.PhyNotes != null)
            {
                reqnotes.Physiciannotes = n.PhyNotes;
                reqnotes.Createdby = user.Firstname;
                reqnotes.Modifiedby = user.Firstname;
                reqnotes.Modifieddate = DateTime.Now;
                _repository.updateRequestNoteTable(reqnotes);
            }
            else if (reqnotes == null)
            {
                Requestnote addreq = new Requestnote
                {
                    Requestid = id,
                    Physiciannotes = n.PhyNotes,
                    Createdby = user.Email,
                    Createddate = DateTime.Now,
                };
                _repository.addRequestNotesTAble(addreq);
            }
        }

        public Requestclient getRequestClientByID(int id)
        {
            return _repository.getRequestClientById(id);
        }
        public int getRequestTypeByRequestID(int id)
        {
            var req = _repository.getRequestByReqID(id);
            return req.Requesttypeid;
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

            var receiver = client.Email;
            var subject = "Review Agreement";
            var message = "Hello " + client.Firstname + " " + client.Lastname + ",Review Your Agreement:https://localhost:44380/Admin/SendAgreement?token=" + token;

            SendEmail(receiver, message, subject);
            var phy = _repository.GetPhyByEmail(email);
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = "Make Your Appointment",
                Emailid = client.Email,
                Roleid = 1,
                Physicianid = phy.Physicianid,
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
                Physicianid = phy?.Physicianid,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Senttries = 1
            };
            _repository.addSmsLogTable(smslog);
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

        public Request GetRequestData(int id)
        {
            return _repository.getRequestByReqID(id);
        }

        public void TransferConfirm(Request reg, string? email)
        {
            Physician phy = _repository.GetPhyByEmail(email);
            Request data = _repository.getRequestByReqID(reg.Requestid);
            data.Physicianid = null;
            data.Status = 1;
            _repository.updateRequestTable(data);
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = data.Requestid,
                Transtoadmin = new BitArray(1, true),
                Notes = reg.Casetag,
                Status = 1,
                Createddate = DateTime.Now,
                Physicianid = phy.Physicianid,

            };
            _repository.addRequestStatusLogTable(statuslog);
        }
        public AdminViewUpload getPatientDocument(int? id)
        {
            AdminViewUpload model = new();
            model.FileList = _repository.getPatientDocument(id);
            model.patientData = _repository.getRequestClientById(id);
            model.confirmationDetail = _repository.getRequestByReqID((int)id);
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
        public void uploadFileAdmin(IFormFile fileToUpload, int id, string email)
        {
            Aspnetuser admin = _repository.getAspnetuserByEmail(email);
            string FileNameOnServer = "D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\UploadedFiles\\";
            FileNameOnServer += fileToUpload.FileName;
            using var stream = System.IO.File.Create(FileNameOnServer);
            fileToUpload.CopyTo(stream);
            var userobj = _repository.getRequestByReqID(id);
            Requestwisefile reqclient = new Requestwisefile
            {
                Requestid = id,
                Filename = fileToUpload.FileName,
                Createddate = DateTime.Now,
                Adminid = admin.Id
            };
            _repository.addRequestWiseFile(reqclient);
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
        public void EncounterFinalize(int id)
        {
            var data = _repository.getRequestByReqID(id);
            data.Completedbyphysician = new BitArray(1, true);
            _repository.updateRequestTable(data);
            EncounterForm info = _repository.getEncounterTable(id);
            info.IsFinalized = new BitArray(1, true);
            _repository.updateEncounterForm(info);
        }

        public void ConsultCall(int id)
        {
            var data = _repository.getRequestByReqID(id);
            data.Status = 6;
            data.Calltype = 2;
            _repository.updateRequestTable(data);
        }
        public void HouseCall(int id)
        {
            var data = _repository.getRequestByReqID(id);
            data.Status = 5;
            data.Calltype = 1; // HouseCall Will Get 1
            _repository.updateRequestTable(data);
        }
        public Encounterformmodel EncounterProvider(int id)
        {
            var patientData = _repository.getRequestClientById(id);
            Encounterformmodel model = new();
            model.patientData = patientData;
            model.confirmationDetail = _repository.getRequestByReqID(id);
            model.DOB = DateOnly.Parse(DateTime.Parse(model.patientData.Intdate + model.patientData.Strmonth + model.patientData.Intyear).ToString("yyyy-MM-dd"));
            var info = _repository.getEncounterTable(id);
            if (info != null)
            {

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
            }
            return model;
        }



        public void saveEncounterForm(Encounterformmodel info)
        {
            EncounterForm model1 = _repository.getEncounterTable(info.confirmationDetail.Requestid);
            if (model1 == null)
            {
                EncounterForm model = new EncounterForm();
                //var model = _repository.getEncounterTable(info.Requestid);
                model.Requestid = info.confirmationDetail.Requestid;
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
                _repository.addEncounterTable(model);
            }
            else
            {

                //var model = _repository.getEncounterTable(info.Requestid);

                model1.Abd = info.Abd;
                model1.Skin = info.Skin;
                model1.Hr = info.Hr;
                model1.O2 = info.O2;
                model1.Rr = info.Rr;
                model1.Cv = info.Cv;
                model1.BpS = info.BpS;
                model1.BpD = info.BpD;
                model1.Temp = info.Temp;
                model1.Allergies = info.Allergies;
                model1.Chest = info.Chest;
                model1.Date = info.Date;
                model1.Diagnosis = info.Diagnosis;
                model1.Extr = info.Extr;
                model1.Heent = info.Heent;
                model1.FollowUp = info.FollowUp;
                model1.HistoryIllness = info.HistoryIllness;
                model1.MedicalHistory = info.MedicalHistory;
                model1.Medications = info.Medications;
                model1.Procedures = info.Procedures;
                model1.MedicationDispensed = info.MedicationDispensed;
                model1.TreatmentPlan = info.TreatmentPlan;
                model1.Neuro = info.Neuro;
                model1.Pain = info.Pain;
                model1.Other = info.Other;
                _repository.updateEncounterForm(model1);
            }
        }

        public void deleteFile(int id)
        {
            Requestwisefile file = _repository.getRequestWiseFile(id);
            file.Isdeleted = new BitArray(1, true);
            _repository.updateRequestWiseFile(file);
        }
        public void SendEmail(int id, string[] filenames, string email)
        {
            List<Requestwisefile> files = new();
            foreach (var filename in filenames)
            {
                files.Add(_repository.getRequesWiseFileList(id, filename));
            }
            Request request = _repository.getRequestByReqID(id);
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Documents of Request ";
            var message = "Hello " + request.Firstname + " " + request.Lastname + ",Find the Files uploaded for your request in below:";
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
            var phy = _repository.GetPhyByEmail(email);
            Emaillog emaillog = new Emaillog
            {
                Emailtemplate = message,
                Subjectname = "Make Your Appointment",
                Emailid = request.Email,
                Roleid = 1,
                Physicianid = phy.Physicianid,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Action = 1
            };
            _repository.addEmailLogTable(emaillog);
        }
        public void HouseCallToConclude(int id)
        {
            var data = _repository.getRequestByReqID(id);
            data.Status = 6;
            _repository.updateRequestTable(data);
        }
        public AdminViewUpload ConcludeCare(int id)
        {
            AdminViewUpload model = new();

            model.patientData = _repository.getRequestClientById(id);

            var note = _repository.getREquestNotes(id);
            DateOnly date = DateOnly.Parse(DateTime.Parse(model.patientData.Intdate + model.patientData.Strmonth + model.patientData.Intyear).ToString("yyyy-MM-dd"));

            model.confirmationDetail = _repository.getRequestByReqID(id);
            model.FileList = _repository.getRequestWiseFileList(id);
            model.DOB = date;
            model.phyNotes = note != null && note.Physiciannotes != null ? note.Physiciannotes : "--";
            return model;
        }

        public void ConcludeFinal(AdminViewUpload model, string? email)
        {
            Request req = _repository.getRequestByReqID(model.confirmationDetail.Requestid);
            Physician phy = _repository.GetPhyByEmail(email);
            req.Status = 8;
            req.Modifieddate = DateTime.Now;
            _repository.updateRequestTable(req);
            var reqnotes = _repository.getREquestNotes(model.confirmationDetail.Requestid);
            if (reqnotes != null)
            {
                reqnotes.Physiciannotes = model.phyNotes;
                reqnotes.Modifieddate = DateTime.Now;
                _repository.updateRequestNoteTable(reqnotes);
            }
            else
            {
                Requestnote addreq = new Requestnote
                {
                    Requestid = model.confirmationDetail.Requestid,
                    Physiciannotes = model.phyNotes,
                    Createdby = phy.Aspnetuserid.ToString(),
                    Createddate = DateTime.Now,
                };
                _repository.addRequestNotesTAble(addreq);
            }
        }

        public AdminProviderModel getPhyProfileData(string? email)
        {
            Physician phy = _repository.GetPhyByEmail(email);
            Aspnetuser asp = _repository.getAspnetuserByID(phy.Aspnetuserid);
            AdminProviderModel model = new AdminProviderModel();
            model.regions = _repository.getRegions();
            model.phyregions = _repository.getPhysicianRegionByPhy(phy.Physicianid);
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

            model.IsTrainingDoc = phy.Istrainingdoc;
            model.isPhoto = phy.Photo != null ? true : false;
            model.isSignature = phy.Signature != null ? true : false;
            model.PhotoName = phy.Photo != null ? phy.Photo : null;
            model.SignatureName = phy.Signature != null ? phy.Signature : null;

            return model;
        }

        public void UpdateProfileRequest(string? email, string? adminnotes)
        {
            List<Admin> admins = _repository.getAdminList();
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var phy = _repository.GetPhyByEmail(email);
            foreach (Admin admin in admins)
            {
                var receiver = "binalmalaviya2002@gmail.com";//admin.Email
                var subject = "Request for Update Profile";
                var message = "Hello " + admin.Firstname + " " + admin.Lastname + " ,It's " + phy.Firstname + "\n" + adminnotes + "\nclick here to update My Profile : https://localhost:44327/Admin/EditPhysicianAccount?id=" + phy.Physicianid;
                var mailMessage = new MailMessage(from: "chaityamehta522003@gmail.com", to: receiver, subject, message);

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };
                client.SendMailAsync(mailMessage);

                Emaillog emaillog = new Emaillog
                {
                    Emailtemplate = message,
                    Subjectname = "Request for Update Profile",
                    Emailid = admin.Email,
                    Roleid = 1,
                    Physicianid = phy.Aspnetuserid,
                    Createdate = DateTime.Now,
                    Sentdate = DateTime.Now,
                    Isemailsent = new BitArray(1, true),
                    Action = 1
                };
                _repository.addEmailLogTable(emaillog);

            }
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
        public ShiftDetailsModel getSchedulingData(string email)
        {
            var phy = _repository.GetPhyByEmail(email);
            ShiftDetailsModel model = new();
            model.physicians = _repository.getPhysicianList();
            model.regions = _repository.getRegions();
            model.shiftDetails = _repository.getshiftDetail(phy.Physicianid);

            return model;
        }
        public ShiftDetailsModel getViewShiftData(int id, int regid)
        {
            ShiftDetailsModel model = new ShiftDetailsModel();
            Shiftdetail sd = _repository.getShiftDetailByShiftDetailId(id);

            Shift s = _repository.getShiftByID(sd.Shiftid);

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
        public SchedulingViewModel openShiftModel(int regionid)
        {
            SchedulingViewModel model = new();

            var region = _repository.getRegions();
            model.regions = region;

            return model;
        }
        public void CreateShift(SchedulingViewModel model, string email)
        {
            var phy = _repository.GetPhyByEmail(email);
            var weekdays = "";
            if (model.daylist != null)
            {
                foreach (var i in model.daylist)
                {
                    weekdays += i;
                }
            }

            var shift = new Shift
            {
                Physicianid = phy.Physicianid,
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
        public void DeleteShiftDetails(int id)
        {
            Shiftdetail sd = _repository.getShiftDetailByShiftDetailId(id);
            sd.Isdeleted = new BitArray(1, true);
            sd.Modifieddate = DateTime.Now;
            _repository.UpdateShiftDetailTable(sd);
        }

        public List<TimeSheetViewModel> MakeTimeSheet(DateTime startDate, string phyid)
        {

            var phy = _repository.GetPhyByEmail(phyid);
            List<TimeSheetViewModel> TimeSheet = _repository.MakeTimeSheet(startDate, phy.Physicianid);

            return TimeSheet;
        }

        public void SaveTimesheet(TimeSheetDataViewModel model, string? phyEmail)
        {
            var phy = _repository.GetPhyByEmail(phyEmail);
            Timesheet invoice1 = _repository.GetInvoicesByPhyId(model.StartDate, model.EndDate, phy.Physicianid);

            if (invoice1 != null)
            {
                int i1 = 0;
                List<TimesheetDetail> timesheets = _repository.GetTimeSheetListByInvoiceId(invoice1.TimesheetId);
                if (timesheets.Count != 0)
                {
                    foreach (TimesheetDetail sheet in timesheets)
                    {
                        sheet.IsWeekend = model.WeekendHoliday.Any(e => e == sheet.Sheetdate.Value.Day);
                        sheet.ShiftHours = model.TotalHours.ElementAt(i1);
                        sheet.Housecall = model.NumberOfHouseCalls.ElementAt(i1);
                        sheet.PhoneConsult = model.NumberOfPhoneConsults.ElementAt(i1);
                        sheet.ModifiedBy = phy.Aspnetuserid ?? 1;
                        sheet.ModifiedDate = DateTime.Now;
                        i1++;
                    }
                    _repository.UpdateTimeSheetDetailTable(timesheets);
                }
                else
                {
                    List<TimesheetDetail> timesheet = new();
                    for (int i = 0; i <= model.EndDate.Value.Day - model.StartDate.Value.Day; i++)
                    {
                        timesheet.Add(new TimesheetDetail
                        {
                            TimesheetId = invoice1.TimesheetId,
                            PhysicianId = phy.Physicianid,
                            Sheetdate = model.StartDate.Value.AddDays(i),
                            ShiftHours = model.TotalHours.ElementAt(i),
                            IsWeekend = model.WeekendHoliday.Any(e => e == model.StartDate.Value.AddDays(i).Day),
                            Housecall = model.NumberOfHouseCalls.ElementAt(i),
                            PhoneConsult = model.NumberOfPhoneConsults.ElementAt(i),
                            CreatedBy = phy.Aspnetuserid ?? 1,
                            CreatedDate = DateTime.Now,
                            //NoHousecallsNight = 0,
                            //NoPhoneConsultNight = 0,
                        });
                    }
                    _repository.AddTimeSheetDetailTable(timesheet);
                }
            }
            else
            {
                Timesheet invoice = new Timesheet();
                invoice.PhysicianId = phy.Physicianid;
                invoice.Startdate = (DateTime)model.StartDate;
                invoice.Enddate = (DateTime)model.EndDate;
                //invoice.Createdby = phy.AspNetUserId ?? 1;
                //invoice.CreatedDate = DateTime.Now;
                _repository.AddTimeSheetTable(invoice);


                List<TimesheetDetail> timesheet = new();
                for (int i = 0; i <= model.EndDate.Value.Day - model.StartDate.Value.Day; i++)
                {
                    timesheet.Add(new TimesheetDetail
                    {
                        TimesheetId = invoice.TimesheetId,
                        PhysicianId = phy.Physicianid,
                        Sheetdate = model.StartDate.Value.AddDays(i),
                        ShiftHours = model.TotalHours.ElementAt(i),
                        IsWeekend = model.WeekendHoliday.Any(e => e == model.StartDate.Value.AddDays(i).Day),
                        Housecall = model.NumberOfHouseCalls.ElementAt(i),
                        PhoneConsult = model.NumberOfPhoneConsults.ElementAt(i),
                        CreatedBy = phy.Aspnetuserid ?? 1,
                        CreatedDate = DateTime.Now,
                        //NoHousecallsNight = 0,
                        //NoPhoneConsultNight = 0,
                    });
                }
                _repository.AddTimeSheetDetailTable(timesheet);

            }
        }

        public void EditReimbursement(DateTime startDate, string item, int amount, int gap, string? phyEmail)
        {
            var phy = _repository.GetPhyByEmail(phyEmail);

            TimesheetReimbursement reim = _repository.GetReimByPhyIdAndStartDate(DateTime.Parse(startDate.AddDays(gap).ToString("MM-dd-yyyy")), phy.Physicianid);
            reim.Amount = amount;
            reim.Item = item;
            reim.ModifiedBy = phy.Aspnetuserid ?? 1;
            reim.ModifiedDate = DateTime.Now;
            _repository.UpdateReimbursementTable(reim);
        }

        public void SaveReimbursement(TimeSheetDataViewModel model, string? phyEmail)
        {
            var phy = _repository.GetPhyByEmail(phyEmail);

            Timesheet invoice1 = _repository.GetInvoicesByPhyId(model.StartDate, model.EndDate, phy.Physicianid);
            int timesheetID = 0;
            if (invoice1 != null)
            {
                timesheetID = invoice1.TimesheetId;
            }
            else
            {
                Timesheet invoice = new Timesheet();
                invoice.PhysicianId = phy.Physicianid;
                invoice.Startdate = (DateTime)model.StartDate;
                invoice.Enddate = (DateTime)model.EndDate;
                //invoice.Createdby = phy.AspNetUserId ?? 1;
                //invoice.CreatedDate = DateTime.Now;
                _repository.AddTimeSheetTable(invoice);
                timesheetID = invoice.TimesheetId;
            }

            TimesheetReimbursement reim = new TimesheetReimbursement
            {
                Amount = model.Amount,
                Item = model.Item,
                ReimbursementDate = model.StartDate.Value.AddDays(model.Gap),
                TimesheetId = timesheetID,
                Filename = model.ReceiptFile.FileName,
                PhysicianId = phy.Physicianid,
                CreatedBy = phy.Aspnetuserid ?? 1,
                CreatedDate = DateTime.Now,

            };

            string filename = model.ReceiptFile.FileName;
            string path = Path.Combine("D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\PhysicianDoc\\" + phy.Physicianid + "\\" + filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using FileStream stream = new(path, FileMode.Create);
            model.ReceiptFile?.CopyTo(stream)
;

            _repository.AddReimbursementTable(reim);
        }

        public void DeleteReimbursement(int gap, DateTime startDate)
        {
            _repository.DeleteReimbursementTable(gap, startDate);
        }

        public void FinalizeTimesheet(int timesheetId)
        {
            Timesheet timesheet = _repository.GetTimeSheetByInvoiceId(timesheetId);
            if (timesheet != null)
            {
                timesheet.IsFinalized = true;
                _repository.UpdateTimeSheetTable(timesheet);
            }
        }

        public bool IsTimesheetFinalized(DateTime startDate,string phyEmail)
        {
            DateTime enddate = startDate.AddDays(15 - startDate.Day);
            var phy = _repository.GetPhyByEmail(phyEmail);
            if (startDate.Day > 15)
            {
                enddate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day);
            }
            Timesheet invoice = _repository.GetInvoicesByPhyId(startDate, enddate, phy.Physicianid);
            if (invoice != null)
            {
                if(invoice.IsFinalized == true)
                    return true;
                else
                return false;
            }
            return false;
        }

        ChatViewModel IProvider_Service._ChatPanel(string? email, int receiver1,int receiver2, string requesterType)
        {

            Physician phy = _repository.GetPhyByEmail(email);
            ChatViewModel model = new ChatViewModel();

            switch (requesterType)
            { 
                case "Patient":
                    Request request = _repository.getRequestByReqID(receiver2);
                    User user = _repository.GetUserByUserId(request.Userid);
                    model.ReceiverName = user.Firstname + " " + user.Lastname;
                    model.Receiver = user.Aspnetuserid;
                    model.Receiver2 = 0;
                    break;
                case "ProviderGroup":
                    Admin admin = _repository.GetAdminByAspId(receiver1);
                    Request request1 = _repository.getRequestByReqID(receiver2);
                    User user1 = _repository.GetUserByUserId(request1.Userid);
                    model.ReceiverName =user1.Firstname+ " &AD." + admin.Firstname;
                    model.Receiver1Name =   admin.Firstname;
                    model.Receiver2Name = user1.Firstname;
                    model.Receiver = receiver1;
                    model.Receiver1 = receiver1;
                    model.Receiver2 = user1.Aspnetuserid;
                    break;
            }
            model.Sender = (int)phy.Physicianid;
            model.SenderType = "Provider";
            model.ReceiverType = requesterType;
            model.SenderName = phy.Firstname + " " + phy.Lastname;
            model.CurrentUserId = (int)phy.Aspnetuserid;
            return model;

            ////if (requesterType == "Provider")
            ////{
            ////    Physician phy = _repository.getPhysicianByID(receiver);
            ////    model.ReceiverName = "Dr." + phy.Firstname + " " + phy.Lastname;
            ////}

            //Request request = _repository.getRequestByReqID(receiver);
            //    User user = _repository.GetUserByUserId(request.Userid);
            //    model.ReceiverName =user.Firstname + " " + user.Lastname;

            //model.Receiver = user.Aspnetuserid;
            //model.Sender = (int)phy.Physicianid;
            //model.SenderType = "Provider";
            //model.ReceiverType = requesterType;
            //model.SenderName = phy.Firstname + " " + phy.Lastname;
            //model.CurrentUserId = (int)phy.Aspnetuserid;
           
        }
    }
}

