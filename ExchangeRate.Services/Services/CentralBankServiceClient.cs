using System;
using System.Threading.Tasks;
using System.Xml;
using BCCRService; // Asegúrate de que este namespace coincida con el generado por la referencia WCF

namespace ExchangeRate.Services.Services
{
    public class CentralBankServiceClient
    {
        // Datos de autenticación y parámetros fijos
        private readonly string _usuario = "Deyler Alvarez";
        private readonly string _correo = "deylerafernandez@gmail.com";
        private readonly string _token = "6NAN2AA77E";

        /// <summary>
        /// Obtiene el tipo de cambio del día indicado.
        /// </summary>
        /// <param name="fecha">Fecha para la consulta (formato dd/MM/yyyy)</param>
        /// <returns>Tipo de cambio (valor numérico)</returns>
        public async Task<decimal> ObtenerTipoCambioAsync(DateTime fecha)
        {
            string fechaStr = fecha.ToString("dd/MM/yyyy");

            try
            {
                using (var client = new wsindicadoreseconomicosSoapClient(wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap))
                {
                    // Se realiza la llamada al método asíncrono del servicio
                    var response = await client.ObtenerIndicadoresEconomicosXMLAsync("317", fechaStr, fechaStr, _usuario, "N", _correo, _token);

                    // Cargar el XML devuelto en un objeto XmlDocument
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(response);

                    // Definir el XPath para extraer el valor del tipo de cambio
                    string xpath = "Datos_de_INGC011_CAT_INDICADORECONOMIC/INGC011_CAT_INDICADORECONOMIC/NUM_VALOR";
                    XmlNode node = xmlDocument.SelectSingleNode(xpath);

                    if (node != null && decimal.TryParse(node.InnerText, out decimal tipoCambio))
                    {
                        return tipoCambio;
                    }
                    else
                    {
                        throw new Exception("No se encontró el nodo con el tipo de cambio o el formato es incorrecto.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes agregar lógica para registrar el error en tu bitácora
                throw new Exception("Error al obtener el tipo de cambio: " + ex.Message);
            }
        }
    }
}
