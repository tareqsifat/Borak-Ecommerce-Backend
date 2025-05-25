using Borak.Ecommerce.Samples;
using Xunit;

namespace Borak.Ecommerce.EntityFrameworkCore.Applications;

[Collection(EcommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<EcommerceEntityFrameworkCoreTestModule>
{

}
