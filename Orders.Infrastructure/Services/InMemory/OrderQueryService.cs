using Orders.Application.Contract.Persistence;
using Orders.Application.Contract.Promotion;
using Orders.Application.Contract.Services;
using Orders.Domain.Dto;

namespace Orders.Infrastructure.Services.InMemory
{
    /// <summary>
    /// Implements functionality for querying order data.
    /// </summary>
    /// <remarks>This service is responsible for retrieving orders from the underlying data repository,
    /// enriching them with customer profile information, and evaluating potential promotions using the promotion
    /// engine. It is designed to aggregate and transform order data into a format suitable for client
    /// consumption.</remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OrderQueryService"/> class,  providing access to order data,
    /// customer profiles, and promotion calculations.
    /// </remarks>
    /// <remarks>This constructor requires dependencies for order management, customer profiles,  and
    /// promotion calculations. Ensure that all dependencies are properly configured  and injected to enable full
    /// functionality of the service.</remarks>
    /// <param name="orderRepository">The repository used to retrieve and manage order data.</param>
    /// <param name="customerProfileService">The service used to access customer profile information.</param>
    /// <param name="promotionEngine">The engine used to calculate and apply promotions to orders.</param>
    public class OrderQueryService(
        IOrderRepository orderRepository,
        ICustomerProfileService customerProfileService,
        IPromotionEngine promotionEngine) : IOrderQueryService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ICustomerProfileService _customerProfileService = customerProfileService;
        private readonly IPromotionEngine _promotionEngine = promotionEngine;

        /// <summary>
        /// Asynchronously retrieves all orders, including detailed information about each order, such as applied and
        /// potential promotions.
        /// </summary>
        /// <remarks>This method fetches all orders from the repository and enriches them with additional
        /// details, including customer-specific promotions and other metadata. The returned collection includes both
        /// applied promotions and potential promotions that could be relevant based on the customer's
        /// profile.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
        /// cref="OrderDto"/> objects, where each object represents an order with its associated details.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a customer profile cannot be retrieved for an order.</exception>
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = (await _orderRepository.GetAllAsync()).ToList();
            var result = new List<OrderDto>(orders.Count);

            foreach (var order in orders)
            {
                var customerProfile = await _customerProfileService.GetProfileAsync(order.CustomerId);
                var potentialPromotions = new List<string>();

                if (customerProfile != null)
                {
                    // Evaluate all rules using the promotion engine
                    var promoResult = _promotionEngine.ApplyPromotions(order, customerProfile
                        ?? throw new InvalidOperationException("Customer profile is null"));
                    potentialPromotions = [.. promoResult.AppliedPromotions];
                }

                result.Add(new OrderDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString(),
                    CreatedAt = order.CreatedAt,
                    FulfilledAt = order.FulfilledAt,
                    DiscountedTotal = order.DiscountedTotal,
                    AppliedPromotions = [.. order.AppliedPromotions],
                    PotentialPromotions = potentialPromotions
                });
            }

            return result;
        }
    }
}
