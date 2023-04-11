using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class InternshipsBook : EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string WorkDays { get; set; }
        public string Summary { get; set; }
        public virtual ICollection<Signature>? Signatures { get; set; }
        public virtual ICollection<BookImage>? BookImages { get; set; }
    }
}
