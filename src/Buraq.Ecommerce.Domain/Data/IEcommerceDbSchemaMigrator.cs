using System.Threading.Tasks;

namespace Buraq.Ecommerce.Data;

public interface IEcommerceDbSchemaMigrator
{
    Task MigrateAsync();
}
