
using HalloDocWebEntity.Data;
using HalloDocWebEntity.ViewModel;
using HalloDocWebRepo.Interface;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HalloDocWebRepo.Implementation
{
    public class Provider_Repository : IProvider_Repository
    {
        private readonly ApplicationContext _context;


        public Provider_Repository(ApplicationContext context)
        {
            _context = context;
        }

        public int getcount(int id, int phyid)
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


            }

            return _context.Requests.Where(i => status.Any(j => j == i.Status) && i.Physicianid == phyid).Count();
        }

        public List<Region> getRegions()
        {
            return _context.Regions.ToList();
        }
        public List<AdminDashboardTableModel> getDashboardTables(int id, int check, int phyid)
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
            var data1 = _context.Requests.Where(m => status.Any(j => j == m.Status) && m.Requesttypeid == check && m.Physicianid == phyid).Include(x => x.Requestclients).Include(x => x.Requesttype).ToList();
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
                    Requesteddate = item.Createddate != null ? item.Createddate : null,
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
        public List<AdminDashboardTableModel> getDashboardTablesWithoutcheck(int id, int phyid)
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


            }
            var data1 = _context.Requests.Where(m => status.Any(j => j == m.Status) && m.Physicianid == phyid).Include(x => x.Requestclients).Include(x => x.Requesttype).ToList();
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
                    Requesteddate = item.Createddate != null ? item.Createddate : null,
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
        public List<Physician> getPhysicianList()
        {
            return _context.Physicians.Where(m => m.Isdeleted == null).Include(e => e.Physiciannotifications).ToList();
        }

        public User getUserByEmail(string? email)
        {
            return _context.Users.FirstOrDefault(m => m.Email == email);
        }

        public Physician GetPhyByEmail(string email)
        {

            return _context.Physicians.FirstOrDefault(m => m.Email == email);
        }

        public void addRequestTable(Request req)
        {
            _context.Requests.Add(req);
            _context.SaveChanges();
        }

        public void adRequestClientTable(Requestclient reqclient)
        {
            _context.Requestclients.Add(reqclient);
            _context.SaveChanges();
        }

        public void addRequestNotesTAble(Requestnote note)
        {
            _context.Requestnotes.Add(note);
            _context.SaveChanges();
        }

        public void addEmailLogTable(Emaillog emaillog)
        {
            _context.Emaillogs.Add(emaillog);
            _context.SaveChanges();
        }

        public void addSmsLogTable(Smslog smslog)
        {
            _context.Smslogs.Add(smslog);
            _context.SaveChanges();
        }

        public Request getRequestByReqID(int id)
        {
            return _context.Requests.FirstOrDefault(e => e.Requestid == id);
        }

        public void updateRequestTable(Request data)
        {
            _context.Requests.Update(data);
            _context.SaveChanges();
        }
        public Region getRegionByRegionId(int regionid)
        {
            return _context.Regions.FirstOrDefault(m => m.Regionid == regionid);
        }
        public Requestclient getRequestClientById(int? id)
        {
            return _context.Requestclients.FirstOrDefault(m => m.Requestid == id);
        }
        public List<Requeststatuslog> getRequestStatusLog(int id)
        {
            return _context.Requeststatuslogs.Where(m => m.Requestid == id).ToList();
        }
        public Requestnote getREquestNotes(int id)
        {
            return _context.Requestnotes.Where(x => x.Requestid == id).FirstOrDefault();
        }

        public Physician getPhysicianById(int? physicianid)
        {
            return _context.Physicians.FirstOrDefault(m => m.Physicianid == physicianid);
        }

        public void updateRequestNoteTable(Requestnote reqnotes)
        {
            _context.Requestnotes.Update(reqnotes);
            _context.SaveChanges();
        }
        public void addRequestStatusLogTable(Requeststatuslog statuslog)
        {
            _context.Requeststatuslogs.Add(statuslog);
            _context.SaveChanges();
        }
        public void addTokenRegister(TokenRegister tokenRegister)
        {
            _context.TokenRegisters.Add(tokenRegister);
            _context.SaveChanges();
        }
        public List<Requestwisefile> getPatientDocument(int? id)
        {
            return _context.Requestwisefiles.Where(m => m.Requestid == id && m.Isdeleted == null).ToList();
        }
        public void updateRequestWiseFile(Requestwisefile file)
        {
            _context.Requestwisefiles.Update(file);
            _context.SaveChanges();
        }
        public Requestwisefile getRequestWiseFileByName(string filename, int id)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestid == id && m.Filename == filename && m.Isdeleted == null);
        }
        public void addRequestWiseFile(Requestwisefile reqclient)
        {
            _context.Requestwisefiles.Add(reqclient);
            _context.SaveChanges();
        }
        public Aspnetuser getAspnetuserByEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
                return _context.Aspnetusers.FirstOrDefault(m => m.Email == email);
            else
                return null;
        }
        public List<Healthprofessional> getHealthProfessionalList()
        {
            return _context.Healthprofessionals.Where(m => m.Isdeleted == null).ToList();
        }

        public List<Healthprofessionaltype> getHealthProfessionalTypeList()
        {
            return _context.Healthprofessionaltypes.ToList();
        }

        public List<Healthprofessional> getHealthProfessional(int businessId)
        {
            return _context.Healthprofessionals.Where(m => m.Profession == businessId).ToList();
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
        public EncounterForm getEncounterTable(int id)
        {
            return _context.EncounterForms.FirstOrDefault(m => m.Requestid == id);
        }

        public void updateEncounterForm(EncounterForm model)
        {
            _context.EncounterForms.Update(model);
            _context.SaveChanges();
        }
        public Requestwisefile getRequestWiseFile(int id)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestwisefileid == id && m.Isdeleted == null);
        }

        public Requestwisefile getRequesWiseFileList(int id, string filename)
        {
            return _context.Requestwisefiles.FirstOrDefault(m => m.Requestid == id && m.Isdeleted == null && m.Filename == filename);
        }

        public void addEncounterTable(EncounterForm model)
        {
            _context.EncounterForms.Add(model);
            _context.SaveChanges();
        }

        public List<Requestwisefile> getRequestWiseFileList(int id)
        {
            return _context.Requestwisefiles.Where(e => e.Requestid == id).ToList();
        }
        public Aspnetuser getAspnetuserByID(int? aspnetuserid)
        {
            return _context.Aspnetusers.FirstOrDefault(m => m.Id == aspnetuserid);
        }
        public List<Physicianregion> getPhysicianRegionByPhy(int id)
        {
            return _context.Physicianregions.Where(m => m.Physicianid == id).ToList();
        }

        public List<Admin> getAdminList()
        {
            return _context.Admins.ToList();
        }
        public void updateAspnetUser(Aspnetuser aspnetuser)
        {
            _context.Aspnetusers.Update(aspnetuser);
            _context.SaveChanges();
        }
        public List<ShiftDetailsModel> getshiftDetail(int phyid)
        {

            return _context.Shiftdetails.Where(e => e.Isdeleted != new BitArray(1, true))
               .Include(e => e.Shift)
               .ThenInclude(e => e.Physician).ThenInclude(e => e.Region).Where(e => e.Shift.Physicianid == phyid).Select(e =>
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
        public Shiftdetail getShiftDetailByShiftDetailId(int id)
        {
            return _context.Shiftdetails.FirstOrDefault(e => e.Shiftdetailid == id);
        }

        public Shift getShiftByID(int shiftid)
        {
            return _context.Shifts.FirstOrDefault(e => e.Shiftid == shiftid);
        }
        public void AddShiftTable(Shift shift)
        {
            _context.Shifts.Add(shift);
            _context.SaveChanges();
        }

        public void AddShiftDetails(List<Shiftdetail> shiftdetails)
        {
            shiftdetails.ForEach(item =>
            {
                _context.Shiftdetails.Add(item);
            });
            _context.SaveChanges();

        }
        public void UpdateShiftDetailTable(Shiftdetail sd)
        {
            _context.Shiftdetails.Update(sd);
            _context.SaveChanges();
        }

        public List<TimeSheetViewModel> MakeTimeSheet(DateTime startDate, int phyid)
        {
            DateTime enddate = startDate.AddDays(15 - startDate.Day);
           
            if (startDate.Day > 15)
            {
                enddate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day);
            }

            Timesheet invoice = _context.Timesheets.FirstOrDefault(e => e.PhysicianId == phyid && e.Startdate == startDate && e.Enddate == enddate);
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
            return timesheets;
        }

        public Timesheet GetInvoicesByPhyId(DateTime? startDate, DateTime? endDate, int physicianid)
        {
            return _context.Timesheets.FirstOrDefault(x => x.PhysicianId == physicianid && x.Startdate == startDate && x.Enddate ==  endDate);
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

        public void AddTimeSheetTable(Timesheet invoice)
        {
            _context.Timesheets.Add(invoice);
            _context.SaveChanges();
        }

        public List<TimesheetReimbursement> GetReimbursementListByInvoiceId(int timesheetId)
        {
            return _context.TimesheetReimbursements.Where(t => t.TimesheetId == timesheetId).ToList();
        }

        public void UpdateReimbursementList(List<TimesheetReimbursement> reimbursements)
        {
            _context.TimesheetReimbursements.UpdateRange(reimbursements);
            _context.SaveChanges();
        }

        public void AddReimbursementListTbale(List<TimesheetReimbursement> reimbursement)
        {
            _context.TimesheetReimbursements.AddRange(reimbursement);
            _context.SaveChanges();
        }

        public TimesheetReimbursement GetReimByPhyIdAndStartDate(DateTime startDate, int physicianid)
        {
            return _context.TimesheetReimbursements.FirstOrDefault(x => x.ReimbursementDate.Value.Date == startDate.Date && x.PhysicianId == physicianid);
        }

        public void UpdateReimbursementTable(TimesheetReimbursement reim)
        {
            _context.TimesheetReimbursements.Update(reim);
            _context.SaveChanges();
        }

        public void AddReimbursementTable(TimesheetReimbursement reim)
        {
            _context.TimesheetReimbursements.Add(reim);
            _context.SaveChanges();
        }

        public void DeleteReimbursementTable(int gap, DateTime startDate)
        {
            var reim = _context.TimesheetReimbursements.FirstOrDefault(e => e.ReimbursementDate == DateTime.Parse(startDate.AddDays(gap).ToString("MM-dd-yyyy")));
            if(reim != null)
            {
                _context.TimesheetReimbursements.Remove(reim);
                _context.SaveChanges();
            }
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

        User IProvider_Repository.GetUserByUserId(int? userid)
        {
            return _context.Users.FirstOrDefault(x => x.Userid == userid);
        }
    }

}
