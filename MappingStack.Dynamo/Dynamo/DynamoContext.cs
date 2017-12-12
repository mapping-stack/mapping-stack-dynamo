namespace MappingStack.Dynamo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using MappingStack.DynamicProperties;
    using MappingStack.Dynamo.Exceptions;
    using MappingStack.LambdaReflection.Extensions;

    /// <summary>
    /// Dynamic - Dynanic set of options. It is unknown in advance.<para />
    /// Options - values of some (scalar?) type. The type is unknown in advance. (is type fixed fore a particular key??)<para />
    /// Isolated - The type contains only dynamic options and (probably?) expandable exposable under-the-hood collection(s?) of the options.<para />
    /// Dto - DTO - Data Transfer Object - for exposing the described above set of dynamic options as a single property of an entity. <para />
    /// Context - parameters of manipulations with options (rename to Params ??). <para />
    /// </summary>
    /// <exception cref="DynamoException"></exception>
    public class DynamoContext
    {
        /// <exception cref="DynamoException"></exception>
        public DynamoContext(
            LambdaExpression dynLambda,
            IEnumerable<LambdaExpression> dynTypedListLambdas,
            IEnumerable<DynamoOptionKey> optionKeys)
        {
            DynLambda = dynLambda;
            DynTypedListLambdas = dynTypedListLambdas;

            DtoType = dynLambda.GetMemberType();

            if (DynTypedListLambdas == null || DynTypedListLambdas.All(_ => _.GetMemberType() == DtoType)) throw new DynamoException();

            OptionKeys = optionKeys;
        }

//            private static MemberExpression GetMember(LambdaExpression lambda) => lambda.Body as MemberExpression; // ?? throw if null ?>?> // using LambdaReflection  _.GetMemberName() extension
//            private static string GetMemberName(LambdaExpression lambda) => GetMember(lambda)?.Member.Name; // ?? throw if null ?>?> // using LambdaReflection  _.GetMemberName() extension

        public Type DtoType { get; }

        public LambdaExpression DynLambda { get; }
        public IEnumerable<LambdaExpression> DynTypedListLambdas { get; }

        public string DynMemberName => DynLambda.GetMemberName();

        public IEnumerable<DynamoOptionKey> OptionKeys { get; }
    }
}
