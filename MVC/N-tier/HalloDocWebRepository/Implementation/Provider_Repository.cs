
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebRepo.Interface;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

namespace HalloDocWebRepo.Implementation
{
    public class Provider_Repository : IProvider_Repository
    {
        private readonly ApplicationContext _context;


        public Provider_Repository(ApplicationContext context)
        {
            _context = context;
        }

        public int getcount(int id, int phyid)
        {
            int[] status = new int[1];
            switch (id)
            {
                case 1:
                    status = new int[] { 1 };
                    break;
                case 2:
                    status = new int[] { 2 };
                    break;
                case 3:
                    status = new int[] { 4, 5 };
                    break;
                case 4:
                    status = new int[] { 6 };
                    break;


            }

            return _context.Requests.Where(i => status.Any(j => j == i.Status) && i.Physicianid == phyid).Count();
        }

        public List<Region> getRegions()
        {
            return _context.Regions.ToList();
        }
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check, int phyid)
        {
            int[] status = new int[1];
            switch (id)
            {
                case 1:
                    status = new int[] { 1 };
                    break;
                case 2:
                    status = new int[] { 2 };
                    break;
                case 3:
                    status = new int[] { 4, 5 };
                    break;
                case 4:
                    status = new int[] { 6 };
                    break;
                case 5:
                    status = new int[] { 3, 7, 8 };
                    break;
                case 6:
                    status = new int[] { 9 };
                    break;

            }
            var data1 = _context.Requests.Where(m => status.Any(j => j == m.Status) && m.Requesttypeid == check && m.Physicianid == phyid).Include(x => x.Requestclients).Include(x => x.Requesttype).ToList();
            List<AdminDashboardTableModel> model = new List<AdminDashboardTableModel>();
            data1?.ForEach(item =>
            {
                model.Add(new AdminDashboardTableModel
                {
                    Name = item.Requestclients.FirstOrDefault().Firstname + ' ' + item.Requestclients.FirstOrDefault().Lastname,
                    Requestor = item.Requesttype.Name + " , " + item.Firstname + " " + item.Lastname,
                    physician = item.Physicianid,
                    Dateofservice = item.Lastreservationdate,
                    Requesteddate = item.Createddate,
                    Phonenumber = item.Requestclients.FirstOrDefault().Phonenumber,
                    Email = item.Email,
                    Address = item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().City + " , " + item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().Zipcode,
                    Requestid = item.Requestid,
                    Notes = item.Requestclients.FirstOrDefault().Notes,
                    RequestTypeId = item.Requesttypeid,
                    RegionID = item.Requestclients.FirstOrDefault().Regionid,
                    RequestTypeName = item.Requesttype.Name,
                    RequestorPhonenumber = item.Phonenumber,
                    Status = item.Status

                });
            });
            return model;
        }
        public List<AdminDashboardTableModel> getDashboardTablesWithoutcheck(int id, int phyid)
        {
            int[] status = new int[1];
            switch (id)
            {
                case 1:
                    status = new int[] { 1 };
                    break;
                case 2:
                    status = new int[] { 2 };
                    break;
                case 3:
                    status = new int[] { 4, 5 };
                    break;
                case 4:
                    status = new int[] { 6 };
                    break;


            }
            var data1 = _context.Requests.Where(m => status.Any(j => j == m.Status) && m.Physicianid == phyid).Include(x => x.Requestclients).Include(x => x.Requesttype).ToList();
            List<AdminDashboardTableModel> model = new List<AdminDashboardTableModel>();
            data1?.ForEach(item =>
            {
                model.Add(new AdminDashboardTableModel
                {
                    Name = item.Requestclients.FirstOrDefault().Firstname + ' ' + item.Requestclients.FirstOrDefault().Lastname,
                    Requestor = item.Requesttype.Name + " , " + item.Firstname + " " + item.Lastname,
                    physician = item.Physicianid,
                    Dateofservice = item.Lastreservationdate,
                    Requesteddate = item.Createddate,
                    Phonenumber = item.Requestclients.FirstOrDefault().Phonenumber,
                    Email = item.Email,
                    Address = item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().City + " , " + item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().Zipcode,
                    Requestid = item.Requestid,
                    Notes = item.Requestclients.FirstOrDefault().Notes,
                    RequestTypeId = item.Requesttypeid,
                    RegionID = item.Requestclients.FirstOrDefault().Regionid,
                    RequestTypeName = item.Requesttype.Name,
                    RequestorPhonenumber = item.Phonenumber,
                    Status = item.Status
                });
            });
            return model;
        }
        public List<Physician> getPhysicianList()
        {
            return _context.Physicians.Where(m => m.Isdeleted == null).Include(e => e.Physiciannotifications).ToList();
        }

        public User getUserByEmail(string? email)
        {
            return _context.Users.FirstOrDefault(m => m.Email == email);
        }

        public Physician GetPhyByEmail(string email)
        {

            return _context.Physicians.FirstOrDefault(m => m.Email == email);
        }

        public void addRequestTable(Request req)
        {
            _context.Requests.Add(req);
            _context.SaveChanges();
        }

        public void adRequestClientTable(Requestclient reqclient)
        {
            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();
        }

        public void addRequestNotesTAble(Requestnote note)
        {
            _context.Requestnotes.Add(note);
            _context.SaveChanges();
        }

        public void addEmailLogTable(Emaillog emaillog)
        {
            _context.Emaillogs.Add(emaillog);
            _context.SaveChanges();
        }

        public void addSmsLogTable(Smslog smslog)
        {
            _context.Smslogs.Add(smslog);
            _context.SaveChanges();
        }

        public Request getRequestByReqID(int id)
        {
            return _context.Requests.FirstOrDefault(e => e.Requestid == id);
        }

        public void updateRequestTable(Request data)
        {
            _context.Requests.Update(data);
            _context.SaveChanges();
        }
        public Region getRegionByRegionId(int regionid)
        {
            return _context.Regions.FirstOrDefault(m => m.Regionid == regionid);
        }
        public Requestclient getRequestClientById(int? id)
        {
            return _context.Requestclients.FirstOrDefault(m => m.Requestid == id);
        }
        public List<Requeststatuslog> getRequestStatusLog(int id)
        {
            return _context.Requeststatuslogs.Where(m => m.Requestid == id).ToList();
        }
        public Requestnote getREquestNotes(int id)
        {
            return _context.Requestnotes.Where(x => x.Requestid == id).FirstOrDefault();
        }

        public Physician getPhysicianById(int? physicianid)
        {
            return _context.Physicians.FirstOrDefault(m => m.Physicianid == physicianid);
        }

        public void updateRequestNoteTable(Requestnote reqnotes)
        {
            _context.Requestnotes.Update(reqnotes);
            _context.SaveChanges();
        }
        public void addRequestStatusLogTable(Requeststatuslog statuslog)
        {
            _context.Requeststatuslogs.Add(statuslog);
            _context.SaveChanges();
        }
        public void addTokenRegister(TokenRegister tokenRegister)
        {
            _context.TokenRegisters.Add(tokenRegister);
            _context.SaveChanges();
        }
    }
}
