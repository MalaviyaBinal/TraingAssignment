
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
    }
}
