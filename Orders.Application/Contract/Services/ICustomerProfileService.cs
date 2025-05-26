using Orders.Domain.ValueObjects;

namespace Orders.Application.Contract.Services
{
    /// <summary>
    /// Defines a service for working with customer profiles.
    /// </summary>
    public interface ICustomerProfileService
    {
        /// <summary>
        /// Asynchronously retrieves the profile of a customer by their unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the customer's profile. If
        /// the customer does not exist, the method returns <see langword="null"/>. Ensure that the <paramref
        /// name="cancellationToken"/> is used to handle cancellation scenarios gracefully.</remarks>
        /// <param name="customerId">The unique identifier of the customer whose profile is to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the customer's profile if found;
        /// otherwise, <see langword="null"/>.</returns>
        Task<CustomerProfile?> GetProfileAsync(Guid customerId, CancellationToken cancellationToken = default);
    }
}
