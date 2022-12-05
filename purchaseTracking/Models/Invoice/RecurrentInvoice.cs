using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Models.Invoice
{
    public class RecurrentInvoice
    {
        public string SlpName { get; set; }
        public int DocNum { get; set; }
        public string DocDate { get; set; }
        public string Comments { get; set; }
        public double DocTotal { get; set; }
        public string TipoVenta { get; set; }
        public string CardName { get; set; }
        public string Unidad_Comercial { get; set; }
        public string Rate { get; set; }
        public string DocCur { get; set; }
        public string Duracion { get; set; }
        public string Periocidad { get; set; }
    }
}