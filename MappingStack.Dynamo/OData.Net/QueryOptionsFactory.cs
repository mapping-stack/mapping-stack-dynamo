namespace MappingStack.OData.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    
    using System.Web.OData;
    using System.Web.OData.Extensions;
    using System.Web.OData.Query;

    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;

    using ODataPath = System.Web.OData.Routing.ODataPath;
    using Q = MappingStack.General.OData.QueryOptionNames;

    public class QueryOptionsFactory // TODO: remake it in cloning-adjusting way
    {
        private readonly Dictionary<string, string> @params = new Dictionary<string, string>();

        // private readonly ODataQueryOptions _options;

        private readonly IEdmModel _edmModel;
        private readonly ODataValidationSettings _validationSettings;
        private readonly Type _elementClrType;

        public QueryOptionsFactory(IEdmModel edmModel, Type elementClrType) : this(edmModel, null, elementClrType) {}

        // TODO: remake it in cloning-adjusting way
        public QueryOptionsFactory(ODataQueryOptions options)
            : this(
                // options,
                options.Context.Model,
                new ODataValidationSettings(),
                options.Context.ElementClrType
            )
        {
        }

        private QueryOptionsFactory(
            // ODataQueryOptions options, 
              IEdmModel edmModel
            , ODataValidationSettings validationSettings // TODO: construct validation settings using Controller; do not validate if null ??
            , Type elementClrType
        )
        {
            // _options = options;
            _edmModel = edmModel;
            _validationSettings = validationSettings;
            _elementClrType = elementClrType; // typeof(TDto);
        }

        //private Dictionary<string, string> @paramsToInclude { get { return @params.Where(_ => _.Value != null); } }
        private void SetParam(string paramName, string value)
        {
            if (value != null) @params[paramName] = value;
            else @params.Remove(paramName);
        }

        public string Expand  { get { return @params[Q.Expand ]; } set { ValidateExpand(value); SetParam(Q.Expand , value); } }
        public string Select  { get { return @params[Q.Select ]; } set { SetParam(Q.Select , value); } }
        public string Filter  { get { return @params[Q.Filter ]; } set { SetParam(Q.Filter , value); } }

        public string OrderBy { get { return @params[Q.OrderBy]; } set { SetParam(Q.OrderBy, value); } }

        public string Skip    { get { return @params[Q.Skip   ]; } set { SetParam(Q.Skip   , value); } }
        public string Top     { get { return @params[Q.Top    ]; } set { SetParam(Q.Top    , value); } }

        public string Count   { get { return @params[Q.Count  ]; } set { SetParam(Q.Count  , value); } }

        // public int? MaxExpansionDepth { get; set; }

        private string requestUri
        {
            get 
            {
                StringBuilder acc = new StringBuilder();
                if (@params.Any())
                {
                    KeyValuePair<string, string> f = @params.First();
                    acc.Append(f.Key).Append("=").Append(f.Value);
                    @params.Skip(1).Aggregate(acc, (a, _) => a.Append("&").Append(_.Key).Append("=").Append(_.Value));
                }
                return $"https://fake.org/?{acc}";
            }
        }

        private void ValidateExpand(string value)
        {   
            if (value == null) return; // we allow setting null
            var regex = new Regex(
                // @"\(\s*[^\$]"
                @"\(\s*[^\$]\w"
            );
            if (regex.IsMatch(value)) throw new ArgumentException(
                $@"$-started odata parameter expected within the parentheses
{Q.Expand} expession: [{value}]", 
                nameof(value));
        }


        public ODataQueryOptions Get(/* bool? validate = null */)
        {
            ODataQueryContext context = GetODataQueryContext();
            HttpRequestMessage request = GetHttpRequestMessage();
            ODataQueryOptions opts = new ODataQueryOptions(context, request);

            if (_validationSettings != null)
            {
                var settings = _validationSettings;
//                opts.Validator = new Validator(){};// opts.Validator;
//                opts.Validate(settings);
            }
            return opts;
        }

        private HttpRequestMessage GetHttpRequestMessage()
        {
            var request = new /*System.Net.Http.*/HttpRequestMessage(/*System.Net.Http.*/HttpMethod.Get, requestUri);

            var config = new System.Web.Http.HttpConfiguration();
//            var config = _options.Request.GetConfiguration();

            config.EnableDependencyInjection(); //1
            config.EnsureInitialized(); //2 

//            config.SetAllowedODataParams(); // config.Count().Filter().OrderBy().Expand().Select().MaxTop(null); //new line

            request.SetConfiguration(config);
            return request;
        }

        private ODataQueryContext GetODataQueryContext()
        {
//            var sets = model.EntityContainer.EntitySets().ToList();
//            // sets.Select(_ => _.Type).OfType<>()
//            var ofType = sets.Select(_ => _.Type).OfType<Microsoft.OData.Edm.EdmCollectionType>();
//            /*var tt = */ofType.ToList().ForEach(_ => System.Diagnostics.Debug.WriteLine(_.ElementType.Definition.FullTypeName()));

        

            IEdmEntitySet set = _edmModel.EntityContainer.EntitySets()
                // .FirstOrDefault(_ => (_.Type as EdmCollectionType)?.ElementType.Definition.FullTypeName() == typeof(TDto).FullName);
                .FirstOrDefault(_ => (_.Type as EdmCollectionType)?.ElementType.Definition.FullTypeName() == $"{_elementClrType.Namespace}.{_elementClrType.Name}");
            // set.Should().NotBeNull($"Check that the model contains the EdmCollectionType for entity {typeof(TDto).FullName}");
//            IEdmEntitySet set1 = model.EntityContainer.FindEntitySet(typeof(TDto).Name);

            // ODataPath path = new ODataPath();
            ODataPath path = new ODataPath(new EntitySetSegment(set));
//          new EntitySetPathSegment("FakeEntitySet"),new KeyValuePathSegment("FakeKey"),new PropertyAccessPathSegment("FakeProperty"),new ValuePathSegment()
                
            ODataQueryContext context = new ODataQueryContext(_edmModel, _elementClrType, path);
            return context;
        }
    }
}
