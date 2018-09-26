using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;

namespace IBI.<%= Name %>.Application.Utils.DataProtection
{
    public static class WebServiceDataProtectionExtensions
    {
        #region Methods

        public static IDataProtectionBuilder PersistKeyToWebService(this IDataProtectionBuilder builder, string webServiceUrl, string key)
        {
            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new WebServiceXMLRepository(webServiceUrl, key);
            });
            return builder;
        }

        #endregion Methods
    }
}