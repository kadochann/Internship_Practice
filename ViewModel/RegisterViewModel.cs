using System.ComponentModel.DataAnnotations;

namespace BookStoresWebAPI.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; } = "";

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4)] //password has to 4-20 characters long
        [StringLength(20)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [MinLength(4)] //password has to 4-20 characters long
        [StringLength(20)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }

}
