﻿using ClosedXML.Excel;
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
    public interface IAdmin_Service
    {

        public AssignCaseModel AssignCaseModel(int regid);
        void blockConfirm(int id, string notes);
        public void cancelConfirm(int id, int reasonid, string notes);
        void clearConfirm(int id);
        public void deleteAllFile(int id, string[] filenames);
        public void deleteFile(int id);
        public MemoryStream downloadFile(string[] filenames);
        public AdminProfile getAdminProfileData(string? v);
        public AdminProfile getAdminData(int v);
        public byte[] getBytesForFile(int id);
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check);

        public SendOrderModel getOrderModel(int id, int profId, int businessId);
        public AdminViewUpload getPatientDocument(int? id);
        public Requestclient getRequestClientByID(int id);
        public Requestwisefile getRequestWiseFileByID(int id);
        public void patientRequestByAdmin(RequestForMe info, string email);
        public void requestAssign(int phyId, string notes, int id, string email);
        public void requestTransfer(int phyId, string notes, int id, string? v);
        public void saveNotes(Notes n, int id);
        void SendEmail(int id, string[] filenames,string email);
        void sendOrder(SendOrderModel info);
        public AdminDashboard setAdminDashboardCount();
        public void uploadFileAdmin(IFormFile fileToUpload, int v, string email);
        public ViewCaseModel ViewCaseModel(int id);
        public Notes ViewNotes(int id);
        public Requestclient sendAgreementService(string id);
        void SendAgreementCancleConfirm(int id, string notes, string? v);
        void SendAgreementConfirm(int id);
        void sendAgreementMail(int id,string email);
        Encounterformmodel EncounterAdmin(int id);
        void saveEncounterForm(Encounterformmodel info);
        byte[] GetBytesForExportAll();
        byte[] GetBytesForExport(int id);
        List<Physician> getPhysicianList();
        List<Region> getRegionList();
        AdminViewUpload closeCase(int id);
        void closeCaseSaveData(int id, AdminViewUpload n);
        void updateadminaddress(AdminProfile info);
        void updateadminform(AdminProfile info);
        void sendLinkAdminDashboard(AdminDashBoardPagination info,string email);
        AdminProviderModel getProviderDataForAdmin(int id);
        void addProviderByAdmin(AdminProviderModel model);
        Physician getPhysicianByID(int id);
        void ContactProviderSendMessage(int id,string email, string phone, string note, int selected);
        AdminProviderModel getProviderByAdmin(int id,string u);
        void savePhysicianPassword(AdminProviderModel info);
        void savePhysicianInfo(AdminProviderModel info);
        void savePhysicianBillingInfo(AdminProviderModel info);
        RoleModel GetMenuData(int check);
        void generateRole(string roleName, string[] selectedRoles, int check, string? v);
        List<Role> getRoleList();
        void updateroleof(RoleModel roleModel);
        RoleModel EditRole(int id);
        void CreateAdminAccount(AdminProfile model, string? v);
        AdminProfile getAdminRoleData();
        public Dictionary<int, string> GetExtension(int Id);
        UserAccess getUserAccessData(int roleid);
        void savePhysicianProfile(AdminProviderModel info);
        void savePhysicianDocuments(AdminProviderModel info);
        List<Physicianlocation> getPhysicianLocation();
        AdminPartnersModel getVenderDetail(int reg, int pagenumber, string searchstr);
        DeleteAccountDetails openDeleteModal(int id, string acntType);
        void deleteVender(int id);
        void deleteAccessRole(int id);
        void deletePhysicianAccount(int id);
        SendOrderModel GetEditBusinessData(int id);
        void UpdateBusinessData(SendOrderModel model);
        List<Requesttype> GetRequestTypes();
        AdminRecordsModel getSearchRecordData(AdminRecordsModel model);
        public void SendSms(string receiverPhoneNumber, string message);
        Email_SMS_LogModel GetEmailLogs(int roleid, string name, string email, string createdDate, string sentDate, int pagenumber);
        Email_SMS_LogModel GetSmsLogs(int roleid, string name, string mobile, string createdDate, string sentDate, int pagenumber);
        SchedulingViewModel openShiftModel(int regionid);
        void CreateShift(SchedulingViewModel info);
        ShiftDetailsModel getSchedulingData(int reg);
        AdminRecordsModel getBlockHistoryData(string searchstr, DateTime date, string email, string mobile, int pagenumber);
        void unblockRequest(int id, string email);
        void deleteRequest(int id);
        PatientHistoryTable PatientHistoryTable(string? fname, string? lname, string? email, string? phone, int pagenumber);
        PatientRecordModel PatientRecord(int id, int pagenumber);
        ShiftDetailsModel getViewShiftData(int id, int regid);
        void UpdateShiftDetailData(ShiftDetailsModel model, string email);
        void DeleteShiftDetails(int id);
        void UpdateShiftDetailsStatus(int id);
        ShiftDetailsModel getReviewShiftData(int reg, bool isCurrentMonth, int pagenumber);
        void DeletShift(string[] selectedShifts);
        void ApproveShift(string[] selectedShifts);
        ShiftDetailsModel getProviderOnCall(int reg);
        void SaveNotificationStatus(string[] phyList);
        void DTYSupportRequest(string notes,string email);
        void AddVendor(SendOrderModel model);
        SendOrderModel getVenderData();
        int getRequestTypeByRequestID(int id);
        void saveAdminPassword(AdminProfile info);
        List<Role> getRolesOfAdmin();
        List<Role> getRolesOfProvider();
        AdminProviderModel getProviderRoles();
        Stream GetExportData(AdminRecordsModel model, int status, string mobile, string email, string pname, DateTime tdate, DateTime fdate, int reqtype, string searchstr );
        PayRateViewModel GetPayRate(int id);
        void UpdatePayRate(PayRateViewModel model, int phyid);
        List<TimeSheetViewModel> MakeTimeSheet(DateTime startDate, int phyid);
        List<bool> IsTimesheetFinalized(DateTime dateTime, int v);
        TimeSheetViewModel GETTimeSheetForApprove(DateTime startDate, int phyid);
        void ApproveTimesheet(int timesheetId, int BonusAmnt, string AdminNote, int InvoiceAmnt);
        void SaveTimesheet(TimeSheetDataViewModel model, int phyid, string? v);
        Admin getAdminByEmail(string? v);
    }
}
