using System.ComponentModel.DataAnnotations;

namespace Api.VM
{
    /// <summary>
    /// Model for ForgotPassword
    /// </summary>
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
