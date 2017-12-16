using System.Web.Http;
using System.Web.OData.Extensions;

using Owin;

namespace MappingStack.Dynamo.Testing.WebSelfHosted.InProcessStartup
{
    /// <summary>
    /// StartupInProcess - includes only necessary things for integration testing
    /// </summary>
    public partial class SelfHostedStartup
    {
        // This code configures Web API. The Startup class is specified as a type parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            ConfigureCors(appBuilder); // TODO: do we really need it in Integation Tests??

            HttpConfiguration config = new HttpConfiguration();
            config.EnableDependencyInjection(); //1   using System.Web.OData.Extensions;
            // config.EnsureInitialized(); //2           using System.Web.Http;

            WebApiConfig.Register(config);
            ODataConfig.Register(config);

            appBuilder.UseWebApi(config);
            // appBuilder.UseWebApiHelpPage(config);
        }
    }
}

