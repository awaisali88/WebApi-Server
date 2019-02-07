using System.Data.SqlClient;
using Common;
using Dapper.Repositories.DbContext;
using Dapper.Repositories.SqlGenerator;
using Microsoft.Extensions.Options;

namespace WebAPI_DataAccess.Context
{
    public partial class ApplicationDbContext : DapperDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(IOptions<DbOptions> dbOptions)
            : base(new SqlConnection(dbOptions.Value.ConnectionString))
        {
            _config = new SqlGeneratorConfig()
            {
                SqlProvider = dbOptions.Value.SqlProvider,
                UseQuotationMarks = dbOptions.Value.UseQuotationMarks,
                LogQuery = dbOptions.Value.LogQuery
            };
        }

        private readonly SqlGeneratorConfig _config;
    }
}
