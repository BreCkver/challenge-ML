using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.API.Web.Entities
{
    public class ResponseDTO
    {
        public ResponseDTO()
        {
            Success = true;
        }
        public Meta Meta { get; set; }

        public bool Success { set; get; }
    }
    public class ResponseDTO<T> : ResponseDTO
    {
        public ResponseDTO() { }
        public T Data { get; set; }
    }
    public class Meta
    {
        public Meta() { }
        public string Status { get; set; }
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        public Message() { }
        public string Code { get; set; }
        public string Text { get; set; }
    }

    public class ResponseUser
    {
        public UserDTO User { set; get; }
    }

    public class WishListResponse
    {
        public OrderDTO WishList { set; get; }
    }

    public class WishListFilterResponse
    {
        public List<WishListDTO> WishLists { set; get; }
    }

    public class BookResponse
    {
        public List<BookDTO> BookList { set; get; }
    }
}