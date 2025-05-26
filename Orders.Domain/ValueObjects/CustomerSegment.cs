namespace Orders.Domain.ValueObjects
{
    /// <summary>
    /// Represents the classification of a customer based on their relationship with the business.
    /// </summary>
    /// <remarks>
    /// This enumeration is used to categorize customers for targeted discounts/promotions.
    /// The segments include: 
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="New"/>: A customer who has recently joined or made their first
    /// purchase.</description>
    /// </item> 
    /// <item><description><see cref="Regular"/>: A customer with a consistent purchase
    /// history or ongoing engagement.</description>
    /// </item> 
    /// <item><description><see cref="VIP"/>: A high-value customer
    /// with significant or frequent purchases.</description>
    /// </item> 
    /// </list>
    /// </remarks>
    public enum CustomerSegment
    {
        /// <summary>
        /// Represents a new customer who has just started engaging with the business.
        /// </summary>
        New,

        /// <summary>
        /// Represents a regular customer who has an established relationship with the business.
        /// </summary>
        Regular,

        /// <summary>
        /// Represents a VIP customer who is highly valued due to their significant contributions to the business.
        /// </summary>
        VIP
    }
}
