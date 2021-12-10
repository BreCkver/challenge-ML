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
    /// <summary>
    /// 
    /// </summary>
    public class BookFactory
    {
        private readonly string connectionString;
        /// <summary>
        /// 
        /// </summary>
        public BookFactory()
        {
            connectionString = Helper.GetConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseGeneric<ICommandHandler<BookFilterRequest, BookResponse>> Create()
        {
            var data = new UserData(connectionString);
            var service = new GoogleApiService(Helper.GetGoogleApi());
            var handler = new BookFilterHandler(service, data);
            return ResponseGeneric.Create((ICommandHandler<BookFilterRequest, BookResponse>)handler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResponseGeneric<ICommandHandler<BookFilterRequest, BookExtendedDTO>> CreateBookFilter()
        {
            var data = new UserData(connectionString);
            var service = new GoogleApiService(Helper.GetGoogleApi());
            var handler = new BookFilterItemHandler(service, data);
            return ResponseGeneric.Create((ICommandHandler<BookFilterRequest, BookExtendedDTO>)handler);
        }
    }

}