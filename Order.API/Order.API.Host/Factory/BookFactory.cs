using Order.API.Business.Contracts;
using Order.API.Business.Products;
using Order.API.Data;
using Order.API.Data.Agent;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Request;
using Order.API.Shared.Entities.Response;
using Order.API.Shared.Framework.Helpers;

namespace Order.API.Host.Factory
{
    public class BookFactory
    {
        private readonly string connectionString;
        public BookFactory()
        {
            connectionString = Helper.GetConnection();
        }

        public ResponseGeneric<ICommandHandler<BookFilterRequest, BookResponse>> Create()
        {
            var data = new UserData(connectionString);
            var service = new GoogleApiService(Helper.GetGoogleApi());
            var handler = new BookFilterHandler(service, data);
            return ResponseGeneric.Create((ICommandHandler<BookFilterRequest, BookResponse>)handler);
        }
    }

}