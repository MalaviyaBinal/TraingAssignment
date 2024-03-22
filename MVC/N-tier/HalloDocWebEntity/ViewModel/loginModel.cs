
using System.ComponentModel.DataAnnotations;

namespace HalloDocWebEntity.ViewModel
{
    public class loginModel
    {
        [Required(ErrorMessage = "required...")]
        public string Usarname { get; set; } = null;

        [Required(ErrorMessage = "required...")]
        public string Passwordhash { get; set; } = null;
        [Compare("Passwordhash" , ErrorMessage = "password And Confirm Password Should match..")]
        public string confirmPassword { get; set; } = null;
       
    }
}
