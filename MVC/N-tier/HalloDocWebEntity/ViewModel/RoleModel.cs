using System.ComponentModel.DataAnnotations;
using HalloDocWebEntity.Data;

namespace HalloDocWebEntity.ViewModel
{
    public class RoleModel
    {
        [Required(ErrorMessage ="Give role name!!")]
        public string? RoleName { get; set; }
        public List<int>? RoleIds { get; set; }
        public List<Menu>? menu { get; set; }
        public List<Rolemenu>? rolemenus { get; set; }
        //[Required(ErrorMessage = "Please select Rolee...")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select Role...")]
        public int? SelectedRole { get; set; }
        public int? RoleId { get; set; }
    }
}