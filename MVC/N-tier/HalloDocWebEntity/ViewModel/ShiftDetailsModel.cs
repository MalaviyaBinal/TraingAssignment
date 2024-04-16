
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class ShiftDetailsModel
    {
        public List<Physician> physicians { get; set; }
        public List<Region> regions { get; set; }
        public List<Shift> shifts { get; set; }
        public Shift shiftData { get; set; }
        public Shiftdetail ShiftDetailData { get; set; }
        public List<Shiftdetail> shiftdetail { get; set; }
        public List<ShiftDetailsModel> shiftDetails { get; set; }
        public string PhysicianName { get; set; }
        public int Physicianid { get; set; }
        public string RegionName { get; set; }
        public int RegionId { get; set; }
        public short Status { get; set; }
        public TimeOnly Starttime { get; set; }
        public DateOnly Shiftdate { get; set; }
        public TimeOnly Endtime { get; set; }
        public int Shiftdetailid { get; set; }
        public int SelectedRegion { get; set; }
        public int TotalRecord { get; set; }
        public int FromRec { get; set; }
        public int ToRec { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
