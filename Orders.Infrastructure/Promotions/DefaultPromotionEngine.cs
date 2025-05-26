using Orders.Application.Contract.Promotion;
using Orders.Domain.Contracts;
using Orders.Domain.Entities;
using Orders.Domain.Promotions;
using Orders.Domain.Services;
using Orders.Domain.ValueObjects;

namespace Orders.Infrastructure.Promotions
{
    /// <summary>
    /// Represents the default implementation of the promotion engine that applies promotional rules to orders.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DefaultPromotionEngine"/> class with the specified promotion
    /// rules.
    /// </remarks>
    /// <param name="rules">A collection of promotion rules to be applied by the engine. Cannot be null.</param>
    public class DefaultPromotionEngine(IEnumerable<IPromotionRule> rules) : IPromotionEngine
    {
        private readonly IEnumerable<IPromotionRule> _rules = rules;

        /// <summary>
        /// Applies promotional discounts to the specified order based on the provided customer profile.
        /// </summary>
        /// <param name="order">The order to which promotions will be applied. Cannot be null.</param>
        /// <param name="customerProfile">The profile of the customer, used to determine applicable promotions. Cannot be null.</param>
        /// <returns>A <see cref="PromotionResult"/> containing the details of the applied promotions, including the updated
        /// order total and any discounts applied.</returns>
        public PromotionResult ApplyPromotions(Order order, CustomerProfile customerProfile)
        {
            return new PromotionEngine(_rules).ApplyDiscounts(order, customerProfile);
        }
    }
}
