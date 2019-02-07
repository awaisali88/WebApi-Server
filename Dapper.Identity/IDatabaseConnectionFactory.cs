using System.Data;
using System.Threading.Tasks;

namespace Dapper.Identity
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}
