using Microsoft.Owin.Cors;
using Owin;

namespace MappingStack.Dynamo.Testing.WebSelfHosted.InProcessStartup
{
    /// <summary>
    /// OWIN Startup Class
    /// Registers Owin Middleware Command Pipeline
    /// We use it for Authentication and CORS configuration
    /// </summary>
    public partial class SelfHostedStartup
    {
        private void ConfigureCors(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
        }
    }
}
