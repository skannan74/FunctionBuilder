using System.Linq.Expressions;

namespace MobiasFunctionBuilder.Interface
{
    public interface IExpressionResult
    {
        LambdaExpression ToExpression();
        TData ToLambda<TData>() where TData : class;
    }
}