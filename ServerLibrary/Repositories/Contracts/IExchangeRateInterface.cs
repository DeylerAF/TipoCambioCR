using ServerLibrary.Entities;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IExchangeRateInterface
    {
        Task<List<ExchangeRateEntity>> GetExchangeRatesAsync(DateTime startDate, DateTime endDate);
    }
}