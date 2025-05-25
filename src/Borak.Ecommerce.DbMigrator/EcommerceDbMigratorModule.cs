using Borak.Ecommerce.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Borak.Ecommerce.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EcommerceEntityFrameworkCoreModule),
    typeof(EcommerceApplicationContractsModule)
)]
public class EcommerceDbMigratorModule : AbpModule
{
}
