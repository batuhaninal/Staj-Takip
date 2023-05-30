using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class InternshipsBookPageListDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool TeacherIsChecked { get; set; }
        public bool CompanyIsChecked { get; set; }
    }
}
