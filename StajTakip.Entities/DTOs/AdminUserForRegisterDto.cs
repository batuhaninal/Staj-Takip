using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StajTakip.Entities.DTOs
{
    public class AdminUserForRegisterDto
    {
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Password { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsCompany { get; set; }
    }
}
