using Volo.Abp.Modularity;

namespace Buraq.Ecommerce;

public abstract class EcommerceApplicationTestBase<TStartupModule> : EcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
