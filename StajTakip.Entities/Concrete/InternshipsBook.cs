using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class InternshipsBook : EntityBase
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompanyChecked { get; set; } = false;
        public bool IsTeacherChecked { get; set; } = false;
        public int StudentUserId { get; set; }
        public virtual StudentUser? StudentUser { get; set; }
        public virtual ICollection<Signature>? Signatures { get; set; }
    }
}
