namespace Orders.Domain.Exceptions
{
    public class InvalidStatusTransitionException : Exception
    {
        public InvalidStatusTransitionException()
        {
        }

        public InvalidStatusTransitionException(string? message) : base(message)
        {
        }

        public InvalidStatusTransitionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
