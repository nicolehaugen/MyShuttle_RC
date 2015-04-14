
namespace MyShuttle.Web.AppBuilderExtensions
{
    using Data;
    using DataContextConfigExtensions;
    using Microsoft.Framework.ConfigurationModel;
    using Microsoft.Framework.DependencyInjection;
    using Microsoft.Data.Entity;
    using System;

    public static class DataContextExtensions
    {
        public static IServiceCollection ConfigureDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            //If this type is present - we're on mono
            var runningOnMono = Type.GetType("Mono.Runtime") != null;

            services.AddEntityFramework()
                    .AddStore(runningOnMono)
                    .AddDbContext<MyShuttleContext>(options =>
                    {
                        if (runningOnMono)
                        {
                            options.UseInMemoryStore();
                        }
                        else
                        {
                            options.UseSqlServer(configuration.Get("Data:DefaultConnection:Connectionstring"));
                        }
                    });

            return services;
        }


    }
}