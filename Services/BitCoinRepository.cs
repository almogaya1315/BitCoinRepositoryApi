using System;
using System.Collections.Generic;
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

        public int ValidateLogin()
    }
}
