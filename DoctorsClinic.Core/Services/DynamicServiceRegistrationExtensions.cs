using DoctorsClinic.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services
{
    public static class DynamicServiceRegistrationExtensions
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !a.FullName!.StartsWith("System") && !a.FullName.StartsWith("Microsoft"));

            var scopedServiceType = typeof(IScopedService);

            var scopedServices = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && scopedServiceType.IsAssignableFrom(type))
                .Select(type => new
                {
                    ServiceInterface = type.GetInterfaces().FirstOrDefault(i => i != scopedServiceType),
                    Implementation = type
                })
                .Where(t => t.ServiceInterface != null);

            foreach (var service in scopedServices)
            {
                services.AddScoped(service.ServiceInterface!, service.Implementation);
            }

            return services;
        }
    }
}