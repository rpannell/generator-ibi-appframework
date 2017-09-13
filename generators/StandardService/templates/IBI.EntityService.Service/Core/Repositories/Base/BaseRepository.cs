using IBI.<%= Name %>.Service.Core.Attributes;
using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using IBI.<%= Name %>.Service.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using static IBI.<%= Name %>.Service.Core.Attributes.Searchable;
using static IBI.<%= Name %>.Service.Core.Models.AdvancedSearch;

namespace IBI.<%= Name %>.Service.Core.Repositories
{
    public class BaseRepository<TEntity, TPrimaryKey> : DbContext, IBaseRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        #region Constructors

        public BaseRepository() : base("DefaultConnection")
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        #endregion Constructors

        #region Properties

        public DbSet<TEntity> Entity { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Used to get the full entity  that includes the
        /// sub entities in the query as long as they are
        /// marked virtual (unless it's the Id property)
        /// </summary>
        /// <returns></returns>
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

            return returnEntity;
        }

        #endregion Methods

        #region CRUD

        public virtual void Delete(TPrimaryKey id)
        {
            if (!typeof(TEntity).IsDefined(typeof(ReadOnlyAttribute), true))
            {
                var item = this.Entity.Find(id);
                this.Entity.Remove(item);
                this.SaveChanges();
            }
        }

        public virtual List<TEntity> Get()
        {
            return GetFullEntity().ToList();
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = this.Entity.Find(id);
            //this ensures that entity can be retrieved and possible changed
            //in another piece of memory later on
            this.Entry(entity).State = EntityState.Detached;
            return entity;
        }

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

        public virtual void MergeValues(TEntity previousEntity, TEntity newEntity)
        {
            this.Entry(previousEntity).CurrentValues.SetValues(newEntity);
        }

        public virtual void Update(TEntity entity)
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
        }

        #endregion CRUD

        #region Get Page

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
            if (query == null) query = this.GetFullEntity();

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

        public Expression GetSearchRestrictions(object searchCriteria, ParameterExpression genericType)
        {
            Expression restrictions = null;

            if (searchCriteria != null)
            {
                foreach (var sc in typeof(TEntity).GetProperties())
                {
                    if (sc.IsDefined(typeof(<%= Name %>.Service.Core.Attributes.Searchable.SearchAbleAttribute), true))
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
        /// <returns></returns>
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

        private int GetPageRowCount(Expression restrictions, ParameterExpression genericType, IQueryable<TEntity> query)
        {
            return query.WhereHelper(restrictions, genericType)
                        .Count();
        }

        private List<TEntity> GetSearchResults(Expression restrictions, int limit, int offset, string sortName, string sortOrder, ParameterExpression genericType, IQueryable<TEntity> query)
        {
            return query.WhereHelper(restrictions, genericType)
                        .OrderByHelper(GetOrderBy(sortName), sortOrder == "desc")
                        .Skip(offset)
                        .Take(limit)
                        .ToList();
        }

        #endregion Get Page

        #region Get Autocomplete

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

        private Expression GetAutoCompleteRestrictions(object searchCriteria, ParameterExpression genericType)
        {
            Expression restrictions = null;

            if (searchCriteria != null)
            {
                foreach (var sc in typeof(TEntity).GetProperties())
                {
                    if (sc.IsDefined(typeof(<%= Name %>.Service.Core.Attributes.AutoComplete), true))
                    {
                        var searchAtts = (<%= Name %>.Service.Core.Attributes.AutoComplete[])sc.GetCustomAttributes(typeof(<%= Name %>.Service.Core.Attributes.AutoComplete), true);
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
                var value = Expression.Constant(LambdaHelpers.ChangeType(valueA, propertyType));
                Expression addedExpression = null;
                switch (adv.TypeOfSearch)
                {
                    case AdvancedSearchType.IsNull:
                        addedExpression = Expression.Equal(key, Expression.Constant(null));
                        break;

                    case AdvancedSearchType.IsNotNull:
                        addedExpression = Expression.NotEqual(key, Expression.Constant(null));
                        break;

                    case AdvancedSearchType.Equal:
                        addedExpression = Expression.Equal(key, value);
                        break;

                    case AdvancedSearchType.LessThan:
                        addedExpression = LambdaHelpers.NullableLessThan(key, value);
                        break;

                    case AdvancedSearchType.GreaterThan:
                        addedExpression = LambdaHelpers.NullableGreaterThan(key, value);
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

        public ParameterExpression GetGenericType()
        {
            return Expression.Parameter(typeof(TEntity));
        }

        #endregion Expression
    }
}