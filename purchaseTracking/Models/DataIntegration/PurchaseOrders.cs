using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileHelpers;

namespace purchaseTracking.Models.DataIntegration
{
    [DelimitedRecord(",")]
    public class PurchaseOrders
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string LineNum { get; set; }
        public string ItemCode { get; set; }   
        public string U_NumeroActividad { get; set; }
        public string U_LineaOV { get; set; }
        public string  U_FechaDespacho { get; set; }
        public string U_FechaEmbarcador { get; set; }
        public string U_FechaArribo { get; set; }
        public string U_FechaIngresoCD { get; set; }
        public string U_Estado { get; set; }
        public string U_MedioImportacion { get; set; }
        public string U_commentsDespacho { get; set; }
        public string U_commentsEmbarcador { get; set; }
        public string U_commentsAduanales { get; set; }
        public string U_commentsPrecioEntrega { get; set; }
    }
}