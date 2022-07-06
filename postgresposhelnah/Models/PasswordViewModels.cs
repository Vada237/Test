using System.ComponentModel.DataAnnotations;

namespace postgresposhelnah.Models
{
    public class ForgotPasswordViewModel { 
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }

    public class ResetPasswordViewModel { 

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]

        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
