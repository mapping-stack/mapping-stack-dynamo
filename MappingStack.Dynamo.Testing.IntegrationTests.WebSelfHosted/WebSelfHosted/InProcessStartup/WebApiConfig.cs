using System.Web.Http;

namespace MappingStack.Dynamo.Testing.WebSelfHosted.InProcessStartup
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Output JSON by Default
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));

            //Enable Tracing
            // config.EnableSystemDiagnosticsTracing();
        }
    }
}
