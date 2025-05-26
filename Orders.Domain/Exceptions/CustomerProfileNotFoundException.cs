namespace Orders.Domain.Exceptions
{
    public class CustomerProfileNotFoundException : Exception
    {
        public CustomerProfileNotFoundException()
        {
        }

        public CustomerProfileNotFoundException(string? message) : base(message)
        {
        }

        public CustomerProfileNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
