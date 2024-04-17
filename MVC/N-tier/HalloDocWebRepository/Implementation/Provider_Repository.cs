
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
                    Status = item.Status,
                    IsFinalize = item.Completedbyphysician
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
                    Status = item.Status,
                    IsFinalize = item.Completedbyphysician
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
        public List<Requestwisefile> getPatientDocument(int? id)
        {
            return _context.Requestwisefiles.Where(m => m.Requestid == id && m.Isdeleted == null).ToList();
        }
        public void updateRequestWiseFile(Requestwisefile file)
        {
            _context.Requestwisefiles.Update(file);
            _context.SaveChanges();
        }
        public Requestwisefile getRequestWiseFileByName(string filename, int id)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestid == id && m.Filename == filename && m.Isdeleted == null);
        }
        public void addRequestWiseFile(Requestwisefile reqclient)
        {
            _context.Requestwisefiles.Add(reqclient);
            _context.SaveChanges();
        }
        public Aspnetuser getAspnetuserByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
                return _context.Aspnetusers.FirstOrDefault(m => m.Email == email);
            else
                return null;
        }
        public List<Healthprofessional> getHealthProfessionalList()
        {
            return _context.Healthprofessionals.Where(m => m.Isdeleted == null).ToList();
        }

        public List<Healthprofessionaltype> getHealthProfessionalTypeList()
        {
            return _context.Healthprofessionaltypes.ToList();
        }

        public List<Healthprofessional> getHealthProfessional(int businessId)
        {
            return _context.Healthprofessionals.Where(m => m.Profession == businessId).ToList();
        }
        public Healthprofessional getHealthProfessionalDetail(int businessId)
        {

            return _context.Healthprofessionals.FirstOrDefault(m => m.Vendorid == businessId);
        }
        public void addOrderDetailTable(Orderdetail orderdetail)
        {
            _context.Orderdetails.Add(orderdetail);
            _context.SaveChanges();
        }
        public EncounterForm getEncounterTable(int id)
        {
            return _context.EncounterForms.FirstOrDefault(m => m.Requestid == id);
        }

        public void updateEncounterForm(EncounterForm model)
        {
            _context.EncounterForms.Update(model);
            _context.SaveChanges();
        }
        public Requestwisefile getRequestWiseFile(int id)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestwisefileid == id && m.Isdeleted == null);
        }

        public Requestwisefile getRequesWiseFileList(int id, string filename)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestid == id && m.Isdeleted == null && m.Filename == filename);
        }

        public void addEncounterTable(EncounterForm model)
        {
            _context.EncounterForms.Add(model);
            _context.SaveChanges();
        }

        public List<Requestwisefile> getRequestWiseFileList(int id)
        {
            return _context.Requestwisefiles.Where(e => e.Requestid == id).ToList();    
        }
        public Aspnetuser getAspnetuserByID(int? aspnetuserid)
        {
            return _context.Aspnetusers.FirstOrDefault(m => m.Id == aspnetuserid);
        }
        public List<Physicianregion> getPhysicianRegionByPhy(int id)
        {
            return _context.Physicianregions.Where(m => m.Physicianid == id).ToList();
        }

        public List<Admin> getAdminList()
        {
            return _context.Admins.ToList();
        }
        public void updateAspnetUser(Aspnetuser aspnetuser)
        {
            _context.Aspnetusers.Update(aspnetuser);
            _context.SaveChanges();
        }
    }
}
