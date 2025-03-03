using ExchangeRate.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ExchangeRate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExchangeRates(
            [FromQuery] string startDate,
            [FromQuery] string endDate)
        {
            try
            {
                // Convertimos las fechas de entrada a DateTime
                if (!DateTime.TryParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start) ||
                    !DateTime.TryParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end))
                {
                    return BadRequest("Formato de fecha incorrecto. Use dd/MM/yyyy.");
                }

                if (start > end)
                {
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
                }

                var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync(start, end);

                if (exchangeRates.Count == 0)
                {
                    return NotFound("No se encontraron tipos de cambio en el rango de fechas proporcionado.");
                }

                return Ok(exchangeRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los tipos de cambio: " + ex.Message);
            }
        }
    }
}