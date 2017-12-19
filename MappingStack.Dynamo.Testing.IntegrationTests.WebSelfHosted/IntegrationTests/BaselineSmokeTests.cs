using System;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MappingStack.Dynamo.Testing.TestFactory.DtoModel;

using static MappingStack.Dynamo.Testing.IntegrationTests.TestAssembly;

namespace MappingStack.Dynamo.Testing.IntegrationTests
{   using TResult = Newtonsoft.Json.Linq.JObject;
    [TestClass]
    public class BaselineSmokeTests
    {
        [TestMethod] public async Task MetadataSmokeTest()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            string uriString = $"{BaseUrl}/$metadata?$format=json";
            Task<HttpResponseMessage> a = client.GetAsync(new Uri(uriString));
            a.Should().NotBeNull();
            HttpResponseMessage response = await a;
            response.Should().NotBeNull();

            response.IsSuccessStatusCode.Should().BeTrue();
            string result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNull().And.StartWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        }
 #region Sample Response here
/*
 <edmx:Edmx Version="4.0">
    <edmx:DataServices>
        <Schema Namespace="MappingStack.Dynamo.Testing.TestFactory">
            <EntityType Name="SampleDto">
                <Key><PropertyRef Name="id"/></Key>
                <Property Name="id" Type="Edm.Int32" Nullable="false"/>
                <Property Name="title" Type="Edm.String"/>
                <Property Name="enum" Type="MappingStack.Dynamo.Testing.TestFactory.EnumScalar"/>
                <Property Name="dynamo" Type="MappingStack.Dynamo.Testing.TestFactory.DynamoDto"/>
                <NavigationProperty Name="inner" Type="MappingStack.Dynamo.Testing.TestFactory.InnerDto"/>
            </EntityType>
            <EntityType Name="InnerDto">
                <Key><PropertyRef Name="id"/></Key>
                <Property Name="id" Type="Edm.Int32" Nullable="false"/>
                <Property Name="level" Type="Edm.Int32" Nullable="false"/>
                <Property Name="description" Type="Edm.String"/>
            </EntityType>
            <ComplexType Name="DynamoDto" OpenType="true">
                <NavigationProperty Name="dynBool" Type="Collection(MappingStack.Dynamo.BaseDto.DynamicBool)"/>
            </ComplexType>
            <EnumType Name="EnumScalar">
                <Member Name="EnumScalarFirst" Value="0"/>
                <Member Name="EnumScalarSecond" Value="1"/>
                <Member Name="EnumScalarThird" Value="2"/>
            </EnumType>
        </Schema>
        <Schema Namespace="MappingStack.Dynamo.BaseDto">
            <EntityType Name="DynamicBool">
                <Key><PropertyRef Name="entityId"/><PropertyRef Name="dynamicKey"/></Key>
                <Property Name="entityId" Type="Edm.Int32" Nullable="false"/>
                <Property Name="dynamicKey" Type="Edm.String" Nullable="false"/>
                <Property Name="value" Type="Edm.Boolean" Nullable="false"/>
            </EntityType>
        </Schema>
        <Schema Namespace="Default">
            <EntityContainer Name="Container">
                <EntitySet Name="SampleDtoController" EntityType="MappingStack.Dynamo.Testing.TestFactory.SampleDto"/>
            </EntityContainer>
        </Schema>
    </edmx:DataServices>
</edmx:Edmx>
 */
#endregion

        [TestMethod] public async Task SampleDtoSmokeTest()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string uriString = $"{BaseUrl}/{nameof(SampleDto)}";
            Task<HttpResponseMessage> a = client.GetAsync(new Uri(uriString));
            a.Should().NotBeNull();
            HttpResponseMessage response = await a;
            response.Should().NotBeNull();
            
            string stringResult = await response.Content.ReadAsStringAsync  (); stringResult.Should().NotBeNull();
            object objectResult = await response.Content.ReadAsAsync<object>(); objectResult.Should().NotBeNull().And.BeOfType<TResult>();
            response.IsSuccessStatusCode.Should().BeTrue();
            var result = objectResult as TResult;
            result.Should().NotBeNull();
            result?.Value<string>("@odata.context").Should().Be($"{BaseUrl}/$metadata#SampleDto");
        }

        [TestMethod]
        public async Task EnumSelfCompareSmokeTest()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string uriString = $"{BaseUrl}/{nameof(SampleDto)}?$filter=enum eq enum";   // MappingStack.Dynamo.Testing.TestFactory.ModelFactory.EnumScalar'EnumScalarSecond'
            Task<HttpResponseMessage> a = client.GetAsync(new Uri(uriString));
            a.Should().NotBeNull();
            HttpResponseMessage response = await a;
            response.Should().NotBeNull();

            var s = await response.Content.ReadAsStringAsync();
            response.IsSuccessStatusCode.Should().BeTrue();
            TResult result = await response.Content.ReadAsAsync<TResult>();
            result.Should().NotBeNull().And.BeOfType<TResult>();
        }
    }
}
