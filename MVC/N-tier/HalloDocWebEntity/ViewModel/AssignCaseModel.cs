
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class AssignCaseModel
    {
        public List<Physician> physicians { get; set; }
        public List<Region> regions { get; set; }
        public int SelectedRegion { get; set; }

    }
}
