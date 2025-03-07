using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateEntity>> GetExchangeRatesAsync(DateTime startDate, DateTime endDate);
    }
}