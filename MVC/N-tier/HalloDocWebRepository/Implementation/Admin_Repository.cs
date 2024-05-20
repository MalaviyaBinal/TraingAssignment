
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebRepo.Interface;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;

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

        public List<AdminDashboardTableModel> getDashboardTables(int id, int check)
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
            var data1 = _context.Requests.Where(m => status.Any(j => j == m.Status) && m.Requesttypeid == check).Include(x => x.Requestclients).Include(x => x.Requesttype).Include(e => e.Requestnotes).Include(e => e.Physician).ToList();
            List<AdminDashboardTableModel> model = new List<AdminDashboardTableModel>();
            data1?.ForEach(item =>
            {
                model.Add(new AdminDashboardTableModel
                {
                    Name = item.Requestclients.Count != 0 ? item.Requestclients.FirstOrDefault().Firstname + ' ' + item.Requestclients.FirstOrDefault().Lastname : "---",
                    Requestor = item.Requesttype.Name != null ? item.Requesttype.Name + " , " + item.Firstname + " " + item.Lastname : "---",
                    physician = item.Physician != null ? item.Physician.Firstname + "  " + item.Physician.Lastname : "---",
                    Dateofservice = item.Lastreservationdate != null ? item.Lastreservationdate : null,
                    DOB = item.Requestclients.Count != 0 ? item.Requestclients.FirstOrDefault().Intdate.ToString() + "/" + item.Requestclients.FirstOrDefault().Strmonth + "/" + item.Requestclients.First().Intyear.ToString() : "---",
                    Requesteddate = item.Createddate!=null ? item.Createddate : null,
                    Phonenumber = item.Requestclients.Count != 0 ? item.Requestclients.FirstOrDefault().Phonenumber : "---",
                    Email = item.Email != null ? item.Email : null,
                    Address = item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().City + " , " + item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().Zipcode,
                    Requestid = item.Requestid != null ? item.Requestid : null,
                    Notes = item.Requestnotes.Count != 0 ? "Admin note:" + item.Requestnotes.FirstOrDefault().Adminnotes != null ? item.Requestnotes.FirstOrDefault().Adminnotes : "---" + "\nPhysician notes:" + item.Requestnotes.FirstOrDefault().Physiciannotes != null ? item.Requestnotes.FirstOrDefault().Physiciannotes : "---" : "--",

                    RequestTypeId = item.Requesttypeid,
                    RegionID = item.Requestclients.FirstOrDefault().Regionid,
                    RequestTypeName = item.Requesttype != null ? item.Requesttype.Name : null,
                    RequestorPhonenumber = item.Phonenumber != null ? item.Phonenumber : null,
                    Status = item.Status,
                    IsFinalize = item.Completedbyphysician != null ? item.Completedbyphysician : new BitArray(1, false)
                });
            });
            return model;
        }

        public List<AdminDashboardTableModel> getDashboardTablesWithoutcheck(int id)
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
            var data1 = _context.Requests.Where(m => status.Any(j => j == m.Status)).Include(x => x.Requestclients).Include(x => x.Requesttype).Include(e => e.Requestnotes).Include(e=> e.Physician).ToList();
            List<AdminDashboardTableModel> model = new List<AdminDashboardTableModel>();
            
            data1?.ForEach(item =>
            {
                model.Add(new AdminDashboardTableModel
                {
                    Name = item.Requestclients.Count != 0 ? item.Requestclients.FirstOrDefault().Firstname + ' ' + item.Requestclients.FirstOrDefault().Lastname : "---",
                    Requestor = item.Requesttype.Name != null ? item.Requesttype.Name + " , " + item.Firstname + " " + item.Lastname : "---",
                    physician = item.Physician != null ? item.Physician.Firstname + "  " + item.Physician.Lastname : "---",
                    physicianId = item.Physician != null ? item.Physician.Physicianid : 1,
                    Dateofservice = item.Lastreservationdate != null ? item.Lastreservationdate : null,
                    DOB = item.Requestclients.Count != 0 ? item.Requestclients.FirstOrDefault().Intdate.ToString() + "/" + item.Requestclients.FirstOrDefault().Strmonth + "/" + item.Requestclients.First().Intyear.ToString() : "---",
                    Requesteddate = item.Createddate != null ? item.Createddate : null,
                    Phonenumber = item.Requestclients.Count != 0 ? item.Requestclients.FirstOrDefault().Phonenumber : "---",
                    Email = item.Email != null ? item.Email : null,
                    Address = item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().City + " , " + item.Requestclients.FirstOrDefault().Street + " , " + item.Requestclients.FirstOrDefault().Zipcode,
                    Requestid = item.Requestid != null ? item.Requestid : null,
                    Notes = item.Requestnotes.Count != 0 ?  item.Requestnotes.FirstOrDefault().Adminnotes != null ? "Admin note:"+ item.Requestnotes.FirstOrDefault().Adminnotes : "---" +  item.Requestnotes.FirstOrDefault().Physiciannotes != null ? "\nPhysician notes:" + item.Requestnotes.FirstOrDefault().Physiciannotes : "---" : "--",

                    RequestTypeId = item.Requesttypeid,
                    RegionID = item.Requestclients.FirstOrDefault().Regionid,
                    RequestTypeName = item.Requesttype != null ? item.Requesttype.Name : null,
                    RequestorPhonenumber = item.Phonenumber != null ? item.Phonenumber : null,
                    Status = item.Status,
                    IsFinalize = item.Completedbyphysician != null ? item.Completedbyphysician : new BitArray(1, false)
                });
            });
            return model;
        }
        //where(i => status.Any(j => j == r.Status))
        public List<Healthprofessional> getHealthProfessional(int businessId)
        {
            return _context.Healthprofessionals.Where(m => m.Profession == businessId).ToList();
        }

        public List<Healthprofessionaltype> getVenderDetail(int reg, string searchstr)
        {
            if (reg == 0)
            {
                if (searchstr == null)
                    return _context.Healthprofessionaltypes.Include(e => e.Healthprofessionals).ToList();
                else
                    return _context.Healthprofessionaltypes.Include(e => e.Healthprofessionals.Where(f => f.Vendorname.ToLower().Contains(searchstr.ToLower()))).ToList();
            }
            else
            {
                if (searchstr == null)
                    return _context.Healthprofessionaltypes.Include(e => e.Healthprofessionals.Where(f => f.Profession == reg)).ToList();
                else
                    return _context.Healthprofessionaltypes.Include(e => e.Healthprofessionals.Where(f => f.Profession == reg && f.Vendorname.ToLower().Contains(searchstr.ToLower()))).ToList();
            }
        }

        public List<Healthprofessional> getHealthProfessionalList()
        {
            return _context.Healthprofessionals.Where(m => m.Isdeleted == null).ToList();
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
            return _context.Physicians.Where(m => m.Isdeleted == null).Include(e => e.Physiciannotifications).ToList();
        }

        public List<Physician> getPhysicianListByregion(int regid)
        {
            return _context.Physicians.Where(m => m.Regionid == regid && m.Isdeleted == null).ToList();
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
            return _context.Requeststatuslogs.Where(m => m.Requestid == id).ToList();
        }

        public Requestwisefile getRequestWiseFile(int id)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestwisefileid == id && m.Isdeleted == null);
        }

        public Requestwisefile getRequestWiseFileByName(string filename, int id)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestid == id && m.Filename == filename && m.Isdeleted == null);
        }

        public Requestwisefile getRequesWiseFileList(int id, string filename)
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

        public TokenRegister getRequestClientByToken(string token)
        {
            return _context.TokenRegisters.FirstOrDefault(m => m.TokenValue == token);
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
        public void addEncounterTable(EncounterForm model)
        {
            _context.EncounterForms.Add(model);
            _context.SaveChanges();
        }
        public List<AdminDashboardTableModel> GetExportAllData()
        {
            var data1 = _context.Requests.Where(r => r.Status != 10).Include(x => x.Requestclients).Include(x => x.Requesttype).ToList();
            List<AdminDashboardTableModel> model = new List<AdminDashboardTableModel>();
            data1?.ForEach(item =>
            {
                model.Add(new AdminDashboardTableModel
                {
                    Name = item.Requestclients.FirstOrDefault().Firstname + ' ' + item.Requestclients.FirstOrDefault().Lastname,
                    Requestor = item.Requesttype.Name + " , " + item.Firstname + " " + item.Lastname,
                    physician = item.Physician != null ? item.Physician.Firstname + "  " + item.Physician.Lastname : "---",
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

        public void updateRewuestClient(Requestclient reqclient)
        {
            _context.Requestclients.Update(reqclient);
            _context.SaveChanges();
        }

        public Admin getAdminByAdminId(Admin admin)
        {
            return _context.Admins.FirstOrDefault(m => m.Adminid == admin.Adminid);
        }
        public Admin getAdminByAdminID(int admin)
        {
            return _context.Admins.FirstOrDefault(m => m.Adminid == admin);
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

        public Aspnetuser getAspnetuserByID(int? aspnetuserid)
        {
            return _context.Aspnetusers.FirstOrDefault(m => m.Id == aspnetuserid);
        }

        public void updateAspnetUser(Aspnetuser aspnetuser)
        {
            _context.Aspnetusers.Update(aspnetuser);
            _context.SaveChanges();
        }

        public void updatePhysician(Physician physician)
        {
            _context.Physicians.Update(physician);
            _context.SaveChanges();
        }
        public List<Menu> getmenudataof()
        {
            return _context.Menus.ToList();
        }

        public List<Menu> getMenuListWithCheck(int check)
        {
            return _context.Menus.Where(m => m.Accounttype == check).ToList();
        }
        public void saveRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }
        public void saveRoleMenu(Rolemenu role)
        {
            _context.Rolemenus.Add(role);
            _context.SaveChanges();
        }

        public List<Role> getRoleList()
        {
            return _context.Roles.Where(m => m.Isdeleted == new BitArray(1, false)).ToList();
        }

        public Role getRoleByID(int id)
        {
            return _context.Roles.FirstOrDefault(m => m.Roleid == id);
        }

        public List<Rolemenu> getSelectedRoleMenuByRoleID(int id)
        {
            return _context.Rolemenus.Where(m => m.Roleid == id).ToList();
        }

        public void removeAllRoleMenu(int roleId)
        {
            var meunus = _context.Rolemenus.Where(m => m.Roleid == roleId).ToList();
            foreach (var m in meunus)
            {
                _context.Rolemenus.Remove(m);
            }
            _context.SaveChanges();
        }

        public void addAdminTable(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }
        public List<Role> getRolesOfAdmin()
        {
            return _context.Roles.Where(m => m.Accounttype == 1).ToList();
        }
        public List<Role> getRolesOfProvider()
        {
            return _context.Roles.Where(m => m.Accounttype == 2).ToList();
        }
       
        public List<Aspnetuser> getAspnetUserList(int roleid)
        {
            switch (roleid)
            {
                case 3:
                    return _context.Aspnetusers.Include(e => e.Admins).Include(e => e.Physicians).Where(m => m.Role == 3).ToList();

                case 1:
                    return _context.Aspnetusers.Include(e => e.Admins).Include(e => e.Physicians).Where(m => m.Role == 1).ToList();

                default:
                    return _context.Aspnetusers.Include(e => e.Admins).Include(e => e.Physicians).Where(m => m.Role == 1 || m.Role == 3).ToList();

            }

        }

        public List<Admin> getAdminList()
        {
            return _context.Admins.ToList();
        }

        public List<Physicianregion> getPhysicianRegionByPhy(int id)
        {
            return _context.Physicianregions.Where(m => m.Physicianid == id).ToList();
        }

        public void removeAllPhysicianRegion(int physicianid)
        {
            var reg = _context.Physicianregions.Where(m => m.Physicianid == physicianid).ToList();
            reg.ForEach(item =>
            {
                _context.Physicianregions.Remove(item);
            });
            _context.SaveChanges();
        }

        public void addAllPhysicianRegion(List<int>? selectedReg, int id)
        {
            selectedReg.ForEach(item =>
            {
                Physicianregion pr = new() { Physicianid = id, Regionid = item };
                _context.Physicianregions.Add(pr);
            });
            _context.SaveChanges();

        }

        public List<Physicianlocation> getPhysicianLocationList()
        {
            return _context.Physicianlocations.ToList();
        }

        public void updateHealthProfessionalTable(Healthprofessional vender)
        {
            _context.Healthprofessionals.Update(vender);
            _context.SaveChanges();
        }

        public Role getAccessroleById(int id)
        {
            return _context.Roles.FirstOrDefault(m => m.Roleid == id);
        }

        public void updateRoleTable(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges(true);
        }

        public List<Blockrequest> getBlockData(string searchstr, DateTime date, string email, string mobile)
        {
            IQueryable<Blockrequest> tabledata = _context.Blockrequests.Include(e => e.Request).ThenInclude(m => m.Requestclients);


            if (date != DateTime.MinValue && tabledata != null)
            {

                tabledata = tabledata.Where(x => DateOnly.FromDateTime(x.Createddate.Value) == DateOnly.FromDateTime(date));

            }
            if (!string.IsNullOrEmpty(email) && tabledata != null)
            {
                tabledata = tabledata.Where(e => e.Email.ToLower().Contains(email.ToLower()));
            }
            if (!string.IsNullOrEmpty(mobile) && tabledata != null)
            {
                tabledata = tabledata.Where(e => e.Phonenumber.Contains(mobile));
            }
            if (!string.IsNullOrEmpty(searchstr) && tabledata != null)
            {
                tabledata = tabledata.Where(e => e.Request.Requestclients.First().Firstname.ToLower().Contains(searchstr.ToLower()) || e.Request.Requestclients.First().Lastname.ToLower().Contains(searchstr.ToLower()));
            }

            return tabledata.ToList();
        }

        public List<Requesttype> getRequestTypeList()
        {
            return _context.Requesttypes.ToList();
        }

        public List<Requestclient> getRequestClientList()
        {
            return _context.Requestclients.Include(e => e.Request).Where(e => e.Request.Isdeleted == null).ToList();
        }

        public List<Requestnote> getREquestNotesList()
        {
            return _context.Requestnotes.ToList();
        }

        public void addSmsLogTable(Smslog smslog)
        {
            _context.Smslogs.Add(smslog);
            _context.SaveChanges();
        }

        public void addEmailLogTable(Emaillog emaillog)
        {

            _context.Emaillogs.Add(emaillog);
            _context.SaveChanges();
        }

        public List<Smslog> GetAllSmsLogs(int roleid, string name, string mobile, string createdDate, string sentDate)
        {
            IQueryable<Smslog> query = _context.Smslogs;

            if (roleid != 0)
            {
                query = query.Where(e => e.Roleid == roleid);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e =>
                    e.Smstemplate.Contains("Hello ") &&
                    e.Smstemplate.Contains(",") &&
                    e.Smstemplate.Substring(e.Smstemplate.IndexOf("Hello ") + "Hello ".Length, e.Smstemplate.IndexOf(",") - e.Smstemplate.IndexOf("Hello ") - "Hello ".Length)
                    .ToLower().Contains(name.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(e => e.Mobilenumber.ToLower().Contains(mobile.ToLower()));
            }

            if (!string.IsNullOrEmpty(createdDate))
            {
                DateTime parsedCreatedDate = DateTime.Parse(createdDate).Date; // Extract the date part
                query = query.Where(e => e.Createdate.Date == parsedCreatedDate);
            }

            if (!string.IsNullOrEmpty(sentDate))
            {
                DateTime parsedSentDate = DateTime.Parse(sentDate);
                query = query.Where(e => e.Sentdate == parsedSentDate);
            }

            return query.ToList();
        }

        public List<Emaillog> GetAllEmailLogs(int roleid, string name, string email, string createdDate, string sentDate)
        {
            IQueryable<Emaillog> query = _context.Emaillogs;

            if (roleid != 0)
            {
                query = query.Where(e => e.Roleid == roleid);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e =>
                    e.Emailtemplate.Contains("Hello ") &&
                    e.Emailtemplate.Contains(",") &&
                    e.Emailtemplate.Substring(e.Emailtemplate.IndexOf("Hello ") + "Hello ".Length, e.Emailtemplate.IndexOf(",") - e.Emailtemplate.IndexOf("Hello ") - "Hello ".Length)
                    .ToLower().Contains(name.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(e => e.Emailid.ToLower().Contains(email.ToLower()));
            }

            if (!string.IsNullOrEmpty(createdDate))
            {
                DateTime parsedCreatedDate = DateTime.Parse(createdDate).Date; // Extract the date part
                query = query.Where(e => e.Createdate.Date == parsedCreatedDate);
            }

            if (!string.IsNullOrEmpty(sentDate))
            {
                DateTime parsedSentDate = DateTime.Parse(sentDate).Date;
                query = query.Where(e => e.Sentdate == parsedSentDate);
            }

            return query.ToList();
        }

        public void AddShiftTable(Shift shift)
        {
            _context.Shifts.Add(shift);
            _context.SaveChanges();
        }

        public List<ShiftDetailsModel> getshiftDetail(int reg)
        {
            if (reg == 0)
                return _context.Shiftdetails.Where(e => e.Isdeleted != new BitArray(1, true))
                   .Include(e => e.Shift)
                   .ThenInclude(e => e.Physician).ThenInclude(e => e.Region).Select(e =>
                       new ShiftDetailsModel
                       {
                           PhysicianName = e.Shift.Physician.Firstname + " " + e.Shift.Physician.Lastname,
                           Physicianid = e.Shift.Physician.Physicianid,
                           RegionName = e.Shift.Physician.Region.Name,
                           Status = e.Status,
                           Starttime = e.Starttime,
                           Endtime = e.Endtime,
                           Shiftdate = e.Shiftdate,
                           Shiftdetailid = e.Shiftdetailid,
                       }
                   ).ToList();
            else
                return _context.Shiftdetails.Where(e => e.Isdeleted != new BitArray(1, true) && e.Regionid == reg)
                .Include(e => e.Shift)
                .ThenInclude(e => e.Physician).ThenInclude(e => e.Region).Select(e =>
                    new ShiftDetailsModel
                    {
                        PhysicianName = e.Shift.Physician.Firstname + " " + e.Shift.Physician.Lastname,
                        Physicianid = e.Shift.Physician.Physicianid,
                        RegionName = e.Shift.Physician.Region.Name,
                        Status = e.Status,
                        Starttime = e.Starttime,
                        Endtime = e.Endtime,
                        Shiftdate = e.Shiftdate,
                        Shiftdetailid = e.Shiftdetailid,
                    }
                ).ToList();

        }

        public void AddShiftDetails(List<Shiftdetail> shiftdetails)
        {
            shiftdetails.ForEach(item =>
            {
                _context.Shiftdetails.Add(item);
            });
            _context.SaveChanges();

        }

        public Blockrequest getBlockRequestById(int id)
        {
            return _context.Blockrequests.FirstOrDefault(m => m.Blockrequestid == id);
        }

        public void updateBlockRequest(Blockrequest req)
        {
            _context.Blockrequests.Update(req);
            _context.SaveChanges();
        }
        public IQueryable<PatientHistoryTable> GetPatientHistoryTable(string? fname, string? lname, string? email, string? phone)
        {
            IQueryable<PatientHistoryTable> tabledata = _context.Users.Select(u =>
                                                         new PatientHistoryTable
                                                         {
                                                             aspId = u.Userid,
                                                             Firstname = u.Firstname ?? "-",
                                                             Lastname = u.Lastname ?? "-",
                                                             Email = u.Email ?? "-",
                                                             phone = u.Mobile ?? "-",
                                                             Address = (u.Street ?? "") + " , " + (u.City ?? "") + " , " + (u.State ?? ""),
                                                         }
                                                        );


            if (!string.IsNullOrEmpty(fname))
            {
                tabledata = tabledata.Where(e => e.Firstname.ToLower().Contains(fname.ToLower()));
            }
            if (!string.IsNullOrEmpty(lname))
            {
                tabledata = tabledata.Where(e => e.Lastname.ToLower().Contains(lname.ToLower()));
            }
            if (!string.IsNullOrEmpty(email))
            {
                tabledata = tabledata.Where(e => e.Email.ToLower().Contains(email.ToLower()));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                tabledata = tabledata.Where(e => e.phone.Contains(phone));
            }
            return tabledata;
        }

        public List<Request> GetAllRequestsByAid(int id)
        {

            return _context.Requests.Where(r => r.Userid == id && r.Isdeleted != new BitArray(1, true) && r.Status != 11).ToList();

        }

        public int GetNumberOfDocsByRid(int requestid)
        {
            return _context.Requestwisefiles.Where(f => f.Requestid == requestid).Count();
        }

        public string? GetStatus(short status)
        {
            switch (status)
            {
                case 1:
                    return "Unassigned";

                case 2:
                    return "Accepted";

                case 3:
                    return "MDEnRoute";

                case 4:
                    return "MDEnRoute";

                case 5:
                    return "Conclude";

                case 6:
                    return "Cancelled";

                case 7:
                    return "CancelledByPatient";

                case 8:
                    return "Closed";

                default:
                    return "Unpaid";


            }
        }
        public Shiftdetail getShiftDetailByShiftDetailId(int id)
        {
            return _context.Shiftdetails.FirstOrDefault(e => e.Shiftdetailid == id);
        }

        public Shift getShiftByID(int shiftid)
        {
            return _context.Shifts.FirstOrDefault(e => e.Shiftid == shiftid);
        }

        public void UpdateShiftDetailTable(Shiftdetail sd)
        {
            _context.Shiftdetails.Update(sd);
            _context.SaveChanges();
        }

        public List<Shiftdetail> getShiftDetailByRegion(int reg)
        {
            if (reg != 0)
                return _context.Shiftdetails.Where(e => e.Status == 0 && e.Isdeleted == new BitArray(1, false) && e.Regionid == reg).Include(e => e.Shift).ThenInclude(e => e.Physician).ThenInclude(e => e.Region).ToList();
            else
                return _context.Shiftdetails.Where(e => e.Status == 0 && e.Isdeleted == new BitArray(1, false)).Include(e => e.Shift).ThenInclude(e => e.Physician).ThenInclude(e => e.Region).ToList();
        }

        public List<Physician> getPhysicianOnCallList(int reg)
        {
            var time = TimeOnly.FromDateTime(DateTime.Now);
            if (reg != 0)
                return _context.Physicians.Where(e => e.Regionid == reg).Include(p => p.Shifts)
                    .ThenInclude(p => p.Shiftdetails.Where(e => e.Shiftdate == DateOnly.FromDateTime(DateTime.Now) && e.Starttime <= time && e.Endtime >= time))
                    .OrderBy(e => e.Physicianid).ToList();
            else
                return _context.Physicians.Include(p => p.Shifts)
                    .ThenInclude(p => p.Shiftdetails.Where(e => e.Shiftdate == DateOnly.FromDateTime(DateTime.Now) && e.Starttime <= time && e.Endtime >= time))
                    .OrderBy(e => e.Physicianid).ToList();
        }

        public List<Physiciannotification> getSelectedPhyNotification(int[] ints)
        {
            return _context.Physiciannotifications.Where(e => ints.Any(f => f == e.Pysicianid)).ToList();
        }

        public List<Physiciannotification> getNotSelectedPhyNotification(int[] ints)
        {
            var mod = _context.Physiciannotifications.Where(e => !ints.Any(f => f != e.Pysicianid)).ToList();
            return mod;
        }


        public void updatePhyNotificationTable(Physiciannotification notification)
        {
            _context.Physiciannotifications.Update(notification);
            _context.SaveChanges();
        }

        public Physiciannotification getPhyNotificationByPhyID(int pysicianid)
        {
            return _context.Physiciannotifications.FirstOrDefault(e => e.Pysicianid == pysicianid);
        }
        public int[] GetUnscheduledPhysicanID()
        {
            var phy = _context.Shifts.ToList();
            int[] ints = new int[phy.Count];
            int t = 0;
            foreach (var e in phy)
            {
                ints[t++] = e.Physicianid;
            }
            return ints;
        }

        public List<Physician> getUnscheduledPhysicianList(int[] data)
        {
            return _context.Physicians.Where(e => !data.Any(f => f == e.Physicianid)).ToList();
        }

        public void addHealthProfessionTable(Healthprofessional business)
        {
            _context.Healthprofessionals.Add(business);
            _context.SaveChanges();
        }

        public void addTokenRegister(TokenRegister tokenRegister)
        {
            _context.TokenRegisters.Add(tokenRegister);
            _context.SaveChanges();
        }

        public TokenRegister getTokenRegisterById(int id)
        {
            return _context.TokenRegisters.First(e => e.Requestid == id);
        }

        public void updateTokenRegisterTable(TokenRegister token)
        {
            _context.TokenRegisters.Update(token);
            _context.SaveChanges();
        }

        public void addPhysicianLocationTable(Physicianlocation phylocation)
        {
            _context.Physicianlocations.Add(phylocation);
            _context.SaveChanges();
        }

        public void AddPhysicianNotificationTable(Physiciannotification phynoti)
        {
            _context.Physiciannotifications.Add(phynoti);
            _context.SaveChanges();
        }

        public void addPhyRegionList(List<Physicianregion> phyreg)
        {
            _context.Physicianregions.AddRange(phyreg);
            _context.SaveChanges();
        }

        public Dictionary<int, int> CountOfOpenRequest(List<Aspnetuser> aspnetuser)
        {
            Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

            foreach (var item in aspnetuser)
            {
                if (item.Role == 1)
                {
                    int adminid = _context.Admins.First(x => x.Aspnetuserid == item.Id).Adminid;
                    int count = _context.Requests.Where(x => x.Isdeleted != new BitArray(1, true)).Count(x => x.Status < 8);
                    keyValuePairs.Add(item.Id, count);
                }
                else
                {
                    int phyid = _context.Physicians.First(x => x.Aspnetuserid == item.Id).Physicianid;
                    int count = _context.Requests.Where(x => x.Physicianid == phyid && x.Isdeleted != new BitArray(1, true)).Count(x => x.Status < 8);
                    keyValuePairs.Add(item.Id, count);
                }


            }
            return keyValuePairs;
        }

        public Admin getAdminByAspId(int aspid)
        {
            return _context.Admins.FirstOrDefault(e => e.Aspnetuserid == aspid);
        }

        public Physician getPhysicianByAspId(int aspid)
        {
            return _context.Physicians.FirstOrDefault(e => e.Aspnetuserid == aspid);
        }

        public List<int> getRoleMenuByRoleid(int? roleid)
        {
           return _context.Rolemenus.ToList().Select(i => i.Menuid).ToList();
        }

        public Payrate GetPayRateByPhyID(int id)
        {
            return _context.Payrates.FirstOrDefault(e => e.PhysicianId == id);
        }

        public void UpdatePayRateTable(Payrate payrate)
        {
            _context.Payrates.Update(payrate);
            _context.SaveChanges();
        }

        public void AddPayrateTable(Payrate payrate)
        {
            _context.Payrates.Add(payrate);
            _context.SaveChanges();
        }
        public List<TimeSheetViewModel> MakeTimeSheet(DateTime startDate, int phyid)
        {
            DateTime enddate = startDate.AddDays(15 - startDate.Day);

            if (startDate.Day > 15)
            {
                enddate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day);
            }

            Timesheet invoice = _context.Timesheets.FirstOrDefault(e => e.PhysicianId == phyid && e.Startdate.Value.Date == startDate.Date && e.Enddate.Value.Date == enddate.Date);
            List<TimesheetDetail> sheets = new List<TimesheetDetail>();
            List<TimesheetReimbursement> reimbursements = new List<TimesheetReimbursement>();
            if (invoice != null)
            {
                sheets = _context.TimesheetDetails.Where(x => x.TimesheetId == invoice.TimesheetId).ToList();
                reimbursements = _context.TimesheetReimbursements.Where(x => x.TimesheetId == invoice.TimesheetId).ToList();
            }


            List<TimeSheetViewModel> timesheets = new();
            for (int i = 0; i <= enddate.Day - startDate.Day; i++)
            {
                double onCallHour = _context.Shiftdetails.Where(x => x.Shift.Physicianid == phyid && x.Isdeleted != new BitArray(1, true) && x.Shiftdate == DateOnly.FromDateTime(startDate.AddDays(i))).Select(x => new
                {
                    x.Shiftdate,
                    Duration = x.Endtime - x.Starttime,

                }).GroupBy(x => x.Shiftdate).Select(x => x.Sum(y => y.Duration.TotalHours)).FirstOrDefault();
                timesheets.Add(new TimeSheetViewModel()
                {
                    InvoiceId = sheets.FirstOrDefault(x => x.Sheetdate == startDate.AddDays(i))?.TimesheetId,
                    Date = startDate.AddDays(i),
                    OnCallHours = (int)onCallHour,
                    PhysicianId = phyid,
                    TotalHours = sheets.FirstOrDefault(x => x.Sheetdate == startDate.AddDays(i))?.ShiftHours ?? (int)onCallHour,
                    WeekendHoliday = sheets.FirstOrDefault(x => x.Sheetdate == startDate.AddDays(i))?.IsWeekend == true,
                    NumberOfHouseCalls = sheets.FirstOrDefault(x => x.Sheetdate == startDate.AddDays(i))?.Housecall != null ? sheets.FirstOrDefault(x => x.Sheetdate == startDate.AddDays(i))?.Housecall : 0,
                    NumberOfPhoneConsults = sheets.FirstOrDefault(x => x.Sheetdate == startDate.AddDays(i))?.PhoneConsult != null ? sheets.FirstOrDefault(x => x.Sheetdate == startDate.AddDays(i))?.PhoneConsult : 0,
                    Item = reimbursements.FirstOrDefault(x => x.ReimbursementDate == startDate.AddDays(i))?.Item ?? null,
                    FileName = reimbursements.FirstOrDefault(x => x.ReimbursementDate == startDate.AddDays(i))?.Filename ?? null,
                    ReceiptFile = null,
                    Amount = reimbursements.FirstOrDefault(x => x.ReimbursementDate == startDate.AddDays(i))?.Amount ?? 0,

                });
            }
            timesheets.First().StartDate = startDate;
            timesheets.First().EndDate = enddate;
            timesheets.First().BonusAmount = invoice.BonusAmount != null ? invoice.BonusAmount :null;
            timesheets.First().AdminNotes = invoice.AdminNote != null ? invoice.AdminNote : null;
            return timesheets;
        }
        public Timesheet GetInvoicesByPhyId(DateTime? startDate, DateTime? endDate, int physicianid)
        {
            return _context.Timesheets.FirstOrDefault(x => x.PhysicianId == physicianid && x.Startdate == startDate && x.Enddate == endDate);
        }
        public Timesheet GetTimeSheetByInvoiceId(int timesheetId)
        {
            return _context.Timesheets.First(e => e.TimesheetId == timesheetId);
        }

        public void UpdateTimeSheetTable(Timesheet timesheet)
        {
            _context.Timesheets.Update(timesheet);
            _context.SaveChanges();
        }
       
        public List<TimesheetDetail> GetTimeSheetListByInvoiceId(int timesheetId)
        {
            return _context.TimesheetDetails.Where(t => t.TimesheetId == timesheetId).ToList();
        }

        public void UpdateTimeSheetDetailTable(List<TimesheetDetail> timesheets)
        {
            _context.TimesheetDetails.UpdateRange(timesheets);
            _context.SaveChanges();
        }


        public void AddTimeSheetDetailTable(List<TimesheetDetail> timesheet)
        {
            _context.TimesheetDetails.AddRange(timesheet);
            _context.SaveChanges();
        }

        User IAdmin_Repository.GetUserByUserId(int? userid)
        {
            return _context.Users.FirstOrDefault(x => x.Userid == userid);
        }
    }
}
