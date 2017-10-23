using Castle.Windsor;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Dependency
{
    /// <summary>
    /// Sets up the IoC dependency injection on the controllers
    /// </summary>
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        private readonly IWindsorContainer _container;

        public WindsorCompositionRoot(IWindsorContainer container)
        {
            _container = container;
        }

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

        private sealed class Release : IDisposable
        {
            private readonly Action _release;

            public Release(Action release)
            {
                _release = release;
            }

            public void Dispose()
            {
                _release();
            }
        }
    }
}