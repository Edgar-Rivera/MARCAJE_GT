using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Project
{
    public class FinancialProject
    {
        public string TipoProyecto { get; set; }
        public string EstatusProyecto { get; set; }
        public string PrjCode { get; set; }
        public string DocNum { get; set; }
        public string CardName { get; set; }
        public string SlpName { get; set; }
        public string Comments { get; set; }
        public string FechaAutorizacion { get; set; }
    }
}