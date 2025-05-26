using Orders.Domain.Entities;
using Orders.Domain.Promotions;
using Orders.Domain.ValueObjects;

namespace Orders.Application.Contract.Promotion
{
    /// <summary>
    /// Defines the contract for a promotion engine that applies promotions to orders.
    /// </summary>
    public interface IPromotionEngine
    {
        /// <summary>
        /// Applies applicable promotions to the specified order based on the customer's profile.
        /// </summary>
        /// <remarks>This method evaluates the provided order and customer profile to determine which
        /// promotions are applicable. The resulting promotions are applied to the order, and the updated details are
        /// returned in the <see cref="PromotionResult"/>.</remarks>
        /// <param name="order">The order to which promotions will be applied. Must not be null.</param>
        /// <param name="customerProfile">The profile of the customer placing the order. Must not be null.</param>
        /// <returns>A <see cref="PromotionResult"/> object containing details of the applied promotions and the updated order
        /// total.</returns>
        PromotionResult ApplyPromotions(Order order, CustomerProfile customerProfile);
    }
}
