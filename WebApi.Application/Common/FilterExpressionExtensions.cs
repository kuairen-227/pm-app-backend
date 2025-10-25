using System.Linq.Expressions;

namespace WebApi.Application.Common;

public static class FilterExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>>? expr1,
        Expression<Func<T, bool>> expr2
    )
    {
        if (expr1 == null) return expr2;

        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
