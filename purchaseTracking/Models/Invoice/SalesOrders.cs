using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Invoice
{
    public class SalesOrders
    {
        public string SlpName { get; set; }
        public int DocNum { get; set; }
        public string DocDate { get; set; }
        public string Comments { get; set; }
        public double DocTotal { get; set; }
        public string TipoVenta { get; set; }
        public string CardName { get; set; }
    }
}