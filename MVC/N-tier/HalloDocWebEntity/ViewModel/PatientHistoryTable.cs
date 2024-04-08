
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class PatientHistoryTable
    {
        public int? aspId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public string Address { get; set; }
    }
}
