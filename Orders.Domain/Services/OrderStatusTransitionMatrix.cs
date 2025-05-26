using Orders.Domain.Contracts;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Services
{
    /// <summary>
    /// This class defines the transition matrix for order statuses. It specifies the 
    /// possible order statuses in the system and the valid transitions between them.
    /// </summary>
    public class OrderStatusTransitionMatrix : IOrderStatusTransitionMatrix
    {
        private readonly Dictionary<OrderStatus, HashSet<OrderStatus>> _transitions = new()
        {
            [OrderStatus.Pending] = [OrderStatus.Confirmed, OrderStatus.Cancelled],
            [OrderStatus.Confirmed] = [OrderStatus.Shipped, OrderStatus.Cancelled],
            [OrderStatus.Shipped] = [OrderStatus.Delivered, OrderStatus.Returned],
            [OrderStatus.Delivered] = [OrderStatus.Closed, OrderStatus.Returned],
            [OrderStatus.Returned] = [OrderStatus.Closed],
            [OrderStatus.Cancelled] = [], // terminal
            [OrderStatus.Closed] = [], // terminal
        };

        /// <summary>
        /// Determines whether a transition from one order status to another is allowed.
        /// </summary>
        /// <remarks>This method checks if the specified transition is defined as valid in the set of
        /// allowed transitions.</remarks>
        /// <param name="from">The current <see cref="OrderStatus"/> of the order.</param>
        /// <param name="to">The target <see cref="OrderStatus"/> to transition to.</param>
        /// <returns><see langword="true"/> if the transition from <paramref name="from"/> to <paramref name="to"/> is allowed;
        /// otherwise, <see langword="false"/>.</returns>
        public bool CanTransition(OrderStatus from, OrderStatus to)
        {
            return _transitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
        }
    }
}
