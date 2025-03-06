using Microsoft.Extensions.Options;
using System.Xml;
using BCCRService;

namespace Application.Services
{
    public class CentralBankServiceClient
    {
        private readonly string _usuario;
        private readonly string _correo;
        private readonly string _token;

        public CentralBankServiceClient(IOptions<CentralBankServiceOptions> options)
        {
            var opts = options.Value;
            _usuario = opts.Usuario;
            _correo = opts.Correo;
            _token = opts.Token;
        }

        public async Task<decimal> ObtenerTipoCambioAsync(DateTime fecha)
        {
            string fechaStr = fecha.ToString("dd/MM/yyyy");

            try
            {
                using (var client = new wsindicadoreseconomicosSoapClient(wsindicadoreseconomicosSoapClient.EndpointConfiguration.wsindicadoreseconomicosSoap))
                {
                    var response = await client.ObtenerIndicadoresEconomicosXMLAsync(
                        "317",
                        fechaStr,
                        fechaStr,
                        _usuario,
                        "N",
                        _correo,
                        _token
                    );

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(response);
                    string xpath = "Datos_de_INGC011_CAT_INDICADORECONOMIC/INGC011_CAT_INDICADORECONOMIC/NUM_VALOR";
                    XmlNode? node = xmlDocument.SelectSingleNode(xpath);

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
                throw new Exception("Error al obtener el tipo de cambio: " + ex.Message);
            }
        }
    }
}