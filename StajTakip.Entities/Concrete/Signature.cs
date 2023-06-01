using Core.Entities.Abstract;

namespace StajTakip.Entities.Concrete
{
    public class Signature : EntityBase
    {
        public byte[] SignatureData { get; set; }
        public string Name { get; set; }
    }
}
