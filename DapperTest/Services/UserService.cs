using Dapper;
using DapperTest.Models;
using System.Data;
using System.Data.SqlClient;

namespace DapperTest.Services
{
    public interface IUserService
    {
        Task<UserListResponseDTO> GetRecordsList(int Page, int PageSize);
    }
    public class UserService : IUserService
    {
        private readonly IDapper _dapper;

        public UserService(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<UserListResponseDTO> GetRecordsList(int Page, int PageSize)
        {
            List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();
            var query = "Usp_GetRecords";

            var parameters = new DynamicParameters();
            parameters.Add("page", Page, DbType.Int32, ParameterDirection.Input);
            parameters.Add("rows", PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("TotalRecords", PageSize, DbType.Int32, ParameterDirection.Output);
            using (var connection = _dapper.GetDbconnection())
            {

                // var companies = await connection.QueryAsync<>(query, parameters, commandType: CommandType.StoredProcedure);
                var reader = await connection.ExecuteReaderAsync(query, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                while (reader.Read())
                {
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        keyValuePairs.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    values.Add(keyValuePairs);
                }
            }
            var TotalRecords = parameters.Get<int>("TotalRecords");
            UserListResponseDTO userListResponseDTO = new UserListResponseDTO
            {
                values = values,
                pagination = new PaginationDTO(Page, PageSize, TotalRecords)
            };

            return userListResponseDTO;
        }

    }
}
