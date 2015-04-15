
namespace MyShuttle.Web.AppBuilderExtensions
{
    using Microsoft.AspNet.Builder;

    public static class SecurityExtensions
    {
        public static IApplicationBuilder ConfigureSecurity(this IApplicationBuilder app)
        {
            return app.UseIdentity();
        }

    }
}