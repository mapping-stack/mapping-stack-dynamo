using Microsoft.VisualStudio.TestTools.UnitTesting;

using MappingStack.Dynamo.Testing.WebSelfHosted.InProcessStartup;

namespace MappingStack.Dynamo.Testing.IntegrationTests
{
    [TestClass]
    public class TestAssembly
    {
        public static string BaseUrl => $"{InProcess.LocalHttp}/{ODataConfig.RoutePrefix}";
        public static InProcessWebApp InProcess { get; private set; }
        public static short Port => 9011;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // AppDomain.CurrentDomain.Load(typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener).Assembly.GetName());
            InProcess = InProcessWebApp.Start<SelfHostedStartup>(Port); // public const string InProcess_LocalHttp = "http://localhost:9011";
        }

        [AssemblyCleanup] public static void AssemblyCleanup() => InProcess.Dispose();
    }
}
