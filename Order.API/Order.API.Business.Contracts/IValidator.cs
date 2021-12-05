using Order.API.Shared.Entities;
using System.Threading.Tasks;

namespace Order.API.Business.Contracts
{
    public interface IValidator<in T>
    {
        Task<ResponseGeneric> IsRequestValid(T request);
    }
}
