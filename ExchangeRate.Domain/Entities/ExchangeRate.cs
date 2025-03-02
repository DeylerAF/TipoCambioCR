namespace ExchangeRate.Domain.Entities
{
    public class ExchangeRate
    {
        public int ExchangeRateId { get; set; }
        public DateTime Date { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
    }
}
