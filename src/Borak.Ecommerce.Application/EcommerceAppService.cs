using Borak.Ecommerce.Localization;
using Volo.Abp.Application.Services;

namespace Borak.Ecommerce;

/* Inherit your application services from this class.
 */
public abstract class EcommerceAppService : ApplicationService
{
    protected EcommerceAppService()
    {
        LocalizationResource = typeof(EcommerceResource);
    }
}
