
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class ShiftDetailsModel
    {
        public List<Physician> physicians { get; set; }
        public List<Region> regions { get; set; }
        public List<Shift> shifts { get; set; }
        public List<Shiftdetail> shiftdetail { get; set; }
        public List<ShiftDetailsModel> shiftDetails { get; set; }
        public string PhysicianName { get; set; }
        public int Physicianid { get; set; }
        public string RegionName { get; set; }
        public short Status { get; set; }
        public TimeOnly Starttime { get; set; }
        public DateOnly Shiftdate { get; set; }
        public TimeOnly Endtime { get; set; }
        public int Shiftdetailid { get; set; }
    }
}
