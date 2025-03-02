//using ExchangeRate.Data.Repositories;
//using ExchangeRate.Domain.Entities;
//using System;
//using System.Threading.Tasks;

//namespace ExchangeRate.Services.Services
//{
//    public class ExchangeRateService
//    {
//        private readonly IExchangeRateRepository _repository;
//        private readonly CentralBankServiceClient _centralBankClient;
//        private readonly ILogRepository _logRepository;

//        public ExchangeRateService(IExchangeRateRepository repository,
//                                   CentralBankServiceClient centralBankClient,
//                                   ILogRepository logRepository)
//        {
//            _repository = repository;
//            _centralBankClient = centralBankClient;
//            _logRepository = logRepository;
//        }

//        // Método para obtener el tipo de cambio actual del servicio WCF y guardarlo
//        public async Task AddCurrentExchangeRateAsync()
//        {
//            try
//            {
//                // Consumir el servicio WCF para obtener el tipo de cambio del día actual
//                var result = _centralBankClient.GetExchangeRate(DateTime.Today);
//                if (result != null)
//                {
//                    var rate = new ExchangeRate
//                    {
//                        Date = DateTime.Today,
//                        BuyRate = result.BuyRate,
//                        SellRate = result.SellRate
//                    };
//                    await _repository.InsertExchangeRateAsync(rate);
//                }
//            }
//            catch (Exception ex)
//            {
//                // Registrar el error en la bitácora
//                await _logRepository.LogErrorAsync(ex.Message);
//                throw;
//            }
//        }

//        // Método para modificar (solo permite el día actual)
//        public async Task UpdateExchangeRateAsync(ExchangeRate rate)
//        {
//            if (rate.Date != DateTime.Today)
//                throw new Exception("No se puede modificar el tipo de cambio de días anteriores");

//            await _repository.UpdateExchangeRateAsync(rate);
//        }

//        // Método para eliminar (solo permite el día actual)
//        public async Task DeleteExchangeRateAsync(int exchangeRateId, DateTime rateDate)
//        {
//            if (rateDate != DateTime.Today)
//                throw new Exception("No se puede eliminar el tipo de cambio de días anteriores");

//            await _repository.DeleteExchangeRateAsync(exchangeRateId);
//        }
//    }
//}
