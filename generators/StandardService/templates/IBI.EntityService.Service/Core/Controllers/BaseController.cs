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
        #region Fields

        public TService CurrentService;

        #endregion Fields

        #region Constructors

        public BaseController()
        {
        }

        #endregion Constructors

        #region Get

        [HttpPost(), Route("api/{controller}/AdvancedPage/{id}/")]
        public virtual IHttpActionResult AdvancedPage(int id, [FromBody] AdvancedPageModel model)
        {
            try
            {
                var pageResult = this.CurrentService.GetAdvancedPage(model);
                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                //return
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(), Route("api/{controller}/{id}")]
        public virtual IHttpActionResult Get(TPrimaryKey id)
        {
            try
            {
                var status = this.CurrentService.Get(id);
                if (status == null)
                {
                    return NotFound();
                }
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(), Route("api/{controller}/")]
        public virtual IHttpActionResult Get()
        {
            try
            {
                var allentities = this.CurrentService.Get();
                return Ok(allentities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(), Route("api/{controller}/GetPage")]
        public virtual IHttpActionResult GetPage(int limit, int offset, string search = null, string sort = null, string sortOrder = "desc")
        {
            try
            {
                var pageResult = this.CurrentService.GetPage(offset, limit, search, sort, sortOrder);
                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion Get

        #region Delete Actions

        /// <summary>
        /// Will delete an entity from the database using the generic service passed in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(), Route("api/{controller}/{id}")]
        public virtual IHttpActionResult Delete(TPrimaryKey id)
        {
            try
            {
                var entity = this.CurrentService.Get(id);
                this.CurrentService.Delete(id);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion Delete Actions

        #region Post Actions

        /// <summary>
        /// Will insert an entity into the database using the generic service passed in
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost(), Route("api/{controller}/")]
        public virtual IHttpActionResult Post([FromBody]TEntity entity)
        {
            try
            {
                this.CurrentService.Insert(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion Post Actions

        #region Put Actions

        /// <summary>
        /// Will update an entity in the database using the generic service passed in
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        [HttpPut(), Route("api/{controller}/{id}")]
        public virtual IHttpActionResult Put(int id, [FromBody]TEntity entity)
        {
            try
            {
                this.CurrentService.Update(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion Put Actions

        #region AutoComplete

        /// <summary>
        /// Gets an auto complete list based on the length of the number of results you want to return
        /// </summary>
        /// <param name="length"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpPost(), Route("api/{controller}/GetAutoComplete/{id}")]
        public virtual IHttpActionResult GetAutoComplete(int id, [FromBody] AutoComplete model)
        {
            try
            {
                var pageResult = this.CurrentService.GetAutocomplete(model.Length, model.SearchTerm);
                return Ok(pageResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion AutoComplete
    }
}