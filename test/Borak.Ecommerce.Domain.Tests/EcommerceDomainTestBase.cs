﻿using Volo.Abp.Modularity;

namespace Borak.Ecommerce;

/* Inherit from this class for your domain layer tests. */
public abstract class EcommerceDomainTestBase<TStartupModule> : EcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
