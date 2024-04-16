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
    }
}
