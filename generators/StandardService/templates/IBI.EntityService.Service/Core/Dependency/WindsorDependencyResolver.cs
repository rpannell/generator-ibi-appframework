using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service.Core.Dependency
{
    /// <summary>
    /// Windsor installer for api controllers
    /// </summary>
    public class ApiControllersInstaller : IWindsorInstaller
    {
        #region Methods

        /// <summary>
        /// Install the necessary dependencies
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
             .BasedOn<ApiController>()
             .LifestylePerWebRequest());
        }

        #endregion Methods
    }

    /// <summary>
    /// Setups up everything needed for the Castle Windsor dependency injection
    /// </summary>
    public class WindsorDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        #region Fields

        private readonly IWindsorContainer _container;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor for <see cref="WindsorDependencyResolver"/>
        /// </summary>
        /// <param name="container"></param>
        public WindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the inital scope
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_container);
        }

        /// <summary>
        /// Garbage cleaning
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// searching the services for a service by the interface
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>object</returns>
        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
        }

        /// <summary>
        /// Gets a list of all of the registered scopes
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!_container.Kernel.HasComponent(serviceType))
            {
                return new object[0];
            }

            return _container.ResolveAll(serviceType).Cast<object>();
        }

        /// <summary>
        /// Gargabe cleaing
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._container.Dispose();
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// Windsor dependency scope
    /// </summary>
    public class WindsorDependencyScope : IDependencyScope
    {
        #region Fields

        private readonly IWindsorContainer _container;
        private readonly IDisposable _scope;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor for <see cref="WindsorDependencyScope"/>
        /// </summary>
        /// <param name="container"></param>
        public WindsorDependencyScope(IWindsorContainer container)
        {
            this._container = container;
            this._scope = container.BeginScope();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gargabe cleaing
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// searching the services for a service by the interface
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns>object</returns>
        public object GetService(Type serviceType)
        {
            if (_container.Kernel.HasComponent(serviceType))
            {
                return _container.Resolve(serviceType);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a list of all of the registered scopes
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.ResolveAll(serviceType).Cast<object>();
        }

        /// <summary>
        /// Gargabe cleaing
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._scope.Dispose();
            }
        }

        #endregion Methods
    }
}