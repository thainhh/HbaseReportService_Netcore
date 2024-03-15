using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security;

namespace HbaseReportService.Hbase
{
    public static class Extensions
    {
        private static readonly string HbaseSectionName = "Hbase";
        public static IServiceCollection AddHbase(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            var hbaseOptions = new HbaseOption();
            config.Bind(HbaseSectionName, hbaseOptions);
            services.AddSingleton(hbaseOptions);
            services.Configure<HbaseOption>(config.GetSection(HbaseSectionName));
            services.AddTransient<IHBaseClient, HBaseClient>();
            return services;
        }
        internal static SecureString ToSecureString(this string value)
        {
            if (value == null)
            {
                return null;
            }

            var rv = new SecureString();
            try
            {
                foreach (char c in value)
                {
                    rv.AppendChar(c);
                }

                return rv;
            }
            catch (Exception)
            {
                rv.Dispose();
                throw;
            }
        }
    }
}
