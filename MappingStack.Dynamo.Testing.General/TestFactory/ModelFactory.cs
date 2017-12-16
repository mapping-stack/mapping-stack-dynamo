using System.Web.OData.Builder;
using System.Web.OData.Query;

using MappingStack.OData.Net;
using Microsoft.OData.Edm;

namespace MappingStack.Dynamo.Testing.TestFactory
{
    public class ModelFactory
    {
        public static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<SampleDto>(nameof(SampleDto));
            return builder.GetEdmModel();
        }

        public static ODataQueryOptions GetOptions(string filter = null)
        {
            var factory = new QueryOptionsFactory(GetModel(), typeof(SampleDto));
            factory.Filter = filter;
            return factory.Get();
        }
    }
}
