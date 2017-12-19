using MappingStack.OData.Parsing.QueryNodeVisitors;

namespace MappingStack.Dynamo.Testing.UnitTests
{
    using System;
    using System.Collections.Generic;

    using Microsoft.OData.UriParser;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass] public class QueryNodeNormalizingWithPasrenthesisVisitorTests : BaseQueryNodeVisitorStringTests
    {
        protected static readonly BaseQueryNodeVisitorStringTests b = new QueryNodeNormalizingWithPasrenthesisVisitorTests();
        protected override QueryNodeVisitor<string> GetNewQueryNodeStringVisitor()  => new QueryNodeNormalizingVisitor(){ IsWrapWithParentheses = true, };

        protected override IDictionary<Action, AssertParams> Dictionary => _d;
        private static readonly Dictionary<Action,AssertParams> _d = new Dictionary<Action, AssertParams>()
        {
            { b.True          , new AssertParams("true")},
            { b.False         , new AssertParams("false")},
            // $it eq null  -- is it a valid filter expression ?? does it work properly ??
            { b.Eq            , new AssertParams("id eq 1"                                  , "(id eq 1)")},
            
            { b.Function0     , new AssertParams("contains('str1', 'str2')")},
            { b.Function1     , new AssertParams("contains(title, 'str2')")},

            { b.Inner0        , new AssertParams("inner ne null"                            , "(inner ne null)")},
            { b.Inner1        , new AssertParams("inner/level eq 3"                         , "(inner/level eq 3)")},

            { b.Collection0   , new AssertParams("collection ne null")},
            { b.CollectionAny0, new AssertParams("collection/any(el: el/level eq 3)"        , "(collection/any(el: (el/level eq 3)))")},
            { b.CollectionAll0, new AssertParams("collection/all(el: el/level eq 3)"        , "(collection/all(el: (el/level eq 3)))")},

            { b.Open0         , new AssertParams("dynamo ne null"                           , "(dynamo ne null)")},
            { b.Open1         , new AssertParams("dynamo/flag1 ne null"                     , "(dynamo/flag1 ne null)")},
            { b.Open2         , new AssertParams("dynamo/flag1 eq null"                     , "(dynamo/flag1 eq null)")},
            { b.Open3         , new AssertParams("dynamo/flag1 eq true"                     , "(dynamo/flag1 eq true)")},

            { b.DeepAny       , new AssertParams(  "dynamo/dynBool/any(o: o/dynamicKey eq 'flag1' and o/value eq true)"  , "(dynamo/dynBool/any(o: ((o/dynamicKey eq 'flag1') and (o/value eq true))))")},
            { b.DeepAnyPar    , new AssertParams( "(dynamo/dynBool/any(o: o/dynamicKey eq 'flag1' and o/value eq true))" , "(dynamo/dynBool/any(o: ((o/dynamicKey eq 'flag1') and (o/value eq true))))")},
            { b.DeepAnyPar2   , new AssertParams("((dynamo/dynBool/any(o: o/dynamicKey eq 'flag1' and o/value eq true)))", "(dynamo/dynBool/any(o: ((o/dynamicKey eq 'flag1') and (o/value eq true))))")},
            /*
            "(dynamo/dynBool/any(o: ((o/dynamicKey eq 'flag1') and (o/value eq true)))))" with a length of 75, but 
            "(dynamo/dynBool/any(o: ((o/dynamicKey eq 'flag1') and (o/value eq true))))" 
              */
            { b.FunctionTypeMismatch     , new AssertParams("contains(id, 'str2')")},
            { b.FunctionOData3           , new AssertParams("substringof('str1', 'str2')")},
        };
    }
}
