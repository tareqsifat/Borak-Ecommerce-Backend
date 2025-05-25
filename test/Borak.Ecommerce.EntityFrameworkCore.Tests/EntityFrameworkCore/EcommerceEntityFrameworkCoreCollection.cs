using Xunit;

namespace Borak.Ecommerce.EntityFrameworkCore;

[CollectionDefinition(EcommerceTestConsts.CollectionDefinitionName)]
public class EcommerceEntityFrameworkCoreCollection : ICollectionFixture<EcommerceEntityFrameworkCoreFixture>
{

}
