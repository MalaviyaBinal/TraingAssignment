
using HalloDocWebEntity.Data;

namespace HalloDocWebEntity.ViewModel
{
    public class UserAccess
    {
        public List<Aspnetuser> Aspnetuser { get; set; }
        public List<Admin> admins { get; set; }
        public List<Physician> physicsian { get; set; }
        public short? adminstatus { get; set; }
        public short? physicianstatus { get; set; }
        public string Usarname { get; set; }
        public string? Phonenumber { get; set; }
        public string? role { get; set; }

        public List<Role> AccessRoles { get; set; }
        public Dictionary<int, int> count { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool PreviousPage { get; set; }
        public bool NextPage { get; set; }
    }
}
