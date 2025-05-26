using Orders.Application.Contract.Persistence;
using Orders.Domain.Entities;
using Orders.Infrastructure.Services.InMemory;

namespace Orders.Infrastructure.Persistence.InMemory
{
    /// <summary>
    /// Represents an in-memory implementation of the <see cref="IOrderRepository"/> interface.
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly Dictionary<Guid, Order> _orders = [];
        private static readonly Random _random = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryOrderRepository"/> class.
        /// </summary>
        /// <remarks>The constructor seeds the repository with initial orders designed to demonstrate
        /// discount rules. This is intended for testing or demonstration purposes.</remarks>
        public InMemoryOrderRepository()
        {
            // Seed with orders that will trigger discount rules
            SeedDiscountDemoOrders();
        }

        /// <summary>
        /// Seeds the system with a set of demo orders to simulate various discount scenarios.
        /// </summary>
        /// <remarks>
        /// This method creates and adds demo orders to the in-memory order collection,
        /// including: 
        /// <list type="bullet"> 
        ///     <item>
        ///         <description>
        ///         A first order for a new customer to simulate a
        ///         first-order discount.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         A high-value order exceeding $500 to simulate
        ///         a high-value discount.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         An order for a VIP customer to simulate VIP-specific rules.
        ///         </description>
        ///     </item> 
        ///     <item>
        ///         <description>
        ///         Several regular orders without discounts for
        ///         testing general behavior.
        ///         </description>
        ///     </item>
        /// </list> 
        /// 
        /// The method also updates the past order count for each
        /// customer in the in-memory customer profile service.
        /// </remarks>
        private void SeedDiscountDemoOrders()
        {
            // Customer IDs must match those in InMemoryCustomerProfileService
            var vipCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var newCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var regularCustomerId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Get the in-memory customer profile service to update PastOrderCount
            var customerProfileService = new InMemoryCustomerProfileService();

            // Helper to increment PastOrderCount
            void IncrementPastOrderCount(Guid customerId)
            {
                var profileNullable = customerProfileService.GetProfileAsync(customerId).Result;
                if (profileNullable.HasValue)
                {
                    var profile = profileNullable.Value;
                    var updated = profile with { PastOrderCount = profile.PastOrderCount + 1 };
                    customerProfileService.Add(updated);
                }
            }

            // 1. First Order Discount (New customer, first order)
            var firstOrder = new Order(Guid.NewGuid(), newCustomerId, 100m, 100m);
            typeof(Order).GetProperty("CreatedAt")?.SetValue(firstOrder, DateTime.UtcNow.AddDays(-1));
            _orders[firstOrder.Id] = firstOrder;
            IncrementPastOrderCount(newCustomerId);

            // 2. High Value Order Discount (any customer, > $500)
            var highValueOrder = new Order(Guid.NewGuid(), regularCustomerId, 600m, 600m);
            typeof(Order).GetProperty("CreatedAt")?.SetValue(highValueOrder, DateTime.UtcNow.AddDays(-2));
            _orders[highValueOrder.Id] = highValueOrder;
            IncrementPastOrderCount(regularCustomerId);

            // 3. VIP customer order (should match any VIP-specific rules)
            var vipOrder = new Order(Guid.NewGuid(), vipCustomerId, 200m, 200m);
            typeof(Order).GetProperty("CreatedAt")?.SetValue(vipOrder, DateTime.UtcNow.AddDays(-3));
            _orders[vipOrder.Id] = vipOrder;
            IncrementPastOrderCount(vipCustomerId);

            // 4. Regular orders (no discount)
            for (int i = 0; i < 10; i++)
            {
                var customerId = i % 3 == 0 ? vipCustomerId : (i % 3 == 1 ? newCustomerId : regularCustomerId);
                var totalAmount = Math.Round((decimal)(_random.NextDouble() * 400 + 20), 2); // $20 - $420
                var order = new Order(Guid.NewGuid(), customerId, totalAmount, totalAmount);
                typeof(Order).GetProperty("CreatedAt")?.SetValue(order, DateTime.UtcNow.AddDays(-4 - i));
                _orders[order.Id] = order;
                IncrementPastOrderCount(customerId);
            }
        }

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>The <see cref="Order"/> associated with the specified <paramref name="orderId"/>, or <see langword="null"/>
        /// if no order is found.</returns>
        public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            _orders.TryGetValue(orderId, out var order);
            return await Task.FromResult(order);
        }

        /// <summary>
        /// Updates the specified order in the system asynchronously.
        /// </summary>
        /// <param name="order">The order to update. The <see cref="Order.Id"/> property must match an existing order in the system.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _orders[order.Id] = order;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves all orders associated with the specified customer ID.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer whose orders are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of  <see
        /// cref="Order"/> objects associated with the specified customer ID. If no orders are found,  the collection
        /// will be empty.</returns>
        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            var customerOrders = _orders.Values.Where(o => o.CustomerId == customerId);
            return await Task.FromResult(customerOrders);
        }

        /// <summary>
        ///Retrieves all orders asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/>
        /// of <see cref="Order"/> objects representing all orders.</returns>
        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(_orders.Values.ToList());
        }
    }
}
