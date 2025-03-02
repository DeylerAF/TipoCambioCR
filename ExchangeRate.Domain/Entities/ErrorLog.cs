namespace ExchangeRate.Domain.Entities
{
    public class ErrorLog
    {
        public int ErrorLogId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ErrorDate { get; set; }
    }
}
