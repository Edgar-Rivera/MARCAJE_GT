using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.eTALENT
{
    public class HISTORICO_VACACIONES
    {
        public int CODIGO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string FECHA_INGRESO { get; set; }
        public string PERIODO { get; set; }
        public Single DIAS { get; set; }
        public Single GOZADOS { get; set; }
        public Single DVCON_DIAS { get; set; }
        public string DESDE { get; set; }
        public string HASTA { get; set; }
        public string CODIGO_EXTERNO { get; set; }
    }
}