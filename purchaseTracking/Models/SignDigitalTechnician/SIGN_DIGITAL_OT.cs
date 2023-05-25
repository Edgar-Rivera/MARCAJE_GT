using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.SignDigitalTechnician
{
    public class SIGN_DIGITAL_OT
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public string Period { get; set; }
        public string Instance { get; set; }
        public string Series { get; set; }
        public string Handwrtten { get; set; }
        public string Canceled { get; set; }
        public string Object { get; set; }
        public string LogInst { get; set; }
        public string UserSign { get; set; }
        public string Transfered { get; set; }
        public string Status { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateTime { get; set; }
        public string DataSource { get; set; }
        public string RequestStatus { get; set; }
        public string Creator { get; set; }
        public string Remark { get; set; }


        /* DATOS CREADOS POR USUARIO PARA CONTROL DE OT'S */
        public string U_InternalKey { get; set; }
        public string U_PathSign { get; set; }
    }
}