
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class SendOrderModel
    {
        public List<Healthprofessionaltype>? professions { get; set; }
        public List<Healthprofessional>? business { get ; set; }
        public Healthprofessional? businessDetail { get; set; }
        public string? prescription { get; set; }
        public int? noOfRetail { get; set; }
        public int? req_id { get; set; } 
        public int? BusinessSelect { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Profession...")]
        public int? SelectedProfession { get; set; }
        [Required(ErrorMessage = "Please Enter BusinessContact... ")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string? businesscontact { get; set; }
        [Required(ErrorMessage = "Email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string? email { get; set; }
        [Required(ErrorMessage = "Please Enter Fax Number...")]
        public string? FaxNumber { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string? Phonenumber { get; set; }
        [Required(ErrorMessage ="Please Enter Business name...")]
        public string? Vendorname { get; set; }
        [Required(ErrorMessage = "Please Enter Address...")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Please Enter city...")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Please Enter state...")]
        public string? State { get; set; }
        [Required(ErrorMessage = "Please Enter zipcode...")]
        public string? Zip { get; set; }


    }
}
