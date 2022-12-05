using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Tracking
{
    public class ItemList
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public string Project { get; set; }
        public string U_IsOV { get; set; }
        public double Price { get; set; }
        public double DiscPrcnt { get; set; }
        public double LineTotal { get; set; }
        public double TotalSumSy { get; set; }
        public string Currency { get; set; }
        public string U_FechaIngresoCD { get; set; }
        public string U_LineaOV { get; set; }
        public string U_Estado { get; set; }
        public string U_FechaDespacho { get; set; }
        public string U_FechaEmbarcador { get; set; }
        public string U_FechaArribo { get; set; }
        public string U_NumeroActividad { get; set; }
        public string Dscription { get; set; }
        public string DocEntryOPDN { get; set; }
        public string FechaIngreso { get; set; }
    }
}