using System;
using System.Collections.Generic;
using Microsoft.Owin.Hosting;

namespace MappingStack.Dynamo.Testing.IntegrationTests
{
    public class InProcessWebApp
    {
        public static InProcessWebApp Start<TStartup>(short port)
        {
            InProcessWebApp app = new InProcessWebApp(port);
            app.Start<TStartup>();
            return app;
        }
        public void Dispose()
        {
            webapp.Dispose();
            started.Remove(port);
        }

        public string LocalHttp { get; }

        private IDisposable webapp;
        private readonly short port;

        private static readonly Dictionary<int, InProcessWebApp> started = new Dictionary<int, InProcessWebApp>();
        private InProcessWebApp(short port)
        {
            this.port = port;
            LocalHttp = $"http://localhost:{port}";
        }
        private void Start<TStartup>()
        {
            AppDomain.CurrentDomain.Load(typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener).Assembly.GetName());

            started.Add(port, this);
            // StartOptions options = new StartOptions(){Urls = new List<string>()};
            webapp = WebApp.Start<TStartup>(
                // options
                LocalHttp
            );
        }
    }
}
