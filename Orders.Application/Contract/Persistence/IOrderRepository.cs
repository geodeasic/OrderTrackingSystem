using Orders.Domain.Entities;

namespace Orders.Application.Contract.Persistence
{
    /// <summary>
    /// Defines the contract for the order repository.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Retrieves a collection of orders associated with the specified customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer whose orders are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IEnumerable{T}> of <Order>
        /// objects associated with the specified customer ID. If no orders are found, the result will be an empty
        /// collection.</returns>
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId);

        /// <summary>
        /// Retrieves an order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Order"/> if
        /// found; otherwise, <see langword="null"/>.</returns>
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified order asynchronously.
        /// </summary>
        /// <remarks>This method updates the provided order in the underlying data store. Ensure that the
        /// <paramref name="order"/> object contains valid and up-to-date information before calling this
        /// method.</remarks>
        /// <param name="order">The order to update. Must not be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateAsync(Order order, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves all orders.
        /// </summary>
        /// <remarks>This method retrieves all orders from the underlying data source. The caller can use
        /// the <paramref name="cancellationToken"/> to cancel the operation if needed.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/>
        /// of <see cref="Order"/> objects representing all orders.</returns>
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
