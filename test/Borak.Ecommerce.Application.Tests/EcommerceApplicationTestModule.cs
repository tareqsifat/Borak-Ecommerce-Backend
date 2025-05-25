using Volo.Abp.Modularity;

namespace Borak.Ecommerce;

[DependsOn(
    typeof(EcommerceApplicationModule),
    typeof(EcommerceDomainTestModule)
)]
public class EcommerceApplicationTestModule : AbpModule
{

}
