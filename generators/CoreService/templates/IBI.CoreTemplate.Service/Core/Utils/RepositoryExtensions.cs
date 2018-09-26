using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IBI.<%= Name %>.Service.Core.Utils
{
    public static class RepositoryExtensions
    {
        #region Enums

        private enum ExpressionType { NotEqual, LessThanOrEqual, LessThan, GreaterThanOrEqual, GreaterThan, Equal }

        #endregion Enums

        #region Methods

        /// <summary>
        /// Change the type of a value to another type
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="conversion">The type to return the object to</param>
        /// <returns>An object converted to Type</returns>
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
        /// Creates a Like expression
        /// </summary>
        /// <param name="property">The expression that represents the property of the entity</param>
        /// <param name="search">The search string</param>
        /// <returns><see cref="Expression"/></returns>
        public static Expression Contains(Expression property, object search)
        {
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(search, typeof(string));
            return Expression.Call(property, method, someValue);
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

        /// <summary>
        /// Gets the ParameterExpression of a Type.  This is used to create expressions to filter
        /// data
        /// </summary>
        /// <param name="type">The Type to create the ParameterExpression</param>
        /// <returns>Linq.ParameterExpression</returns>
        public static ParameterExpression GetGenericType(this Type type) => Expression.Parameter(type);

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
        /// Creates an "IN" clause expression for a specific property and list of values
        /// </summary>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="propertyName">The name of the property to create the clause on</param>
        /// <param name="values">The list of values to check for</param>
        /// <returns>Linq.Expression</returns>
		public static Expression InExpression<T>(this ParameterExpression genericType, string propertyName, List<object> values)
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

        /// <summary>
        /// Test if a string is an integer
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Boolean</returns>
        public static bool IsInt(this string str) => int.TryParse(str, out var _);

        /// <summary>
        /// Test if a string is a long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLong(this string str) => long.TryParse(str, out var _);

        /// <summary>
        /// Checks to see if a type is nullable or not
        /// </summary>
        /// <param name="t">The Type to check</param>
        /// <returns>True if the type is nullable</returns>
        public static bool IsNullableType(this Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// Test if a string is a numeric value
        /// </summary>
        /// <param name="str"></param>
        /// <returns>bool</returns>
        public static bool IsNumeric(this string str) => float.TryParse(str, out var _);

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

        /// <summary>
        /// Gets the Key of property by the name of the property
        /// </summary>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>Linq.Expression</returns>
        public static Expression Key<T>(this ParameterExpression genericType, string propertyName) => typeof(T).GetPropertyExpressionFromSubProperty(propertyName, genericType);

        /// <summary>
        /// Creates a NotEqual to value expression on property by the name
        /// </summary>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to check</param>
        /// <returns>Linq.Expression</returns>
        public static Expression NotEqual<T>(this ParameterExpression genericType, string propertyName, object value) => Expression.NotEqual(genericType.Key<T>(propertyName), Expression.Constant(value));

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a equal Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableEqual(Expression left, Expression right) => NullableExpression(left, right, ExpressionType.Equal);

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a greater than Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableGreaterThan(Expression left, Expression right) => NullableExpression(left, right, ExpressionType.GreaterThan);

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a greater than or equal to Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableGreaterThanOrEqualTo(Expression left, Expression right) => NullableExpression(left, right, ExpressionType.GreaterThanOrEqual);

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a less than Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableLessThan(Expression left, Expression right) => NullableExpression(left, right, ExpressionType.LessThan);

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a less than or equal to Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableLessThanOrEqualTo(Expression left, Expression right) => NullableExpression(left, right, ExpressionType.LessThanOrEqual);

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a not equal to Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableNotEqual(Expression left, Expression right) => NullableExpression(left, right, ExpressionType.NotEqual);

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

        /// <summary>
        /// Handle the public nullable expression helpers
        /// </summary>
        /// <param name="left">The left expression</param>
        /// <param name="right">The right side expression</param>
        /// <param name="type">The type of the expression</param>
        /// <returns><see cref="Expression"/></returns>
        private static Expression NullableExpression(Expression left, Expression right, ExpressionType type)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);

            var rtnVal = (Expression)null;

            switch (type)
            {
                case ExpressionType.NotEqual:
                    rtnVal = Expression.NotEqual(left, right);
                    break;

                case ExpressionType.LessThanOrEqual:
                    rtnVal = Expression.LessThanOrEqual(left, right);
                    break;

                case ExpressionType.LessThan:
                    rtnVal = Expression.LessThan(left, right);
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    rtnVal = Expression.GreaterThanOrEqual(left, right);
                    break;

                case ExpressionType.GreaterThan:
                    rtnVal = Expression.GreaterThan(left, right);
                    break;

                case ExpressionType.Equal:
                    rtnVal = Expression.Equal(left, right);
                    break;

                default:
                    break;
            }

            return rtnVal;
        }

        #endregion Methods
    }
}