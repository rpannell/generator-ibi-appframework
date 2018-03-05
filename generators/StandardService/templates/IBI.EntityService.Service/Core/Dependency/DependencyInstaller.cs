using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IBI.<%= Name %>.Service.Utils.Config;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service.Core.Dependency
{
    /// <summary>
    /// Installs the necessary dependencies to the windsor container
    /// </summary>
    public class DependencyInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Setups up the dependency injection for the Repositories and Services and 
        /// any custom injections needed by the developer
        /// </summary>
        /// <param name="container">The Castle Windsor container</param>
        /// <param name="store">The Configuration store</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().InNamespace("IBI.<%= Name %>.Service.Entities").WithService.DefaultInterfaces().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().InNamespace("IBI.<%= Name %>.Service.Repositories").WithService.DefaultInterfaces().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().InNamespace("IBI.<%= Name %>.Service.Services").WithService.DefaultInterfaces().LifestyleTransient());
            //custom dependency injections can be added here

            //setups up the AutoMapper imapper injection to the AutoMapperProfile
            var autoMapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
            container.Register(Component.For<MapperConfiguration>().UsingFactoryMethod(() => autoMapperConfiguration));
            container.Register(Component.For<IMapper>().UsingFactoryMethod(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper(ctx.Resolve)));
        }
    }
}