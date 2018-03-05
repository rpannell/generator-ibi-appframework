using Castle.Windsor;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service.Core.Dependency
{
    /// <summary>
    /// Sets up the IoC dependency injection on the controllers
    /// </summary>
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        #region Fields

        private readonly IWindsorContainer _container;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor for <see cref="WindsorCompositionRoot"/>
        /// </summary>
        /// <param name="container"></param>
        public WindsorCompositionRoot(IWindsorContainer container)
        {
            _container = container;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Create the initial Controller activiator
        /// </summary>
        /// <param name="request"></param>
        /// <param name="controllerDescriptor"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller =
                (IHttpController)_container.Resolve(controllerType);

            request.RegisterForDispose(
                new Release(
                    () => _container.Release(controller)));

            return controller;
        }

        #endregion Methods

        #region Classes

        private sealed class Release : IDisposable
        {
            #region Fields

            private readonly Action _release;

            #endregion Fields

            #region Constructors

            public Release(Action release)
            {
                _release = release;
            }

            #endregion Constructors

            #region Methods

            public void Dispose()
            {
                _release();
            }

            #endregion Methods
        }

        #endregion Classes
    }
}
