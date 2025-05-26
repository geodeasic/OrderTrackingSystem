using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Orders.Domain.ValueObjects;

namespace Orders.ApiService.Examples
{
    /// <summary>
    /// Schema filter to document all possible values for OrderStatus in Swagger.
    /// </summary>
    public class OrderStatusSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(string) && context.MemberInfo?.Name == "Status")
            {
                schema.Description += "\nPossible values: " + string.Join(", ", OrderStatus.All.Select(s => s.Value));
            }
        }
    }

    /// <summary>
    /// Schema filter to document all possible values for CustomerSegment in Swagger.
    /// </summary>
    public class CustomerSegmentSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CustomerSegment))
            {
                schema.Description += "\nPossible values: New, Regular, VIP";
            }
        }
    }
}
