namespace ServerLibrary.Entities
{
    public class ExchangeRateEntity
    {
        public required string Fecha { get; set; }
        public decimal TipoCambioCompra { get; set; }
        public decimal TipoCambioVenta { get; set; }
    }
}