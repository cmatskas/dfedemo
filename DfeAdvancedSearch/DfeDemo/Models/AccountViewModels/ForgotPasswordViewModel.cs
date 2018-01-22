using System.ComponentModel.DataAnnotations;

namespace DfeDemo.Models.AccountViewModels
{
	public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
