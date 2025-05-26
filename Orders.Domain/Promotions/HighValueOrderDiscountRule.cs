using Orders.Domain.Contracts;
using Orders.Domain.Entities;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Promotions
{
    /// <summary>
    /// Represents a promotion rule that applies a discount to high-value orders.
    /// </summary>
    /// <remarks>This rule applies a 5% discount to orders with a total amount exceeding 500.</remarks>
    public class HighValueOrderDiscountRule : IPromotionRule
    {
        /// <summary>
        /// Gets the name of the discount.
        /// </summary>
        public string Name => "High Value Order Discount";

        /// <summary>
        /// Calculates the discount for the specified order.
        /// </summary>
        /// <param name="order">The order for which the discount is to be calculated. Must not be null.</param>
        /// <returns>The discount amount as a decimal, calculated as 5% of the order's total amount.</returns>
        public decimal CalculateDiscount(Order order)
        {
            return order.TotalAmount * 0.05m;
        }

        /// <summary>
        /// Determines whether the specified order matches the criteria for a high-value transaction.
        /// </summary>
        /// <param name="order">The order to evaluate. Must not be <c>null</c>.</param>
        /// <param name="customer">The customer profile associated with the order. Must not be <c>null</c>.</param>
        /// <returns><see langword="true"/> if the order's total amount exceeds 500; otherwise, <see langword="false"/>.</returns>
        public bool IsMatch(Order order, CustomerProfile customer)
        {
            return order.TotalAmount > 500m;
        }
    }
}
