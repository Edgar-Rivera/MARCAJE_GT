using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.CuentaPorPagar
{
    public class CuentaPorPagarG
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Tipo_Proveedor { get; set; }
        public double Saldo_QTZ { get; set; }
        public double Saldo_USD { get; set; }
    }
}