using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class InternshipsBookPageAddDto
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        [MaxLength(100, ErrorMessage = "{0} alani {1} karakterden büyük olmamalıdır.")]
        [MinLength(2, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır")]
        public string Title { get; set; }
        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} alani bos geçilmemelidir.")]
        [MinLength(2, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır")]
        public string Content { get; set; }
        [DisplayName("Tarih")]
        [DataType(DataType.Date,ErrorMessage ="Lütfen '29-12-2020' formatında giriş yapınız!")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        public DateTime Date { get; set; }
        [DisplayName("Çalışma Günleri")]
        [Required(ErrorMessage = "{0} alanı boş geçilmemelidir.")]
        [MaxLength(4, ErrorMessage = "{0} alani {1} karakterden büyük olmamalıdır.")]
        [MinLength(1, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır")]
        public string WorkDays { get; set; }
        public string Summary { get; set; }
    }
}
