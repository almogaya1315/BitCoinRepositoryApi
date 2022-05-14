using BitCoinManagerModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BitCoinRepositoryApi.Services
{
    public class BitCoinRepository
    {
        private const string CONNECTION_STRING = "Data Source=DESKTOP-8UGEGIK;Initial Catalog=BitCoin;Integrated Security=True";
        private readonly string _connectionString;

        public BitCoinRepository(string connectionString = null)
        {
            _connectionString = connectionString ?? CONNECTION_STRING;
        }

        private SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public User GetUser(User userModel)
        {
            using (var con = OpenConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", userModel.Email, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parameters.Add("Password", userModel.Password, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                var lookUp = new Dictionary<int, User>();
                con.Query<User, Order, User>("GetUser",
                    (user, order) =>
                    {
                        User innerUser = null;
                        if (!lookUp.TryGetValue(user.Id, out innerUser)) lookUp.Add(user.Id, innerUser = user);
                        userModel.Id = user.Id;
                        if (!innerUser.Orders.Exists(o => o.Id == order.Id))
                            innerUser.Orders.Add(order);
                        return innerUser;
                    }, parameters, commandType: CommandType.StoredProcedure);
                if (userModel.Id > 0)
                    userModel = lookUp.FirstOrDefault().Value;
                return userModel;
            }
        }

        public int InsertUser(User user)
        {
            using (var con = OpenConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", user.Email, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parameters.Add("Password", user.Password, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                return con.QuerySingle<int>("InsertUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int InsertOrder(int userId, Order order)
        {
            using (var con = OpenConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Amount", order.Amount, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                parameters.Add("Price", order.Price, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                parameters.Add("OperationId", order.Operation, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parameters.Add("CreatorId", userId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                return con.QuerySingle<int>("InsertOrder", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
