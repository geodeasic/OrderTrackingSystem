using Orders.Domain.Entities;
using Orders.Domain.Promotions;
using Orders.Domain.Services;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void ApplyDiscount_SetsDiscountedTotalAndPromotions()
        {
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, 100m);
            var promoResult = new PromotionResult(100m, 90m, ["TestPromo"]);
            order.ApplyDiscount(promoResult);
            Assert.Equal(90m, order.DiscountedTotal);
            Assert.Contains("TestPromo", order.AppliedPromotions);
        }

        [Fact]
        public void ChangeStatus_ValidTransition_UpdatesStatusAndFulfilledAt()
        {
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, 100m);
            var matrix = new OrderStatusTransitionMatrix();
            order.ChangeStatus(OrderStatus.Confirmed, matrix);
            Assert.Equal(OrderStatus.Confirmed, order.Status);
            order.ChangeStatus(OrderStatus.Shipped, matrix);
            Assert.Equal(OrderStatus.Shipped, order.Status);
            order.ChangeStatus(OrderStatus.Delivered, matrix);
            Assert.Equal(OrderStatus.Delivered, order.Status);
            Assert.NotNull(order.FulfilledAt);
        }
    }

    public class PromotionRuleTests
    {
        [Fact]
        public void FirstOrderDiscountRule_AppliesOnlyToNewCustomerWithNoOrders()
        {
            var rule = new FirstOrderDiscountRule();
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, 100m);
            var newCustomer = new CustomerProfile(order.CustomerId, CustomerSegment.New, 0, 0);
            var notNewCustomer = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 1, 100);
            Assert.True(rule.IsMatch(order, newCustomer));
            Assert.False(rule.IsMatch(order, notNewCustomer));
            Assert.Equal(10m, rule.CalculateDiscount(order));
        }

        [Fact]
        public void HighValueOrderDiscountRule_AppliesOnlyToHighValueOrders()
        {
            var rule = new HighValueOrderDiscountRule();
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 600m, 600m);
            var lowOrder = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, 100m);
            var customer = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 2, 1000);
            Assert.True(rule.IsMatch(order, customer));
            Assert.False(rule.IsMatch(lowOrder, customer));
            Assert.Equal(30m, rule.CalculateDiscount(order));
        }

        [Fact]
        public void LoyaltyDiscountRule_AppliesOnlyToLoyalCustomers()
        {
            var rule = new LoyaltyDiscountRule();
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, 100m);
            var loyalCustomer = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 5, 10);
            var notLoyalCustomer = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 1, 2);
            Assert.True(rule.IsMatch(order, loyalCustomer));
            Assert.False(rule.IsMatch(order, notLoyalCustomer));
            Assert.Equal(10m, rule.CalculateDiscount(order));
        }

        [Fact]
        public void VipCustomerDiscountRule_AppliesOnlyToVipCustomers()
        {
            var rule = new VipCustomerDiscountRule();
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 200m, 200m);
            var vipCustomer = new CustomerProfile(order.CustomerId, CustomerSegment.VIP, 10, 5000);
            var notVipCustomer = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 10, 5000);
            Assert.True(rule.IsMatch(order, vipCustomer));
            Assert.False(rule.IsMatch(order, notVipCustomer));
            Assert.Equal(30m, rule.CalculateDiscount(new Order(Guid.NewGuid(), Guid.NewGuid(), 200m, 200m)));
        }
    }

    public class PromotionEngineTests
    {
        [Fact]
        public void PromotionEngine_AppliesAllApplicableDiscounts()
        {
            var rules = new List<Orders.Domain.Contracts.IPromotionRule>
            {
                new FirstOrderDiscountRule(),
                new HighValueOrderDiscountRule(),
                new LoyaltyDiscountRule(),
                new VipCustomerDiscountRule()
            };
            var engine = new PromotionEngine(rules);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 1000m, 1000m);
            var customer = new CustomerProfile(order.CustomerId, CustomerSegment.VIP, 0, 10000);
            var result = engine.ApplyDiscounts(order, customer);
            Assert.Contains("VIP Discount", result.AppliedPromotions);
            Assert.Contains("High Value Order Discount", result.AppliedPromotions);
            Assert.Contains("Loyalty Discount", result.AppliedPromotions);
            Assert.Equal(1000m, result.OriginalTotal);
            Assert.True(result.DiscountedTotal < 1000m);
        }
    }

    public class PromotionEngineEdgeCaseTests
    {
        [Fact]
        public void PromotionEngine_NoRules_ReturnsOriginalTotal()
        {
            var engine = new PromotionEngine(new List<Orders.Domain.Contracts.IPromotionRule>());
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 100m, 100m);
            var customer = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 0, 0);
            var result = engine.ApplyDiscounts(order, customer);
            Assert.Equal(100m, result.OriginalTotal);
            Assert.Equal(100m, result.DiscountedTotal);
            Assert.Empty(result.AppliedPromotions);
        }

        [Fact]
        public void PromotionEngine_NoApplicableRules_ReturnsOriginalTotal()
        {
            var rules = new List<Orders.Domain.Contracts.IPromotionRule> { new HighValueOrderDiscountRule() };
            var engine = new PromotionEngine(rules);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 50m, 50m);
            var customer = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 0, 0);
            var result = engine.ApplyDiscounts(order, customer);
            Assert.Equal(50m, result.OriginalTotal);
            Assert.Equal(50m, result.DiscountedTotal);
            Assert.Empty(result.AppliedPromotions);
        }

        [Fact]
        public void PromotionEngine_ZeroOrderAmount_NoDiscountApplied()
        {
            var rules = new List<Orders.Domain.Contracts.IPromotionRule> { new FirstOrderDiscountRule() };
            var engine = new PromotionEngine(rules);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 0m, 0m);
            var customer = new CustomerProfile(order.CustomerId, CustomerSegment.New, 0, 0);
            var result = engine.ApplyDiscounts(order, customer);
            Assert.Equal(0m, result.OriginalTotal);
            Assert.Equal(0m, result.DiscountedTotal);
        }

        [Fact]
        public void PromotionEngine_MultipleRules_CumulativeDiscounts()
        {
            var rules = new List<Orders.Domain.Contracts.IPromotionRule>
            {
                new FirstOrderDiscountRule(),
                new HighValueOrderDiscountRule()
            };
            var engine = new PromotionEngine(rules);
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), 600m, 600m);
            var customer = new CustomerProfile(order.CustomerId, CustomerSegment.New, 0, 0);
            var result = engine.ApplyDiscounts(order, customer);
            Assert.Contains("First Order Discount", result.AppliedPromotions);
            Assert.Contains("High Value Order Discount", result.AppliedPromotions);
            Assert.True(result.DiscountedTotal < 600m);
        }
    }
}
