using Microsoft.AspNetCore.Mvc;
using Moq;
using Orders.ApiService.Controllers;
using Orders.Application.Contract.Persistence;
using Orders.Application.Contract.Promotion;
using Orders.Application.Contract.Services;
using Orders.Domain.Dto;
using Orders.Domain.Promotions;
using Orders.Domain.ValueObjects;

namespace Orders.ApiService.Tests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderAnalyticsService> _analyticsService = new();
        private readonly Mock<IOrderQueryService> _queryService = new();
        private readonly Mock<IOrderStatusService> _statusService = new();
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<ICustomerProfileService> _customerProfileService = new();
        private readonly Mock<IPromotionEngine> _promotionEngine = new();
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _controller = new OrdersController(
                _analyticsService.Object,
                _queryService.Object,
                _statusService.Object,
                _orderRepository.Object,
                _customerProfileService.Object,
                _promotionEngine.Object
            );
        }

        [Fact]
        public async Task ApplyDiscountDirect_ReturnsNotFound_WhenOrderNotFound()
        {
            _orderRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync((Orders.Domain.Entities.Order)null!);
            var result = await _controller.ApplyDiscountDirect(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ApplyDiscountDirect_ReturnsNotFound_WhenCustomerProfileNotFound()
        {
            var order = new Orders.Domain.Entities.Order(Guid.NewGuid(), Guid.NewGuid(), 100, 100);
            _orderRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(order);
            _customerProfileService.Setup(s => s.GetProfileAsync(order.CustomerId, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync((CustomerProfile?)null);
            var result = await _controller.ApplyDiscountDirect(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ApplyDiscountDirect_ReturnsOk_WhenDiscountApplied()
        {
            var order = new Orders.Domain.Entities.Order(Guid.NewGuid(), Guid.NewGuid(), 100, 100);
            var profile = new CustomerProfile(order.CustomerId, CustomerSegment.Regular, 0, 0);
            var promoResult = new PromotionResult(100, 90, ["TestPromo"]);
            _orderRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(order);
            _customerProfileService.Setup(s => s.GetProfileAsync(order.CustomerId, It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(profile);
            _promotionEngine.Setup(e => e.ApplyPromotions(order, profile)).Returns(promoResult);
            _orderRepository.Setup(r => r.UpdateAsync(order, It.IsAny<System.Threading.CancellationToken>())).Returns(Task.CompletedTask);
            var result = await _controller.ApplyDiscountDirect(Guid.NewGuid());
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(promoResult, okResult.Value);
        }

        [Fact]
        public async Task GetOrderAnalytics_ReturnsOkWithAnalytics()
        {
            var analytics = new OrderAnalyticsDto();
            _analyticsService.Setup(s => s.GetOrderAnalyticsAsync()).ReturnsAsync(analytics);
            var result = await _controller.GetOrderAnalytics();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(analytics, okResult.Value);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOkWithOrders()
        {
            var orders = new List<OrderDto> { new() };
            _queryService.Setup(s => s.GetAllOrdersAsync()).ReturnsAsync(orders);
            var result = await _controller.GetAllOrders();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orders, okResult.Value);
        }

        [Fact]
        public async Task AdvanceOrderStatus_ReturnsNoContent_WhenSuccess()
        {
            _statusService.Setup(s => s.AdvanceOrderStatusAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);
            var result = await _controller.AdvanceOrderStatus(Guid.NewGuid(), "Confirmed");
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AdvanceOrderStatus_ReturnsBadRequest_WhenInvalid()
        {
            _statusService.Setup(s => s.AdvanceOrderStatusAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            var result = await _controller.AdvanceOrderStatus(Guid.NewGuid(), "InvalidStatus");
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid status transition or order not found.", badRequest.Value);
        }
    }
}
