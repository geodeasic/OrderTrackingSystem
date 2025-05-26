using Orders.Domain.Dto;
using Orders.Domain.Promotions;
using Swashbuckle.AspNetCore.Filters;

namespace Orders.ApiService.Examples
{
    /// <summary>
    /// Provides an example response for the order analytics endpoint.
    /// </summary>
    public class OrderAnalyticsDtoExample : IExamplesProvider<OrderAnalyticsDto>
    {
        public OrderAnalyticsDto GetExamples() => new()
        {
            AverageOrderValue = 419.64,
            AverageFulfillmentTimeHours = 110,
            AverageDailyOrders = 0.8,
            TotalOrdersLastSevenDays = 2,
            ReportDaysCovered = 27,
            AverageDiscount = 10.5
        };
    }

    /// <summary>
    /// Provides an example response for the PromotionResult endpoints.
    /// </summary>
    public class PromotionResultExample : IExamplesProvider<PromotionResult>
    {
        public PromotionResult GetExamples() => new(
            OriginalTotal: 100,
            DiscountedTotal: 90,
            AppliedPromotions: ["First Order Discount"]
        );
    }
}
