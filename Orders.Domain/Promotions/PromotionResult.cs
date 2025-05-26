namespace Orders.Domain.Promotions
{
    /// <summary>
    /// Represents the result of applying promotions to an order, including the original total, the discounted
    /// total, and the list of applied promotions.
    /// </summary>
    /// <param name="OriginalTotal">The original total amount before any promotions were applied.</param>
    /// <param name="DiscountedTotal">The total amount after applying promotions.</param>
    /// <param name="AppliedPromotions">A read-only list of promotion identifiers or descriptions that were 
    /// applied to the transaction.</param>
    public record PromotionResult(
        decimal OriginalTotal,
        decimal DiscountedTotal,
        IReadOnlyList<string> AppliedPromotions);
}
