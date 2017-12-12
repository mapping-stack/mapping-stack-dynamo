namespace MappingStack.OData.Parsing.Extensions
{
    using Microsoft.OData.UriParser;

    using MappingStack.General.Exceptions;
    using MappingStack.General.OData;

    public static class BinaryOperatorKindExtensions
    {
        public static string GetKeyword(this BinaryOperatorKind @enum)
        {
            // TODO: find somewhere in odata libs the mapping of this enum to literal like 'eq', 'and'
            switch (@enum)
            {
                case BinaryOperatorKind.Equal               : return Keywords.Operators.EQ;
                case BinaryOperatorKind.NotEqual            : return Keywords.Operators.NE;
                case BinaryOperatorKind.GreaterThan         : return Keywords.Operators.GT;
                case BinaryOperatorKind.GreaterThanOrEqual  : return Keywords.Operators.GE;
                case BinaryOperatorKind.LessThan            : return Keywords.Operators.LT;
                case BinaryOperatorKind.LessThanOrEqual     : return Keywords.Operators.LE;

                case BinaryOperatorKind.Or                  : return Keywords.Operators.OR;
                case BinaryOperatorKind.And                 : return Keywords.Operators.AND;

                case BinaryOperatorKind.Add                 : return Keywords.Operators.ADD;
                case BinaryOperatorKind.Subtract            : return Keywords.Operators.SUB;
                case BinaryOperatorKind.Multiply            : return Keywords.Operators.MUL;
                case BinaryOperatorKind.Divide              : return Keywords.Operators.DIV;
                case BinaryOperatorKind.Modulo              : return Keywords.Operators.MOD;

                case BinaryOperatorKind.Has                 : return Keywords.Operators.HAS;

                default: throw new NotSupportedByMappingStackException($"UNKNOWN_BINARY_OPERATOR: {@enum.ToString()}");
            }
        }
    }
}
