using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class BookImage : EntityBase
    {
        public string Path { get; set; }
        public int InternshipBookId { get; set; }
        public virtual InternshipsBook? InternshipsBook { get; set; }
    }
}
