using Order.API.Shared.Entities;
using System.Threading.Tasks;

namespace Order.API.Business.Contracts
{
    public interface IProductHandler<in T, R>
    {
        Task<ResponseGeneric<bool>> IsValid(T request);

        Task<ResponseGeneric<R>> Execute(T request);
    }
}
