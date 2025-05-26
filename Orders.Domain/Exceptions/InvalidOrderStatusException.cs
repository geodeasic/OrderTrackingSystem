namespace Orders.Domain.Exceptions
{
    public class InvalidOrderStatusException : Exception
    {
        public InvalidOrderStatusException()
        {
        }

        public InvalidOrderStatusException(string? message) : base(message)
        {
        }

        public InvalidOrderStatusException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
