
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class Notes
    {
        public string AdminNote { get; set; }
        public string PhyNotes { get; set; }
        public List<string> transfreNotes { get; set; }
        public int req_id { get; set; } 

    }
}
