using System.Threading.Tasks;

namespace Borak.Ecommerce.Data;

public interface IEcommerceDbSchemaMigrator
{
    Task MigrateAsync();
}
