namespace Orders.Application.Contract.Services
{
    /// <summary>
    /// Defines the contract for managing the status of orders.
    /// </summary>
    public interface IOrderStatusService
    {
        /// <summary>
        /// Advances the status of an order to the specified new status.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="newStatus"/> is valid and applicable for the current
        /// state of the order. Invalid transitions or unrecognized statuses may result in the operation
        /// failing.</remarks>
        /// <param name="orderId">The unique identifier of the order to update.</param>
        /// <param name="newStatus">The new status to assign to the order. Must be a valid 
        /// status recognized by the system.</param>
        /// <returns><see langword="true"/> if the order status was successfully updated; 
        /// otherwise, <see langword="false"/>.</returns>
        Task<bool> AdvanceOrderStatusAsync(Guid orderId, string newStatus);
    }
}
