using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.CuentaPorPagar
{
    public class CuentaPorPagar
    {
        public string TipoDocumento { get; set; }
        public string DocNum { get; set; }
        public DateTime FechaDocumento { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string CodigoSN { get; set; }
        public string NombreSN { get; set; }
        public string TipoProveedor { get; set; }
        public int dias { get; set; }
        public string OC { get; set; }
        public string numeroRef { get; set; }
        public string moneda { get; set; }
        public double totalQTZ { get; set; }
        public double totalUSD { get; set; }
        public double saldoQTZ { get; set; }
        public double saldoUSD { get; set; }
        public string corriente { get; set; }
        public string D0_30 { get; set; }
        public string D30_60 { get; set; }
        public string D60_90 { get; set; }
        public string D90_120 { get; set; }
        public string D_120 { get; set; }
        public string Estatus { get; set; }
        public string Estatus_Pago { get; set; }
        public string origen { get; set; }
        public int TT { get; set; }
        public string EP { get; set; }
    }
}