using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace purchaseTracking.Connection
{
    public class connectionHana
    {
        public static HanaConnection connectionResult()
        {
            var connString = ConfigurationManager.ConnectionStrings["HanaConnection"].ConnectionString;
            HanaConnection conn = new HanaConnection(connString);
            conn.Open();
            return conn;
        }
    }
}

