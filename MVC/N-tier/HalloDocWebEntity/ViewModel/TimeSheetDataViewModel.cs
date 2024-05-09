
using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class TimeSheetDataViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> OnCallHours { get; set; } = new List<int>();

        public List<int> TotalHours { get; set; } = new List<int>();

        public List<int> WeekendHoliday { get; set; } = new List<int>();

        public List<int> NumberOfHouseCalls { get; set; } = new List<int>();

        public List<int> NumberOfPhoneConsults { get; set; } = new List<int>();

        public List<string> Item { get; set; } = new List<string>();
        public List<string> FileName { get; set; } = new List<string>();
        public List<IFormFile> ReceiptFile { get; set; }
        public List<int> Amount { get; set; } = new List<int>();

    }

}
