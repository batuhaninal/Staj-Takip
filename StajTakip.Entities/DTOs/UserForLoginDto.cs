using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class UserForLoginDto
    {
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Username { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        public string Password { get; set; }
    }
}
