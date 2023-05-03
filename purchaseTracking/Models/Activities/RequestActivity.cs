using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Activities
{
    public class RequestActivity
    {
        public string CardCode { get; set; }
        public string Notes { get; set; }
        public string ActivityDate { get; set; }
         public string Priority { get; set; }
        public string DocType { get; set; }      
        public string DocNum { get; set; }
        public string DocEntry { get; set; }
        public string Details { get; set; }
        public string Activity { get; set; }
        public int ActivityType { get; set; }
        public string EndDueDate { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string U_FechaActualizacion { get; set; }
        public string DurationType { get; set; }
        public string U_Solicitante { get; set; }
        public string U_retrasoDias { get; set; }
        public int HandledBy { get; set; }
        public string U_Correo { get; set; }
        public string U_internalKey { get; set; }
    }
}