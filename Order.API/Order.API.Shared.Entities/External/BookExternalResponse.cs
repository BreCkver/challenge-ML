using System.Collections.Generic;

namespace Order.API.Shared.Entities.External
{
    public class BookExternalResponse
    {
        public string kind { get; set; }
        public int totalItems { get; set; }
        public List<BookExternal> items { get; set; }
    }
}
