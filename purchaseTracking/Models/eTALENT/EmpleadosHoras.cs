using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.eTALENT
{
	public class EmpleadosHoras
	{
        public decimal ID { get; set; }
        public decimal CODIGO { get; set; }
        public DateTime? FECHA_MARCA { get; set; }
        public DateTime? MARCA { get; set; }
        public string ORDEN { get; set; }
    }
}