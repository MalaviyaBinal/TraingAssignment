using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class AdminDashBoardPagination
    {
        public List<Physician> physicians {  get; set; }    
        public List<Region> regions { get; set; }
        public List<AdminDashboardTableModel> tabledata { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool PreviousPage  { get; set; }
        public bool NextPage { get; set; }
        public AdminDashboard adminCount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Notes { get; set; }
        public int TotalRecord { get; set; }
        public int FromRec { get; set; }
        public int ToRec { get; set; }

    }
}
