
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class SendOrderModel
    {
        public List<Healthprofessionaltype> professions { get; set; }
        public List<Healthprofessional> business { get ; set; }
        public Healthprofessional businessDetail { get; set; }
        public string? prescription { get; set; }
        public int? noOfRetail { get; set; }
        public int? req_id { get; set; } 
        public int? BusinessSelect { get; set; }
        public int? SelectedProfession { get; set; }
        public string? businesscontact { get; set; }
        public string? email { get; set; }
        public string? FaxNumber { get; set; }


    }
}
