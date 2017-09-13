using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IBI.<%= Name %>.Service.Utils.Config;

namespace IBI.<%= Name %>.Service.Core.Dependency
{
    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().InNamespace("IBI.<%= Name %>.Service.Entities").WithService.DefaultInterfaces().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().InNamespace("IBI.<%= Name %>.Service.Repositories").WithService.DefaultInterfaces().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().InNamespace("IBI.<%= Name %>.Service.Services").WithService.DefaultInterfaces().LifestyleTransient());

            var autoMapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
            container.Register(Component.For<MapperConfiguration>().UsingFactoryMethod(() => autoMapperConfiguration));
            container.Register(Component.For<IMapper>().UsingFactoryMethod(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper(ctx.Resolve)));
        }
    }
}