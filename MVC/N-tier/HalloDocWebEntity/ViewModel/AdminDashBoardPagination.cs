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
        [Required(ErrorMessage ="Required.")]
        [EmailAddress(ErrorMessage ="Provide Valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "First name Required....")]
        public string Fname { get; set; }
        [Required(ErrorMessage = "Last name Required....")]
        public string Lname { get; set; }
        public string Notes { get; set; }
        public int TotalRecord { get; set; }
        public int FromRec { get; set; }
        public int ToRec { get; set; }

    }
}
