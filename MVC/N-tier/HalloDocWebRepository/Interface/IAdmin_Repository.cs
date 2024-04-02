
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebRepo.Interface
{
    public interface IAdmin_Repository
    {
        public void addAspnetuserTable(Aspnetuser aspuser);
        public void addRequestNotesTAble(Requestnote note);
        public void addRequestStatusLogTable(Requeststatuslog statuslog);
        public  void addRequestTable(Request req);
        void addRequestWiseFile(Requestwisefile reqclient);
        public void addUserTable(User users);
        public void adRequestClientTable(Requestclient reqclient);
        public Aspnetuser getAspnetuserByEmail(string email);
        public Casetag getCasetag(int reasonid);
        public int getcount(int id);
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check);
        public List<AdminDashboardTableModel> getDashboardTablesWithoutcheck(int id);
        List<Healthprofessional> getHealthProfessional(int businessId);
        public List<Healthprofessional> getHealthProfessionalList();
        List<Healthprofessionaltype> getHealthProfessionalTypeList();
        List<Healthprofessionaltype> getHealthProfessionalType(int profId);
        List<Requestwisefile> getPatientDocument(int? id);
        public Physician getPhysicianById(int? physicianid);
        public List<Physician> getPhysicianList();
        public List<Physician> getPhysicianListByregion(int regid);
        public Region getRegionByRegionId(int regionid);
        public List<Region> getRegions();
        public Request getRequestByID(int? id);
        public Requestclient getRequestClientById(int? id);
        public Requestnote getREquestNotes(int id);
        public List<Requeststatuslog> getRequestStatusLog(int id);
        public Requestwisefile getRequestWiseFile(int id);
        public Requestwisefile getRequestWiseFileByName(string filename,int id);
        public Requestwisefile getRequesWiseFileList(int id,string filename);
        public User getUserByEmail(string? email);
        public void updateRequest(Request request);
        public void updateRequestNote(Requestnote reqnotes);
        public void updateRequestWiseFile(Requestwisefile file);
        Healthprofessional getHealthProfessionalDetail(int businessId);
        void addOrderDetailTable(Orderdetail orderdetail);
        void addBlockRequestTable(Blockrequest blockrequest);
        public Admin getAdminTableDataByEmail(string? email);
        public List<Adminregion> getAdminRegionByAdminId(int adminid);
        Request getRequestClientByToken(string token);
        EncounterForm getEncounterTable(int id);
        void updateEncounterForm(EncounterForm model);
        IQueryable<AdminDashboardTableModel> GetExportAllData();
        void updateRewuestClient(Requestclient reqclient);
        Admin getAdminByAdminId(Admin admin);
        void updateAdmin(Admin model);
        void addAdminReg(Adminregion ar);
        void RemoveAdminReg(Adminregion ar);
        void addPhysician(Physician model);
        Aspnetuser getAspnetuserByID(int? aspnetuserid);
        void updateAspnetUser(Aspnetuser aspnetuser);
        void updatePhysician(Physician physician);
        List<Menu> getMenuListWithCheck(int check);
        List<Menu> getmenudataof();
        void saveRole(Role role);
        void saveRoleMenu(Rolemenu rolemenu);
        List<Role> getRoleList();
        Role getRoleByID(int id);
        List<Rolemenu> getSelectedRoleMenuByRoleID(int id);
        void removeAllRoleMenu(int roleId);
        void addAdminTable(Admin admin);
        List<Role> getRolesOfAdmin();
        List<Aspnetuser> getAspnetUserList();
        List<Admin> getAdminList();
    }
}
