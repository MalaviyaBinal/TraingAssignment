using System.ComponentModel.DataAnnotations;
using HalloDocWebEntity.Data;

namespace HalloDocWebEntity.ViewModel
{
    public class RoleModel
    {
        public string? RoleName { get; set; }
        public List<int> RoleIds { get; set; }
        public List<Menu> menu { get; set; }
        public List<Rolemenu> rolemenus { get; set; }
        public int? SelectedRole { get; set; }
        public int RoleId { get; set; }
    }
}