using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Utils
{
    /// <summary>
    /// Helper functions for Linq lambda queries
    /// </summary>
    public static class LambdaHelpers
    {
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
        /// Checks to see if a type is nullable or not
        /// </summary>
        /// <param name="t">The Type to check</param>
        /// <returns>True if the type is nullable</returns>
        public static bool IsNullableType(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null 
        /// for a less than Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableLessThan(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.LessThan(left, right);
        }

		/// <summary>
        /// Ensures you check for the nullable value instead of the value null 
        /// for a less than or equal to Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
		public static Expression NullableLessThanOrEqualTo(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.LessThanOrEqual(left, right);
        }
		
        /// <summary>
        /// Ensures you check for the nullable value instead of the value null 
        /// for a greater than Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableGreaterThan(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.GreaterThan(left, right);
        }

		/// <summary>
        /// Ensures you check for the nullable value instead of the value null 
        /// for a greater than or equal to Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
		public static Expression NullableGreaterThanOrEqualTo(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.GreaterThanOrEqual(left, right);
        }
		
        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a equal Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableEqual(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.Equal(left, right);
        }

        /// <summary>
        /// Ensures you check for the nullable value instead of the value null
        /// for a not equal to Expression
        /// </summary>
        /// <param name="left">The left side of the Expression</param>
        /// <param name="right">The right side of the Expression</param>
        /// <returns>Expression</returns>
        public static Expression NullableNotEqual(Expression left, Expression right)
        {
            if (IsNullableType(left.Type) && !IsNullableType(right.Type))
                right = Expression.Convert(right, left.Type);
            else if (!IsNullableType(left.Type) && IsNullableType(right.Type))
                left = Expression.Convert(left, right.Type);
            return Expression.NotEqual(left, right);
        }

        /// <summary>
        /// Checks to see if a Property is a Virtual type or not
        /// </summary>
        /// <param name="self">The information on a property</param>
        /// <returns>True if the property is Virtual null or false if not</returns>
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
        /// Creates an "IN" clause expression for a specific property and list of integer values
        /// </summary>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="propertyName">The name of the property to create the clause on</param>
        /// <param name="values">The list of values to check for</param>
        /// <returns>Linq.Expression</returns>
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

        /// <summary>
        /// Gets the Key of property by the name of the property
        /// </summary>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>Linq.Expression</returns>
        public static Expression Key<T>(this ParameterExpression genericType, string propertyName)
        {
            return typeof(T).GetPropertyExpressionFromSubProperty(propertyName, genericType);
        }

        /// <summary>
        /// Creates a NotEqual to value expression on property by the name 
        /// </summary>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to check</param>
        /// <returns>Linq.Expression</returns>
        public static Expression NotEqual<T>(this ParameterExpression genericType, string propertyName, object value)
        {
            var key = genericType.Key<T>(propertyName);
            return Expression.NotEqual(key, Expression.Constant(value));
        }

        /// <summary>
        /// Creates an EqualTo to value expression on property by the name
        /// </summary>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="value">The value to check</param>
        /// <returns>Linq.Expression</returns>
        public static Expression BooleanExpression<T>(this ParameterExpression genericType, string propertyName, bool value)
        {
            var key = typeof(T).GetPropertyExpressionFromSubProperty(propertyName, genericType);
            return Expression.Equal(key, Expression.Constant(value));
        }

        /// <summary>
        /// Adds an "AND" expression to an expression with a new expression
        /// </summary>
        /// <param name="expr">The expression to add the AND expression to</param>
        /// <param name="newExpr">The new expression to AND</param>
        /// <returns>Linq.Expression</returns>
        public static Expression AndExpression(this Expression expr, Expression newExpr)
        {
            return Expression.AndAlso(expr, newExpr);
        }

        /// <summary>
        /// Gets the ParameterExpression of a Type.  This is used to create expressions to filter
        /// data 
        /// </summary>
        /// <param name="type">The Type to create the ParameterExpression</param>
        /// <returns>Linq.ParameterExpression</returns>
        public static ParameterExpression GetGenericType(this Type type)
        {
            return Expression.Parameter(type);
        }

        /// <summary>
        /// Get the PropertyInfo of a property by the dot notation path
        /// that can includes sub-entities
        /// </summary>
        /// <param name="type">The Type to follow the path</param>
        /// <param name="path">The Path</param>
        /// <returns>PropertyInfo</returns>
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
        /// Convert a property to another type
        /// Think CAST() in a database query
        /// </summary>
        /// <param name="prop">The property to convert</param>
        /// <param name="typeCode">The type to convert the property to</param>
        /// <returns>MethodCallExpression</returns>
        public static MethodCallExpression ConvertToType(this MemberExpression prop, TypeCode typeCode)
        {
            var convertProp = Expression.Convert((Expression)prop, typeof(object));
            var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(TypeCode) });
            var callExpressionReturningObject = Expression.Call(changeTypeMethod, convertProp, Expression.Constant(typeCode));
            return callExpressionReturningObject;
        }

        /// <summary>
        /// Get the property expression by a property name 
        /// </summary>
        /// <param name="type">The type to get the Property Expression</param>
        /// <param name="orderByProperty">The name of the property using dot notation</param>
        /// <param name="parameter">The current ParameterExprssion of an object</param>
        /// <returns>Linq.Expression</returns>
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
        /// Add a OrderBy clause to a query expression
        /// </summary>
        /// <param name="source">The source IQueryable object</param>
        /// <param name="orderByProperty">The dot path of the property to order by</param>
        /// <param name="desc">True if its a descending order</param>
        /// <returns>IQueryable of an Entity</returns>
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

        /// <summary>
        /// Add a where clause to a query Expression
        /// </summary>
        /// <param name="source">The source IQueryable object</param>
        /// <param name="expression">The expression to check</param>
        /// <param name="genericType">The GenericType to create the where clause for</param>
        /// <returns>IQueryable of an Entity</returns>
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