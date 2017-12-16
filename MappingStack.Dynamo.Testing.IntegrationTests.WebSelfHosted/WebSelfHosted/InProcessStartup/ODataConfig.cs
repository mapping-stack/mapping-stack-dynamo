using System.Collections.Generic;

using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Routing.Conventions;

using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

using ModelFactory = MappingStack.Dynamo.Testing.TestFactory.ModelFactory;

namespace MappingStack.Dynamo.Testing.WebSelfHosted.InProcessStartup
{
    public static class ODataConfig
    {
        public static readonly string RoutePrefix = "odata";
        public static IEdmModel EdmModel = ModelFactory.GetModel();

        private static string _routeName;

        public static void Register(HttpConfiguration config)
        {
            // TODO: ?? do we need eliminating necessity using full-qualified names ??:
            // http://odata.github.io/WebApi/#06-01-custom-url-parsing
//            config.EnableCaseInsensitive(caseInsensitive: true);
//            config.EnableUnqualifiedNameCall(unqualifiedNameCall: true);

            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null); //new line

            _routeName = "DefaultODataRoute";
            config.MapODataServiceRoute(_routeName, RoutePrefix, 
                // edmModel
                builder => ContainerBuilderExtensions.AddService(builder, ServiceLifetime.Singleton, sp => EdmModel)
                .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton,
                    sp => ODataRoutingConventions.CreateDefaultWithAttributeRouting(_routeName, config))
                .AddService<ODataUriResolver>(ServiceLifetime.Singleton, sp => new StringAsEnumResolver { EnableCaseInsensitive = true })
            );
            config.EnsureInitialized();
        }
    }
}
