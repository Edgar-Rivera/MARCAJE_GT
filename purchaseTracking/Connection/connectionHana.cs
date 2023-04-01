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
            conn.ConnectionString = "Server=10.93.110.52:30015;UID=SAPDBA;PWD=M4nag3R2023+.; Current Schema=SBO_ISERTEC_GT";
            conn.Open();
            return conn;
        }
    }
}

