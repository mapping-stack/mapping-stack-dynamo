namespace MappingStack.Dynamo
{
    using System.Collections.Generic;

    using System.Web.OData.Query;

    using MappingStack.Dynamo.Parsing;
    using MappingStack.OData.Net;

    /// <summary>
    /// Container class for static auxilliary methods related to Dynamo library
    /// </summary>
    public static partial class DynamoAux
    {
        public static ODataQueryOptions AdjustOptions(ODataQueryOptions options, IEnumerable<DynamoContext> props)
        {
            ODataQueryOptions newOptions = null;
            if (options.Filter != null) // dyn filter needs adjustment
            {   // string filterRawValue = options.Filter.RawValue;
                var factory = new QueryOptionsFactory(options);
                factory.Filter = options.Filter?.FilterClause.Expression.Accept(new DynamoQueryNodeVisitor(props));
                if (options.SelectExpand != null)
                {
                    factory.Expand = options.SelectExpand?.RawExpand;
                    factory.Select = options.SelectExpand?.RawSelect;
                }
                factory.Top   = options.Top?.RawValue;
                factory.Skip  = options.Skip?.RawValue;
                factory.Count = options.Count?.RawValue;
                newOptions = factory.Get();
            }
            return newOptions ?? options;
        }
    }
}
