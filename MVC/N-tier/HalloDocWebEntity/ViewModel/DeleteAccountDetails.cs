
using HalloDocWebEntity.Data;
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class DeleteAccountDetails
    {
        public string accountTypeName {  get; set; }    
        public int venderID { get; set; }
        public int roleId { get; set; }
        public int PhyId { get; set; }

    }
}
