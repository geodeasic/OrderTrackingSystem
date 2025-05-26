using Orders.Domain.Contracts;
using Orders.Domain.Entities;
using Orders.Domain.Promotions;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Services
{
    /// <summary>
    /// Implements a promotion engine that applies various promotional rules to 
    /// orders based on customer profiles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PromotionEngine"/> class with the 
    /// with a set of promotion rules.
    /// </remarks>
    /// <param name="rules">A collection of promotion rules to be applied by the engine. Cannot be null.</param>
    public class PromotionEngine(IEnumerable<IPromotionRule> rules)
    {
        private readonly List<IPromotionRule> _rules = [.. rules];

        /// <summary>
        /// Applies all applicable discounts to the specified order based on the provided customer profile.
        /// </summary>
        /// <param name="order">The order to the discounts are applied.</param>
        /// <param name="customer">The profile of the customer owning the order.</param>
        /// <returns>The result of applying the promotions to the <paramref name="order"/></returns>
        public PromotionResult ApplyDiscounts(Order order, CustomerProfile customer)
        {
            var appliedPromotions = new List<string>();
            decimal totalDiscount = 0m;

            foreach (var rule in _rules)
            {
                if (rule.IsMatch(order, customer))
                {
                    decimal discount = rule.CalculateDiscount(order);
                    if (discount > 0)
                    {
                        appliedPromotions.Add(rule.Name);
                        totalDiscount += discount;
                    }
                }
            }

            var discountedTotal = Math.Max(order.TotalAmount - totalDiscount, 0);

            return new PromotionResult(
                OriginalTotal: order.TotalAmount,
                DiscountedTotal: discountedTotal,
                AppliedPromotions: appliedPromotions
            );
        }
    }
}
