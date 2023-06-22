using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class StudentUserForRegisterDto
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
        [MinLength(7,ErrorMessage ="{0} alanı minimum {1} karakter olmalıdır!")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrarı")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [MinLength(7, ErrorMessage = "{0} alanı minimum {1} karakter olmalıdır!")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmamaktadır!")]
        public string RepeatPassword { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? StudentNumber { get; set; }
    }
}
