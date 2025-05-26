using Volo.Abp.Modularity;

namespace Buraq.Ecommerce;

[DependsOn(
    typeof(EcommerceDomainModule),
    typeof(EcommerceTestBaseModule)
)]
public class EcommerceDomainTestModule : AbpModule
{

}
