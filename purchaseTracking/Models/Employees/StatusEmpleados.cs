using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Employees
{
    public class StatusEmpleados
    {


        public int ClgCode { get; set; }
        public string CntctDate { get; set; }
        public string Actividad { get; set; }
        public string Name { get; set; }
        public string JefeInmediato { get; set; }
        public string Solicitante { get; set; }
        public string Details { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string MedioDia { get; set; }
        public string Status { get; set; }
        public string ComentariosSolicitante { get; set; }
        public string ComentariosJefe { get; set; }
        public int Dias { get; set; }


    }
}