using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.DataIntegration
{
    public class DocumentLines
    {
        public int LineNum { get; set; }
        public string U_NumeroActividad { get; set; }
        public string U_FechaDespacho { get; set; }
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