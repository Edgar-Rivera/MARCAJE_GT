using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Sap.Data.Hana;

namespace purchaseTracking.Connection
{
    public class eTalentConnection
    {
        public static SqlConnection connectionResult()
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=10.93.110.57;Initial Catalog=db_nomina_isertec_v5_prod;User ID=sa;Password=M@n4g3rS!st3m$+*";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            return cnn;

        }


        public static HanaConnection ConexionHana()
        {


            string CadenaConexionHana = "Server=10.93.110.52:30015;UID=SAPDBA;PWD=M4nag3R2023+.; Current Schema=SBO_ISERTEC_GT";

            try
            {
                HanaConnection conn = new HanaConnection(CadenaConexionHana);
                return conn;
            }
            catch (HanaException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
    }
}