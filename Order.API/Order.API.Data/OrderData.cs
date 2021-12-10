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
using Order.API.Shared.Framework.Helpers;
using Order.API.Business.Contracts.Error;

namespace Order.API.Data
{
    public class OrderData : IOrderRepository
    {
        private readonly string connectionString;
        public OrderData(string connectionString) => this.connectionString = connectionString;
        public async Task<ResponseGeneric<IEnumerable<OrderDTO>>> GetAllByUser(int userIdentifier)
        {
            var query = "[dbo].[usp_Order_GETL]";
            List<OrderDTO> orderList = new List<OrderDTO>();
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
                        command.Parameters.Add(new SqlParameter("@pi_UserId", userIdentifier));
                        var reader = await command.ExecuteReaderAsync();
                        var columnIdentifier = reader.GetOrdinal("Identifier");
                        var columnName = reader.GetOrdinal("Name");
                        var columnOrderStatusId = reader.GetOrdinal("OrderStatusId");

                        while (reader.Read())
                        {
                            orderList.Add(new OrderDTO
                            {
                                Identifier = !reader.IsDBNull(columnIdentifier) ? reader.GetInt32(columnIdentifier) : default,
                                Name = !reader.IsDBNull(columnName) ? reader.GetString(columnName) : string.Empty,
                                Status = !reader.IsDBNull(columnOrderStatusId) ? reader.GetInt32(columnOrderStatusId) : default,
                            });
                        }
                        reader.Close();
                        return ResponseGeneric.Create(orderList.AsEnumerable());
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<IEnumerable<OrderDTO>>(new Error(ErrorCode.INTERNAL_ERROR, ex));
            }
        }
        public async Task<ResponseGeneric<OrderDTO>> GetOrder(OrderDTO order, int userIdentifier)
        {
            var query = "[dbo].[usp_Order_GETL]";
            OrderDTO orderDTO = new OrderDTO();
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
                        command.Parameters.Add(new SqlParameter("@pi_UserId", userIdentifier));
                        command.Parameters.Add(new SqlParameter("@pi_Identifier", HelperConvert.ValidateOrderIdentifier(order.Identifier)));
                        command.Parameters.Add(new SqlParameter("@pc_Name", order.Name));
                        var reader = await command.ExecuteReaderAsync();
                        var columnIdentifier = reader.GetOrdinal("Identifier");
                        var columnName = reader.GetOrdinal("Name");
                        var columnOrderStatusId = reader.GetOrdinal("OrderStatusId");

                        while (reader.Read())
                        {
                            orderDTO = new OrderDTO
                            {
                                Identifier = !reader.IsDBNull(columnIdentifier) ? reader.GetInt32(columnIdentifier) : default,
                                Name = !reader.IsDBNull(columnName) ? reader.GetString(columnName) : string.Empty,
                                Status = !reader.IsDBNull(columnOrderStatusId) ? reader.GetInt32(columnOrderStatusId) : default,
                            };
                        }
                        reader.Close();
                        return ResponseGeneric.Create(orderDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<OrderDTO>(new Error(ErrorCode.INTERNAL_ERROR, ex));
            }
        }
        public async Task<ResponseGeneric<OrderDTO>> Insert(OrderDTO order, int userIdentifier)
        {
            try
            {
                var query = "[dbo].[usp_Order_INS]";
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
                        command.Parameters.Add(new SqlParameter("@pi_UserId", userIdentifier));
                        command.Parameters.Add(new SqlParameter("@pc_Name", order.Name));
                        command.Parameters.Add(new SqlParameter("@pi_OrderTypeId", EnumOrderType.WishList));
                        command.Parameters.Add(new SqlParameter("@pi_OrderStatusId", EnumOrderStatus.Active));
                        var entityIdentifier = await command.ExecuteScalarAsync();
                        order.Identifier = entityIdentifier != null ? Convert.ToInt32(entityIdentifier) : 0;
                        return ResponseGeneric.Create(order);
                    }
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<OrderDTO>(new Error(ErrorCode.INTERNAL_ERROR, ex));
            }
        }
        public async Task<ResponseGeneric<bool>> Update(OrderDTO order)
        {
            try
            {
                var query = "[dbo].[usp_Order_UPD]";
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
                        command.Parameters.Add(new SqlParameter("@pi_OrderId", order.Identifier));
                        command.Parameters.Add(new SqlParameter("@pc_Name", order.Name));
                        command.Parameters.Add(new SqlParameter("@pi_OrderStatusId", order.Status));
                        await command.ExecuteNonQueryAsync();
                    }
                    return ResponseGeneric.Create(true);
                }
            }
            catch (Exception ex)
            {
                return ResponseGeneric.CreateError<bool>(new Error(ErrorCode.INTERNAL_ERROR, ex));
            }
        }
    }
}
