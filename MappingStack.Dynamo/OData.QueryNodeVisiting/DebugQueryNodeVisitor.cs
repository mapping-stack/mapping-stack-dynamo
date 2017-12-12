namespace MappingStack.DynamicProperties
{
    using System;
    using System.Collections.Generic;

    using Microsoft.OData.Edm;
    using Microsoft.OData.UriParser;

    using System.Diagnostics;

    using T = System.Object;

    public class DebugQueryNodeVisitor 
        : QueryNodeVisitor<T> /* trivial object implementation */

    {
        private const bool isBreakIfAttachedDefault = true;

        public List<QueryNode> BypassNonDistinctList = new List<QueryNode>();  // TODO: exposing it as public for outer debugging purposes

        private void AdjustPathsAndMembersForOptionen_1(CollectionNavigationNode nodeIn)
        {
            //   basically approach is to accept self to each subnode if any
            //       and detect special cases that affect projection list
        }

//        private readonly List      <string                    >  _paths   = new List<string>();                            // ?? collect here and return this from Accept ??
//        private readonly List<Tuple<string, IEdmStructuredType>> _members = new List<Tuple<string, IEdmStructuredType>>(); // ?? collect here return this from Accept ??
        
        public override T Visit(AllNode                               nodeIn)  { SkipDebuggerBreak(nodeIn); nodeIn.Source.Accept(this); nodeIn.Body .Accept(this); return Visited; }
        public override T Visit(AnyNode                               nodeIn)  { SkipDebuggerBreak(nodeIn); nodeIn.Source.Accept(this); nodeIn.Body .Accept(this); return Visited; }
        public override T Visit(BinaryOperatorNode                    nodeIn)  { SkipDebuggerBreak(nodeIn); nodeIn.Left  .Accept(this); nodeIn.Right.Accept(this); return Visited; }

        public override T Visit(CountNode                             nodeIn)  { SkipDebuggerBreak(nodeIn); /* nodeIn.Source.Accept(this); */                      return Visited; }

        public override T Visit(CollectionNavigationNode              nodeIn)  { SkipDebuggerBreak(nodeIn); AdjustPathsAndMembersForOptionen_1(nodeIn);            return Visited; }


        public override T Visit(ConstantNode                          nodeIn)  { SkipDebuggerBreak(nodeIn);                                                        return Visited; }
        public override T Visit(ConvertNode                           nodeIn)  { SkipDebuggerBreak(nodeIn); nodeIn.Source.Accept(this);                            return Visited; }

        public override T Visit(ResourceRangeVariableReferenceNode    nodeIn)  { SkipDebuggerBreak(nodeIn);                                                        return Visited; }

        public override T Visit(SingleValuePropertyAccessNode         nodeIn)  { SkipDebuggerBreak(nodeIn); nodeIn.Source.Accept(this);                            return Visited; }
//        public override T Visit(SingleValueOpenPropertyAccessNode     nodeIn)  { SkipDebuggerBreak(nodeIn); nodeIn.Source.Accept(this);                            return Visited; }

        #region override Just debuggable // uncommeneted means not researched yet // commented means 

//      public override T Visit(AllNode                               nodeIn)  => DebuggerBreakVisited(nodeIn);
//      public override T Visit(AnyNode                               nodeIn)  => DebuggerBreakVisited(nodeIn);
//      public override T Visit(BinaryOperatorNode                    nodeIn)  => DebuggerBreakVisited(nodeIn);
//
//      public override T Visit(CountNode                             nodeIn)  => DebuggerBreakVisited(nodeIn);
//
//      public override T Visit(CollectionNavigationNode              nodeIn)  => DebuggerBreakVisited(nodeIn);
//
//      public override T Visit(ConstantNode                          nodeIn)  => DebuggerBreakVisited(nodeIn);
//      public override T Visit(ConvertNode                           nodeIn)  => DebuggerBreakVisited(nodeIn);
//
//      public override T Visit(ResourceRangeVariableReferenceNode    nodeIn)  => DebuggerBreakVisited(nodeIn);
//
//      public override T Visit(SingleValuePropertyAccessNode         nodeIn)  => DebuggerBreakVisited(nodeIn);


        public override T Visit(CollectionPropertyAccessNode          nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(CollectionOpenPropertyAccessNode      nodeIn)  => DebuggerBreakVisited(nodeIn);

//      public override T Visit(ConstantNode                          nodeIn)  => DebuggerBreakVisited(nodeIn);
//      public override T Visit(ConvertNode                           nodeIn)  => DebuggerBreakVisited(nodeIn);

        public override T Visit(CollectionResourceCastNode            nodeIn)  => DebuggerBreakVisited(nodeIn);

//      public override T Visit(ResourceRangeVariableReferenceNode    nodeIn)  => DebuggerBreakVisited(nodeIn);

        public override T Visit(NonResourceRangeVariableReferenceNode nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(SingleResourceCastNode                nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(SingleNavigationNode                  nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(SingleResourceFunctionCallNode        nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(SingleValueFunctionCallNode           nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(CollectionResourceFunctionCallNode    nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(CollectionFunctionCallNode            nodeIn)  => DebuggerBreakVisited(nodeIn);

        public override T Visit(SingleValueOpenPropertyAccessNode     nodeIn)  => DebuggerBreakVisited(nodeIn);
//      public override T Visit(SingleValuePropertyAccessNode         nodeIn)  => DebuggerBreakVisited(nodeIn);

        public override T Visit(UnaryOperatorNode                     nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(NamedFunctionParameterNode            nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(ParameterAliasNode                    nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(SearchTermNode                        nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(SingleComplexNode                     nodeIn)  => DebuggerBreakVisited(nodeIn);
        public override T Visit(CollectionComplexNode                 nodeIn)  => DebuggerBreakVisited(nodeIn);

        #endregion

        #region private Just debuggable

        // [DebuggerStepThrough]
        private void SkipDebuggerBreak(QueryNode nodeIn) => DebuggerBreak(nodeIn, false);

        // [DebuggerStepThrough]
        private T DebuggerBreakVisited(QueryNode nodeIn, bool isBreakIfAttached = isBreakIfAttachedDefault)
        {
            DebuggerBreak(nodeIn, isBreakIfAttached); return Visited;
        }

        private static T Visited
        {
            // [DebuggerStepThrough]
            get
            {
                return null; // ?? return (object) this ?? return some kind of current castable to T ??
            }
        } 

        // [DebuggerStepThrough]
        private void DebuggerBreak(QueryNode nodeIn, bool isBreakIfAttached = isBreakIfAttachedDefault)
        {
            BypassNonDistinctList.Add(nodeIn);
            if (isBreakIfAttached) if (Debugger.IsAttached) Debugger.Break();
        }


        //   /*if(Debugger.IsAttached)Debugger.Break();*/   // means no more nodes inside this kind of node
        //   if(Debugger.IsAttached)Debugger.Break();       // means this type of node has not been researched yet
        #endregion

        // ReSharper restore BuiltInTypeReferenceStyle

    }
}
