﻿namespace MappingStack.Dynamo.Parsing
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.OData.UriParser;

    using MappingStack.DynamicProperties;
    using MappingStack.LambdaReflection.Extensions;
    using MappingStack.OData.Parsing.Extensions;
    using MappingStack.OData.QueryNodeVisiting;

    public class DynamoQueryNodeVisitor
        : QueryNodeNormalizingVisitor
    {
        private readonly IEnumerable<DynamoContext> props;

        public DynamoQueryNodeVisitor(IEnumerable<DynamoContext> props)
        {
            this.props = props;
            this.IsWrapWithParentheses = true;
        }

        public override string Visit(BinaryOperatorNode nodeIn)
        {
            string factoryFilter = null;
            if (
                nodeIn.Left is SingleValueOpenPropertyAccessNode
                ||
                nodeIn.Left is ConvertNode && ((ConvertNode) nodeIn.Left).Source is SingleValueOpenPropertyAccessNode
                // SingleValueOpenPropertyAccessNode.Left.Kind == QueryNodeKind.CollectionOpenPropertyAccess
//                ||
//                nodeIn.Right.Kind == QueryNodeKind.CollectionOpenPropertyAccess
            )
            {
                string opKeyword = nodeIn.OperatorKind.GetKeyword();
                string left = nodeIn.Left.Accept(this);
                string right = nodeIn.Right.Accept(this);

                SingleValueOpenPropertyAccessNode l = (nodeIn.Left as SingleValueOpenPropertyAccessNode) ?? (((ConvertNode)nodeIn.Left).Source as SingleValueOpenPropertyAccessNode);

                string name = l?.Name;

                string b =  $"{left} {opKeyword} {right}";

                foreach (DynamoContext prop in props)
                {
                    string dynFlag = prop.DynTypedListLambdas.FirstOrDefault().GetMemberName();
                    string dynamo = left.Replace($"/{name}", "");
                    
                    DynamoOptionKey key = prop.OptionKeys.FirstOrDefault(_ => _.jsonId == name);

                    if (key == null) continue;

                    // odata
                    string any = "any";
                    string all = "all";
                    string eq = "eq";
                    string and = "and";

                    // identifiers
                    string dynamicKey = "dynamicKey"; // IDynamicOptionGeneral.optionKey
                    string value = "value"; // IDynamicOptionGeneral.value
                    string o = "o";

                    factoryFilter = WrapWithParentheses(() => $"{dynamo}/{dynFlag}/{any}({o}: {o}/{dynamicKey} {eq} '{key.key}' {and} {o}/{value} {opKeyword} {right})");
                    break;
                }
            }
            return factoryFilter ?? base.Visit(nodeIn);
        }
    }
}