using IBI.<%= Name %>.Service.Core.Attributes;
using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using IBI.<%= Name %>.Service.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using static IBI.<%= Name %>.Service.Core.Attributes.Searchable;
using static IBI.<%= Name %>.Service.Core.Models.AdvancedSearch;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Repositories
{
    /// <summary>
    /// Base class that defines the default functions for every entity in the web-api service
    /// </summary>
    public class BaseRepository<TEntity, TPrimaryKey> : DbContext, IBaseRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        #region Constructors
        /// <summary>
        /// The default constructor that sets the DbContext to the DefaultConnection in the web.config
        /// </summary>
        public BaseRepository() : base("DefaultConnection")
        {
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Represents the Entity in the database
        /// </summary>
        public DbSet<TEntity> Entity { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the full entity also joining any sub-entities in 
        /// one call to the database
        /// </summary>
        /// <returns>IQueryable</returns>
        public virtual IQueryable<TEntity> GetFullEntity()
        {
            var genericType = this.GetGenericType();
            var returnEntity = (IQueryable<TEntity>)this.Entity;
            var typeExpression = Expression.Parameter(typeof(TEntity), "x");
            foreach (var sc in typeof(TEntity).GetProperties())
            {
                var notMappedAttributes = (NotMappedAttribute[])sc.GetCustomAttributes(typeof(NotMappedAttribute), true);
                var isVirtual = sc.IsVirtual();
                if (sc.Name != "Id" && isVirtual.HasValue && isVirtual.Value && (notMappedAttributes == null || notMappedAttributes.Count() == 0))
                {
                    var propertyExpression = Expression.Property(typeExpression, sc.Name);
                    returnEntity = returnEntity.Include(Expression.Lambda<Func<TEntity, object>>(propertyExpression, typeExpression));
                }
            }

            return returnEntity.AsNoTracking();
        }

        #endregion Methods

        #region CRUD

        /// <summary>
        /// Delete an entity by the primary key
        /// /// If it's ReadOnly, nothing is done
        /// </summary>
        /// <param name="Id">The primary key</param>
        public virtual void Delete(TPrimaryKey id)
        {
            if (!typeof(TEntity).IsDefined(typeof(ReadOnlyAttribute), true))
            {
                var item = this.Entity.Find(id);
                this.Entity.Remove(item);
                this.SaveChanges();
            }
        }

        /// <summary>
        /// Gets a list of all of the entities from a database view or table
        /// </summary>
        /// <returns>A List of Entity</returns>
        public virtual List<TEntity> Get()
        {
            return GetFullEntity().ToList();
        }

        /// <summary>
        /// Gets a single row of the entity by the value of the primary key
        /// </summary>
        /// <param name="Id">The value of the primary key</param>
        /// <returns>An Entity by the primary key</returns>
        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = this.Entity.Find(id);
            //this ensures that entity can be retrieved and possible changed
            //in another piece of memory later on
            this.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        /// <summary>
        /// Will get the entity by the primary key and keep 
        /// it attached to the session to allow for update
        /// </summary>
        /// <param name="id">The primary key value of the entity</param>
        /// <returns>TEntity</returns>
        public virtual TEntity GetForUpdate(TPrimaryKey id)
        {
            return this.Entity.Find(id);
        }

        /// <summary>
        /// Insert an entity in the database
        /// If it's ReadOnly nothing is done
        /// </summary>
        /// <param name="newItem">The entity to insert</param>
        /// <returns>The Entity after the database insert</returns>
        public virtual TEntity Insert(TEntity newItem)
        {
            if (!typeof(TEntity).IsDefined(typeof(ReadOnlyAttribute), true))
            {
                this.Entity.Add(newItem);
                this.SaveChanges();
                return newItem;
            }

            return null;
        }

        /// <summary>
        /// Used to merge the values of one entity into another entity
        /// </summary>
        /// <param name="previousEntity">The current entity</param>
        /// <param name="newEntity">The updated entity containing the new values</param>
        public virtual void MergeValues(TEntity previousEntity, TEntity newEntity)
        {
            this.Entry(previousEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Updates the row in the database that represents the entity
        /// If it's ReadOnly nothing is done
        /// And InsertOnly properties are ignored
        /// </summary>
        /// <param name="Entity">The Entity to update</param>
        /// <returns>The Entity after the database update</returns>
        public virtual TEntity Update(TEntity entity)
        {
            if (!typeof(TEntity).IsDefined(typeof(ReadOnlyAttribute), true))
            {
                this.Entry(entity).State = EntityState.Modified;

                var dbEntityEntry = this.Entry(entity);
                foreach (var property in dbEntityEntry.CurrentValues.PropertyNames)
                {
                    var sc = typeof(TEntity).GetProperty(property);
                    if (sc.IsDefined(typeof(InsertOnlyFieldAttribute), true))
                    {
                        dbEntityEntry.Property(property).IsModified = false;
                    }
                }

                this.SaveChanges();
            }

            return entity;
        }

        #endregion CRUD

        #region Get Page

        /// <summary>
        /// Returns a page of data of the entity
        /// </summary>
        /// <param name="offSet">The row offset on the database</param>
        /// <param name="limit">The number of records to return</param>
        /// <param name="searchCriteria">The criteria to search on the searchable attribute</param>
        /// <param name="sortName">The name of the property to sort on</param>
        /// <param name="sortOrder">The direction to sort (desc/asc)</param>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="extraExpr">Any extra Linq.Expression to filter down in the database</param>
        /// <param name="query">Extra query to filter down the entity</param>
        /// <returns>A PaginationResult that contains the entities</returns>
        public virtual PaginationResult<TEntity> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc",
                                                        ParameterExpression genericType = null, Expression extraExpr = null, IQueryable<TEntity> query = null)
        {
            //create the restrictions if needed
            genericType = genericType == null ? typeof(TEntity).GetGenericType() : genericType;
            var restrictions = !string.IsNullOrWhiteSpace(searchCriteria)
                                            ? this.GetSearchRestrictions(searchCriteria, genericType)
                                            : null;
            var compiledExpression = restrictions == null ? null : Expression.Lambda<Func<TEntity, bool>>(restrictions, genericType).Compile();

            //if the user passes in any extra
            if (extraExpr != null)
            {
                restrictions = restrictions != null
                                    ? Expression.AndAlso(extraExpr, restrictions)
                                    : extraExpr;
            }

            //set the query
            if (query == null)
            {
                query = this.GetFullEntity();
            }

            //get the row count
            var rowCount = GetPageRowCount(restrictions, genericType, query);
            //get the search results
            var searchResults = GetSearchResults(restrictions, limit, offSet, sortName, sortOrder, genericType, query);
            //return the pagination model
            return new PaginationResult<TEntity>
            {
                rows = searchResults,
                total = rowCount
            };
        }

        /// <summary>
        /// Creates an Linq.Expression for the Searchable properties
        /// </summary>
        /// <param name="searchCriteria">The search string</param>
        /// <param name="genericType">The ParameterExpression that represents the Entity</param>
        /// <returns>Expression</returns>
        public Expression GetSearchRestrictions(object searchCriteria, ParameterExpression genericType)
        {
            Expression restrictions = null;

            if (searchCriteria != null)
            {
                foreach (var sc in typeof(TEntity).GetProperties())
                {
                    if (sc.IsDefined(typeof(SearchAbleAttribute), true))
                    {
                        var searchAtts = (SearchAbleAttribute[])sc.GetCustomAttributes(typeof(SearchAbleAttribute), true);
                        //walk each searchable attribute on the property
                        foreach (var att in searchAtts)
                        {
                            var propertyName = string.IsNullOrEmpty(att.AliasName) ? sc.Name : att.AliasName;
                            var key = typeof(TEntity).GetPropertyExpressionFromSubProperty(propertyName, genericType);
                            var propertyType = typeof(TEntity).FollowPropertyPath(propertyName).PropertyType;
                            var value = Expression.Constant(LambdaHelpers.ChangeType(searchCriteria, propertyType));
                            Expression addedExpression = null;
                            switch (att.SearchType)
                            {
                                case SearchAbleType.Equal:
                                    addedExpression = Expression.Equal(key, value);
                                    break;

                                case SearchAbleType.NotEqual:
                                    addedExpression = Expression.NotEqual(key, value);
                                    break;

                                case SearchAbleType.GreaterThan:
                                    addedExpression = LambdaHelpers.NullableGreaterThan(key, value);
                                    break;

                                case SearchAbleType.GreaterThanEqual:
                                    addedExpression = Expression.GreaterThanOrEqual(key, value);
                                    break;

                                case SearchAbleType.LessThan:
                                    addedExpression = LambdaHelpers.NullableLessThan(key, value);
                                    break;

                                case SearchAbleType.LessThanEqual:
                                    addedExpression = Expression.LessThanOrEqual(key, value);
                                    break;

                                case SearchAbleType.Contains:
                                    var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                    var someValue = Expression.Constant(searchCriteria, typeof(string));
                                    addedExpression = Expression.Call(key, method, someValue);
                                    break;

                                case SearchAbleType.StartsWith:
                                    var methodsw = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                    var swValue = Expression.Constant(searchCriteria, typeof(string));
                                    addedExpression = Expression.Call(key, methodsw, swValue);
                                    break;

                                case SearchAbleType.EndsWith:
                                    var methodew = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                    var ewValue = Expression.Constant(searchCriteria, typeof(string));
                                    addedExpression = Expression.Call(key, methodew, ewValue);
                                    break;
                            }

                            //add the new expression to the list of restrictions
                            restrictions = restrictions == null
                                                ? addedExpression
                                                : Expression.OrElse(restrictions, addedExpression);
                        }
                    }
                }
            }

            return restrictions;
        }

        /// <summary>
        /// if the orderby is null or empty find the name of the first key for the
        /// entity and return that as the name of the order by else just return the name
        /// </summary>
        /// <param name="orderBy">current order by string</param>
        /// <returns>the name of the property to order</returns>
        private string GetOrderBy(string orderBy)
        {
            if (orderBy == null || orderBy == string.Empty)
            {
                //get the list of key name for this entity and return the first one
                var objectContext = ((IObjectContextAdapter)this).ObjectContext;
                var set = objectContext.CreateObjectSet<TEntity>();
                var keyNames = set.EntitySet.ElementType
                                        .KeyMembers
                                        .Select(k => k.Name);
                return keyNames.FirstOrDefault();
            }

            return orderBy;
        }

        /// <summary>
        /// Get the number of rows a search would contain if not set to a specific amount
        /// </summary>
        /// <param name="restrictions">The current Expression</param>
        /// <param name="genericType">The ParameterExpression that represents the Entity</param>
        /// <param name="query">Current state of the query</param>
        /// <returns>The count of rows in the query</returns>
        private int GetPageRowCount(Expression restrictions, ParameterExpression genericType, IQueryable<TEntity> query)
        {
            return query.WhereHelper(restrictions, genericType)
                        .Count();
        }

        /// <summary>
        /// Gets the results of a search from the database
        /// </summary>
        /// <param name="restrictions">The current Expression</param>
        /// <param name="limit">The number of records to return</param>
        /// <param name="offset"The row offset on the database</param>
        /// <param name="sortName">The name of the property to sort on</param>
        /// <param name="sortOrder">The direction to sort (desc/asc)</param>
        /// <param name="genericType">The ParameterExpression that represents the Entity</param>
        /// <param name="query">Current state of the query</param>
        /// <returns>List of entities</returns>
        private List<TEntity> GetSearchResults(Expression restrictions, int limit, int offset, string sortName, string sortOrder, ParameterExpression genericType, IQueryable<TEntity> query)
        {
            return query.AsNoTracking()
                        .WhereHelper(restrictions, genericType)
                        .OrderByHelper(GetOrderBy(sortName), sortOrder == "desc")
                        .Skip(offset)
                        .Take(limit)
                        .ToList();
        }

        #endregion Get Page

        #region Get Autocomplete

        /// <summary>
        /// Gets a set a records that are triggered by the AutoComplete search
        /// </summary>
        /// <param name="length">The number of records to return</param>
        /// <param name="term">The search term of the AutoComplete properties</param>
        /// <returns>List of the Entity</returns>
        public virtual List<TEntity> GetAutocomplete(int length, object term)
        {
            var genericType = Expression.Parameter(typeof(TEntity));
            var restrictions = term != null
                                    ? this.GetAutoCompleteRestrictions(term, genericType)
                                    : null;

            return GetFullEntity().WhereHelper(restrictions, genericType)
                               .Take(length)
                               .ToList();
        }

        /// <summary>
        /// Gets the Expression for the AutoComplete query
        /// </summary>
        /// <param name="searchCriteria">The search string</param>
        /// <param name="genericType">The ParameterExpression that represents the Entity</param>
        /// <returns>Expression</returns>
        private Expression GetAutoCompleteRestrictions(object searchCriteria, ParameterExpression genericType)
        {
            Expression restrictions = null;

            if (searchCriteria != null)
            {
                foreach (var sc in typeof(TEntity).GetProperties())
                {
                    if (sc.IsDefined(typeof(Attributes.AutoComplete), true))
                    {
                        var searchAtts = (Attributes.AutoComplete[])sc.GetCustomAttributes(typeof(Attributes.AutoComplete), true);
                        var isSearchTermString = searchCriteria.GetType() == typeof(string);
                        var isSearchTermInt = searchCriteria.GetType() == typeof(int);
                        var containsmethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                        //walk each searchable attribute on the property
                        foreach (var att in searchAtts)
                        {
                            var propertyName = string.IsNullOrEmpty(att.PropertyName) ? sc.Name : att.PropertyName;
                            var key = typeof(TEntity).GetPropertyExpressionFromSubProperty(propertyName, genericType);
                            var propertyType = typeof(TEntity).FollowPropertyPath(propertyName).PropertyType;
                            var value = Expression.Constant(Convert.ChangeType(searchCriteria, propertyType));
                            var stringvalue = Expression.Constant(searchCriteria, typeof(string));
                            Expression addedExpression = null;
                            if (att.IsEqual)
                            {
                                switch (att.PropertyType)
                                {
                                    case AutoCompletePropertyType.Int:
                                        addedExpression = Expression.Equal(key, value);
                                        break;

                                    default:
                                        addedExpression = Expression.Equal(key, value);
                                        break;
                                }
                            }
                            else
                            {
                                switch (att.PropertyType)
                                {
                                    case AutoCompletePropertyType.Int:
                                        addedExpression = Expression.Call(key, containsmethod, stringvalue);
                                        break;

                                    default:
                                        addedExpression = Expression.Call(key, containsmethod, stringvalue);
                                        break;
                                }
                            }

                            //add the new expression to the list
                            restrictions = restrictions == null
                                                ? addedExpression
                                                : Expression.OrElse(restrictions, addedExpression);
                        }
                    }
                }
            }

            return restrictions;
        }

        #endregion Get Autocomplete

        #region Get Advanced Page

        /// <summary>
        /// Returns a page of data of the entity
        /// </summary>
        /// <param name="offSet">The row offset on the database</param>
        /// <param name="limit">The number of records to return</param>
        /// <param name="searchCriteria">The criteria to search on the searchable attribute</param>
        /// <param name="sortName">The name of the property to sort on</param>
        /// <param name="sortOrder">The direction to sort (desc/asc)</param>
        /// <param name="model">The AdvancedPageModel</param>
        /// <param name="query">Extra query to filter down the entity</param>
        /// <returns>A PaginationResult that contains the entities</returns>
        public virtual PaginationResult<TEntity> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null, IQueryable<TEntity> query = null)
        {
            //create the restrictions if needed
            var genericType = Expression.Parameter(typeof(TEntity));
            var restrictions = !string.IsNullOrWhiteSpace(searchCriteria)
                                            ? this.GetSearchRestrictions(searchCriteria, genericType)
                                            : null;
            var advRestrictions = this.GetAdvancedSearchRestrictions(model, genericType);
            var expr = restrictions != null ? Expression.AndAlso(restrictions, advRestrictions) : advRestrictions;
            //set the query
            if (query == null) query = this.GetFullEntity();
            //get the row count
            var rowCount = GetPageRowCount(expr, genericType, query);
            //get the search results
            var searchResults = GetSearchResults(expr, limit, offSet, sortName, sortOrder, genericType, query);
            //return the pagination model
            return new PaginationResult<TEntity>
            {
                rows = searchResults,
                total = rowCount
            };
        }

        /// <summary>
        /// Gets the Linq.Expression that will represent the AdvancedPaging query
        /// </summary>
        /// <param name="model">The AdvancedPageModel</param>
        /// <param name="genericType">The ParameterExpression that represents the Entity</param>
        /// <returns>The Expression for the AdvancedPage</returns>
        public Expression GetAdvancedSearchRestrictions(AdvancedPageModel model, ParameterExpression genericType)
        {
            Expression restrictions = null;
            foreach (var adv in model.AdvancedSearch)
            {
                var valueA = (object)(adv.IntValue.HasValue ? adv.IntValue.Value : adv.Value);
                var key = typeof(TEntity).GetPropertyExpressionFromSubProperty(adv.PropertyName, genericType);

                //if (key.Type == typeof(int))
                //{
                //    key = ((MemberExpression)key).ConvertToType(TypeCode.String);
                //};

                var propertyType = typeof(TEntity).FollowPropertyPath(adv.PropertyName).PropertyType;
                var value = valueA != null ? Expression.Constant(LambdaHelpers.ChangeType(valueA, propertyType)) : null;
                Expression addedExpression = null;
                switch (adv.TypeOfSearch)
                {
                    case AdvancedSearchType.IsNull:
                        addedExpression = Expression.Equal(key, Expression.Constant(null));
                        break;

                    case AdvancedSearchType.IsNotNull:
                        addedExpression = Expression.NotEqual(key, Expression.Constant(null));
                        break;

                    case AdvancedSearchType.In:
                        addedExpression = LambdaHelpers.InExpression<TEntity>(genericType, adv.PropertyName, adv.ListValue);
                        break;

                    case AdvancedSearchType.Equal:
                        addedExpression = Expression.Equal(key, value);
                        break;

                    case AdvancedSearchType.NotEqual:
                        addedExpression = Expression.NotEqual(key, value);
                        break;

                    case AdvancedSearchType.LessThan:
                        addedExpression = LambdaHelpers.NullableLessThan(key, value);
                        break;

                    case AdvancedSearchType.LessThanEqual:
                        addedExpression = LambdaHelpers.NullableLessThanOrEqualTo(key, value);
                        break;

                    case AdvancedSearchType.GreaterThan:
                        addedExpression = LambdaHelpers.NullableGreaterThan(key, value);
                        break;

                    case AdvancedSearchType.GreaterThanEqual:
                        addedExpression = LambdaHelpers.NullableGreaterThanOrEqualTo(key, value);
                        break;

                    case AdvancedSearchType.Between:
                        var lowerBound = Expression.GreaterThanOrEqual(key, Expression.Constant(Convert.ChangeType(adv.Value, propertyType)));
                        var upperBound = Expression.LessThanOrEqual(key, Expression.Constant(Convert.ChangeType(adv.Value2, propertyType)));
                        addedExpression = Expression.AndAlso(lowerBound, upperBound);

                        break;

                    case AdvancedSearchType.Like:
                    default:
                        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var someValue = Expression.Constant(valueA, typeof(string));
                        addedExpression = Expression.Call(key, method, someValue);
                        break;
                }

                //add the new expression to the list
                restrictions = restrictions == null
                                            ? addedExpression
                                            : Expression.AndAlso(restrictions, addedExpression);
            }

            return restrictions;
        }

        #endregion Get Advanced Page

        #region Expression

        /// <summary>
        /// Creates an Express.Parameter of the current type of the Entity
        /// </summary>
        /// <returns>ParameterExpression</returns>
        public ParameterExpression GetGenericType()
        {
            return Expression.Parameter(typeof(TEntity));
        }

        #endregion Expression

        #region Stored Proc Helpers

        #region Input Parameters

        /// <summary>
        /// Create an input SqlParameter of type bigint
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetInputBigInt(string parameterName, int length, string value)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.BigInt, value, length);
        }

        /// <summary>
        /// Create an input SqlParameter of type bit
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetInputBit(string parameterName, int length, string value)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.Bit, value, length);
        }

        /// <summary>
        /// Create an input SqlParameter of type int
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetInputInt(string parameterName, int length, string value)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.Int, value, length);
        }

        /// <summary>
        /// Create an input SqlParameter of type nvarchar
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetInputNVarChar(string parameterName, int length, string value)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.NVarChar, value, length);
        }

        /// <summary>
        /// Create an input SqlParameter of type varchar
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetInputVarChar(string parameterName, int length, string value)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.VarChar, value, length);
        }

        #endregion Input Parameters

        #region Output Parameters

        /// <summary>
        /// Create an output SqlParameter of type bigint
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetOutputBigInt(string parameterName, int length)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.BigInt, null, length, ParameterDirection.Output);
        }

        /// <summary>
        /// Create an output SqlParameter of type bit
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetOutputBit(string parameterName, int length)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.Bit, null, length);
        }

        /// <summary>
        /// Create an output SqlParameter of type int
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetOutputInt(string parameterName, int length)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.Int, null, length, ParameterDirection.Output);
        }

        /// <summary>
        /// Create an output SqlParameter of type nvarchar
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetOutputNVarChar(string parameterName, int length)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.NVarChar, null, length, ParameterDirection.Output);
        }

        /// <summary>
        /// Create an output SqlParameter of type varchar
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="length">The size of the parameter</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter GetOutputVarChar(string parameterName, int length)
        {
            return this.GetSqlParameter(parameterName, SqlDbType.VarChar, null, length, ParameterDirection.Output);
        }

        #endregion Output Parameters

        #region Run Stored Procedure

        /// <summary>
        /// Run a stored procedure with multiple parameters that returns a single row
        /// </summary>
        /// <param name="storedProcName">The name of the stored proc</param>
        /// <param name="args">The parameters</param>
        /// <returns>TEntity</returns>
        public TEntity RunEntityStoredProc(string storedProcName, params SqlParameter[] args)
        {
            return this.RunEntityStoredProc<TEntity>(storedProcName, args);
        }

        /// <summary>
        /// Run a stored procedure with multiple parameters that returns a single row
        /// of a specific type
        /// </summary>
        /// <typeparam name="T">The type of the return</typeparam>
        /// <param name="storedProcName">The name of the stored proc</param>
        /// <param name="args">The parameters</param>
        /// <returns>T</returns>
        public T RunEntityStoredProc<T>(string storedProcName, params SqlParameter[] args)
        {
            var proc = this.StoredProcQueryString(storedProcName, args);
            return this.Database.SqlQuery<T>(proc, args).FirstOrDefault();
        }

        /// <summary>
        /// Run a stored procedure with multiple parameters that returns a list
        /// </summary>
        /// <param name="storedProcName">The name of the stored proc</param>
        /// <param name="args">The parameters</param>
        /// <returns>List of TEntity</returns>
        public List<TEntity> RunListEntityStoredProc(string storedProcName, params SqlParameter[] args)
        {
            return this.RunListEntityStoredProc<TEntity>(storedProcName, args);
        }

        /// <summary>
        /// Runs a stored procedure and returns a list of a specific type
        /// </summary>
        /// <typeparam name="T">The type that is returned from the proc</typeparam>
        /// <param name="storedProcName">The name of the stored proc</param>
        /// <param name="args">The parameters</param>
        /// <returns>List of T</returns>
        public List<T> RunListEntityStoredProc<T>(string storedProcName, params SqlParameter[] args)
        {
            var proc = this.StoredProcQueryString(storedProcName, args);
            return this.Database.SqlQuery<T>(proc, args).ToList();
        }

        /// <summary>
        /// Runs a stored procedure that doesn't return an object
        /// </summary>
        /// <param name="storedProcName">The name of the stored procedure</param>
        /// <param name="args">The SqlParameter to use</param>
        public void RunVoidStoredProc(string storedProcName, params SqlParameter[] args)
        {
            try
            {
                var proc = this.StoredProcQueryString(storedProcName, args);
                this.Database.SqlQuery(null, proc, args);
            }
            catch (Exception) { }
        }

        #endregion Run Stored Procedure

        /// <summary>
        /// Generic function to create a SqlParameter
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="type">The SqlDbType of the parameter</param>
        /// <param name="value">The value of the parameter (defaults to null)</param>
        /// <param name="length">The length of the parameter (defaults to 0)</param>
        /// <param name="direction">The direction of the parameter (defaults to Input)</param>
        /// <returns>SqlParameter</returns>
        private SqlParameter GetSqlParameter(string parameterName, SqlDbType type, object value = null, int length = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            var parameter = new SqlParameter(parameterName.StartsWith("@") ? parameterName : string.Format("@{0}", parameterName), value);
            parameter.SqlDbType = type;
            parameter.Direction = direction;
            if (length != 0)
            {
                parameter.Size = length;
            }

            return parameter;
        }

        /// <summary>
        /// Helper function to create the query to run a stored procedure
        /// </summary>
        /// <param name="storedProcName">The name of the stored procedure</param>
        /// <param name="args">The list of SqlParameters that are needed for the stored proc</param>
        /// <returns>A string of the EXEC query</returns>
        private string StoredProcQueryString(string storedProcName, params SqlParameter[] args)
        {
            var parameters = string.Empty;
            foreach (var param in args)
            {
                parameters += string.Format("{1}{0}", param.ParameterName, parameters != string.Empty ? ", " : string.Empty);
            }

            var proc = string.Format("exec {0} {1}", storedProcName, parameters);
            return proc;
        }

        #endregion Stored Proc Helpers
    }
}