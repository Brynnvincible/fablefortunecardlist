using System.ComponentModel.DataAnnotations;

namespace FableFortuneCardList.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
