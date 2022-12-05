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
            connetionString = @"Data Source=10.93.110.173;Initial Catalog=db_eaccess_isertec_hn;User ID=sa;Password=manag3RS";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            return cnn;

        }
    }
}