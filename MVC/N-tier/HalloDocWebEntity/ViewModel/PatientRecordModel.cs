
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class PatientRecordModel
    {
        public List<PatientRecordModel> records { get; set; }
        public int rid { get; set; }
        public string Name { get; set; }
        public string createdDate { get; set; }
        public string conNo { get; set; }
        public string phyName { get; set; }
        public string concludeDate { get; set; }
        public string status { get; set; }
        public int docNo { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
