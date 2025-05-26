using Microsoft.Extensions.Caching.Memory;
using Orders.Application.Contract.Persistence;
using Orders.Application.Contract.Services;
using Orders.Domain.Dto;

namespace Orders.Infrastructure.Services.InMemory
{
    /// <summary>
    /// Provides analytics and statistical insights for orders, including metrics such as average order value, average
    /// fulfillment time, average daily orders, total orders in the last seven days, and average discount.
    /// </summary>
    /// <remarks>This service aggregates data from the underlying order repository to compute various
    /// analytics metrics. It is designed to provide high-level insights into order trends and performance over
    /// time.</remarks>
    /// <param name="orderRepository">The repository with the orders data.</param>
    public class OrderAnalyticsService(
        IOrderRepository orderRepository,
        IMemoryCache cache) : IOrderAnalyticsService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMemoryCache _cache = cache;
        private static readonly string _cacheKey = "OrderAnalytics";
        private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Asynchronously retrieves analytics data for orders, including average order value,  average fulfillment
        /// time, average daily orders, total orders in the last seven days,  and average discount.
        /// </summary>
        /// <remarks>This method calculates various metrics based on all available orders, including:
        /// <list type="bullet"> 
        ///     <item>
        ///         <description>
        ///         The average value of all orders.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         The average time taken to fulfill orders, in hours.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         The average number of orders placed per day over the reporting
        ///         period.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         The total number of orders placed in the last seven
        ///         days.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///         The average discount applied to orders.
        ///         </description>
        ///     </item>
        /// </list> 
        /// If no orders are available, the method returns default values (e.g., 0 for all metrics).
        /// </remarks>
        /// <returns>An <see cref="OrderAnalyticsDto"/> object containing the calculated analytics data.</returns>
        public async Task<OrderAnalyticsDto> GetOrderAnalyticsAsync()
        {
            if (_cache.TryGetValue(_cacheKey, out OrderAnalyticsDto? cachedAnalytics))
            {
                return cachedAnalytics!;
            }

            var orders = (await _orderRepository.GetAllAsync()).ToList();
            if (orders.Count == 0)
            {
                var emptyResult = new OrderAnalyticsDto { AverageOrderValue = 0, AverageFulfillmentTimeHours = 0, AverageDailyOrders = 0, TotalOrdersLastSevenDays = 0, ReportDaysCovered = 0, AverageDiscount = 0 };
                _cache.Set(_cacheKey, emptyResult, _cacheDuration);
                return emptyResult;
            }

            var avgValue = Math.Round((double)orders.Average(o => o.TotalAmount), 2);
            var fulfilledOrders = orders.Where(o => o.FulfilledAt.HasValue).ToList();
            double avgFulfillment = fulfilledOrders.Count != 0
                ? Math.Round(fulfilledOrders.Average(o => (o.FulfilledAt!.Value - o.CreatedAt).TotalHours))
                : 0;

            // Calculate average daily orders
            var minDate = orders.Min(o => o.CreatedAt).Date;
            var maxDate = orders.Max(o => o.CreatedAt).Date;
            var totalDays = (maxDate - minDate).Days + 1; // +1 to include both endpoints
            double avgDailyOrders = totalDays > 0 ? Math.Round(orders.Count / (double)totalDays, 2) : orders.Count;

            // Calculate total orders in the last seven days
            var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6); // include today
            int totalOrdersLastSevenDays = orders.Count(o => o.CreatedAt.Date >= sevenDaysAgo);

            // Calculate average discount (TotalAmount - DiscountedTotal)
            double avgDiscount = Math.Round(orders.Average(o => (double)(o.TotalAmount - o.DiscountedTotal)), 2);

            var analytics = new OrderAnalyticsDto
            {
                AverageOrderValue = avgValue,
                AverageFulfillmentTimeHours = avgFulfillment,
                AverageDailyOrders = avgDailyOrders,
                TotalOrdersLastSevenDays = totalOrdersLastSevenDays,
                ReportDaysCovered = totalDays,
                AverageDiscount = avgDiscount
            };

            _cache.Set(_cacheKey, analytics, _cacheDuration);
            return analytics;
        }
    }
}
