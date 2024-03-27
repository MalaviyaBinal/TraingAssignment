
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebRepo.Interface;
using HalloDocWebService.Authentication;
using HalloDocWebServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;

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
                Requestid = request.Requestid.ToString(),
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
            //request.Physicianid = 0;
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
            //DateOnly date = DateOnly.Parse(DateTime.Parse(patientData.Intdate + patientData.Strmonth + patientData.Intyear).ToString("yyyy-MM-dd"));
            Encounterformmodel model = new();
            var info = _repository.getEncounterTable(id);
            model.patientData = patientData;
            model.confirmationDetail = _repository.getRequestByID(id);
            //model.DOB = date;
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
            //profile.region ="jgb" /*_repository.getRegionByRegionId(adminRegion.Regionid)*/;
            profile.regions = _repository.getRegions();
            profile.adminregion = adminRegion;
            return profile;
        }

        public byte[] getBytesForFile(int id)
        {
            var file = _repository.getRequestWiseFile(id);
            var filepath = "D:\\Projects\\HelloDOC\\MVC\\N-tier\\HalloDoc.Web\\wwwroot\\UploadedFiles\\" + Path.GetFileName(file.Filename);
            var bytes = System.IO.File.ReadAllBytes(filepath);
            return bytes;
        }

        public IQueryable<AdminDashboardTableModel> getDashboardTables(int id, int check)
        {
            IQueryable<AdminDashboardTableModel> tabledata;
            if (check == 0)
            {
                tabledata = _repository.getDashboardTablesWithoutcheck(id);

            }
            else
            {
                tabledata = _repository.getDashboardTables(id, check);
            }
            return tabledata;

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

            //request.Physicianid = 0;
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
        }

        public void sendAgreementMail(int id)
        {
            Requestclient client = _repository.getRequestClientById(id);
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var token = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Review Agreement";
            var message = "Review Your Agreement:https://localhost:44380/Admin/SendAgreement?token=" + token;

            Request req = _repository.getRequestByID(id);
            req.Ip = token;
            _repository.updateRequest(req);


            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);

            mailclient.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
        }

        public Requestclient sendAgreementService(string token)
        {
            var data = _repository.getRequestClientByToken(token);

            return _repository.getRequestClientById(data.Requestid);
        }

        public void SendEmail(int id, string[] filenames)
        {
            List<Requestwisefile> files = new();
            foreach (var filename in filenames)
            {
                files.Add(_repository.getRequesWiseFileList(id, filename));
            }


            Request request = _repository.getRequestByID(id);
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Documents of Request " /*+ request.Confirmationnumber?.ToUpper()*/;
            var message = "Find the Files uploaded for your request in below:";
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
            IQueryable<AdminDashboardTableModel> data = _repository.getDashboardTablesWithoutcheck(state);
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add headers
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

                // Add data rows
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

        public byte[] GetBytesForExportAll()
        {
            IQueryable<AdminDashboardTableModel> data = _repository.GetExportAllData();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add headers
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

                // Add data rows
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
        public static string ConvertToFormattedDate(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("MMM dd, yyyy");
            }
            return string.Empty;
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
            //DateOnly date = DateOnly.Parse(DateTime.Parse(patientData.Intdate + patientData.Strmonth + patientData.Intyear).ToString("yyyy-MM-dd"));
            AdminViewUpload model = new();
            model.patientData = patientData;
            model.confirmationDetail = _repository.getRequestByID(id);
            model.FileList = _repository.getPatientDocument(id);
            //model.DOB = date;
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
            //var model1 = _repository.getregionById(model.Regionid);
            model.Address1 = info.admin.Address1;
            model.Address2 = info.admin.Address2;
            model.City = info.admin.City;
            model.Zip = info.admin.Zip;
            model.Altphone = info.admin.Altphone;
            model.Modifieddate = DateTime.Now;
            //model1.Name = info.region.Name;
            _repository.updateAdmin(model);
            //_repository.saveadmindata(model1);          
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

        public void sendLinkAdminDashboard(AdminDashboardDataWithRegionModel info)
        {

            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = "binalmalaviya2002@gmail.com";
            var subject = "Make Your Appointment";
            var message = "You are invited to visit :https://localhost:44380/";



            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);

            //mailclient.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
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
            //model.Username = user.UserName;
            //model.Password = user.PasswordHash;
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
            _repository.addPhysician(model);
        }

        public Physician getPhysicianByID(int id)
        {
            return _repository.getPhysicianById(id);
        }

        public void ContactProviderSendMessage(Physician info)
        {
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = "binalmalaviya2002@gmail.com";
            //var receiver = info.Email;
            var subject = "Mail From Admin";
            var message = info.Adminnotes;



            var mailclient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            var mailMessage = new MailMessage(from: "tatva.dotnet.binalmalaviya@outlook.com", to: receiver, subject, message);

            mailclient.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
        }

        public AdminProviderModel getProviderByAdmin(int id)
        {
            Physician phy = _repository.getPhysicianById(id);
            Aspnetuser asp = _repository.getAspnetuserByID(phy.Aspnetuserid);
            AdminProviderModel model = new AdminProviderModel();
            model.physician = phy;
            //model.Username = user.UserName;
            //model.Password = user.PasswordHash;
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
            return model;
        }

        public void savePhysicianPassword(AdminProviderModel info)
        {
            var aspnetuser = _repository.getAspnetuserByID(info.aspnetuser.Id);
            aspnetuser.Passwordhash = info.Passwordhash;
            aspnetuser.Modifieddate = DateTime.Now;
            _repository.updateAspnetUser(aspnetuser);
        }

        public void savePhysicianInfo(AdminProviderModel model)
        {
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

            physician.Regionid = Int32.Parse(model.City);
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
    }
}
