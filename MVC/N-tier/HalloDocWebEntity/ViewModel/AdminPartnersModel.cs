
using HalloDocWebEntity.Data;

namespace HalloDocWebEntity.ViewModel
{
    public class AdminPartnersModel
    {
        public List<Healthprofessionaltype> vendorList { get; set; }
        public List<Healthprofessionaltype> professions { get; set; }
        public int SelectedRegion { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
