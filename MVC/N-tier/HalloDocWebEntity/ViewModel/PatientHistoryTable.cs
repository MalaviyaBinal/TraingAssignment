
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class PatientHistoryTable
    {
        public List<PatientHistoryTable> patientHistoryData {  get; set; }
        public int? aspId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }
        public string Address { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
