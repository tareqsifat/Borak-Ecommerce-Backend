using Volo.Abp.Identity;

namespace Buraq.Ecommerce;

public static class EcommerceConsts
{
    public const string DbTablePrefix = "Buraq";
    public const string? DbSchema = null;
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
}
