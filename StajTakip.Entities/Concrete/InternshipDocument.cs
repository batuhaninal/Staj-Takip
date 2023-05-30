using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class InternshipDocument : EntityBase
    {
        public byte[] Data { get; set; }
        public int StudentUserId { get; set; }

        public virtual StudentUser? StudentUser { get; set; }
    }
}
