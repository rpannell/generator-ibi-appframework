using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IBI.<%= Name %>.Service.Core.Repositories.Interfaces
{
    public partial interface IBaseRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        IQueryable<TEntity> GetFullEntity();

        void Delete(TPrimaryKey Id);

        TEntity Insert(TEntity Entity);

        void Update(TEntity Entity);

        void MergeValues(TEntity previousEntity, TEntity newEntity);

        TEntity Get(TPrimaryKey Id);

        List<TEntity> Get();

        //IQueryable<TEntity> GetAll();

        List<TEntity> GetAutocomplete(int length, object term);

        PaginationResult<TEntity> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null, IQueryable<TEntity> query = null);

        PaginationResult<TEntity> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null, IQueryable<TEntity> query = null);
    }
}