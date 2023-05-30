using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class InternshipsBookPageUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} alani bos geçilmemelidir.")]
        [MinLength(2, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır")]
        public string Content { get; set; }

        [DisplayName("Tarih")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Lutfen giris yapiniz!")]
        public int StudentUserId { get; set; }
    }
}
