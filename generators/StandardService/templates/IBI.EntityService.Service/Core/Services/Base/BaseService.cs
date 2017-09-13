using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using IBI.<%= Name %>.Service.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IBI.<%= Name %>.Service.Core.Services
{
    public partial class BaseService<TRepository, TEntity, TPrimaryKey> : IBaseService<TRepository, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TRepository : IBaseRepository<TEntity, TPrimaryKey>
    {
        public TRepository Repository = default(TRepository);

        public virtual List<TEntity> Get()
        {
            return this.Repository.Get();
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            return this.Repository.Get(id);
        }

        public virtual void Delete(TPrimaryKey id)
        {
            this.Repository.Delete(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return this.Repository.Insert(entity);
        }

        public virtual void MergeValues(TEntity previousEntity, TEntity newEntity)
        {
            this.Repository.MergeValues(previousEntity, newEntity);
        }

        public virtual void Update(TEntity entity)
        {
            this.Repository.Update(entity);
        }

        public virtual PaginationResult<TEntity> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null)
        {
            return this.Repository.GetPage(offSet, limit, searchCriteria, sortName, sortOrder, genericType, extraExpr);
        }

        public virtual List<TEntity> GetAutocomplete(int length, object term)
        {
            return this.Repository.GetAutocomplete(length, term);
        }

        public virtual PaginationResult<TEntity> GetAdvancedPage(AdvancedPageModel model)
        {
            return this.Repository.GetAdvancedPage(model.SearchOffSet, model.SearchLimit, model.SearchString, model.SortString, model.SortOrder, model);
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.Get().AsQueryable();
        }

        public PaginationResult<TEntity> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null)
        {
            return this.Repository.GetAdvancedPage(offSet, limit, searchCriteria, sortName, sortOrder, model);
        }
    }
}