namespace MappingStack.OData.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Http;
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
        
        private readonly ODataQueryOptions _originalOptions;
        private readonly IEdmModel _edmModel;
        private readonly Type _elementClrType;
        private readonly ODataValidationSettings _validationSettings;

        private string BaseRequestUri => Uri != null ? $"{Uri.Scheme}{Uri.SchemeDelimiter}{Uri.Authority}{Uri.LocalPath}" : "https://fake.org/odata/controller";

        private HttpConfiguration Config  => GetOriginalHttpConfig()               ?? GetNewHttpConfig();
        private ODataQueryContext Context => _originalOptions?.Context             ?? GetODataQueryContext();
        private Uri Uri                   => _originalOptions?.Request?.RequestUri /*?? new Uri("https://fake.org/odata/controller")*/;

        public QueryOptionsFactory(IEdmModel edmModel, Type elementClrType)
        {
            _edmModel = edmModel;
            _elementClrType = elementClrType;
            _validationSettings = null; // new ODataValidationSettings();
        }

        public QueryOptionsFactory(ODataQueryOptions options)
            : this(options.Context.Model, options.Context.ElementClrType)
        {
            _originalOptions = options;
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

        private string RequestUri
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
                // return $"https://fake.org/odata/controller?{acc}";
                return $"{BaseRequestUri}?{acc}";
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
            HttpRequestMessage request = GetNewHttpRequestMessage();
//            foreach(var p in _originalOptions.Request.Properties)
//                if (! request.Properties.Keys.Contains(p.Key))
//                    request.Properties.Add(p.Key, p.Value);

            ODataQueryOptions opts = new ODataQueryOptions(Context, request);

            if (_validationSettings != null)
            {
                var settings = _validationSettings;
//                opts.Validator = new Validator(){};// opts.Validator;
//                opts.Validate(settings);
            }
            return opts;
        }

        private HttpConfiguration GetOriginalHttpConfig() => _originalOptions?.Request?.Properties["MS_HttpConfiguration"] as System.Web.Http.HttpConfiguration;

        private HttpRequestMessage GetNewHttpRequestMessage()
        {
            var request = new /*System.Net.Http.*/HttpRequestMessage(/*System.Net.Http.*/HttpMethod.Get, RequestUri);
            request.SetConfiguration(Config);
            return request;
        }

        private static HttpConfiguration GetNewHttpConfig()
        {
            HttpConfiguration config = new System.Web.Http.HttpConfiguration();
            config.EnableDependencyInjection(); //1
            config.EnsureInitialized(); //2 
            return config;
        }

        private ODataQueryContext GetODataQueryContext()
        {
            IEdmEntitySet set = _edmModel.EntityContainer.EntitySets()
                // .FirstOrDefault(_ => (_.Type as EdmCollectionType)?.ElementType.Definition.FullTypeName() == typeof(TDto).FullName);
                .FirstOrDefault(_ => (_.Type as EdmCollectionType)?.ElementType.Definition.FullTypeName() == $"{_elementClrType.Namespace}.{_elementClrType.Name}");
            // ODataPath path = new ODataPath();
            ODataPath path = new ODataPath(new EntitySetSegment(set));
//          new EntitySetPathSegment("FakeEntitySet"),new KeyValuePathSegment("FakeKey"),new PropertyAccessPathSegment("FakeProperty"),new ValuePathSegment()
                
            ODataQueryContext context = new ODataQueryContext(_edmModel, _elementClrType, path);
            return context;
        }
    }
}
