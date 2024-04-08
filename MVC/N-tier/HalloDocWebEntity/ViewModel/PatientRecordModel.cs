
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class PatientRecordModel
    {
        public int rid { get; set; }
        public string Name { get; set; }
        public string createdDate { get; set; }
        public string conNo { get; set; }
        public string phyName { get; set; }
        public string concludeDate { get; set; }
        public string status { get; set; }
        public int docNo { get; set; }
    }
}
