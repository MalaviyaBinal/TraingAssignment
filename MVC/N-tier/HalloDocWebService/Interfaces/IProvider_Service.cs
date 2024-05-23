using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocWebServices.Interfaces
{
    public interface IProvider_Service
    {
        public AdminDashboard setProviderDashboardCount(string email);
        List<Region> getRegionList();
        List<AdminDashboardTableModel> getDashboardTables(int id, int check,string email);
        List<Physician> getPhysicianList();
        void patientRequestByProvider(RequestForMe info, string? v);
        void sendLinkProviderDashboard(AdminDashBoardPagination info, string? v);
        void AcceptConsultRequest(int id,string email);
        ViewCaseModel ViewCaseModel(int id);
        Notes ViewNotes(int id);
        void saveNotes(Notes n, int id);
        Requestclient getRequestClientByID(int id);
        int getRequestTypeByRequestID(int id);
        void sendAgreementMail(int id, string? v);
        Request GetRequestData(int id);
        void TransferConfirm(Request reg, string? v);
        AdminViewUpload getPatientDocument(int? v);
        Dictionary<int, string> GetExtension(int v);
        void deleteAllFile(int id, string[] filenames);
        void uploadFileAdmin(IFormFile fileToUpload, int v1, string? v2);
        MemoryStream downloadFile(string[] filenames);
        SendOrderModel getOrderModel(int id, int profId, int businessId);
        void sendOrder(SendOrderModel info);
        void ConsultCall(int id);
        void HouseCall(int id);
        void EncounterFinalize(int id);
        Encounterformmodel EncounterProvider(int id);
        void saveEncounterForm(Encounterformmodel info);
        void deleteFile(int id);
        void SendEmail(int id, string[] filenames, string? v);
        void HouseCallToConclude(int id);
        AdminViewUpload ConcludeCare(int id);
        void ConcludeFinal(AdminViewUpload model, string? v);
        AdminProviderModel getPhyProfileData(string? v);
        void UpdateProfileRequest(string? v, string? adminnotes);
        void savePhysicianPassword(AdminProviderModel info);
        ShiftDetailsModel getSchedulingData(string email);
        ShiftDetailsModel getViewShiftData(int id, int regid);
        SchedulingViewModel openShiftModel(int regionid);
        void CreateShift(SchedulingViewModel info,string email);
        void DeleteShiftDetails(int id);
        List<TimeSheetViewModel> MakeTimeSheet(DateTime startDate, string phyid);
        void SaveTimesheet(TimeSheetDataViewModel model, string? v);

        void EditReimbursement(DateTime dateTime, string item, int amount, int gap, string? v);
        void SaveReimbursement(TimeSheetDataViewModel model, string? v);
        void DeleteReimbursement(int gap, DateTime startDate);
        void FinalizeTimesheet(int timesheetId);
        bool IsTimesheetFinalized(DateTime startDate, string? v);
        ChatViewModel _ChatPanel(string? email, int receiver, int receiver2, string requesterType);
    }
}
