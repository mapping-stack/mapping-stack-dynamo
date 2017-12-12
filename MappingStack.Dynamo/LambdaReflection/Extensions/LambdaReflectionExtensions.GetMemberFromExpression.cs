namespace MappingStack.LambdaReflection.Extensions
{
    using System.Linq.Expressions;
    using System.Reflection;

    // using JetBrains.Annotations;

    partial class LambdaReflectionExtensions
    {
        /*[CanBeNull]*/ public static MemberExpression GetMemberExpr(/*[NotNull]*/ this LambdaExpression e) => e.Body as MemberExpression;
        /*[CanBeNull]*/ public static System.Type      GetMemberType(/*[NotNull]*/ this LambdaExpression e) => e.GetMemberExpr()?.Type   ;
        /*[CanBeNull]*/ public static MemberInfo       GetMemberInfo(/*[NotNull]*/ this LambdaExpression e) => e.GetMemberExpr()?.Member ;
        /*[CanBeNull]*/ public static string           GetMemberName(/*[NotNull]*/ this LambdaExpression e) => e.GetMemberInfo()?.Name   ;

//        /*[CanBeNull]*/ public static MemberInfo GetMemberInfo<T1, T2>(/*[NotNull]*/ this Expression<Func<T1, T2>> e) => ((LambdaExpression) e).GetMemberInfo(); //(e.Body as MemberExpression)?.Member;
//        /*[CanBeNull]*/ public static string     GetMemberName<T1, T2>(/*[NotNull]*/ this Expression<Func<T1, T2>> e) => ((LambdaExpression) e).GetMemberName(); //e.GetMemberInfo()?.Name;
    }
}
