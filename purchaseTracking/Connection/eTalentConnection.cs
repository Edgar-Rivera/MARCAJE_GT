using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace purchaseTracking.Connection
{
    public class eTalentConnection
    {
        public static SqlConnection connectionResult()
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=192.168.1.185;Initial Catalog=db_nomina_isertec_v5_prod;User ID=sa;Password=manag3RS";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            return cnn;

        }
    }
}