//using ExchangeRate.Domain.Entities;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//namespace ExchangeRate.Data.Repositories
//{
//    public class ExchangeRateRepository : IExchangeRateRepository
//    {
//        private readonly string _connectionString;

//        public ExchangeRateRepository(string connectionString)
//        {
//            _connectionString = connectionString;
//        }

//        public async Task InsertExchangeRateAsync(ExchangeRateRepository rate)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            using (SqlCommand cmd = new SqlCommand("usp_InsertExchangeRate", con))
//            {
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("@Date", rate.Date);
//                cmd.Parameters.AddWithValue("@BuyRate", rate.BuyRate);
//                cmd.Parameters.AddWithValue("@SellRate", rate.SellRate);
//                await con.OpenAsync();
//                await cmd.ExecuteNonQueryAsync();
//            }
//        }

//        public async Task UpdateExchangeRateAsync(ExchangeRate rate)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            using (SqlCommand cmd = new SqlCommand("usp_UpdateExchangeRate", con))
//            {
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("@ExchangeRateId", rate.ExchangeRateId);
//                cmd.Parameters.AddWithValue("@BuyRate", rate.BuyRate);
//                cmd.Parameters.AddWithValue("@SellRate", rate.SellRate);
//                await con.OpenAsync();
//                await cmd.ExecuteNonQueryAsync();
//            }
//        }

//        public async Task DeleteExchangeRateAsync(int exchangeRateId)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            using (SqlCommand cmd = new SqlCommand("usp_DeleteExchangeRate", con))
//            {
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("@ExchangeRateId", exchangeRateId);
//                await con.OpenAsync();
//                await cmd.ExecuteNonQueryAsync();
//            }
//        }
//    }
//}
