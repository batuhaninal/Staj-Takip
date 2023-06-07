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
        public string Password { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsCompany { get; set; }

        public int? StudentId { get; set; }
    }
}
