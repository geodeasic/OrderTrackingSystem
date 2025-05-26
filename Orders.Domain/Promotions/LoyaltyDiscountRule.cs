using Orders.Domain.Contracts;
using Orders.Domain.Entities;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Promotions
{
    /// <summary>
    /// Represents a promotion rule that applies a discount based on customer loyalty.
    /// </summary>
    /// <remarks>This rule checks if a customer's total order value exceeds a predefined 
    /// threshold and applies a percentage discount to the current order if the condition is met.</remarks>
    public class LoyaltyDiscountRule : IPromotionRule
    {
        /// <summary>
        /// Gets the name of the discount.
        /// </summary>
        public string Name => "Loyalty Discount";

        /// <summary>
        /// Determines whether the specified order matches the criteria based on the customer's profile.
        /// </summary>
        /// <param name="order">The order to evaluate.</param>
        /// <param name="customer">The customer profile used to evaluate the match criteria. Cannot be null.</param>
        /// <returns><see langword="true"/> if the customer's total order value exceeds 5; otherwise, <see langword="false"/>.</returns>
        public bool IsMatch(Order order, CustomerProfile customer)
        {
            return customer.TotalOrderValue > 5;
        }

        /// <summary>
        /// Calculates the discount amount for the order.
        /// </summary>
        /// <param name="order">The order for which the discount is to be calculated. Must not be null.</param>
        /// <returns>The discount amount as a decimal value, calculated as 10% of the order's total amount.</returns>
        public decimal CalculateDiscount(Order order)
        {
            return order.TotalAmount * 0.10m;
        }
    }
}
