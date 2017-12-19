namespace MappingStack.OData.Parsing.QueryNodeVisitors
{
    using System;
    using System.Linq;
    using System.Text;

    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;

    using MappingStack.OData.Parsing.Extensions;

    /// <inheritdoc />
    /// <summary>
    /// Normalizes (rewrites) the OData filter expesstion by calling <see cref="QueryNodeVisitor{T}.Visit(SingleValueNode)"></see> // options.Filter?.FilterClause.Expression.Accept(new QueryNodeNormalizingVisitor())
    /// </summary>
    /// <remarks>
    /// implementation started from sample in https://github.com/OData/ODataSamples/blob/master/Components/Common/Extensions/QueryNodeToStringVisitor.cs
    /// </remarks>
    public class QueryNodeNormalizingVisitor : QueryNodeVisitor<string>
    {
        public bool IsWrapWithParentheses { get; set; }

        public override string Visit(ConstantNode nodeIn)
        {
            EdmEnumType edmEnumType = (nodeIn.TypeReference as EdmTypeReference)?.Definition as EdmEnumType;
            bool isEnumType = edmEnumType != null;
            bool isQualifiedEnumLiteral = isEnumType && nodeIn.LiteralText.StartsWith(edmEnumType.Namespace);
            if (isEnumType && ! isQualifiedEnumLiteral)
            {
                IEdmPrimitiveType edmPrimitiveType = edmEnumType.UnderlyingType;
                if (   edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.Byte
                    || edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.Int16
                    || edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.Int32
                    || edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.Int64
                    || edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.SByte
                )
                {
                    string normalized = $"{edmEnumType.Namespace}.{edmEnumType.Name}'{nodeIn.LiteralText}'";
                    return normalized;
                }
            }
            return nodeIn.LiteralText;
        }

        public override string Visit(ConvertNode  nodeIn) => nodeIn.Source.Accept(this); // WrapWithIdent(string.Format("ConvertNode:[{0}<={1}]", nodeIn.TypeReference, nodeIn.Source.Accept(this)));
        public override string Visit(SingleValuePropertyAccessNode     nodeIn) => $"{AcceptAsPrefix(nodeIn.Source)}{nodeIn.Property.Name}"; // WrapWithIdent(string.Format(    "Property:[{0}<={1}]", nodeIn.Property.Name,nodeIn.Source.Accept(this)));
        public override string Visit(SingleValueOpenPropertyAccessNode nodeIn) => $"{AcceptAsPrefix(nodeIn.Source)}{nodeIn         .Name}"; // WrapWithIdent(string.Format("OpenProperty:[{0}]", nodeIn.Name));
        public override string Visit(SingleValueFunctionCallNode       nodeIn) => $"{nodeIn.Name}{WrapWithParentheses(() => string.Join($"{_CM}{_SP}", nodeIn.Parameters.Select(_ => _.Accept(this))))}";
        public override string Visit(SingleComplexNode                 nodeIn) => $"{nodeIn.Property.Name}{nodeIn.Source.Accept(this)}"; // return base.Visit(nodeIn);
        public override string Visit(SingleNavigationNode nodeIn)
        {
            string r = $"{AcceptAsPrefix(nodeIn.Source)}{nodeIn.NavigationProperty.Name}";
            return r; // WrapWithIdent(string.Format("SingleNavigationNode:[{0}<={1}]", nodeIn.TypeReference, nodeIn.NavigationSource));
        }
        public override string Visit(CollectionNavigationNode nodeIn)
        {
            string r = $"{AcceptAsPrefix(nodeIn.Source)}{nodeIn.NavigationProperty.Name}";
            return r; // WrapWithIdent(string.Format("CollectionNavigationNode:[{0}<={1}]", nodeIn.CollectionType, nodeIn.Source.Accept(this)));
        }

        public override string Visit(BinaryOperatorNode nodeIn) => WrapBinary(() => $"{nodeIn.Left.Accept(this)}{_SP}{nodeIn.OperatorKind.GetKeyword()}{_SP}{nodeIn.Right.Accept(this)}");
        public override string Visit(NonResourceRangeVariableReferenceNode nodeIn) => nodeIn.Name; // return base.Visit(nodeIn);
        public override string Visit(ResourceRangeVariableReferenceNode nodeIn) // EntityRangeVariableReferenceNode
        {
            // assert nodeIn.Name == "$it"
            // var r = string.Empty; // nodeIn.Name;
            var r = nodeIn.RangeVariable.CollectionResourceNode == null /* nodeIn.Name == "$it" */ ? string.Empty : nodeIn.Name;
            return r; // + nodeIn.
            // return WrapWithIdent(string.Format("EntityRangeVariableReferenceNode:[{0}<={1}]", nodeIn.TypeReference, nodeIn.Name));
        }

        public override string Visit(AnyNode nodeIn) => WrapBinary(() => $"{nodeIn.Source.Accept(this)}/any({nodeIn.CurrentRangeVariable.Name}: {nodeIn.Body.Accept(this)})"); // return WrapWithIdent(string.Format("AnyNode:[{0}<={1}\nExp={2}]", nodeIn.TypeReference, nodeIn.Source.Accept(this),nodeIn.Body.Accept(this)));
        public override string Visit(AllNode nodeIn) => WrapBinary(() => $"{nodeIn.Source.Accept(this)}/all({nodeIn.CurrentRangeVariable.Name}: {nodeIn.Body.Accept(this)})"); // return WrapWithIdent(string.Format("AllNode:[{0}<={1}\nExp={2}]", nodeIn.TypeReference, nodeIn.Source.Accept(this),nodeIn.Body.Accept(this)));

        public override string Visit(CollectionOpenPropertyAccessNode nodeIn) => $"{AcceptAsPrefix(nodeIn.Source)}{nodeIn         .Name}"; // return base.Visit(nodeIn);

        private string AcceptAsPrefix(SingleValueNode nodeInSource)
        {
            string prefix = nodeInSource.Accept(this);
            return prefix.Length > 0 ? $"{prefix}{_SL}": prefix;
        }

        private string WrapBinary(Func<string> e) => IsWrapWithParentheses ? WrapWithParentheses(e) : NoWrap(e);
        private static string NoWrap(Func<string> e) => e.Invoke();
        protected string WrapWithParentheses(Func<string> e) => $@"({NoWrap(e)})";

        private static readonly string _NL = Environment.NewLine;
        private static readonly string _SL = "/"; // slash  -  Navigation Operator
        private static readonly string _SP = " "; // space  - separator
        private static readonly string _CM = ","; // comma  - separator (of actual parameters in function call)

        #region override, still not researched or encountered, to move up eventually

        public override string Visit(CollectionPropertyAccessNode nodeIn)
        {
            return base.Visit(nodeIn);
        }

        public override string Visit(NamedFunctionParameterNode nodeIn)
        {
            return WrapWithIdent(
                string.Format("Parameter:[{0}]", nodeIn.Name),
                () => nodeIn.Value.Accept(this));
        }

        #endregion

#region Ident from Sample, to eliminate eventually
// This part of code is taken from https://github.com/OData/ODataSamples/blob/master/Components/Common/Extensions/QueryNodeToStringVisitor.cs
// It is licensed with https://github.com/OData/ODataSamples/blob/master/License.txt
// The code is used for debug purposes and will be eliminated as soon as normalizing is adjusted properly
//
// OData-Samples
//
// Copyright (c) Microsoft. All rights reserved.
//
// The MIT License (MIT)
// 
//
        private const string Ident = "  ";
        private string _currentIdent = string.Empty;
        private string WrapWithIdent(string current, Func<string> inner = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_currentIdent);
            builder.Append("{");
            builder.Append(current);
            if (inner != null)
            {
                builder.Append(_NL);
                string saveIdent = _currentIdent;
                _currentIdent = _currentIdent + Ident;
                builder.Append(inner());
                _currentIdent = saveIdent;
                builder.Append(_NL);
                builder.Append(_currentIdent);
            }
            builder.Append("}");
            return builder.ToString();
        }
#endregion
    }
}
