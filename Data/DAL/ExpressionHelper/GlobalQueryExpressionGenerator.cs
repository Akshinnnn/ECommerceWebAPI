using Data.Entities.PropertyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL.ExpressionHelper
{
    public static class GlobalQueryExpressionGenerator
    {
        public static LambdaExpression GenerateExpression(Type type)
        {
            var parameter = Expression.Parameter(type, "t");
            var propAccess = Expression.PropertyOrField(parameter, nameof(ISoftDelete.IsDeleted));
            var equality = Expression.Equal(propAccess, Expression.Constant(false));
            var lambdaExpression = Expression.Lambda(equality, parameter);

            return lambdaExpression;
        }
    }
}
