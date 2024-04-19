
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class ForgotPwd
    {
        [Required(ErrorMessage = "required...")]
        public string Usarname { get; set; } = null;

        [Required(ErrorMessage = "required...")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[@@$!%*?&])(?=.*\d).{8,}$", ErrorMessage = "invalid password formate")]
        public string Passwordhash { get; set; } = null;
        //[Compare("Passwordhash" , ErrorMessage = "password And Confirm Password Should match..")]
        public string? confirmPassword { get; set; } = null;
       
    }
}
