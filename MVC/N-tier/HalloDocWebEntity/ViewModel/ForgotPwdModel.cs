
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel

{
    public class ForgotPwdModel
    {
        [Required(ErrorMessage = "required...")]
        public string Email { get; set; } = null;

        
       
    }
}
