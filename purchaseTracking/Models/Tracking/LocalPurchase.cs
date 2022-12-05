using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Tracking
{
    public class LocalPurchase
    {
        public int ClgCode { get; set; }
        public DateTime CntctDate { get; set; }
        public string U_Solicitante { get; set; }
        public string U_NAME { get; set; }
        public int OC { get; set; }
        public string CardName { get; set; }
        public string FechaOC { get; set; }
        public string U_FechaIngresoCD { get; set; }
        public int EM { get; set; }
        public string FechaEM { get; set; }
        public string Bodega { get; set; }
        public string cantidad { get; set; }
        public string U_FechaCliente { get; set; }
        public string U_FechaProveedor { get; set; }
        public string U_FechaEnvioProveedor { get; set; }
    }
}