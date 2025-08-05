using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace purchaseTracking.Connection
{
    public class eTalentConnection
    {
        public static SqlConnection connectionResult()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["eTalentConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            return cnn;
        }
    }
}