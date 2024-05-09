
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebRepo.Interface
{
    public interface IProvider_Repository
    {
        Physician GetPhyByEmail(string email);
        int getcount(int v, int phyid);
        List<AdminDashboardTableModel> getDashboardTables(int id, int check,int phyid);
        List<AdminDashboardTableModel> getDashboardTablesWithoutcheck(int id, int phyid);
        List<Physician> getPhysicianList();
        List<Region> getRegions();
        User getUserByEmail(string? email);
        void addRequestTable(Request req);
        void adRequestClientTable(Requestclient reqclient);
        void addRequestNotesTAble(Requestnote note);
        void addEmailLogTable(Emaillog emaillog);
        void addSmsLogTable(Smslog smslog);
        Request getRequestByReqID(int id);
        void updateRequestTable(Request data);
        Requestclient getRequestClientById(int? id);
        Region getRegionByRegionId(int regionid);
        Requestnote getREquestNotes(int id);
        List<Requeststatuslog> getRequestStatusLog(int id);
        Physician getPhysicianById(int? physicianid);
        void updateRequestNoteTable(Requestnote reqnotes);
        void addRequestStatusLogTable(Requeststatuslog statuslog);
        void addTokenRegister(TokenRegister tokenRegister);
        List<Requestwisefile> getPatientDocument(int? id);
        Requestwisefile getRequestWiseFileByName(string filename, int id);
        void updateRequestWiseFile(Requestwisefile file);
        Aspnetuser getAspnetuserByEmail(string email);
        void addRequestWiseFile(Requestwisefile reqclient);
        List<Healthprofessional> getHealthProfessionalList();
        List<Healthprofessional> getHealthProfessional(int profId);
        Healthprofessional getHealthProfessionalDetail(int businessId);
        List<Healthprofessionaltype> getHealthProfessionalTypeList();
        void addOrderDetailTable(Orderdetail orderdetail);
        void updateEncounterForm(EncounterForm info);
        EncounterForm getEncounterTable(int id);
        Requestwisefile getRequestWiseFile(int id);
        Requestwisefile getRequesWiseFileList(int id, string filename);
        void addEncounterTable(EncounterForm model);
        List<Requestwisefile> getRequestWiseFileList(int id);
        Aspnetuser getAspnetuserByID(int? aspnetuserid);
        List<Physicianregion> getPhysicianRegionByPhy(int physicianid);
        List<Admin> getAdminList();
        void updateAspnetUser(Aspnetuser aspnetuser);
        List<ShiftDetailsModel> getshiftDetail(int reg);
        Shiftdetail getShiftDetailByShiftDetailId(int id);
        Shift getShiftByID(int shiftid);
        void AddShiftTable(Shift shift);
        void AddShiftDetails(List<Shiftdetail> shiftdetails);
        void UpdateShiftDetailTable(Shiftdetail sd);
        List<TimeSheetViewModel> MakeTimeSheet(DateTime startDate, int phyid);
        Timesheet GetInvoicesByPhyId(DateTime? startDate, DateTime? endDate, int physicianid);
        List<TimesheetDetail> GetTimeSheetListByInvoiceId(int timesheetId);
        void UpdateTimeSheetDetailTable(List<TimesheetDetail> timesheets);
        void AddTimeSheetDetailTable(List<TimesheetDetail> timesheet);
        void AddTimeSheetTable(Timesheet invoice);
        List<TimesheetReimbursement> GetReimbursementListByInvoiceId(int timesheetId);
        void UpdateReimbursementList(List<TimesheetReimbursement> reimbursements);
        void AddReimbursementListTbale(List<TimesheetReimbursement> reimbursement);
        TimesheetReimbursement GetReimByPhyIdAndStartDate(DateTime dateTime, int physicianid);
        void UpdateReimbursementTable(TimesheetReimbursement reim);
        void AddReimbursementTable(TimesheetReimbursement reim);
    }
}
