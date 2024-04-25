
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class AdminProfile
    {
        public Admin admin { get; set; }  
        public Aspnetuser adminuser { get; set; }
        public Region region { get; set; }
        public List<Adminregion> adminregion { get; set; }
        public List<Region> regions { get; set; }
        public List<int>? SelectedReg { get; set; }      
        public List<Role> roles { get; set; }
        
        public int roleid { get; set; }
        public bool isAdminProfile { get; set; }
        [Required(ErrorMessage = "Password required...")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[@@$!%*?&])(?=.*\d).{8,}$", ErrorMessage = "invalid password formate")]
        public string Passwordhash { get; set; }

        [Required(ErrorMessage = "First name Required....")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Last name Required....")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        [Compare(nameof(Email), ErrorMessage = "Email and Confirm Email must be same..")]
        public string Con_Email { get; set; }
        [Required(ErrorMessage = "Address1 Required....")]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "Address2 Required....")]
        public string Address2 { get; set; }
        [Required(ErrorMessage = "City Required....")]
        public string City { get; set; }
        [Required(ErrorMessage = "Zipcode Required....")]
        public string Zip { get; set; }
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string Altphone { get; set; }
        public int regionid { get; set; }

    }
}
