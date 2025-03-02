//using System.Data;

//namespace ExchangeRate.Data.Repositories
//{
//    public class ErrorLogRepository : ILogRepository
//    {
//        private readonly string _connectionString;

//        public ErrorLogRepository(string connectionString)
//        {
//            _connectionString = connectionString;
//        }

//        public async Task LogErrorAsync(string message)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            using (SqlCommand cmd = new SqlCommand("usp_InsertErrorLog", con))
//            {
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("@ErrorMessage", message);
//                await con.OpenAsync();
//                await cmd.ExecuteNonQueryAsync();
//            }
//        }
//    }
//}
