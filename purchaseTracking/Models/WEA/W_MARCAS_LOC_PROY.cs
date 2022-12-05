using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.WEA
{
    public class W_MARCAS_LOC_PROY
    {
        public decimal CODIGO { get; set; }
        public DateTime FECHA_MARCA { get; set; }
        public DateTime MARCA { get; set; }
        public string TIPO { get; set; }
        public string PROJECT { get; set; }
        public string ORDEN { get; set; }
        public string NOM_CLIENTE { get; set; }
        public string NOM_PROJECT { get; set; }
        public string CLIENTE { get; set; }
        public string TIPO_V { get; set; }
        public string PROSPECTO { get; set; }
    }
}