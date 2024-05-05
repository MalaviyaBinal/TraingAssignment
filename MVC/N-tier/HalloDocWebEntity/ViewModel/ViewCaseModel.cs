
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class ViewCaseModel
    {
        public string Notes { get; set; }
        public int requestId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DOB { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string RegionName { get;  set; }
        public string Address { get; set; }
        public int status { get; set; }
    }
}
