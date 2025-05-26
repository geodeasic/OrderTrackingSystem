using Orders.Domain.Dto;

namespace Orders.Application.Contract.Services
{
    /// <summary>
    /// Defines a service for querying order data.
    /// </summary>
    /// <remarks>
    /// This interface provides methods to retrieve order information asynchronously. 
    /// Implementations of this interface should handle data retrieval and any necessary 
    /// transformations to return the requested order details.<
    /// /remarks>
    public interface IOrderQueryService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    }
}
