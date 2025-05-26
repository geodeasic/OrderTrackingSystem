namespace Orders.Domain.Dto
{
    /// <summary>
    /// DTO for order analytics response.
    /// </summary>
    public class OrderAnalyticsDto
    {
        /// <summary>
        /// Gets or sets the average value of all orders.
        /// </summary>
        public double AverageOrderValue { get; set; }
        /// <summary>
        ///  Gets or sets the average fulfillment time in hours for fulfilled orders.
        /// </summary>
        public double AverageFulfillmentTimeHours { get; set; }
        /// <summary>
        ///  Gets or sets the average number of orders placed per day (rounded to 2 decimal places).
        /// </summary>
        public double AverageDailyOrders { get; set; }
        /// <summary>
        ///  Gets or sets the total number of orders placed in the last seven days.
        /// </summary>
        public int TotalOrdersLastSevenDays { get; set; }
        /// <summary>
        ///  Gets or sets the number of days covered by the analytics report.
        /// </summary>
        public int ReportDaysCovered { get; set; }
        /// <summary>
        ///  Gets or sets the average discount applied to all orders (rounded to 2 decimal places).
        /// </summary>
        public double AverageDiscount { get; set; }
    }
}
