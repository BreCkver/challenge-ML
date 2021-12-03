using System.Collections.Generic;

namespace Order.API.Shared.Entities.External
{
    public class BookExternal
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string etag { get; set; }
        public VolumeInfo volumeInfo { set; get; }
    }

    public class VolumeInfo
    {
        public string title { get; set; }
        public string subTitle { get; set; }
        public List<string> authors { set; get; }
        public string publisher { set; get; }
        public string publishedDate { set; get; }
        public string description { set; get; }
        public ImageLinks imageLinks { set; get; }
        public string infoLink { set; get; }

    }

    public class ImageLinks
    {
        public string thumbnail { set; get; }
    }
}
