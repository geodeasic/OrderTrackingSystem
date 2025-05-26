namespace Orders.Domain.ValueObjects
{
    /// <summary>
    /// This enum represents the status of an order. This is strongly-typed as 
    /// opposed to using an <see cref="Enum"/> or string. It's advantages are 
    /// easy validation using <c>From(...)</c>, prevents invalid status assignment, and 
    /// is extensible for features like localization, state grouping, etc.
    /// </summary>
    public sealed class OrderStatus : IEquatable<OrderStatus>
    {
        /// <summary>
        /// Gets the value of this order status.
        /// </summary>
        public string Value { get; }

        private OrderStatus(string value)
        {
            Value = value;
        }

        // Static instances for each known status
        public static readonly OrderStatus Pending = new("Pending");
        public static readonly OrderStatus Confirmed = new("Confirmed");
        public static readonly OrderStatus Shipped = new("Shipped");
        public static readonly OrderStatus Delivered = new("Delivered");
        public static readonly OrderStatus Returned = new("Returned");
        public static readonly OrderStatus Cancelled = new("Cancelled");
        public static readonly OrderStatus Closed = new("Closed");

        /// <summary>
        /// Gets all possible order statuses for validation and iteration.
        /// </summary>
        public static IEnumerable<OrderStatus> All =>
            [
                Pending, Confirmed, Shipped, Delivered, Returned, Cancelled, Closed
            ];

        /// <summary>
        /// Gets an <see cref="OrderStatus"/> given its name.
        /// </summary>
        /// <param name="status">The string representation of the order status.</param>
        /// <returns>The instance of <see cref="OrderStatus"/> corresponding to 
        /// <paramref name="status"/> or <c>null</c> if no order status corresponds to 
        /// <paramref name="status"/>.</returns>
        public static OrderStatus? From(string status)
        {
            return All.FirstOrDefault(s =>
                string.Equals(s.Value, status, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether the current <see cref="OrderStatus"/> is equal to the specified <see
        /// cref="OrderStatus"/>.
        /// </summary>
        /// <param name="other">The <see cref="OrderStatus"/> to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="OrderStatus"/> is not <see langword="null"/>  and has the
        /// same value as the current instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(OrderStatus? other)
        {
            return other is not null && Value == other.Value;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="OrderStatus"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. This can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is an <see cref="OrderStatus"/> and is equal to the current
        /// instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is OrderStatus other && Equals(other);
        }

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is derived from the <see cref="Value"/> property.  It is suitable for
        /// use in hashing algorithms and data structures such as a hash table.</remarks>
        /// <returns>An integer that represents the hash code for the current object.</returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Returns the string representation of the current order status.
        /// </summary>
        /// <returns>The string value of the <see cref="Value"/> property.</returns>
        public override string ToString() => Value;

        /// <summary>
        /// Converts an <see cref="OrderStatus"/> instance to its string representation.
        /// </summary>
        /// <param name="status">The <see cref="OrderStatus"/> instance to convert. Must not be <c>null</c>.</param>
        public static implicit operator string(OrderStatus status) => status.Value;
    }
}
