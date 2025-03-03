using ExchangeRate.Domain.Entities;

namespace ExchangeRate.Services.Services
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateEntity>> GetExchangeRatesAsync(DateTime startDate, DateTime endDate);
    }
}