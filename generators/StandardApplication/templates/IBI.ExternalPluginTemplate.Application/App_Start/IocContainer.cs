using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.App_Start
{
    public static class IocContainer
    {
        #region Fields

        private static IWindsorContainer _container;

        #endregion Fields

        #region Methods

        public static void Setup()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());

            WindsorControllerFactory controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        #endregion Methods
    }

    public class BusinessLogicInstaller : IWindsorInstaller
    {
        #region Methods

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().InSameNamespaceAs(typeof(Services.BaseService)).WithService.DefaultInterfaces().LifestyleTransient());
        }

        #endregion Methods
    }

    public class ControllersInstaller : IWindsorInstaller
    {
        #region Methods

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                .Pick().If(t => t.Name.EndsWith("Controller"))
                .Configure(configurer => configurer.Named(configurer.Implementation.Name))
                .LifestylePerWebRequest());
        }

        #endregion Methods
    }

    public class WindsorControllerFactory : DefaultControllerFactory
    {
        #region Fields

        private readonly IKernel _kernel;

        #endregion Fields

        #region Constructors

        public WindsorControllerFactory(IKernel kernel)
        {
            this._kernel = kernel;
        }

        #endregion Constructors

        #region Methods

        public override void ReleaseController(IController controller)
        {
            _kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }
            return (IController)_kernel.Resolve(controllerType);
        }

        #endregion Methods
    }
}