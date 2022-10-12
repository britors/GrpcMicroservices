using System.Linq.Expressions;

namespace ProductGrpc.Helpers
{
    public static class FilterHelper<T>
        where T : class
    {
        public static Expression<Func<T, bool>> UnionWithAnd(Expression<Func<T, bool>> left, Expression<Func<T, bool>> rigth)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = Expression.And(
                Expression.Invoke(left, param),
                Expression.Invoke(rigth, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        public static Expression<Func<T, bool>> UnionWithOr(Expression<Func<T, bool>> left, Expression<Func<T, bool>> rigth)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = Expression.Or(
                Expression.Invoke(left, param),
                Expression.Invoke(rigth, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
