using Borak.Ecommerce.Samples;
using Xunit;

namespace Borak.Ecommerce.EntityFrameworkCore.Domains;

[Collection(EcommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EcommerceEntityFrameworkCoreTestModule>
{

}
