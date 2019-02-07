using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper.Repositories.Attributes;
using Dapper.Repositories.Extensions;

namespace Dapper.Repositories
{
    public class DapperSProcRepository : IDapperSProcRepository
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperSProcRepository(IDbConnection connection)
        {
            Connection = connection;
        }

        /// <inheritdoc />
        public IDbConnection Connection { get; }

        private DynamicParameters BuildProcedureQuery<TSpParam>(TSpParam spParameters, out string query) where TSpParam : class, ISProcParam
        {
            StringBuilder queryString = new StringBuilder();
            queryString.Append($"EXEC {spParameters.ProcedureName} ");
            DynamicParameters sParams = null;
            //parameters.Add("", null, DbType.Binary, ParameterDirection.Input, );
            var paramProperties = spParameters.GetType().GetProperties()
                .Where(x => x.CanWrite && x.GetCustomAttributes<ProcedureParamAttribute>().Any());

            var propertyInfos = paramProperties as PropertyInfo[] ?? paramProperties.ToArray();
            if (propertyInfos.Any())
            {
                sParams = new DynamicParameters();
                foreach (var paramProperty in propertyInfos)
                {
                    ProcedureParamAttribute attributeData = paramProperty.GetCustomAttribute<ProcedureParamAttribute>();
                    queryString.Append($"{attributeData.ParameterName} ");
                    sParams.Add(attributeData.ParameterName, paramProperty.GetValue(spParameters), attributeData.SelectedDbType, attributeData.ParameterDirection);
                }
            }

            query = queryString.ToString();
            return sParams;
        }

        public IEnumerable<TSpModel> Execute<TSpModel, TSpParam>(TSpParam spParameters, IDbTransaction transaction = null) where TSpParam : class, ISProcParam
        {
            var parameters = BuildProcedureQuery(spParameters, out var procedureQuery);
            return Connection.Query<TSpModel>(procedureQuery, parameters, commandType: CommandType.StoredProcedure);
        }

        public Task<IEnumerable<TSpModel>> ExecuteAsync<TSpModel, TSpParam>(TSpParam spParameters, IDbTransaction transaction = null) where TSpParam : class, ISProcParam
        {
            var parameters = BuildProcedureQuery(spParameters, out var procedureQuery);
            return Connection.QueryAsync<TSpModel>(procedureQuery, parameters, commandType: CommandType.StoredProcedure);
        }

    }
}
