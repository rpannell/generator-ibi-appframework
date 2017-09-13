using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IBI.<%= Name %>.Service.Core.Utils
{
    public static class LambdaHelpers
    {
        /// <summary>
        /// Change the type of a value to another type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Checks to see if the type is nullable
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Ensures you check fo the nullable value instead of the null
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression NullableLessThan(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.LessThan(left, right);
        }

        /// <summary>
        /// Ensures you check fo the nullable value instead of the null
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Expression NullableGreaterThan(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.GreaterThan(left, right);
        }

        public static bool? IsVirtual(this PropertyInfo self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            bool? found = null;

            foreach (MethodInfo method in self.GetAccessors())
            {
                if (found.HasValue)
                {
                    if (found.Value != method.IsVirtual)
                        return null;
                }
                else
                {
                    found = method.IsVirtual;
                }
            }

            return found;
        }

        public static Expression InExpression<T>(this ParameterExpression genericType, string propertyName, List<int> values)
        {
            var orExpr = (Expression)null;
            var key = typeof(T).GetPropertyExpressionFromSubProperty(propertyName, genericType);
            foreach (var value in values)
            {
                var ex = Expression.Equal(key, Expression.Convert(Expression.Constant(value), key.Type));
                orExpr = orExpr == null ? ex : Expression.OrElse(orExpr, ex);
            }

            return orExpr;
        }

        public static Expression Key<T>(this ParameterExpression genericType, string propertyName)
        {
            return typeof(T).GetPropertyExpressionFromSubProperty(propertyName, genericType);
        }

        public static Expression NotEqual<T>(this ParameterExpression genericType, string propertyName, object value)
        {
            var key = genericType.Key<T>(propertyName);
            return Expression.NotEqual(key, Expression.Constant(value));
        }

        public static Expression BooleanExpression<T>(this ParameterExpression genericType, string propertyName, bool value)
        {
            var key = typeof(T).GetPropertyExpressionFromSubProperty(propertyName, genericType);
            return Expression.Equal(key, Expression.Constant(value));
        }

        public static Expression AndExpression(this Expression expr, Expression newExpr)
        {
            return Expression.AndAlso(expr, newExpr);
        }

        public static ParameterExpression GetGenericType(this Type type)
        {
            return Expression.Parameter(type);
        }

        public static PropertyInfo FollowPropertyPath(this Type type, string path)
        {
            PropertyInfo property = null;
            var currentType = type;
            foreach (string propertyName in path.Split('.'))
            {
                property = currentType.GetProperty(propertyName);
                currentType = property.PropertyType;
            }
            return property;
        }

        public static MethodCallExpression ConvertToType(this MemberExpression prop, TypeCode typeCode)
        {
            var convertProp = Expression.Convert((Expression)prop, typeof(object));
            var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(TypeCode) });
            var callExpressionReturningObject = Expression.Call(changeTypeMethod, convertProp, Expression.Constant(typeCode));
            return callExpressionReturningObject;
        }

        public static Expression GetPropertyExpressionFromSubProperty(this Type type, string orderByProperty, ParameterExpression parameter)
        {
            //var parameter = Expression.Parameter(type, "p");
            Expression parent = parameter;

            if (orderByProperty.Contains("."))
            {
                var parts = orderByProperty.Split('.');
                foreach (var part in parts)
                {
                    parent = Expression.Property(parent, part);
                }
            }
            else
            {
                parent = Expression.Property(parent, orderByProperty);
            }

            return parent;
        }

        /// <summary>
        /// Used to create a order by clause
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> OrderByHelper<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            //get the type of the entity
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type);
            //get the property info we wish to order by
            var property = type.FollowPropertyPath(orderByProperty);
            //create the expression to the property tree
            var parent = type.GetPropertyExpressionFromSubProperty(orderByProperty, parameter);
            //create the order by expression (p => p.*)
            var orderByExpression = Expression.Lambda(parent, parameter);
            //create the method call to order the property
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<TEntity> WhereHelper<TEntity>(this IQueryable<TEntity> source, Expression expression, ParameterExpression genericType)
        {
            if (expression == null) return source;
            var whereCallExpression = Expression.Call(
                        typeof(Queryable),
                        "Where",
                        new Type[] { source.ElementType },
                        source.Expression,
                        Expression.Lambda<Func<TEntity, bool>>(expression, new ParameterExpression[] { genericType }));
            return source.Provider.CreateQuery<TEntity>(whereCallExpression);
        }
    }
}