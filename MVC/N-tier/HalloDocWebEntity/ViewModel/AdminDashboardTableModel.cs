using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class AdminDashboardTableModel
    {
        public string? physician;
        public int? physicianId;

        public string? Name { get; set; }
        public string? DOB { get; set; }
        public string? Requestor { get; set; }
        public string? Phonenumber { get; set; }
        public string? RequestorPhonenumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public int? Requestid { get; set; }
        public int? statusId { get; set; }
        public DateTime? Requesteddate { get; set; }
        public int? RequestTypeId { get; set; }
        public int? Status { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Dateofservice { get; set; }
        public int? RegionID { get; set; }
        public string? RequestTypeName { get; set; }
        public BitArray IsFinalize { get; set; }

    }
}
