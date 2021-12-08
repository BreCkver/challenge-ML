using Order.API.Business.Contracts;
using Order.API.Shared.Entities;
using Order.API.Shared.Entities.Enums;
using Order.API.Shared.Entities.Parent;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Order.API.Data
{
    public class OrderDetailData : IOrderDetailRepository
    {
        private readonly string connectionString;
        public OrderDetailData(string connectionString) => this.connectionString = connectionString;

        public async Task<ResponseGeneric<bool>> Add(OrderDTO order, IEnumerable<BookDTO> bookList)
        {
            try
            {
                var query = "[dbo].[usp_OrderDetail_INS]";
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
                        foreach(var book in bookList)
                        {
                            command.Parameters.Add(new SqlParameter("@pi_OrderIdentifier", order.Identifier));
                            command.Parameters.Add(new SqlParameter("@pc_ExternalIdentifier", book.ExternalIdentifier));
                            command.Parameters.Add(new SqlParameter("@pc_Title", book.Title));
                            command.Parameters.Add(new SqlParameter("@pc_Author", string.Join(",", book.Authors.Select(a => a))));
                            command.Parameters.Add(new SqlParameter("@pc_Publisher", book.Publisher));
                            command.Parameters.Add(new SqlParameter("@pi_OrderStatusId", (int)EnumProductStatus.Active));
                            var entityIdentifier = await command.ExecuteScalarAsync();
                            book.Identifier = entityIdentifier != null ? Convert.ToInt32(entityIdentifier) : 0;
                            command.Parameters.Clear();
                        }
                        return ResponseGeneric.Create(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<bool>(new Error("exception", ex));
            }
        }

        public async Task<ResponseGeneric<IEnumerable<BookDTO>>> GetAllByOrder(OrderDTO order)
        {
            var query = "[dbo].[usp_OrderDetail_GETL]";
            List<BookDTO> productList = new List<BookDTO>();
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
                        command.Parameters.Add(new SqlParameter("@pi_OrderIdentifier", order.Identifier));
                        command.Parameters.Add(new SqlParameter("@pi_OrderStatusId", EnumProductStatus.Active));
                        var reader = await command.ExecuteReaderAsync();
                        var columnIdentifier = reader.GetOrdinal("Identifier");
                        var columnOrderIdentifier = reader.GetOrdinal("OrderIdentifier");
                        var columnExternalIdentifier = reader.GetOrdinal("ExternalIdentifier");
                        var columnDesciption = reader.GetOrdinal("Description");
                        var columnKeyWords = reader.GetOrdinal("KeyWords");
                        var columnTitle =  reader.GetOrdinal("Title");
                        var columnAuthor = reader.GetOrdinal("Author");
                        var columnPublisher = reader.GetOrdinal("Publisher");

                        while (reader.Read())
                        {
                            productList.Add(new BookDTO
                            {
                                Identifier = !reader.IsDBNull(columnIdentifier) ? reader.GetInt32(columnIdentifier) : default,
                                ExternalIdentifier = !reader.IsDBNull(columnExternalIdentifier) ? reader.GetString(columnExternalIdentifier) : string.Empty,                              
                                Keyword = !reader.IsDBNull(columnKeyWords) ? reader.GetString(columnKeyWords) : string.Empty,
                                Title = !reader.IsDBNull(columnTitle) ? reader.GetString(columnTitle) : string.Empty,
                                Authors = !reader.IsDBNull(columnTitle) ? new List<string> { reader.GetString(columnTitle) } : new List<string>(),
                                Publisher = !reader.IsDBNull(columnTitle) ? reader.GetString(columnTitle) : string.Empty,
                            });
                        }
                        reader.Close();
                        return ResponseGeneric.Create(productList.AsEnumerable());
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<IEnumerable<BookDTO>>(new Error("exception", ex));
            }
        }

        public async Task<ResponseGeneric<bool>> Update(OrderDTO order, IEnumerable<BookDTO> bookList)
        {
            try
            {
                var query = "[dbo].[usp_OrderDetail_UPD]";
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
                        foreach (var book in bookList)
                        {
                            command.Parameters.Add(new SqlParameter("@pi_OrderDetailId", book.Identifier));
                            command.Parameters.Add(new SqlParameter("@pi_OrderStatusId", book.Status));
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    return ResponseGeneric.Create(true);
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<bool>(new Error("exception", ex));
            }
        }
    }
}
