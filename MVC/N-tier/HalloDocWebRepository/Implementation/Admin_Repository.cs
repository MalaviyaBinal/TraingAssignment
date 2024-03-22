
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebRepo.Interface;
using Microsoft.CodeAnalysis;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HalloDocWebRepo.Implementation
{
    public class Admin_Repository : IAdmin_Repository
    {
        private readonly ApplicationContext _context;

        public Admin_Repository()
        {
        }

        public Admin_Repository(ApplicationContext context)
        {
            _context = context;
        }

        public void addAspnetuserTable(Aspnetuser aspuser)
        {
            _context.Aspnetusers.Add(aspuser);
            _context.SaveChanges();
        }

        public void addRequestNotesTAble(Requestnote note)
        {
            _context.Requestnotes.Add(note);
            _context.SaveChanges();
        }

        public void addRequestStatusLogTable(Requeststatuslog statuslog)
        {
            _context.Requeststatuslogs.Add(statuslog);
            _context.SaveChanges();
        }

        public void addRequestTable(Request req)
        {
            _context.Requests.Add(req);
            _context.SaveChanges();
        }

        public void addRequestWiseFile(Requestwisefile reqclient)
        {
            _context.Requestwisefiles.Add(reqclient);
            _context.SaveChanges();
        }

        public void addUserTable(User users)
        {
            _context.Users.Add(users);

            _context.SaveChanges();
        }

        public void adRequestClientTable(Requestclient reqclient)
        {
            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();
        }

        public Aspnetuser getAspnetuserByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
                return _context.Aspnetusers.FirstOrDefault(m => m.Email == email);
            else
                return null;
        }

        public Casetag getCasetag(int reasonid)
        {
            return _context.Casetags.FirstOrDefault(c => c.Casetagid == reasonid);
        }

        public int getcount(int id)
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
            
            return _context.Requests.Where(i => status.Any(j => j == i.Status)).Count();
        }

        public IQueryable<AdminDashboardTableModel> getDashboardTables(int id, int check)
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
            return from rc in _context.Requestclients
                   join r in _context.Requests on rc.Requestid equals r.Requestid
                   //join phy in _context.Physicians on r.Physicianid equals phy.Physicianid
                   join rt in _context.Requesttypes on r.Requesttypeid equals rt.Requesttypeid
                   join reg in _context.Regions on rc.Regionid equals reg.Regionid
                   where status.Any(j => j == r.Status) && r.Requesttypeid == check
                   orderby r.Createddate descending
                   select new AdminDashboardTableModel
                   {
                       Name = rc.Firstname + ' ' + rc.Lastname,
                       Requestor = rt.Name + " , " + r.Firstname + ' ' + r.Lastname,
                       physician = r.Physicianid,
                       Dateofservice = r.Lastreservationdate,
                       Requesteddate = r.Createddate,
                       Phonenumber = rc.Phonenumber,
                       Email = r.Email,
                       Address = rc.Street + " , " + rc.City + " , " + rc.Street + " , " + rc.Zipcode,
                       Requestid = r.Requestid,
                       Notes = rc.Notes,
                       RequestTypeId = r.Requesttypeid,
                       RegionID = rc.Regionid,
                       RequestTypeName = rt.Name
                   };
        }

        public IQueryable<AdminDashboardTableModel> getDashboardTablesWithoutcheck(int id)
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
                    status = new int[] { 4,5 };
                    break;
                case 4:
                    status = new int[] { 6 };
                    break;
                case 5:
                    status = new int[] { 3,7,8 };
                    break;
                case 6:
                    status = new int[] { 9 };
                    break;

            }
            var model = from rc in _context.Requestclients
                        join r in _context.Requests on rc.Requestid equals r.Requestid
                        //join phy in _context.Physicians on r.Physicianid equals phy.Physicianid
                        join rt in _context.Requesttypes on r.Requesttypeid equals rt.Requesttypeid
                        join reg in _context.Regions on rc.Regionid equals reg.Regionid
                        where status.Any(j => j == r.Status)
                        select new AdminDashboardTableModel
                        {
                            Name = rc.Firstname + ' ' + rc.Lastname,
                            Requestor = rt.Name + " , " + r.Firstname + ' ' + r.Lastname,
                            physician = r.Physicianid,
                            Dateofservice = r.Lastreservationdate,
                            Requesteddate = r.Createddate,
                            Phonenumber = rc.Phonenumber,
                            Email = r.Email,
                            Address = rc.Street + " , " + rc.City + " , " + rc.Street + " , " + rc.Zipcode,
                            Requestid = r.Requestid,
                            Notes = rc.Notes,
                            RequestTypeId = r.Requesttypeid,
                            RegionID = rc.Regionid,
                            RequestTypeName = rt.Name,
                            RequestorPhonenumber = r.Phonenumber,
                            Status = r.Status
                        };
            return model;
        }
        //where(i => status.Any(j => j == r.Status))
        public List<Healthprofessional> getHealthProfessional(int businessId)
        {
            return _context.Healthprofessionals.Where(m => m.Profession == businessId).ToList();
        }

        public List<Healthprofessional> getHealthProfessionalList()
        {
            return _context.Healthprofessionals.ToList();
        }

        public List<Healthprofessionaltype> getHealthProfessionalTypeList()
        {
            return _context.Healthprofessionaltypes.ToList(); 
        }
        public List<Healthprofessionaltype> getHealthProfessionalType(int profId)
        {
            return _context.Healthprofessionaltypes.Where(m => m.Healthprofessionalid == profId).ToList(); 
        }

        public List<Requestwisefile> getPatientDocument(int? id)
        {
            return _context.Requestwisefiles.Where(m => m.Requestid == id && m.Isdeleted == null).ToList();
        }

        public Physician getPhysicianById(int? physicianid)
        {
           return _context.Physicians.FirstOrDefault(m => m.Physicianid == physicianid);
        }

        public List<Physician> getPhysicianList()
        {
            return _context.Physicians.ToList();
        }

        public List<Physician> getPhysicianListByregion(int regid)
        {
            return _context.Physicians.Where(m => m.Regionid == regid).ToList();
        }

        public Region getRegionByRegionId(int regionid)
        {
            return _context.Regions.FirstOrDefault(m => m.Regionid == regionid);
        }

        public List<Region> getRegions()
        {
            return _context.Regions.ToList();
        }

        public Request getRequestByID(int? id)
        {
            return _context.Requests.FirstOrDefault(r => r.Requestid == id);
        }

        public Requestclient getRequestClientById(int? id)
        {
            return _context.Requestclients.FirstOrDefault(m => m.Requestid == id);
        }

        public Requestnote getREquestNotes(int id)
        {
            return _context.Requestnotes.Where(x => x.Requestid == id).FirstOrDefault();
        }

        public List<Requeststatuslog> getRequestStatusLog(int id)
        {
           return  _context.Requeststatuslogs.Where(m => m.Requestid == id).ToList();
        }

        public Requestwisefile getRequestWiseFile(int id)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestwisefileid == id && m.Isdeleted == null);
        }

        public Requestwisefile getRequestWiseFileByName(string filename ,int id)
        {
           return _context.Requestwisefiles.FirstOrDefault(m => m.Requestid == id && m.Filename == filename && m.Isdeleted == null);
        }

        public Requestwisefile getRequesWiseFileList(int id,string filename)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestid == id && m.Isdeleted == null && m.Filename == filename);
        }

        public User getUserByEmail(string? email)
        {
            return _context.Users.FirstOrDefault(m => m.Email == email);
        }

        public void updateRequest(Request request)
        {
            _context.Requests.Update(request);
            _context.SaveChanges();
        }

        public void updateRequestNote(Requestnote reqnotes)
        {
            _context.Requestnotes.Update(reqnotes);
            _context.SaveChanges();
        }

        public void updateRequestWiseFile(Requestwisefile file)
        {
            _context.Requestwisefiles.Update(file);
            _context.SaveChanges();
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

        public void addBlockRequestTable(Blockrequest blockrequest)
        {
            _context.Blockrequests.Add(blockrequest);
            _context.SaveChanges();
        }

        public Admin getAdminTableDataByEmail(string? email)
        {
            return _context.Admins.FirstOrDefault(a => a.Email == email);
        }

        public List<Adminregion> getAdminRegionByAdminId(int adminid)
        {
            return _context.Adminregions.Where(r => r.Adminid == adminid).ToList();
        }

        public Request getRequestClientByToken(string token)
        {
            return _context.Requests.FirstOrDefault(m => m.Ip == token);
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

        public IQueryable<AdminDashboardTableModel> GetExportAllData()
        {
            return from rc in _context.Requestclients
                   join r in _context.Requests on rc.Requestid equals r.Requestid
                   join phy in _context.Physicians on r.Physicianid equals phy.Physicianid
                   join rt in _context.Requesttypes on r.Requesttypeid equals rt.Requesttypeid
                   join reg in _context.Regions on rc.Regionid equals reg.Regionid                  
                   orderby r.Createddate descending
                   select new AdminDashboardTableModel
                   {
                       Name = rc.Firstname + ' ' + rc.Lastname,
                       Requestor = rt.Name + " , " + r.Firstname + ' ' + r.Lastname,
                       physician = r.Physicianid,
                       Dateofservice = r.Lastreservationdate,
                       Requesteddate = r.Createddate,
                       Phonenumber = rc.Phonenumber,
                       Email = r.Email,
                       Address = rc.Street + " , " + rc.City + " , " + rc.Street + " , " + rc.Zipcode,
                       Requestid = r.Requestid,
                       Notes = rc.Notes,
                       RequestTypeId = r.Requesttypeid,
                       RegionID = rc.Regionid,
                       RequestTypeName = rt.Name,
                       RequestorPhonenumber = r.Phonenumber,
                       Status = r.Status
                   };
        }

        public void updateRewuestClient(Requestclient reqclient)
        {
            _context.Requestclients.Update(reqclient);
            _context.SaveChanges();
        }

        public Admin getAdminByAdminId(Admin admin)
        {
            return _context.Admins.FirstOrDefault(m => m.Adminid == admin.Adminid);
        }

        public void updateAdmin(Admin model)
        {
           _context.Admins.Update(model);
            _context.SaveChanges();
        }

        public void addAdminReg(Adminregion ar)
        {
            _context.Adminregions.Add(ar);
            _context.SaveChanges();
        }
        public void RemoveAdminReg(Adminregion ar)
        {
            var data = _context.Adminregions.FirstOrDefault(x => x.Adminid == ar.Adminid && x.Regionid == ar.Regionid);
            _context.Adminregions.Remove(data);
            _context.SaveChanges();
        }

        public void addPhysician(Physician model)
        {
            _context.Physicians.Add(model);
            _context.SaveChanges();
        }
    }
}
