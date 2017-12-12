namespace MappingStack.OData.Parsing.Extensions
{
    using Microsoft.OData.UriParser;

    using MappingStack.General.Exceptions;
    using MappingStack.General.OData;

    public static class UnaryOperatorKindExtensions
    {
        public static string GetKeyword(this UnaryOperatorKind @enum)
        {
            switch (@enum)
            {
                case UnaryOperatorKind.Negate  : return Symbols.Operators.Minus;
                case UnaryOperatorKind.Not     : return Keywords.Operators.NOT;

                default: throw new NotSupportedByMappingStackException($"UNKNOWN_UNARY_OPERATOR: {@enum.ToString()}");
            }
        }
    }
}
