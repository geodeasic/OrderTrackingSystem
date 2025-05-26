using Orders.Domain.Dto;

namespace Orders.Application.Contract.Services
{
    /// <summary>
    /// Provides methods for retrieving analytics data related to orders.
    /// </summary>
    /// <remarks>This service is designed to aggregate and return analytical insights about orders. Use the
    /// <see cref="GetOrderAnalyticsAsync"/> method to retrieve the analytics data asynchronously.</remarks>
    public interface IOrderAnalyticsService
    {
        /// <summary>
        /// Retrieves analytics data for orders asynchronously.
        /// </summary>
        /// <remarks>This method returns a summary of order analytics, including key metrics and insights.
        /// The returned data can be used for reporting or decision-making purposes.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="OrderAnalyticsDto"/> object with the analytics data for orders.</returns>
        Task<OrderAnalyticsDto> GetOrderAnalyticsAsync();
    }
}
