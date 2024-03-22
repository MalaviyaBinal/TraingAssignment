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
        public byte[] getBytesForFile(int id);
        public IQueryable<AdminDashboardTableModel> getDashboardTables(int id, int check);
        //public IQueryable<AdminDashboardTableModel> getDashboardTablesWithRegion(int id, int check,int region);
        public SendOrderModel getOrderModel(int id, int profId, int businessId);
        public AdminViewUpload getPatientDocument(int? id);
        public Requestclient getRequestClientByID(int id);
        public Requestwisefile getRequestWiseFileByID(int id);
        public void patientRequestByAdmin(RequestForMe info,string email);
        public void requestAssign(int phyId, string notes, int id,string email);
        public void requestTransfer(int phyId, string notes, int id, string? v);
        public void saveNotes(Notes n, int id);       
        void SendEmail(int id, string[] filenames);
        void sendOrder(SendOrderModel info);
        public AdminDashboard setAdminDashboardCount();
        public void uploadFileAdmin(IFormFile fileToUpload, int v,string email);
        public ViewCaseModel ViewCaseModel(int id);
        public Notes ViewNotes(int id);
        public Requestclient sendAgreementService(string id);
        void SendAgreementCancleConfirm(int id, string notes, string? v);
        void SendAgreementConfirm(int id);
        void sendAgreementMail(int id);
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
        void sendLinkAdminDashboard(AdminDashboardDataWithRegionModel info);
        AdminProviderModel getProviderDataForAdmin(int id);
        void addProviderByAdmin(AdminProviderModel model);
        Physician getPhysicianByID(int id);
        void ContactProviderSendMessage(Physician info);
    }
}
