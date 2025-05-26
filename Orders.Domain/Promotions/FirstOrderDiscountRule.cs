using Orders.Domain.Contracts;
using Orders.Domain.Entities;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Promotions
{
    /// <summary>
    /// The rule for applying a discount to a customer's first order.
    /// </summary>
    public class FirstOrderDiscountRule : IPromotionRule
    {
        /// <summary>
        /// Gets the name of the discount.
        /// </summary>
        public string Name => "First Order Discount";

        /// <summary>
        /// Determines whether the specified order matches the criteria for a new customer with no past orders.
        /// </summary>
        /// <param name="order">The order to evaluate. This parameter is not used in the 
        /// current implementation but may be relevant in future extensions.</param>
        /// <param name="customer">The customer profile to evaluate. Must not be 
        /// <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the customer belongs to the "New" segment and has no past orders; otherwise, <see
        /// langword="false"/>.</returns>
        public bool IsMatch(Order order, CustomerProfile customer)
        {
            return customer.Segment == CustomerSegment.New && customer.PastOrderCount == 0;
        }

        /// <summary>
        /// Calculates the discount amount for the specified order.
        /// </summary>
        /// <param name="order">The order for which the discount is to be calculated. The <see cref="Order.TotalAmount"/> property must be
        /// greater than or equal to 0.</param>
        /// <returns>The discount amount as a decimal, calculated as 10% of the order's total amount.</returns>
        public decimal CalculateDiscount(Order order)
        {
            return order.TotalAmount * 0.10m;
        }
    }
}
