﻿using Buraq.Ecommerce.Localization;
using Volo.Abp.Application.Services;

namespace Buraq.Ecommerce;

/* Inherit your application services from this class.
 */
public abstract class EcommerceAppService : ApplicationService
{
    protected EcommerceAppService()
    {
        LocalizationResource = typeof(EcommerceResource);
    }
}
