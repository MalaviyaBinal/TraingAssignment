using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace HalloDocWebEntity.ViewModel
{
    public class TimeSheetViewModel
    {
        public List<Timesheet>? DateList { get; set; }
        public int? PhysicianId { get; set; }

        public int? InvoiceId { get; set; }

        public DateTime SheetDate { get; set; }

        public int? TotalHours { get; set; }

        public bool? WeekendHoliday { get; set; }

        public int? NoHousecalls { get; set; }

        public int? NoHousecallsNight { get; set; }

        public int? NoPhoneConsult { get; set; }

        public int? NoPhoneConsultNight { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime Date { get; set; }
        public int? OnCallHours { get; set; }
        public int? NumberOfHouseCalls { get; set; }
        public int? NumberOfPhoneConsults { get; set; }

        public string? Item { get; set; }
        public string? FileName { get; set; }
        public int? Amount { get; set; }
        public IFormFile? ReceiptFile { get; set; }
               
        public int? NightshiftWeekend { get; set; }
        public int? TotalNightshiftWeekend { get; set; }
        public int? Shift { get; set; }       
        public int? TotalShift { get; set; }       
        public int? PhoneConsults { get; set; }       
        public int? TotalPhoneConsults { get; set; }       
        public int? Housecall { get; set; }
        public int? TotalHousecall { get; set; }
        public int? TotalInvoice{ get; set; }
    }
}
