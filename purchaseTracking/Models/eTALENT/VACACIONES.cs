using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.eTALENT
{
    public class VACACIONES
    {
        public int TEMP_CODCIA { get; set; }
        public string GRL_DESC { get; set; }    
        public int EPDO_CODIGO { get;set; }
        public string EPDO_NOMBRE_COMPLETO { get; set; }    
        public string FECHA { get; set; }
        public int UND { get; set; }    
        public string UND_DESCRIPCION { get; set; }
        public int UND_CODIGO { get; set; }
        public string UND_NOMBRE { get; set; }
        public int PERIODOS { get; set; }
        public decimal DIAS { get; set; }
        public decimal GOZADOS { get; set; }    
        public decimal PENDIENTE { get; set; }
        public int JEFE { get; set; }
        public string UNIDAD_SAP { get; set; }
    }

}
