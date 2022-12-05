using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Activities
{
    public class List
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
        public string ODC { get; set; }
        public string Recontact { get; set; }
        public string endDate { get; set; }
        public string U_retrasoDias { get; set; }
        public string status { get; set; }
        public string Estado { get; set; }
    }
}