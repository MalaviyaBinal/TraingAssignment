using HalloDocWebEntity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace HalloDocWebEntity.ViewModel
{
    public class SchedulingViewModel
    {

        public int ShiftId { get; set; }
        public int PhysicianId { get; set; }
        public DateOnly StartDate { get; set; }
        public BitArray IsRepeat { get; set; } = null!;
        public string? WeekDays { get; set; }
        public int? RepeatUpto { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? Ip { get; set; }
        public int ShiftDetailId { get; set; }
        public DateTime ShiftDate { get; set; }
        public int? RegionId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public short Status { get; set; }
        public BitArray IsDeleted { get; set; } = null!;
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? LastRunningDate { get; set; }
        public string? EventId { get; set; }
        public BitArray? IsSync { get; set; }

    }
}
