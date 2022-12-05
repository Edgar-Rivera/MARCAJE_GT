using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Tracking
{
    public class Details
    {
        public int DocNum {get;set;}
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public double DocRate { get; set; }
        public double DocTotal { get; set; }
        public double VatSum { get; set; }
        public double DocTotalSy { get; set; }
        public double VatSumSy { get; set; }
        public double TotalExpSC { get; set; }
        public double TotalExpns { get; set; }
        public string SlpName { get; set; }
        public string Comments { get; set; }
        public string DocCur { get; set; }
        public int Series { get; set; }
    }
}