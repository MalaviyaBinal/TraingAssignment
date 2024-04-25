﻿
using HalloDocWebEntity.Data;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class AdminProviderModel
    {
        public List<Region>? regions { get; set; }
        public List<Physicianregion>? phyregions { get; set; }
        public List<Physician>? physicians { get; set; }
        public List<Role>? roles { get; set; }
        public List<int>? SelectedReg { get; set; }
        public Aspnetuser? aspnetuser { get; set; }
        public Physician? physician { get; set; }
        public int roleid { get; set; }
        public int regionid { get; set; }
        [Required(ErrorMessage = "First name Required....")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Last name Required....")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "password Required...")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[@@$!%*?&])(?=.*\d).{8,}$", ErrorMessage = "Password Should contain One Upercase,lowercase letter,one digit special character.")]

        public string Passwordhash { get; set; }
        [Required(ErrorMessage = "Businesswebsite Required...")]
        public string Businesswebsite { get; set; }
        [Required(ErrorMessage = "Businessname Required...")]
        public string Businessname { get; set; }
        //[Required(ErrorMessage = "Mobile Required....")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string? Altphone { get; set; }
        public string? Adminnotes { get; set; }
        [Required(ErrorMessage = "Zipcode Required....")]
        public string? Zip { get; set; }
        public Decimal? log { get; set; }
        public Decimal? lat { get; set; }
        [Required(ErrorMessage = "Address1 Required....")]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "Address2 Required....")]
        public string Address2 { get; set; }
        [Required(ErrorMessage = "City Required....")]
        public string City { get; set; }
        [Required(ErrorMessage = "Sync email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string? Syncemailaddress { get; set; }
        [Required(ErrorMessage = "Npinumber Required....")]
        public string? Npinumber { get; set; }
        [Required(ErrorMessage = "Medicallicense Required...")]
        public string? Medicallicense { get; set; }
        [Required(ErrorMessage = "Mobile Number is Required.")]
        [RegularExpression(@"^\+[0-9\-\(\)\/\.]{6,15}[0-9]$", ErrorMessage = "Enter a valid phone number with country code.")]
        public string? Mobile { get; set; }
        [Required(ErrorMessage = "Email Required....")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string? Email { get; set; }
        public string? PhotoName { get; set; }
        public string? SignatureName { get; set; }
        public bool isProviderEdit { get; set; }
        public IFormFile? AgreementDoc { get; set; } 
        public IFormFile? backgroundDoc { get; set; }
        public IFormFile? TrainingDoc { get; set; }
        public IFormFile? NonDisclosure { get; set; }
        public IFormFile? LisenceDoc { get; set; }
        public IFormFile? Photo { get; set; }
        public IFormFile? Signature { get; set; }
        public BitArray? Isdeleted { get; set; }
        public BitArray? Isagrementdoc { get; set; }
        public BitArray? IsbackgroundDoc { get; set; }
        public BitArray? IsTrainingDoc { get; set; }
        public BitArray? IsNonDisclosure { get; set; }
        public BitArray? IsLisenceDoc { get; set; }
        public bool? isPhoto { get; set; }
        public bool? isSignature { get; set; }

    }
}
