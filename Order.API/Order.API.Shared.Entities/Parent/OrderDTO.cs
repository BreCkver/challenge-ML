using Order.API.Shared.Entities.Enums;

namespace Order.API.Shared.Entities.Parent
{
    public class OrderDTO
    {
        public long Identifier { set; get; }

        public int Status { set; get; }

        public string Name { set; get; }
    }
}
