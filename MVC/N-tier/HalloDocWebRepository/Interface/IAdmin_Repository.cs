﻿
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
        public void addRequestTable(Request req);
        void addRequestWiseFile(Requestwisefile reqclient);
        public void addUserTable(User users);
        public void adRequestClientTable(Requestclient reqclient);
        public Aspnetuser getAspnetuserByEmail(string email);
        public Casetag getCasetag(int reasonid);
        public int getcount(int id);
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check);
        public List<AdminDashboardTableModel> getDashboardTablesWithoutcheck(int id);
        List<Healthprofessional> getHealthProfessional(int businessId);
        public List<Healthprofessionaltype> getVenderDetail(int reg, string searchstr);
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
        public Requestwisefile getRequestWiseFileByName(string filename, int id);
        public Requestwisefile getRequesWiseFileList(int id, string filename);
        public User getUserByEmail(string? email);
        public void updateRequest(Request request);
        public void updateRequestNote(Requestnote reqnotes);
        public void updateRequestWiseFile(Requestwisefile file);
        Healthprofessional getHealthProfessionalDetail(int businessId);
        void addOrderDetailTable(Orderdetail orderdetail);
        void addBlockRequestTable(Blockrequest blockrequest);
        public Admin getAdminTableDataByEmail(string? email);
        public List<Adminregion> getAdminRegionByAdminId(int adminid);
        TokenRegister getRequestClientByToken(string token);
        EncounterForm getEncounterTable(int id);
        void updateEncounterForm(EncounterForm model);
        List<AdminDashboardTableModel> GetExportAllData();
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
        List<Aspnetuser> getAspnetUserList(int roleid);
        List<Admin> getAdminList();
        List<Physicianregion> getPhysicianRegionByPhy(int id);
        void removeAllPhysicianRegion(int physicianid);
        void addAllPhysicianRegion(List<int>? selectedReg, int id);
        List<Physicianlocation> getPhysicianLocationList();
        void updateHealthProfessionalTable(Healthprofessional vender);
        Role getAccessroleById(int id);
        void updateRoleTable(Role role);
        List<Blockrequest> getBlockData(string searchstr, DateTime date, string email, string mobile);
        List<Requesttype> getRequestTypeList();
        List<Requestclient> getRequestClientList();
        List<Requestnote> getREquestNotesList();
        void addSmsLogTable(Smslog smslog);
        void addEmailLogTable(Emaillog emaillog);
        List<Smslog> GetAllSmsLogs(int roleid, string name, string mobile, string createdDate, string sentDate);
        List<Emaillog> GetAllEmailLogs(int roleid, string name, string email, string createdDate, string sentDate);
        void AddShiftTable(Shift shift);
        List<ShiftDetailsModel> getshiftDetail(int reg);
        void AddShiftDetails(List<Shiftdetail> shiftdetails);
        Blockrequest getBlockRequestById(int id);
        void updateBlockRequest(Blockrequest req);
        IQueryable<PatientHistoryTable> GetPatientHistoryTable(string? fname, string? lname, string? email, string? phone);
        List<Request> GetAllRequestsByAid(int id);
        string? GetStatus(short status);
        int GetNumberOfDocsByRid(int requestid);
        Shiftdetail getShiftDetailByShiftDetailId(int id);
        Shift getShiftByID(int shiftid);
        void UpdateShiftDetailTable(Shiftdetail sd);
        List<Shiftdetail> getShiftDetailByRegion(int reg);
        List<Physician> getPhysicianOnCallList(int reg);
        Admin getAdminByAdminID(int id);
        List<Physiciannotification> getSelectedPhyNotification(int[] ints);
        List<Physiciannotification> getNotSelectedPhyNotification(int[] ints);
        void updatePhyNotificationTable(Physiciannotification notification);
        Physiciannotification getPhyNotificationByPhyID(int pysicianid);
        public int[] GetUnscheduledPhysicanID();
        List<Physician> getUnscheduledPhysicianList(int[] data);
        void addHealthProfessionTable(Healthprofessional business);
        void addTokenRegister(TokenRegister tokenRegister);
        TokenRegister getTokenRegisterById(int id);
        void updateTokenRegisterTable(TokenRegister token);
        void addPhysicianLocationTable(Physicianlocation phylocation);
        void AddPhysicianNotificationTable(Physiciannotification phynoti);
        void addEncounterTable(EncounterForm model);
        List<Role> getRolesOfProvider();
        void addPhyRegionList(List<Physicianregion> phyreg);
        Dictionary<int, int> CountOfOpenRequest(List<Aspnetuser> aspnetuser);
        Admin getAdminByAspId(int aspid);
        Physician getPhysicianByAspId(int v);
        List<int> getRoleMenuByRoleid(int? roleid);
        Payrate GetPayRateByPhyID(int id);
        void UpdatePayRateTable(Payrate payrate);
        void AddPayrateTable(Payrate payrate);
        List<TimeSheetViewModel> MakeTimeSheet(DateTime startDate, int phyid);
        Timesheet GetInvoicesByPhyId(DateTime? startDate, DateTime? enddate, int phyid);
        Timesheet GetTimeSheetByInvoiceId(int timesheetId);
        void UpdateTimeSheetTable(Timesheet timesheet);
        
        List<TimesheetDetail> GetTimeSheetListByInvoiceId(int timesheetId);
        void UpdateTimeSheetDetailTable(List<TimesheetDetail> timesheets);
        void AddTimeSheetDetailTable(List<TimesheetDetail> timesheet);
        User GetUserByUserId(int? userid);
    }
}
