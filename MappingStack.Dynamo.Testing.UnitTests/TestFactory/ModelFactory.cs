namespace MappingStack.Dynamo.Testing.TestFactory
{
    using System.Web.OData.Builder;
    using System.Web.OData.Query;
    using MappingStack.OData.Net;
    using Microsoft.OData.Edm;

    public partial class ModelFactory
    {
        public static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Dto>($@"{nameof(Dto)}FakeController");
            //builder.EntitySet<InnerDto>($@"{nameof(InnerDto)}FakeController");
            IEdmModel model = builder.GetEdmModel();
            return model;
        }

        public static ODataQueryOptions GetOptions(string filter = null)
        {
            QueryOptionsFactory factory = new QueryOptionsFactory(ModelFactory.GetModel(), typeof(ModelFactory.Dto));
            // if (filter != null)
            factory.Filter = filter;
            ODataQueryOptions options = factory.Get();
            return options;
        }
    }
}
