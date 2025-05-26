namespace Orders.Domain.Dto
{
    /// <summary>
    /// The DTO for returning order details via API.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the total monetary amount for the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the current status of the order.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the order was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was fulfilled.
        /// </summary>
        public DateTime? FulfilledAt { get; set; }

        /// <summary>
        /// Gets or sets the total price after applying all applicable discounts.
        /// </summary>
        public decimal DiscountedTotal { get; set; }

        /// <summary>
        /// Gets or sets the list of promotions that have been applied.
        /// </summary>
        public List<string> AppliedPromotions { get; set; } = [];

        /// <summary>
        /// The list of discount rules that would be applicable to this order based on 
        /// the current customer profile.
        /// </summary>
        public List<string> PotentialPromotions { get; set; } = [];
    }
}
