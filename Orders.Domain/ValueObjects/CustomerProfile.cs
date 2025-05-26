namespace Orders.Domain.ValueObjects
{
    /// <summary>
    /// Represents a customer's profile, including their unique identifier, segment classification,  and order history
    /// details.
    /// </summary>
    /// <remarks>This structure is immutable and provides a concise way to encapsulate customer-related data. 
    /// It is used for customer segmentation purposes.</remarks>
    /// <param name="CustomerId">The unique identifier of the customer.</param>
    /// <param name="Segment">The customer segment.</param>
    /// <param name="PastOrderCount">The number of orders previously placed by this customer.</param>
    /// <param name="TotalOrderValue">The total of all orders placed by this customer to date.</param>
    public readonly record struct CustomerProfile
        (Guid CustomerId,
         CustomerSegment Segment,
         int PastOrderCount,
         decimal TotalOrderValue);
}
