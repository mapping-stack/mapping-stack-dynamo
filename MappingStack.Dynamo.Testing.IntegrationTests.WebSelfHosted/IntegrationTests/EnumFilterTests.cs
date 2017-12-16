using System;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MappingStack.Dynamo.Testing.TestFactory;
using MappingStack.Dynamo.Testing.TestFactory.DtoModel;

using static MappingStack.Dynamo.Testing.IntegrationTests.TestAssembly;
using static MappingStack.Dynamo.Testing.WebSelfHosted.InProcessStartup.ODataConfig;

namespace MappingStack.Dynamo.Testing.IntegrationTests
{   using TResult = Newtonsoft.Json.Linq.JObject;
    [TestClass]
    public class EnumFilterTests
    {
        private readonly string _enumString =        EnumScalar.EnumScalarSecond .ToString();
        private readonly string _enumInt    = ((int) EnumScalar.EnumScalarSecond).ToString();
        private readonly string _enumType = $"{typeof(EnumScalar).Namespace}.{typeof(EnumScalar).Name}";

        [TestMethod] public async Task EnumQualifiedTest  () => await EnumLiteralTest($"{_enumType}'{_enumString}'");
        [TestMethod] public async Task EnumUnqualifiedTest() => await EnumLiteralTest(           $"'{_enumString}'");
        [TestMethod] public async Task  IntQualifiedTest  () => await EnumLiteralTest($"{_enumType}'{_enumInt   }'");
        [TestMethod] public async Task  IntUnqualifiedTest() => await EnumLiteralTest(           $"'{_enumInt   }'");

        private static async Task EnumLiteralTest(string enumLiteral)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SampleDto dto = null;
            string uriString = $"{InProcess.LocalHttp}/{RoutePrefix}/{nameof(SampleDto)}?$filter={nameof(dto.@enum)} eq {enumLiteral}";
            Task<HttpResponseMessage> a = client.GetAsync(new Uri(uriString));
            a.Should().NotBeNull();
            HttpResponseMessage response = await a;
            response.Should().NotBeNull();

            var s = await response.Content.ReadAsStringAsync();
            response.IsSuccessStatusCode.Should().BeTrue(s.Replace("{", "{{").Replace("}", "}}"));
            TResult result = await response.Content.ReadAsAsync<TResult>();
            result.Should().NotBeNull().And.BeOfType<TResult>();
        }
    }
}
