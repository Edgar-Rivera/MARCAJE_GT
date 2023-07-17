using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Activities
{
    public class details
    {
        public int ClgCode { get; set; }
        public DateTime CntctDate { get; set; }
        public string Actividad { get; set; }
        public string Name { get; set; }
        public string AttendUser { get; set; }
        public string U_NAME { get; set; }
        public string U_Solicitante { get; set; }
        public int DocNum { get; set; }
        public string Details { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int status { get; set; }
        public int Priority { get; set; }
        public string ClaseDocumento { get; set; }
        public int DocType { get; set; }
        public string Notes { get; set; }
        public DateTime Recontact { get; set; }
        public DateTime endDate { get; set; }
        public string U_Comentarios { get; set; }
        public string ODC { get; set; }
        public string U_retrasoDias { get; set; }
        public string Proveedores { get; set; }
        public string U_Correo { get; set; }
        public string E_Mail { get; set; }
        public string Estado { get; set; }
        public string FechaActualizacion { get; set; }
        public double LineasP { get; set; }
        public double LineasI { get; set; }
        public string DireccionFacturacion { get; set; }
        public double MontoFacturar { get; set; }
        public int OC { get; set; }
        public string Entregas { get; set; }
        public string NombreContacto { get; set; }
        public string observaciones { get; set; }
        public string TipoEnvio { get; set; }
        public string Moneda { get; set; }
        public int Refacturacion { get; set; }
        public string U_Concepto { get; set; }
        public string UnidadComercial { get; set; }
        public string U_InternalKey { get; set; }
    }
}