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

        private (string, DynamicParameters) BuildProcedureQuery<TSpParam>(TSpParam spParameters) where TSpParam : class, ISProcParam
        {
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
                    sParams.Add(attributeData.ParameterName, paramProperty.GetValue(spParameters), attributeData.SelectedDbType, attributeData.ParameterDirection);
                }
            }
            return (spParameters.ProcedureName, sParams);
        }

        public IEnumerable<TSpModel> Execute<TSpModel, TSpParam>(TSpParam spParameters, IDbTransaction transaction = null) where TSpParam : class, ISProcParam
        {
            (string, DynamicParameters) parameters = BuildProcedureQuery(spParameters);
            return Connection.Query<TSpModel>(parameters.Item1, parameters.Item2, commandType: CommandType.StoredProcedure, transaction:transaction);
        }

        public Task<IEnumerable<TSpModel>> ExecuteAsync<TSpModel, TSpParam>(TSpParam spParameters, IDbTransaction transaction = null) where TSpParam : class, ISProcParam
        {
            (string, DynamicParameters) parameters = BuildProcedureQuery(spParameters);
            return Connection.QueryAsync<TSpModel>(parameters.Item1, parameters.Item2, commandType: CommandType.StoredProcedure, transaction:transaction);
        }

    }
}
