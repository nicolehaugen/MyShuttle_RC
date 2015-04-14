
namespace MyShuttle.Web
{
    using AppBuilderExtensions;
    using Data;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Diagnostics;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Framework.Caching.Memory;
    using Microsoft.Framework.ConfigurationModel;
    using Microsoft.Framework.DependencyInjection;
    using Model;

    public class Startup
    {
        public Startup()
        {
            //Below code demonstrates usage of multiple configuration sources. For instance a setting say 'setting1' is found in both the registered sources,
            //then the later source will win. By this way a Local config can be overridden by a different setting while deployed remotely.
            Configuration = new Configuration()
                        .AddJsonFile("config.json")
                        .AddEnvironmentVariables(); //All environment variables in the process's context flow in as configuration values.
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.ConfigureDataContext(Configuration);

            // Register MyShuttle dependencies
            services.ConfigureDependencies();

            //Add Identity services to the services container
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyShuttleContext>()
                .AddDefaultTokenProviders();

            CookieServiceCollectionExtensions.ConfigureCookieAuthentication(services, options =>
            {
                options.LoginPath = new Microsoft.AspNet.Http.PathString("/Carrier/Login");
            });

            // Add MVC services to the services container
            services.AddMvc();

            services
                .AddSignalR(options =>
                {
                    options.Hubs.EnableDetailedErrors = true;
                });

            //Add InMemoryCache
            services.AddSingleton<IMemoryCache, MemoryCache>();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Add static files to the request pipeline
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline
            app.UseIdentity();

            /* Error page middleware displays a nice formatted HTML page for any unhandled exceptions in the request pipeline.
             * Note: ErrorPageOptions.ShowAll to be used only at development time. Not recommended for production.
             */
            app.UseErrorPage(ErrorPageOptions.ShowAll);

            app.ConfigureSecurity();

            //Configure SignalR
            app.UseSignalR();

            // Add MVC to the request pipeline
            app.ConfigureRoutes();

            MyShuttleDataInitializer.InitializeDatabaseAsync(app.ApplicationServices).Wait();

        }
    }

}
