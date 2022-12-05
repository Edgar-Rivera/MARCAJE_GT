using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Invoice
{
    public class Invoice
    {
     
            public string FechaFactura { get; set; }
            public int DocNum { get; set; }
            public int OrdenVenta { get; set; }
            public string FechaOV { get; set; }
            public string DTE { get; set; }
            public string Cliente { get; set; }
            public double TotalUSD { get; set; }
            public double PagoAplicado { get; set; }
            public string Vendedor { get; set; }
            public string TipoFactura { get; set; }
            public string EstadoFactura { get; set; }
        
    }
}