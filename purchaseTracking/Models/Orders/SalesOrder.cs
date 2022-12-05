using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Orders
{
    public class SalesOrder
    {
        public string SlpName { get; set; }
        public int DocNum { get; set; }
        public string DocDate { get; set; }
        public string Comments { get; set; }
        public double TotalUSD { get; set; }
        public double TotalQ { get; set; }
        public string Cur { get; set; }
        public string Tipo_Venta { get; set; }
        public string Cliente { get; set; }
        public string Rate { get; set; }
        public string GerenteComercial { get; set; }
        public string UnidadComercial { get; set; }
    }
}