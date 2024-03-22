using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class Profile
    {
        public List<Request>? Request { get; set; }
        public User? User { get; set; }
        public DateTime? dob { get; set; } 
    }
}
