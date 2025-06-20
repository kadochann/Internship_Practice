using System.ComponentModel.DataAnnotations;

namespace BookStoresWebAPI.ViewModel
{
    public class LoginViewModel
{
        [Required] 
        [Display(Name = "Username")]
        public string UserName { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)] //password has to 4-20 characters long
        [StringLength(20)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";
        
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false; // Default value is false, meaning the user will not be remembered unless explicitly checked


    }
}
