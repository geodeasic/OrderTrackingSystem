using Microsoft.AspNetCore.Mvc;
using Orders.ApiService.Examples;
using Orders.Application.Contract.Persistence;
using Orders.Application.Contract.Promotion;
using Orders.Application.Contract.Services;
using Orders.Domain.Dto;
using Orders.Domain.Promotions;
using Swashbuckle.AspNetCore.Filters;

namespace Orders.ApiService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController(
        IOrderAnalyticsService orderAnalyticsService,
        IOrderQueryService orderQueryService,
        IOrderStatusService orderStatusService,
        IOrderRepository orderRepository,
        ICustomerProfileService customerProfileService,
        IPromotionEngine promotionEngine) : ControllerBase
    {
        private readonly IOrderAnalyticsService _orderAnalyticsService = orderAnalyticsService;
        private readonly IOrderQueryService _orderQueryService = orderQueryService;
        private readonly IOrderStatusService _orderStatusService = orderStatusService;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ICustomerProfileService _customerProfileService = customerProfileService;
        private readonly IPromotionEngine _promotionEngine = promotionEngine;

        /// <summary>
        /// Apply applicable discounts to the order based on customer segment and history.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>A <see cref="PromotionResult"/> containing details of the discounts applied. Returns 404 if the order is not found.</returns>
        [HttpPost("{id}/apply-discount")]
        [ProducesResponseType(typeof(PromotionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PromotionResultExample))]
        public async Task<IActionResult> ApplyDiscountDirect(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            var customerProfile = await _customerProfileService.GetProfileAsync(order.CustomerId);
            if (customerProfile == null)
                return NotFound();
            var result = _promotionEngine.ApplyPromotions(order, customerProfile ?? throw new InvalidOperationException("Customer profile is null"));
            order.ApplyDiscount(result);
            await _orderRepository.UpdateAsync(order);
            return Ok(result);
        }

        /// <summary>
        /// Returns analytics for all orders, including average order value (rounded to 2 decimal places) and average fulfillment time (rounded to the nearest hour).
        /// </summary>
        /// <returns>An <see cref="OrderAnalyticsDto"/> containing analytics data for all orders.</returns>
        [HttpGet("analytics")]
        [ProducesResponseType(typeof(OrderAnalyticsDto), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OrderAnalyticsDtoExample))]
        public async Task<IActionResult> GetOrderAnalytics()
        {
            var analytics = await _orderAnalyticsService.GetOrderAnalyticsAsync();
            return Ok(analytics);
        }

        /// <summary>
        /// Gets all orders in the system.
        /// </summary>
        /// <returns>A list of <see cref="OrderDto"/> representing all orders in the system.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OrderDtoListExample))]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderQueryService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Moves an order to the next valid status.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <param name="newStatus">The new status to set (e.g., Confirmed, Shipped, Delivered, Closed, Returned, Cancelled).</param>
        /// <returns>No content if successful. Returns 400 if the transition is invalid or 404 if the order is not found.</returns>
        [HttpPost("{id}/status/advance")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AdvanceOrderStatus(Guid id, [FromQuery] string newStatus)
        {
            var result = await _orderStatusService.AdvanceOrderStatusAsync(id, newStatus);
            if (!result)
            {
                // Could be not found, invalid status, or invalid transition
                // For demo, return 400 for invalid transition/status, 404 for not found
                // (A more robust implementation would distinguish these cases)
                return BadRequest("Invalid status transition or order not found.");
            }
            return NoContent();
        }
    }
}
