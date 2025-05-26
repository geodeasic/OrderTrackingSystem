using Orders.Domain.Entities;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Contracts
{
    /// <summary>
    /// This interface declares rules for applying promotional discounts to orders.
    /// </summary>
    public interface IPromotionRule
    {
        /// <summary>
        /// Human-readable name of the rule.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Determines whether the specified order matches the criteria for the given customer profile.
        /// </summary>
        /// <param name="order">The order to evaluate against the criteria.</param>
        /// <param name="customer">The customer profile used to evaluate the order.</param>
        /// <returns><see langword="true"/> if the order matches the criteria for the customer profile; otherwise, <see
        /// langword="false"/>.</returns>
        bool IsMatch(Order order, CustomerProfile customer);

        /// <summary>
        /// Calculates the discount to be applied to the specified order.
        /// </summary>
        /// <param name="order">The order for which the discount is to be calculated. Must not be null.</param>
        /// <returns>The discount amount as a decimal. Returns 0 if no discount is applicable.</returns>
        decimal CalculateDiscount(Order order);
    }
}
