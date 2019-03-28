using System.Data.SqlClient;
using AutoMapper;
using Common;
using Dapper.Repositories;
using Dapper.Repositories.DbContext;
using Dapper.Repositories.SqlGenerator;
using Microsoft.Extensions.Options;

namespace WebAPI_DataAccess.WebApiContext
{
    public partial class WebApiDbContext : DapperDbContext, IWebApiDbContext
    {
        private readonly SqlGeneratorConfig _config;
        private readonly IMapper _mapper;

        public WebApiDbContext(IOptions<WebApiDbOptions> dbOptions, IMapper mapper)
            : base(new SqlConnection(dbOptions.Value.ConnectionString))
        {
            _config = new SqlGeneratorConfig()
            {
                SqlProvider = dbOptions.Value.SqlProvider,
                UseQuotationMarks = dbOptions.Value.UseQuotationMarks,
                LogQuery = dbOptions.Value.LogQuery
            };
            _mapper = mapper;
        }

        public override IDapperRepository<TModel> GetRepository<TModel>(bool defaultConnection = true)
        {
            if (defaultConnection)
                return new DapperRepository<TModel>(Connection, _mapper, _config);
            return new DapperRepository<TModel>(ConnectionWithoutMultipleActiveResultSets, _mapper, _config);
        }

        public override IDapperSProcRepository GetSpRepository(bool defaultConnection = true)
        {
            if (defaultConnection)
                return new DapperSProcRepository(Connection);
            return new DapperSProcRepository(ConnectionWithoutMultipleActiveResultSets);
        }
    }
}
