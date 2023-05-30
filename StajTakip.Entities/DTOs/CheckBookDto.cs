using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Entities.DTOs
{
    public class CheckBookDto
    {
        public int Id { get; set; }
        public bool IsTeacherChecked { get; set; }
        public bool IsCompanyChecked { get; set; }
    }
}
