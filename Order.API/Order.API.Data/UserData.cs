using Order.API.Business.Contracts;
using Order.API.Business.Contracts.Error;
using Order.API.Shared.Entities;
using Order.API.Shared.Framework.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Order.API.Data
{
    public class UserData : IUserRepository
    {
        private string connectionString;
        public UserData(string connectionString) => this.connectionString = connectionString;
        public async Task<ResponseGeneric<UserDTO>> GetByUser(UserDTO entity)
        {
            var query = "[dbo].[usp_Users_GETL]";
            UserDTO user = new UserDTO();
            try
            {
                using (var connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    var command = new SqlCommand(query)
                    {
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };
                    using (command.Connection)
                    {
                        command.Parameters.Add(new SqlParameter("@pi_UserId", entity.Identifier));
                        command.Parameters.Add(new SqlParameter("@pc_userName", entity.UserName));
                        command.Parameters.Add(new SqlParameter("@pc_PassWord", entity.Password));
                        var reader = await command.ExecuteReaderAsync();
                        var columnIdentifier = reader.GetOrdinal("Identifier");
                        var columnUserName = reader.GetOrdinal("UserName");
                        var columnPassWord = reader.GetOrdinal("PassWord");
                        var columnStatusId = reader.GetOrdinal("UserStatusId");
                        while (reader.Read())
                        {
                            user = new UserDTO
                            {
                                Identifier = !reader.IsDBNull(columnIdentifier) ? reader.GetInt32(columnIdentifier) : default,
                                UserName = !reader.IsDBNull(columnUserName) ? reader.GetString(columnUserName) : string.Empty,
                                Password = !reader.IsDBNull(columnPassWord) ? reader.GetString(columnPassWord) : string.Empty,
                                StatusIdentifier = !reader.IsDBNull(columnStatusId) ? reader.GetInt32(columnStatusId) : default,
                            };                            
                        }
                        reader.Close();
                        return ResponseGeneric.Create(user);
                    }
                }               
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<UserDTO>(new Error(ErrorCode.INTERNAL_ERROR, ex));
            }            
        }

        public async Task<ResponseGeneric<UserDTO>> Insert(UserDTO entity)
        {
            try
            {
                var query = "[dbo].[usp_User_INS]";
                using (var connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    var command = new SqlCommand(query)
                    {
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };
                    using (command.Connection)
                    {
                        command.Parameters.Add(new SqlParameter("@pc_userName", entity.UserName));
                        command.Parameters.Add(new SqlParameter("@pc_PassWord", HelperConvert.Hash(entity.Password)));
                        command.Parameters.Add(new SqlParameter("@pi_StatusId", entity.StatusIdentifier));
                        var entityIdentifier = await command.ExecuteScalarAsync();
                        entity.Identifier = entityIdentifier != null ? Convert.ToInt32(entityIdentifier) : 0;
                        return ResponseGeneric.Create(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<UserDTO>(new Error(ErrorCode.INTERNAL_ERROR, ex));
            }
        }
    }    
}
