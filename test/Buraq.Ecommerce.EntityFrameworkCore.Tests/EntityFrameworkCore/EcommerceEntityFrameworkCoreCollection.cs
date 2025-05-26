using Xunit;

namespace Buraq.Ecommerce.EntityFrameworkCore;

[CollectionDefinition(EcommerceTestConsts.CollectionDefinitionName)]
public class EcommerceEntityFrameworkCoreCollection : ICollectionFixture<EcommerceEntityFrameworkCoreFixture>
{

}
