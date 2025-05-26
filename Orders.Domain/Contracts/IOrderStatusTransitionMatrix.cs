using Orders.Domain.ValueObjects;

namespace Orders.Domain.Contracts
{
    /// <summary>
    /// Defines a contract for determining whether a transition between two order statuses is allowed.
    /// </summary>
    /// <remarks>
    /// This interface is used to enforce business rules regarding valid state transitions for orders.
    /// </remarks>
    public interface IOrderStatusTransitionMatrix
    {
        /// <summary>
        /// Determines whether a transition from one order status to another is allowed.
        /// </summary>
        /// <remarks>
        /// Use this method to validate whether a specific status change is permissible based on
        /// the business rules governing order status transitions.
        /// </remarks>
        /// <param name="from">The current status of the order.</param>
        /// <param name="to">The desired status to transition to.</param>
        /// <returns><see langword="true"/> if the transition from <paramref name="from"/> to <paramref name="to"/> is valid;
        /// otherwise, <see langword="false"/>.</returns>
        bool CanTransition(OrderStatus from, OrderStatus to);
    }
}
