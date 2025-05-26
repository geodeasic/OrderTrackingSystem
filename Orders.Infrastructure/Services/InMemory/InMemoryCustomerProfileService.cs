using Orders.Application.Contract.Services;
using Orders.Domain.ValueObjects;

namespace Orders.Infrastructure.Services.InMemory
{
    /// <summary>
    /// Provides an in-memory implementation of the <see cref="ICustomerProfileService"/> interface for managing
    /// customer profiles.
    /// </summary>
    /// <remarks>This service stores customer profiles in memory and is suitable for testing or scenarios
    /// where persistence is not required. It includes pre-seeded profiles for demonstration purposes.</remarks>
    public class InMemoryCustomerProfileService : ICustomerProfileService
    {
        private readonly Dictionary<Guid, CustomerProfile> _profiles = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCustomerProfileService"/> class with a set of
        /// pre-seeded customer profiles.
        /// </summary>
        /// <remarks>This constructor populates the service with three predefined customer profiles: a VIP
        /// customer, a new customer, and a regular customer. These profiles are stored in memory and can be used for
        /// testing or demonstration purposes.</remarks>
        public InMemoryCustomerProfileService()
        {
            // Sample seeded profiles
            var vipCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var newCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var regularCustomerId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            _profiles[vipCustomerId] = new CustomerProfile(
                vipCustomerId,
                CustomerSegment.VIP,
                12,
                750.00m
            );

            _profiles[newCustomerId] = new CustomerProfile(
                newCustomerId,
                CustomerSegment.New,
                0,
                0.00m
            );

            _profiles[regularCustomerId] = new CustomerProfile(
                regularCustomerId,
                CustomerSegment.Regular,
                3,
                240.00m
            );
        }

        public async Task<CustomerProfile?> GetProfileAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            _profiles.TryGetValue(customerId, out var profile);
            return await Task.FromResult(profile);
        }

        public async Task<Dictionary<Guid, CustomerProfile>> GetProfilesAsync(IEnumerable<Guid> customerIds, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<Guid, CustomerProfile>();
            foreach (var id in customerIds)
            {
                if (_profiles.TryGetValue(id, out var profile))
                {
                    result[id] = profile;
                }
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Utility method for seeding orders.
        /// </summary>
        /// <param name="profile">The <see cref="CustomerProfile"/> to be added to 
        /// the customers store.</param>
        public void Add(CustomerProfile profile) => _profiles[profile.CustomerId] = profile;
    }
}
