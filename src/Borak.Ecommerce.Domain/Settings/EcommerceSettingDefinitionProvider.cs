using Volo.Abp.Settings;

namespace Borak.Ecommerce.Settings;

public class EcommerceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EcommerceSettings.MySetting1));
    }
}
