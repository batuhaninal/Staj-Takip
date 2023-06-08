using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class InternshipDocument : EntityBase
    {
        public byte[] Data { get; set; }
        public string DocumentName { get; set; }
        public bool IsTeacherChecked { get; set; }
        public bool? IsCompanyChecked { get; set; }
        public int StudentUserId { get; set; }

        public virtual StudentUser? StudentUser { get; set; }
    }
}
