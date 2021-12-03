using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.External;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using System.Linq;

namespace Order.API.Shared.Framework.Helpers
{
    public static class HelperConvert
    {
        public static UserDTO ConverToUserDTO(this UserCreatedRequest request)
        {
            return new UserDTO
            {
                UserName = request.UserName,
                Password = request.Password,
            };
        }

        public static ResponseGeneric<BookResponse> MapBooksExternal(BookExternalResponse external)
        {
            if(external == null || external.items == null || external.items.Count <= 0)
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
                }).ToList()
            };

            return ResponseGeneric.Create(bookList);
        }

        public static ResponseGeneric<BookExtendedDTO> MapBookExternal(BookExternal external)
        {
            if (external == null || external.volumeInfo == null )
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
                InfoLink = external.volumeInfo?.infoLink
            };

            return ResponseGeneric.Create(book);
        }
    }
}
