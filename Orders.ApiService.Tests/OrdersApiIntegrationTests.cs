using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Orders.Domain.Dto;
using Xunit;

namespace Orders.ApiService.Tests
{
    public class OrdersApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public OrdersApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOkAndList()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/orders");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
            Assert.NotNull(orders);
            Assert.True(orders.Count >= 1); // Should match seeded data
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOrderDtoStructure()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/orders");
            var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
            Assert.NotNull(orders);
            var order = orders.First();
            Assert.NotEqual(Guid.Empty, order.Id);
            Assert.NotEqual(Guid.Empty, order.CustomerId);
            Assert.True(order.TotalAmount > 0);
            Assert.False(string.IsNullOrWhiteSpace(order.Status));
            Assert.True(order.CreatedAt > DateTime.MinValue);
        }

        [Fact]
        public async Task GetOrderAnalytics_ReturnsOkWithAnalytics()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/orders/analytics");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var analytics = await response.Content.ReadFromJsonAsync<object>();
            Assert.NotNull(analytics);
        }

        [Fact]
        public async Task ApplyDiscountDirect_ReturnsNotFound_WhenOrderNotFound()
        {
            var client = _factory.CreateClient();
            var randomId = Guid.NewGuid();
            var response = await client.PostAsync($"/api/orders/{randomId}/apply-discount", null);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AdvanceOrderStatus_ReturnsNoContent_WhenSuccess()
        {
            var client = _factory.CreateClient();
            var orders = await client.GetFromJsonAsync<List<OrderDto>>("/api/orders");
            var orderId = orders!.First().Id;
            var response = await client.PostAsync($"/api/orders/{orderId}/status/advance?newStatus=Confirmed", null);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task AdvanceOrderStatus_ReturnsBadRequest_WhenInvalid()
        {
            var client = _factory.CreateClient();
            var orders = await client.GetFromJsonAsync<List<OrderDto>>("/api/orders");
            var orderId = orders!.First().Id;
            var response = await client.PostAsync($"/api/orders/{orderId}/status/advance?newStatus=NotAStatus", null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
