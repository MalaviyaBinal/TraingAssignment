
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

        public string Item { get; set; } 
        public string FileName { get; set; }
        public IFormFile ReceiptFile { get; set; }
        public int Amount { get; set; } 
        public int Gap { get; set; } 

    }

}
