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
            Physician physician=_repository.GetPhyByEmail(email);
            var viewmodel = new AdminDashboard
            {
                NewCount = _repository.getcount(1,physician.Physicianid),
                PendingCount = _repository.getcount(2,physician.Physicianid),
                ActiveCount = _repository.getcount(3,physician.Physicianid),
                ConcludeCount = _repository.getcount(4,physician.Physicianid),
                TocloseCount = _repository.getcount(5,physician.Physicianid),
                UnpaidCount = _repository.getcount(6,physician.Physicianid),
            };
            return viewmodel;
        }
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check, string email)
        {
            Physician physician = _repository.GetPhyByEmail(email);
            List<AdminDashboardTableModel> tabledata;
            if (check == 0)
            {
                return _repository.getDashboardTablesWithoutcheck(id,physician.Physicianid);
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
            //mailclient.SendMailAsync(mailMessage);
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
                Notes = phy.Firstname + " " + phy.Lastname +"has Accepted The request",
                Status = 1,
                Createddate = DateTime.Now,
                Physicianid = phy.Physicianid,
                
            };
            _repository.addRequestStatusLogTable(statuslog);
        }
        public ViewCaseModel ViewCaseModel(int id)
        {
            Requestclient data = _repository.getRequestClientById(id);
            Region region = _repository.getRegionByRegionId(data.Regionid);
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
    }
}

