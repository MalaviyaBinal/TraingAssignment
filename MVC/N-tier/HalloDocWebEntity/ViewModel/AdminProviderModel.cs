
using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class AdminProviderModel
    {
        public List<Region> regions { get; set; }
        public List<Physicianregion> phyregions { get; set; }
        public List<Physician> physicians{ get; set; }
        public List<Role> roles{ get; set; }
        public List<int>? SelectedReg { get; set; }
        public Aspnetuser aspnetuser { get; set; }
        public Physician physician { get; set; }    
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Required...")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[@@$!%*?&])(?=.*\d).{8,}$", ErrorMessage = "Password Should contain One Upercase,lowercase letter,one digit special character.")]

        public string Passwordhash { get; set; }
        public string Businesswebsite { get; set; }
        public string Businessname { get; set; }
        public string? Altphone { get; set; }
        public string? Adminnotes { get; set; }
        public string? Zip { get; set; }
        public Decimal? log { get; set; }
        public Decimal? lat { get; set; }
        public string? City { get; set; }
        public string? Address2 { get; set; }
        public string? Address1 { get; set; }
        public string? Syncemailaddress { get; set; }
        public string? Npinumber { get; set; }
        public string? Medicallicense { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? PhotoName { get; set; }
        public string? SignatureName { get; set; }
        public bool isProviderEdit { get; set; }    
        public IFormFile AgreementDoc { get; set; }
        public IFormFile backgroundDoc { get; set; }
        public IFormFile TrainingDoc { get; set; }
        public IFormFile NonDisclosure { get; set; }
        public IFormFile LisenceDoc { get; set; }
        public IFormFile Photo { get; set; }
        public IFormFile Signature { get; set; }
        public BitArray? Isdeleted { get; set; }
        public BitArray? Isagrementdoc { get; set; }
        public BitArray? IsbackgroundDoc { get; set; }
        public BitArray? IsTrainingDoc { get; set; }
        public BitArray? IsNonDisclosure { get; set; }
        public BitArray? IsLisenceDoc { get; set; }
        public bool isPhoto { get; set; }
        public bool isSignature { get; set; }

    }
}
