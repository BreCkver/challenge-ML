using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Response;
using System.Threading.Tasks;

namespace Order.API.Business.Contracts
{
    public interface IGoogleApiService
    {
        Task<ResponseGeneric<BookResponse>> GetBooksFilter(string keyword, string title, string publisher, string author);

        Task<ResponseGeneric<BookExtendedDTO>> GetBookById(string externalIdentifier);
    }
}
