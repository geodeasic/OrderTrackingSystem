using Orders.Application.Contract.Persistence;
using Orders.Application.Contract.Promotion;
using Orders.Application.Contract.Services;
using Orders.Domain.Contracts;
using Orders.Domain.Promotions;
using Orders.Infrastructure.Persistence.InMemory;
using Orders.Infrastructure.Promotions;
using Orders.Infrastructure.Services.InMemory;
using System.Reflection;

namespace Orders.ApiService.ServiceInstallers
{
    /// <summary>
    /// This static class implements extension methods for adding services 
    /// into the dependency injection container.
    /// </summary>
    public static class ServiceInstaller
    {
        /// <summary>
        /// Adds services to the dependency injection container.
        /// </summary>
        /// <param name="services">The dependency injection container.</param>
        /// <returns>The dependency injection container.</returns>
        public static IServiceCollection InstallServices(this IServiceCollection services)
        {
            // Register In-Memory Repositories and Services
            services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
            services.AddSingleton<ICustomerProfileService, InMemoryCustomerProfileService>();
            services.AddSingleton<IOrderAnalyticsService, OrderAnalyticsService>();
            services.AddSingleton<IOrderStatusService, OrderStatusService>();
            services.AddSingleton<IPromotionEngine, DefaultPromotionEngine>();
            services.AddSingleton<IOrderQueryService, OrderQueryService>();

            // Register all promotion rules
            services.AddSingleton<IPromotionRule, FirstOrderDiscountRule>();
            services.AddSingleton<IPromotionRule, HighValueOrderDiscountRule>();
            services.AddSingleton<IPromotionRule, LoyaltyDiscountRule>();
            services.AddSingleton<IPromotionRule, VipCustomerDiscountRule>();

            // Add other rules here as needed
            services.AddSingleton<IPromotionEngine, DefaultPromotionEngine>();
            services.AddSingleton<IOrderQueryService, OrderQueryService>();

            return services;
        }
    }
}
