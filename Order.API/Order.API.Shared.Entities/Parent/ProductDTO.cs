
using Order.API.Shared.Entities.Enums;

namespace Order.API.Shared.Entities.Parent
{
    public class ProductDTO
    {
        public int Identifier { set; get; }
        public string ExternalIdentifier { set; get; }
        public string Description { set; get; }
        public EnumProductStatus Status { set; get; }
    }
}
