using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class BookImage : EntityBase
    {
        public string ImageName { get; set; }
        public byte[] Data { get; set; }
        public int InternshipsBookId { get; set; }
        public InternshipsBook? InternshipsBook { get; set; }
    }
}
