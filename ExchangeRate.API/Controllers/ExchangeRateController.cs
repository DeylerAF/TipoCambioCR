using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;
using BCCRService; // Asegúrate de que este namespace es correcto

namespace ExchangeRate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
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

                List<object> exchangeRates = new List<object>();

                using (var client = new wsindicadoreseconomicosSoapClient(
                           wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap))
                {
                    for (DateTime date = start; date <= end; date = date.AddDays(1))
                    {
                        string fecha = date.ToString("dd/MM/yyyy");

                        // Llamada para obtener el tipo de cambio COMPRA (indicador "317")
                        var resultCompra = await client.ObtenerIndicadoresEconomicosXMLAsync(
                            "317",
                            fecha,
                            fecha,
                            "Deyler Alvarez",
                            "N",
                            "deylerafernandez@gmail.com",
                            "6NAN2AA77E"
                        );

                        // Llamada para obtener el tipo de cambio VENTA (indicador "318")
                        var resultVenta = await client.ObtenerIndicadoresEconomicosXMLAsync(
                            "318",
                            fecha,
                            fecha,
                            "Deyler Alvarez",
                            "N",
                            "deylerafernandez@gmail.com",
                            "6NAN2AA77E"
                        );

                        // Procesar XML
                        XmlDocument xmlCompra = new XmlDocument();
                        xmlCompra.LoadXml(resultCompra);
                        XmlNode? nodeCompra = xmlCompra.SelectSingleNode("Datos_de_INGC011_CAT_INDICADORECONOMIC/INGC011_CAT_INDICADORECONOMIC/NUM_VALOR");

                        XmlDocument xmlVenta = new XmlDocument();
                        xmlVenta.LoadXml(resultVenta);
                        XmlNode? nodeVenta = xmlVenta.SelectSingleNode("Datos_de_INGC011_CAT_INDICADORECONOMIC/INGC011_CAT_INDICADORECONOMIC/NUM_VALOR");

                        if (nodeCompra != null && decimal.TryParse(nodeCompra.InnerText, out decimal tipoCambioCompra) &&
                            nodeVenta != null && decimal.TryParse(nodeVenta.InnerText, out decimal tipoCambioVenta))
                        {
                            exchangeRates.Add(new
                            {
                                Fecha = fecha,
                                TipoCambioCompra = tipoCambioCompra,
                                TipoCambioVenta = tipoCambioVenta
                            });
                        }
                    }
                }

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
