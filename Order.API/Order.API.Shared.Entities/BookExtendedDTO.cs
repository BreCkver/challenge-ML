namespace Order.API.Shared.Entities
{
    public class BookExtendedDTO : BookDTO
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string SubTitle { get; set; }
        public string PublishedDate { set; get; }
        public string Thumbnail { set; get; }
        public string InfoLink { set; get; }

    }
}
