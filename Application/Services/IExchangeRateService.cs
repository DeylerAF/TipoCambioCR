using Domain.Entities;

namespace Application.Services
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateEntity>> GetExchangeRatesAsync(DateTime startDate, DateTime endDate);
    }
}