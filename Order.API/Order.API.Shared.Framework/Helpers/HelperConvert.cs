using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.External;
using Order.API.Shared.Entities.Parent;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using System.Collections.Generic;
using System.Linq;

namespace Order.API.Shared.Framework.Helpers
{
    public static class HelperConvert
    {
        public static UserDTO ConverToUserDTO(this UserRequest request)
        => new UserDTO
        {
            UserName = request.UserName,
            Password = request.Password,
            StatusIdentifier = request.StatusIdentifier,
        };
        public static ResponseGeneric<BookResponse> MapBooksExternal(BookExternalResponse external)
        {
            if (external == null || external.items == null || external.items.Count <= 0)
            {
                return ResponseGeneric.CreateError<BookResponse>(new Error(ErrorCode.EXTERNALAPI_EMPTY, ErrorMessage.EXTERNALAPI_EMPTY, ErrorType.BUSINESS));
            }

            var bookList = new BookResponse
            {
                BookList = external.items.Select(i => new BookDTO
                {
                    Authors = i.volumeInfo?.authors,
                    Description = i.volumeInfo?.description,
                    ExternalIdentifier = i.id,
                    Title = i.volumeInfo?.title,
                    Publisher = i.volumeInfo?.publisher,
                    Status = Entities.Enums.EnumProductStatus.New,
                    Keyword = string.Empty
                }).ToList()
            };

            return ResponseGeneric.Create(bookList);
        }

        public static ResponseGeneric<BookExtendedDTO> MapBookExternal(BookExternal external)
        {
            if (external == null || external.volumeInfo == null)
            {
                return ResponseGeneric.CreateError<BookExtendedDTO>(new Error(ErrorCode.EXTERNALAPI_EMPTY, ErrorMessage.EXTERNALAPI_EMPTY, ErrorType.BUSINESS));
            }

            var book = new BookExtendedDTO
            {
                Authors = external.volumeInfo?.authors,
                Description = external.volumeInfo?.description ?? string.Empty,
                ExternalIdentifier = external.id,
                Title = external.volumeInfo?.title,
                Publisher = external.volumeInfo?.publisher,
                Etag = external.etag,
                Kind = external.kind,
                PublishedDate = external.volumeInfo?.publishedDate,
                SubTitle = external.volumeInfo?.subTitle,
                Thumbnail = external.volumeInfo?.imageLinks?.thumbnail,
                InfoLink = external.volumeInfo?.infoLink,
                Keyword = string.Empty,
                Status = Entities.Enums.EnumProductStatus.Active,
                
            };

            return ResponseGeneric.Create(book);
        }

        public static OrderDTO ConverToOrderDTO(this WishListRequest request)
           => new OrderDTO
           {
               Name = request.WishList.Name,
               Identifier = request.WishList.Identifier,
               Status = request.WishList.Status,
           };

        public static List<WishListDTO> ConvertToWishLists(this IEnumerable<OrderDTO> list)
            => list.Select(l => new WishListDTO
            {
                Identifier = l.Identifier,
                Name = l.Name,
                Status = l.Status
            }).ToList();

        public static long? ValidateOrderIdentifier(long orderIdentifier)
            =>
                orderIdentifier == 0 ? (long?)null : orderIdentifier;
        public static UserDTO ConverToUserDTONameOnly(UserDTO user)
            =>
                new UserDTO { UserName = user.UserName };
    }
}
