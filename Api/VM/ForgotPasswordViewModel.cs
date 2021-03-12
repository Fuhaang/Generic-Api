using System.ComponentModel.DataAnnotations;

namespace Api.VM
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
