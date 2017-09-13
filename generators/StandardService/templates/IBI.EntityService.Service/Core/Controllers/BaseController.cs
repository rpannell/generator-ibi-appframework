using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using IBI.<%= Name %>.Service.Core.Services.Interfaces;
using System;
using System.Web.Http;

namespace IBI.<%= Name %>.Service.Core.Controllers
{
    [Route("api/{controller}"), Authorize()]
    public partial class BaseController<TService, TRepository, TEntity, TPrimaryKey> : ApiController
        where TEntity : Entity<TPrimaryKey>
        where TRepository : IBaseRepository<TEntity, TPrimaryKey>
        where TService : IBaseService<TRepository, TEntity, TPrimaryKey>
    {
        public TService CurrentService;

        public BaseController()
        {
        }

        #region Get

        [Route("api/{controller}/{id}")]
        [HttpGet()]
        public virtual IHttpActionResult Get(TPrimaryKey id)
        {
            var status = this.CurrentService.Get(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpGet]
        public virtual IHttpActionResult Get()
        {
            try
            {
                var allentities = this.CurrentService.Get();
                return Ok(allentities);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public virtual IHttpActionResult GetPage(int limit, int offset, string search = null, string sort = null, string sortOrder = "desc")
        {
            var pageResult = this.CurrentService.GetPage(offset, limit, search, sort, sortOrder);
            return Ok(pageResult);
        }

        public virtual IHttpActionResult AdvancedPage(int id, [FromBody] AdvancedPageModel model)
        {
            var pageResult = this.CurrentService.GetAdvancedPage(model);

            return Ok(pageResult);
        }

        #endregion Get

        #region Delete Actions

        /// <summary>
        /// Will delete an entity from the database using the generic service passed in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete()]
        [Route("api/{controller}/{id}")]
        public virtual IHttpActionResult Delete(TPrimaryKey id)
        {
            var entity = this.CurrentService.Get(id);
            this.CurrentService.Delete(id);
            return Ok(entity);
        }

        #endregion Delete Actions

        #region Post Actions

        /// <summary>
        /// Will insert an entity into the database using the generic service passed in
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IHttpActionResult Post([FromBody]TEntity entity)
        {
            this.CurrentService.Insert(entity);
            return Ok(entity);
        }

        #endregion Post Actions

        #region Put Actions

        /// <summary>
        /// Will update an entity in the database using the generic service passed in
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        [HttpPut()]
        public virtual void Put(int id, [FromBody]TEntity entity)
        {
            this.CurrentService.Update(entity);
        }

        #endregion Put Actions

        #region AutoComplete

        /// <summary>
        /// Gets an auto complete list based on the length of the number of results you want to return
        /// </summary>
        /// <param name="length"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IHttpActionResult GetAutoComplete(int id, [FromBody] AutoComplete model)
        {
            var pageResult = this.CurrentService.GetAutocomplete(model.Length, model.SearchTerm);
            return Ok(pageResult);
        }

        #endregion AutoComplete
    }
}