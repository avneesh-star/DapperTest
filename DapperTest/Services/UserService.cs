using Dapper;
using DapperTest.Models;
using System.Data;
using System.Data.SqlClient;

namespace DapperTest.Services
{
    public interface IUserService
    {
        Task<UserListResponseDTO> GetDrRecordsList(int Page, int PageSize);
        //Task<UserListResponseDTO> GetDtRecordsList(int Page, int PageSize);
        Task<UserListResponseDTO> GetDlRecordsList(int Page, int PageSize);
    }
    public class UserService : IUserService
    {
        private readonly IDapper _dapper;

        public UserService(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<UserListResponseDTO> GetDrRecordsList(int Page, int PageSize)
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
        public async Task<UserListResponseDTO> GetDlRecordsList(int Page, int PageSize)
        {
            List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();
            var query = "Usp_GetRecords";

            var parameters = new DynamicParameters();
            parameters.Add("page", Page, DbType.Int32, ParameterDirection.Input);
            parameters.Add("rows", PageSize, DbType.Int32, ParameterDirection.Input);
            parameters.Add("TotalRecords", PageSize, DbType.Int32, ParameterDirection.Output);
            using (var connection = _dapper.GetDbconnection())
            {
               
                var reader = await connection.QueryMultipleAsync(query, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                var data = reader.Read<dynamic>();
               // var VIEW = reader.Read().Take(1);
                var VIEW = reader.Read<dynamic>(); 
                var prop = reader.Read<dynamic>();
                var TotalRecords = parameters.Get<int>("TotalRecords");
                UserListResponseDTO userListResponseDTO = new UserListResponseDTO
                {
                    values = data,
                    view = VIEW,
                    prop= prop,
                    pagination = new PaginationDTO(Page, PageSize, TotalRecords)
                };
                return userListResponseDTO;
            }
            
        }

      
        //public async Task<UserListResponseDTO> GetDtRecordsList(int Page, int PageSize)
        //{
        //    List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();
        //    var datatable = new DataTable();
        //    var query = "Usp_GetRecords";

        //    var parameters = new DynamicParameters();
        //    parameters.Add("page", Page, DbType.Int32, ParameterDirection.Input);
        //    parameters.Add("rows", PageSize, DbType.Int32, ParameterDirection.Input);
        //    parameters.Add("TotalRecords", PageSize, DbType.Int32, ParameterDirection.Output);
        //    using (var connection = _dapper.GetDbconnection())
        //    {

        //       var cmd = connection.CreateCommand();
        //        cmd.CommandText = query;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        var Pageparam = cmd.CreateParameter();
        //        Pageparam.ParameterName = "Page";
        //        Pageparam.DbType = DbType.Int32;
        //        Pageparam.Value = Page;
        //        cmd.Parameters.Add(Pageparam);
        //        var rowsParam = cmd.CreateParameter();
        //        rowsParam.ParameterName = "rows";
        //        rowsParam.DbType = DbType.Int32;
        //        rowsParam.Value = PageSize;
        //        cmd.Parameters.Add(rowsParam);

        //        var TotalRecordsParam = cmd.CreateParameter();
        //        TotalRecordsParam.ParameterName = "TotalRecords";
        //        TotalRecordsParam.DbType = DbType.Int32;
        //        TotalRecordsParam.Value = PageSize;
        //        TotalRecordsParam.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(TotalRecordsParam);

        //        SqlDataAdapter dataAdapter = new SqlDataAdapter();

        //        dataAdapter.Fill(datatable);
        //        // var companies = await connection.QueryAsync<>(query, parameters, commandType: CommandType.StoredProcedure);

        //    }
        //    var TotalRecords = parameters.Get<int>("TotalRecords");
        //    UserListResponseDTO userListResponseDTO = new UserListResponseDTO
        //    {
        //        values = values,
        //        pagination = new PaginationDTO(Page, PageSize, TotalRecords)
        //    };

        //    return userListResponseDTO;
        //}

    }
}
