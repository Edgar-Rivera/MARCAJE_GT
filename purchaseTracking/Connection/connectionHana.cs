using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace purchaseTracking.Connection
{
    public class connectionHana
    {
        public static HanaConnection connectionResult()
        {
            //Connexion con SAP HANA - Se utiliza un usuario de solo lectura en la base de datos debido al acuerdo de licencia
            HanaConnection conn = new HanaConnection();
            conn.ConnectionString = "Server=192.168.1.221:30015;UID=WEBMASTER;PWD=BTS!st3ms!; Current Schema=SBO_ISERTEC_GT";
            conn.Open();
            return conn;
        }
    }
}

