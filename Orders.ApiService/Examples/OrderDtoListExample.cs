using Orders.Domain.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace Orders.ApiService.Examples
{
    /// <summary>
    /// Provides an example response for the GetAllOrders endpoint.
    /// </summary>
    public class OrderDtoListExample : IExamplesProvider<IEnumerable<OrderDto>>
    {
        public IEnumerable<OrderDto> GetExamples()
        {
            return
            [
                new OrderDto
                {
                    Id = Guid.Parse("3e31d9e4-01de-4d8d-a30e-ad5dd76b6c23"),
                    CustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    TotalAmount = 100,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    FulfilledAt = null,
                    DiscountedTotal = 90,
                    AppliedPromotions = ["First Order Discount"],
                    PotentialPromotions = ["First Order Discount"]
                },
                new OrderDto
                {
                    Id = Guid.Parse("5aa1af3d-c54c-411d-8a9e-04a70321b38d"),
                    CustomerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    TotalAmount = 600,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    FulfilledAt = null,
                    DiscountedTotal = 600,
                    AppliedPromotions = [],
                    PotentialPromotions = ["High Value Order Discount"]
                }
            ];
        }
    }
}
