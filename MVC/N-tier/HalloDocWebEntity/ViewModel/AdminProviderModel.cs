
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class AdminProviderModel
    {
        public List<Region> regions { get; set; }
        public List<Physician> physicians{ get; set; }
        public Aspnetuser aspnetuser { get; set; }
        public Physician physician { get; set; }    
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Passwordhash { get; set; }
        public string Businesswebsite { get; set; }
        public string Businessname { get; set; }
        public string? Altphone { get; set; }
        public string? Adminnotes { get; set; }
        public string? Zip { get; set; }
        public string? City { get; set; }
        public string? Address2 { get; set; }
        public string? Address1 { get; set; }
        public string? Syncemailaddress { get; set; }
        public string? Npinumber { get; set; }
        public string? Medicallicense { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }

    }
}
