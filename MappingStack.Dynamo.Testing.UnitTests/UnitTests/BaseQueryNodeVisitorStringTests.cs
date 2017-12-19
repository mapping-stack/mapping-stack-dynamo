using MappingStack.Dynamo.Testing.TestFactory;
using MappingStack.Dynamo.Testing.TestFactory.DtoModel;

namespace MappingStack.Dynamo.Testing.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.OData.Query;

    using Microsoft.OData.UriParser;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
    using IAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;
    using M = System.Reflection.MethodBase;
    using S = System.String; // string

    public abstract class BaseQueryNodeVisitorStringTests
    {
        [T] public void Empty    () => RewriteFilter(ModelFactory.GetOptions()).Should().BeNull();

        [T] public void True     () => A(M.GetCurrentMethod());
        [T] public void False    () => A(M.GetCurrentMethod());
        [T] public void Eq       () => A(M.GetCurrentMethod());

        [T] public void Function0() => A(M.GetCurrentMethod());
        [T] public void Function1() => A(M.GetCurrentMethod());

        [T] public void Inner0   () => A(M.GetCurrentMethod());
        [T] public void Inner1   () => A(M.GetCurrentMethod());

        [T, I] public void Collection0   () => A(M.GetCurrentMethod());
        [T]    public void CollectionAny0() => A(M.GetCurrentMethod());
        [T]    public void CollectionAll0() => A(M.GetCurrentMethod());

        [T] public void Open0    () => A(M.GetCurrentMethod());
        [T] public void Open1    () => A(M.GetCurrentMethod());
        [T] public void Open2    () => A(M.GetCurrentMethod());
        [T] public void Open3    () => A(M.GetCurrentMethod());
        [T] public void Open4    () => A(M.GetCurrentMethod());

        [T] public void DeepAny      () => A(M.GetCurrentMethod());
        [T] public void DeepAnyPar   () => A(M.GetCurrentMethod());
        [T] public void DeepAnyPar2  () => A(M.GetCurrentMethod());

        [T, I] public void FunctionTypeMismatch() => A(M.GetCurrentMethod()); // expect exception ?? which ??
        [T, I] public void FunctionOData3      () => A(M.GetCurrentMethod()); // expect exception ?? which ?? 

        protected abstract QueryNodeVisitor<S> GetNewQueryNodeStringVisitor(); // => new QueryNodeRewritingVisitor();
        protected abstract IDictionary<Action, AssertParams> Dictionary { get; }
        protected static SampleDto e => null; // throw nameof only exception  
        protected class AssertParams
        {
            public string Filter { get; }
            public string Expected { get; }

            public AssertParams(string filter, string expected = null)
            {
                Filter = filter;
                Expected = expected;
            }
        }

        protected void A(M method)
        {
            IDictionary<Action, AssertParams> dictionary = Dictionary;
            Action key = dictionary.Keys.SingleOrDefault(_ => _.Method == method);
            if (key == null)
                Assert.Fail(" no such test in dictionary ");
            AssertParams assertParams = dictionary[key];
            AssertRewrittenFilter(assertParams.Filter, assertParams.Expected);
        }

        private void AssertRewrittenFilter(string filter, string expected = null)
        {
            ODataQueryOptions options = ModelFactory.GetOptions(filter);
            RewriteFilter(options).Should().Be(expected ?? filter);
        }

        private S RewriteFilter(ODataQueryOptions options)
        {
            QueryNodeVisitor<S> rewrite = GetNewQueryNodeStringVisitor();
            return options.Filter?.FilterClause.Expression.Accept(rewrite);
        }
    }
}
