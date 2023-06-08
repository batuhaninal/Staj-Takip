using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StajTakip.MVC.Models
{
    public class ChangePasswordVM
    {
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Username { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Password { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string NewPassword { get; set; }
    }
}
