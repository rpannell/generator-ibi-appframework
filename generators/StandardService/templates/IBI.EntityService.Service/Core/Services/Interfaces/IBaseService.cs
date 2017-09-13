using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IBI.<%= Name %>.Service.Core.Services.Interfaces
{
    public interface IBaseService { }

    public partial interface IBaseService<TRepository, TEntity, TPrimaryKey> : IBaseService
        where TRepository : IBaseRepository<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
    {
        void Delete(TPrimaryKey Id);

        TEntity Insert(TEntity Entity);

        void Update(TEntity Entity);

        TEntity Get(TPrimaryKey Id);

        List<TEntity> Get();

        IQueryable<TEntity> GetAll();

        List<TEntity> GetAutocomplete(int length, object term);

        PaginationResult<TEntity> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null);

        PaginationResult<TEntity> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null);

        PaginationResult<TEntity> GetAdvancedPage(AdvancedPageModel model);
    }
}