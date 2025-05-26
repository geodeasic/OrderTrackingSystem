using System;
using System.Linq;
using System.Threading.Tasks;
using Orders.Infrastructure.Persistence.InMemory;
using Orders.Infrastructure.Services.InMemory;
using Orders.Domain.Entities;
using Orders.Domain.ValueObjects;
using Xunit;

namespace Orders.Infrastructure.Tests
{
    public class InMemoryOrderRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsOrder_WhenExists()
        {
            var repo = new InMemoryOrderRepository();
            var allOrders = await repo.GetAllAsync();
            var firstOrder = allOrders.First();
            var result = await repo.GetByIdAsync(firstOrder.Id);
            Assert.NotNull(result);
            Assert.Equal(firstOrder.Id, result!.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesOrder()
        {
            var repo = new InMemoryOrderRepository();
            var allOrders = await repo.GetAllAsync();
            var order = allOrders.First();
            order.ApplyDiscount(new Orders.Domain.Promotions.PromotionResult(order.TotalAmount, order.TotalAmount - 10, ["Test"]));
            await repo.UpdateAsync(order);
            var updated = await repo.GetByIdAsync(order.Id);
            Assert.Equal(order.DiscountedTotal, updated!.DiscountedTotal);
            Assert.Contains("Test", updated.AppliedPromotions);
        }

        [Fact]
        public async Task GetOrdersByCustomerIdAsync_ReturnsOrdersForCustomer()
        {
            var repo = new InMemoryOrderRepository();
            var allOrders = await repo.GetAllAsync();
            var customerId = allOrders.First().CustomerId;
            var customerOrders = await repo.GetOrdersByCustomerIdAsync(customerId);
            Assert.All(customerOrders, o => Assert.Equal(customerId, o.CustomerId));
        }
    }

    public class InMemoryCustomerProfileServiceTests
    {
        [Fact]
        public async Task GetProfileAsync_ReturnsProfile_WhenExists()
        {
            var service = new InMemoryCustomerProfileService();
            var vipId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var profile = await service.GetProfileAsync(vipId);
            Assert.NotNull(profile);
            Assert.Equal(vipId, profile!.Value.CustomerId);
        }

        [Fact]
        public async Task Add_AddsOrUpdatesProfile()
        {
            var service = new InMemoryCustomerProfileService();
            var id = Guid.NewGuid();
            var profile = new CustomerProfile(id, CustomerSegment.Regular, 1, 100);
            service.Add(profile);
            var result = await service.GetProfileAsync(id);
            Assert.NotNull(result);
            Assert.Equal(id, result!.Value.CustomerId);
        }
    }

    public class OrderStatusServiceTests
    {
        [Fact]
        public async Task AdvanceOrderStatusAsync_ValidTransition_UpdatesStatus()
        {
            var repo = new InMemoryOrderRepository();
            var service = new OrderStatusService(repo);
            var order = (await repo.GetAllAsync()).First();
            var result = await service.AdvanceOrderStatusAsync(order.Id, "Confirmed");
            Assert.True(result);
            var updated = await repo.GetByIdAsync(order.Id);
            Assert.Equal("Confirmed", updated!.Status.Value);
        }

        [Fact]
        public async Task AdvanceOrderStatusAsync_InvalidStatus_ReturnsFalse()
        {
            var repo = new InMemoryOrderRepository();
            var service = new OrderStatusService(repo);
            var order = (await repo.GetAllAsync()).First();
            var result = await service.AdvanceOrderStatusAsync(order.Id, "NotAStatus");
            Assert.False(result);
        }
    }
}
