using Orders.Domain.Contracts;
using Orders.Domain.Entities;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Promotions
{
    /// <summary>
    /// Represents a promotion rule that applies a discount for VIP customers.
    /// </summary>
    /// <remarks>
    /// This rule checks if a customer belongs to the VIP segment and, if so, applies a 15% discount
    /// to the total amount of the order.
    /// </remarks>
    public class VipCustomerDiscountRule : IPromotionRule
    {
        public string Name => "VIP Discount";

        public bool IsMatch(Order order, CustomerProfile customer)
        {
            return customer.Segment == CustomerSegment.VIP;
        }

        public decimal CalculateDiscount(Order order)
        {
            return order.TotalAmount * 0.15m;
        }
    }
}
