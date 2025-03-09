using BCCRService;
using Microsoft.Extensions.Options;
using ServerLibrary.Entities;
using ServerLibrary.Repositories.Contracts;
using System.Globalization;
using System.Xml;
using ServerLibrary.Helpers;

namespace ServerLibrary.Repositories.Implementations
{
    public class ExchangeRateRepository : IExchangeRateInterface
    {
        private readonly CentralBankServiceOptions _bankOptions;

        public ExchangeRateRepository(IOptions<CentralBankServiceOptions> bankOptions)
        {
            _bankOptions = bankOptions.Value;
        }

        public async Task<List<ExchangeRateEntity>> GetExchangeRatesAsync(DateTime startDate, DateTime endDate)
        {
            List<ExchangeRateEntity> exchangeRates = new List<ExchangeRateEntity>();

            using (var client = new wsindicadoreseconomicosSoapClient(
                       wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap))
            {
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    string fecha = date.ToString("dd/MM/yyyy");

                    // Llamada para obtener el tipo de cambio COMPRA (indicador "317")
                    var resultCompra = await client.ObtenerIndicadoresEconomicosXMLAsync(
                        "317",
                        fecha,
                        fecha,
                        _bankOptions.Usuario,
                        "N",
                        _bankOptions.Correo,
                        _bankOptions.Token
                    );

                    // Llamada para obtener el tipo de cambio VENTA (indicador "318")
                    var resultVenta = await client.ObtenerIndicadoresEconomicosXMLAsync(
                        "318",
                        fecha,
                        fecha,
                        _bankOptions.Usuario,
                        "N",
                        _bankOptions.Correo,
                        _bankOptions.Token
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
                        exchangeRates.Add(new ExchangeRateEntity
                        {
                            Fecha = fecha,
                            TipoCambioCompra = tipoCambioCompra,
                            TipoCambioVenta = tipoCambioVenta
                        });
                    }
                }
            }

            return exchangeRates;
        }
    }
}