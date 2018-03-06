using IBI.Plugin.Utilities.Logging.Interfaces;
using System.Web.Mvc;

// Created by Genie on <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= PluginName %>.Plugin.Controllers
{
    /// <summary>
    /// Plugin Controller for <%= ControllerName %>
    /// </summary>
    public class <%= ControllerName %>Controller : BaseController
    {
        #region Fields

        /*
         * Add list of inject services as private readonly here
         */

        //private readonly IErpActiveDirectoryService erpActiveDirectoryService;
        private readonly ILogAdapter Log = <%= PluginName %>Logger<<%= ControllerName %>Controller>.GetLogger();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor for <%= ControllerName %>
        /// </summary>
        public <%= ControllerName %>Controller()
        {
            /*
             * Update the constructor to pass in services needed by injecting the interface of the service needed and
             * set the private variable to the service passed in
             *
             * public <%= ControllerName %>Controller(IErpActiveDirectoryService erpActiveDirectoryService)
             * {
             *     this.erpActiveDirectoryService = erpActiveDirectoryService;
             * }
             */
        }

        #endregion Constructors

        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion Methods
    }
}