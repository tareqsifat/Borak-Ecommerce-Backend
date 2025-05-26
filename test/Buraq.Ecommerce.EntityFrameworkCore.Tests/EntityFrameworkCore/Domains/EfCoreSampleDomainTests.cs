using Buraq.Ecommerce.Samples;
using Xunit;

namespace Buraq.Ecommerce.EntityFrameworkCore.Domains;

[Collection(EcommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EcommerceEntityFrameworkCoreTestModule>
{

}
