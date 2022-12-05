using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Activities
{
    public class OrderDetails
    {
        public int ClgCode { get; set; }
        public string DocNum { get; set; }
        public string CardCode { get; set; }
        public string endDate { get; set; }
        public string CardName { get; set; }
        public string U_retrasoDias { get; set; }
        public string Notes { get; set; }
        public string U_Comentarios { get; set; }
        public string U_Correo { get; set; }
        public string Recontact { get; set; }
        public string FechaActualizacion { get; set; }
        public int Status { get; set; }

    }
}