using Orders.Application.Contract.Persistence;
using Orders.Application.Contract.Services;
using Orders.Domain.Services;
using Orders.Domain.ValueObjects;

namespace Orders.Infrastructure.Services.InMemory
{
    /// <summary>
    /// Provides functionality for managing and advancing the status of orders.
    /// </summary>
    /// <remarks>This service is responsible for validating and performing status transitions for orders based
    /// on a predefined set of rules. It interacts with an order repository to retrieve and update order data and uses a
    /// status transition matrix to enforce valid transitions.</remarks>
    /// <param name="orderRepository">The repository where all orders are stored.</param>
    public class OrderStatusService(IOrderRepository orderRepository) : IOrderStatusService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly OrderStatusTransitionMatrix _statusMatrix = new();

        /// <summary>
        /// Advances the status of an order to the specified new status if the transition is valid.
        /// </summary>
        /// <remarks>The method performs the following checks before updating the order status:
        /// <list type="bullet">
        ///     <item>
        ///         <description>
        ///         Ensures the order exists in the repository.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         Validates that the specified new status is a valid status.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         Checks if the transition from the current status to the new status is allowed based on
        ///         the status matrix.
        ///         </description>
        ///     </item>
        /// </list> 
        /// 
        /// If any of these checks fail, the method returns <see
        /// langword="false"/> without making any changes.</remarks>
        /// <param name="orderId">The unique identifier of the order to update.</param>
        /// <param name="newStatus">The new status to which the order should be transitioned.</param>
        /// <returns><see langword="true"/> if the order status was successfully updated; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> AdvanceOrderStatusAsync(Guid orderId, string newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                return false;

            var statusObj = OrderStatus.From(newStatus);

            if (statusObj == null)
                return false;

            if (!_statusMatrix.CanTransition(order.Status, statusObj))
                return false;

            order.ChangeStatus(statusObj, _statusMatrix);
            await _orderRepository.UpdateAsync(order);

            return true;
        }
    }
}
