using System.Data.SqlClient;
using AutoMapper;
using Common;
using Dapper.Repositories.DbContext;
using Dapper.Repositories.SqlGenerator;
using Microsoft.Extensions.Options;

namespace WebAPI_DataAccess.WebApiContext
{
    public partial class WebApiDbContext : DapperDbContext, IWebApiDbContext
    {
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

        private readonly SqlGeneratorConfig _config;
        private readonly IMapper _mapper;
    }
}
