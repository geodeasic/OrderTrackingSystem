using Orders.Domain.Contracts;
using Orders.Domain.Exceptions;
using Orders.Domain.Promotions;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Entities
{
    /// <summary>
    /// This class represents an order placed by a customer. It is a database entity.
    /// </summary>
    public class Order(Guid id, Guid customerId, decimal totalAmount, decimal total)
    {
        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; private set; } = id;

        /// <summary>
        /// Gets the unique identifier for the customer.
        /// </summary>
        public Guid CustomerId { get; private set; } = customerId;

        /// <summary>
        /// Gets the total monetary amount associated with the current operation or transaction.
        /// </summary>
        public decimal TotalAmount { get; private set; } = totalAmount;

        /// <summary>
        /// Gets the current status of the order.
        /// </summary>
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        /// <summary>
        /// Gets the date and time when the order was created, in Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the date and time when the order was fulfilled, or <see langword="null"/> 
        /// if it has not been fulfilled.
        /// </summary>
        public DateTime? FulfilledAt { get; private set; }

        /// <summary>
        /// Gets the order total after applying all applicable discounts.
        /// </summary>
        public decimal DiscountedTotal { get; private set; } = total; // Default to full price

        /// <summary>
        /// Gets the list of promotions that have been applied to this order.
        /// </summary>
        public List<string> AppliedPromotions { get; private set; } = [];

        /// <summary>
        /// Changes the status of the order to the specified new status.
        /// </summary>
        /// <remarks>If the new status is <see cref="OrderStatus.Delivered"/> or <see
        /// cref="OrderStatus.Closed"/>,  the <c>FulfilledAt</c> property is updated to 
        /// the current UTC time.</remarks>
        /// <param name="newStatus">The new status to transition the order to.</param>
        /// <param name="matrix">An implementation of <see cref="IOrderStatusTransitionMatrix"/> 
        /// that determines whether the transition  from the current status to the specified new
        /// status is allowed.</param>
        /// <exception cref="InvalidStatusTransitionException">Thrown if the transition from the 
        /// current status to <paramref name="newStatus"/> is not allowed  according
        /// to the provided <paramref name="matrix"/>.</exception>
        public void ChangeStatus(OrderStatus newStatus, IOrderStatusTransitionMatrix matrix)
        {
            if (!matrix.CanTransition(Status, newStatus))
                throw new InvalidStatusTransitionException(Status);

            Status = newStatus;

            if (newStatus == OrderStatus.Delivered || newStatus == OrderStatus.Closed)
                FulfilledAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Applies the specified discount to this order based on the provided promotion result.
        /// </summary>
        /// <remarks>This method updates the <see cref="DiscountedTotal"/> and <see
        /// cref="AppliedPromotions"/> properties  based on the values in the provided <see cref="PromotionResult"/>. If
        /// the <see cref="PromotionResult"/>  contains no applied promotions, <see cref="AppliedPromotions"/> will be
        /// set to an empty list.</remarks>
        /// <param name="result">The <see cref="PromotionResult"/> containing the discount details 
        /// to apply.  If <paramref name="result"/> is <see langword="null"/>, no changes are made.</param>
        public void ApplyDiscount(PromotionResult result)
        {
            if (result == null)
                return;

            DiscountedTotal = result.DiscountedTotal;
            AppliedPromotions = result.AppliedPromotions?.ToList() ?? [];
        }
    }
}
