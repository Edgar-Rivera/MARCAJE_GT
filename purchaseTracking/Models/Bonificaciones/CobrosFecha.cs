using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Bonificaciones
{
    public class CobrosFecha
    {
        public string Vendedor { get; set; }
        public string OV { get; set; }
        public string FechaOrden { get; set; }
        public string No_Factura { get; set; }
        public string DTE  { get; set; }
        public string FechaFactura  { get; set; }
        public string Cliente  { get; set; }
        public double ValorOrden  { get; set; }
        public double ValorFactura  { get; set; }
        public double ValorCobro  { get; set; }
        public string NumeroRecibo  { get; set; }
        public string NumeroPago { get; set; }
        public string FechaPago  { get; set; }
        public string PorcentajeFacturacion { get; set; }
        public string Medio  { get; set; }
    }
}