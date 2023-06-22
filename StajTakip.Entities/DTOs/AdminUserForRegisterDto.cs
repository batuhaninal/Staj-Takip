using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StajTakip.Entities.DTOs
{
    public class AdminUserForRegisterDto
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Username { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MinLength(7, ErrorMessage = "{0} alanı minimum {1} karakter olmalıdır!")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrarı")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MinLength(7, ErrorMessage = "{0} alanı minimum {1} karakter olmalıdır!")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmamaktadır!")]
        public string RepeatPassword { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsCompany { get; set; }

        public int? StudentId { get; set; }
    }
}
