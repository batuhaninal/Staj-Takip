using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class Signature : EntityBase
    {
        public string Path { get; set; }
        public bool IsManager { get; set; }
        public bool IsLecturer { get; set; }
        public int InternshipBookId { get; set; }
        public virtual InternshipsBook? InternshipsBook { get; set; }
    }
}
