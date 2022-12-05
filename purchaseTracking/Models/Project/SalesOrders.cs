using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Project
{
    public class SalesOrders
    {
        public int DocNum { get; set; }
        public string FechaOrden { get; set; }
        public double QTZ { get; set; }
        public double USD { get; set; }
        public string CANCELED { get; set; }
        public string SlpCode { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Project { get; set; }
        public int Series { get; set; }
        public string  Indicator {get;set;}

    }
}