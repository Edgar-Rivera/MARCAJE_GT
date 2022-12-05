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
    }
}