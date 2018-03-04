using System.ComponentModel.DataAnnotations;

namespace FableFortuneCardList.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
