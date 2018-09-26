using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace IBI.<%= Name %>.Service.Core.Utils
{
    public static class DIHelper
    {
        #region Methods

        public static IServiceCollection AddNamespaceServices(this IServiceCollection builder, string interfaceNamespace, string mainNamespace, string endingKey = "Service")
        {
            var currentInterfaces = Assembly.GetEntryAssembly().GetTypes()
                                            .Where(x => x.Namespace == interfaceNamespace)
                                            .ToList();

            foreach (var iter in currentInterfaces)
            {
                //gets the entity name by removing the I and the common name at the end
                var name = iter.Name.Substring(1).Replace(endingKey, string.Empty);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var main = Assembly.GetEntryAssembly().GetTypes().FirstOrDefault(x => x.Name == string.Format("{0}{1}", name, endingKey) && x.Namespace == mainNamespace);
                    if (main != null)
                    {
                        builder.AddTransient(iter, main);
                    }
                }
            }

            return builder;
        }

        #endregion Methods
    }
}