
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class AdminProfile
    {
        public Admin admin { get; set; }  
        public Aspnetuser adminuser { get; set; }
        public Region region { get; set; }
        public List<Adminregion> adminregion { get; set; }
        public List<Region> regions { get; set; }
        public List<int>? SelectedReg { get; set; }      
        public List<Role> roles { get; set; }
        public int regionid { get; set; }
        public int roleid { get; set; }
        public bool isAdminProfile { get; set; }

    }
}
