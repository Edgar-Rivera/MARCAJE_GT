using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.eTALENT
{
    public class VACACIONES_DISPONIBLES
    {
        public int CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public string FECHA_INGRESO { get; set; }
        public int TOTAL_VACACIONES { get; set; }
        public double DIAS_TOMADOS { get; set; }
        public string CODIGO_EXTERNO { get; set; }
        public double DIAS_PENDIENTES { get; set; }
        public int CODIGO_J { get; set; }
    }
}