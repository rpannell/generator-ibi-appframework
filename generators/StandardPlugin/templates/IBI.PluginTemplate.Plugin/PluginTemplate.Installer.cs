using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin
{
    /// <summary>
    /// Installs any necessary Castle Windsor IoC
    /// </summary>
    public class <%= Name %>Installer : IWindsorInstaller
    {
        #region Methods

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            /*
             * Need too add the services you would like to use within castle Windsor
             */
            container.Register(
                Classes.FromThisAssembly().InSameNamespaceAs(typeof(IBI.<%= Name %>.Plugin.Services.BaseService)).WithService.DefaultInterfaces().LifestyleTransient()
            );
        }

        #endregion Methods
    }
}