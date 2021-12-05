using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Constants;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using System.Threading.Tasks;

namespace Order.API.Business.Products
{
    /// <summary>
    /// Clase has responsibility of search products in external api
    /// </summary>
    public class BookFilterHandler : IProductHandler<BookFilterRequest, BookResponse>
    {
        private readonly IGoogleApiService googleApiService;
        private readonly IUserRepository repository;
        public BookFilterHandler(IGoogleApiService googleApiService, IUserRepository repository)
        {
            this.googleApiService = googleApiService;
            this.repository = repository;
        }
        public async Task<ResponseGeneric<BookResponse>> Execute(BookFilterRequest request)
        {
            var requestValid = await IsValid(request);
            if (requestValid.Success)
            {
                var respose = await googleApiService.GetBooksFilter(request.Book.Keyword, request.Book.Title, request.Book.Publisher, request.Book.Authors[0]);
                if(respose.Failure)
                {
                    return ResponseGeneric.CreateError<BookResponse>(respose.ErrorList);
                }
                return ResponseGeneric.Create(respose.Value);
            }
            return ResponseGeneric.CreateError<BookResponse>(requestValid.ErrorList);

        }

        public async Task<ResponseGeneric<bool>> IsValid(BookFilterRequest request)
        {
            if (request == null || request.Book == null || request.User == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_NULL, ErrorMessage.REQUEST_NULL, ErrorType.BUSINESS));
            }

            if (ValidateRequest(request))
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.REQUEST_EMPTY, ErrorMessage.REQUEST_EMPTY, ErrorType.BUSINESS));
            }

            var UserExists = await repository.GetByUser(request.User);
            if (UserExists.Failure)
            {
                return UserExists.AsError<bool>();
            }

            if (UserExists.Value == null)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.USER_NOEXISTS, ErrorMessage.USER_NOEXISTS, ErrorType.BUSINESS));
            }
            return ResponseGeneric.Create(true);
        }

        private bool ValidateRequest(BookFilterRequest request) =>
        string.IsNullOrWhiteSpace(request.User.Token) ||
        string.IsNullOrWhiteSpace(request.Book.Keyword) ||
             (
                (request.Book.Authors == null || request.Book.Authors.Count <= 0) && 
                  string.IsNullOrWhiteSpace(request.Book.Title) &&
                  string.IsNullOrWhiteSpace(request.Book.Publisher)
             );
    }
}
