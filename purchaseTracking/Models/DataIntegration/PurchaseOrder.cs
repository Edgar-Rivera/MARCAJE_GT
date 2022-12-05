using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.DataIntegration
{
    public class PurchaseOrder
    {
        public List<DocumentLines> DocumentLines { get; set; }
    }
}