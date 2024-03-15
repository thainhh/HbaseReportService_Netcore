using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HbaseReportService.Services.Base;
using Scrutor;

namespace HbaseReportService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<IReportBaseService>()
                 .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
            return services;
        }
    }
}
