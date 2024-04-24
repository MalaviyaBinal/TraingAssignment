using HalloDocWebEntity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HalloDocWebEntity.ViewModel
{
    public class SchedulingViewModel
    {

        public List<Region> regions { get; set; }
        public List<Physician> physics { get; set; }
        public int CreatedBy { get; set; }
        public int RegionId { get; set; }
        public int PhysicianId { get; set; }
        public DateOnly ShiftDate { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeOnly StartTime { get; set; }
        public bool IsRepeat { get; set; }
        public string? RepeatDays { get; set; }
        public int RepeatCount { get; set; }
        public int? SelectedRegion { get; set; }
        public List<int> daylist { get; set; }
        public  bool IsValidTime(object value)
        {
            
            return (StartTime- EndTime > TimeSpan.Zero);
        }

    }
}
