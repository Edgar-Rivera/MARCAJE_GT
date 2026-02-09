using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models
{
    public class UserNameData
    {
        public string InternalKey { get; set; }
        public string UserName { get; set; }
        public string eMail { get; set; }
        public string Path { get; set; }
        public string AsignacionMarcaje { get; set; } = "No";
    }
}