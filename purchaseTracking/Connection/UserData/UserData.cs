using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Sap.Data.Hana;

namespace purchaseTracking.Connection.UserData
{
    public class UserData
    {
        // FUNCION QUE INGRESA LOS DATOS DE LAS MARCAS COMERCIALES
        public bool InsertWEA_CP(int CODIGO, string TIPO, string LATITUD, string LONGITUD, string PROJECT, string UUID, string CODIGO_EXTERNO, string ORDEN, string CLIENTE, string PROSPECTO, string PRES_V, string N_CLIENTE, string N_PROJECT)
        {
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            string commandText = "INSERT INTO W_MARCASACCESO_LOC_PROY VALUES(@CODE,CAST(CAST(GETDATE() AS DATE) AS DATETIME),GETDATE(),@TYPE_M,'WEB',@LATITUD,@LONGITUD,@PROJECT,@UUID,@CODIGO_EXTERNO," +
                "NULL,'',@PROSPECTO,@PRES_V,'','');";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", CODIGO);
            cmd.Parameters.AddWithValue("@TYPE_M", TIPO);
            cmd.Parameters.AddWithValue("@LATITUD", LATITUD);
            cmd.Parameters.AddWithValue("@LONGITUD", LONGITUD);
            cmd.Parameters.AddWithValue("@PROJECT", PROJECT);
            cmd.Parameters.AddWithValue("@UUID", UUID);
            cmd.Parameters.AddWithValue("@CODIGO_EXTERNO", CODIGO_EXTERNO);
            cmd.Parameters.AddWithValue("@ORDEN", ORDEN);
            cmd.Parameters.AddWithValue("@CLIENTE", CLIENTE);
            cmd.Parameters.AddWithValue("@PROSPECTO", PROSPECTO);
            cmd.Parameters.AddWithValue("@PRES_V", PRES_V);
            cmd.Parameters.AddWithValue("@N_CLIENTE", N_CLIENTE);
            cmd.Parameters.AddWithValue("@N_PROJECT", N_PROJECT);
            if (cmd.ExecuteNonQuery() == 1)
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }
        public bool InsertWEA_C(int CODIGO, string TIPO, string LATITUD, string LONGITUD, string PROJECT, string UUID, string CODIGO_EXTERNO, string ORDEN, string CLIENTE, string PROSPECTO, string PRES_V, string N_CLIENTE, string N_PROJECT)
        {
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            string commandText = "INSERT INTO W_MARCASACCESO_LOC_PROY VALUES(@CODE,CAST(CAST(GETDATE() AS DATE) AS DATETIME),GETDATE(),@TYPE_M,'WEB',@LATITUD,@LONGITUD,@PROJECT,@UUID,@CODIGO_EXTERNO," +
                "'',@CLIENTE,NULL,@PRES_V,@N_CLIENTE,@N_PROJECT);";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", CODIGO);
            cmd.Parameters.AddWithValue("@TYPE_M", TIPO);
            cmd.Parameters.AddWithValue("@LATITUD", LATITUD);
            cmd.Parameters.AddWithValue("@LONGITUD", LONGITUD);
            cmd.Parameters.AddWithValue("@PROJECT", PROJECT);
            cmd.Parameters.AddWithValue("@UUID", UUID);
            cmd.Parameters.AddWithValue("@CODIGO_EXTERNO", CODIGO_EXTERNO);
            cmd.Parameters.AddWithValue("@ORDEN", ORDEN);
            cmd.Parameters.AddWithValue("@CLIENTE", CLIENTE);
            cmd.Parameters.AddWithValue("@PROSPECTO", PROSPECTO);
            cmd.Parameters.AddWithValue("@PRES_V", PRES_V);
            cmd.Parameters.AddWithValue("@N_CLIENTE", N_CLIENTE);
            cmd.Parameters.AddWithValue("@N_PROJECT", N_PROJECT);
            if (cmd.ExecuteNonQuery() == 1)
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }
        // FUNCION QUE INSERTA DATOS EN SQL 
        public bool InsertWEA(int CODIGO, string TIPO, string LATITUD, string LONGITUD, string PROJECT, string UUID, string CODIGO_EXTERNO, string ORDEN, string CLIENTE, string PROSPECTO, string PRES_V, string N_CLIENTE, string N_PROJECT)
        {
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            string commandText = "INSERT INTO W_MARCASACCESO_LOC_PROY VALUES(@CODE,CAST(CAST(GETDATE() AS DATE) AS DATETIME),GETDATE(),@TYPE_M,'WEB',@LATITUD,@LONGITUD,@PROJECT,@UUID,@CODIGO_EXTERNO," +
                "@ORDEN,@CLIENTE,NULL,@PRES_V,@N_CLIENTE,@N_PROJECT);";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", CODIGO);
            cmd.Parameters.AddWithValue("@TYPE_M", TIPO);
            cmd.Parameters.AddWithValue("@LATITUD", LATITUD);
            cmd.Parameters.AddWithValue("@LONGITUD", LONGITUD);
            cmd.Parameters.AddWithValue("@PROJECT", PROJECT);
            cmd.Parameters.AddWithValue("@UUID", UUID);
            cmd.Parameters.AddWithValue("@CODIGO_EXTERNO", CODIGO_EXTERNO);
            cmd.Parameters.AddWithValue("@ORDEN", ORDEN);
            cmd.Parameters.AddWithValue("@CLIENTE", CLIENTE);
            cmd.Parameters.AddWithValue("@PROSPECTO", PROSPECTO);
            cmd.Parameters.AddWithValue("@PRES_V", PRES_V);
            cmd.Parameters.AddWithValue("@N_CLIENTE", N_CLIENTE);
            cmd.Parameters.AddWithValue("@N_PROJECT", N_PROJECT);
            if(cmd.ExecuteNonQuery() == 1)
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }
        // FUNCION QUE TOMA LOS DATOS DE LA ORDEN DE VENTA
        public Models.WEA.ORDR_DATA GetSAPDataName(string CardCode)
        {
            var data = new Models.WEA.ORDR_DATA();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT DISTINCT A.\"DocNum\", A.\"Project\", A.\"Comments\", A.\"CardName\" FROM ORDR A WHERE A.\"CardCode\" = ? LIMIT 1;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = CardCode;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.DocNum = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                data.Project = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                data.Comments = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                data.CardName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            }
            conn.Close();
            return data;
        }
        // FUNCION QUE TOMA LOS DATOS DE LA ORDEN DE VENTA
        public Models.WEA.ORDR_DATA GetSAPData(int ordr)
        {
            var data = new Models.WEA.ORDR_DATA();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"DocNum\", A.\"Project\", A.\"Comments\", A.\"CardName\" FROM ORDR A WHERE A.\"DocNum\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = ordr;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.DocNum = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                data.Project = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                data.Comments = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                data.CardName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);            
            }
            conn.Close();
            return data;
        }
        // FUNCION QUE OBTIENE DATOS DE USUARIOS EN EMPLEADOS - SAP//
        public Models.WEA.W_MARCAS_LOC_PROY LASTWEA(int user)
        {
            Models.WEA.W_MARCAS_LOC_PROY data = new Models.WEA.W_MARCAS_LOC_PROY();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT TOP 1 CODIGO, FECHA_MARCA, MARCA, CASE WHEN TIPO = '1' THEN 'ENTRADA' WHEN TIPO = '2' THEN 'INICIO COMIDA'  WHEN TIPO = '3' THEN 'FIN COMIDA' WHEN TIPO = '4' THEN 'SALIDA' WHEN TIPO = '10' THEN 'FIN TRASLADO' WHEN TIPO = '9' THEN 'INICIO TRASLADO' ELSE 'N/A' END \"TIPO\", PROJECT, ORDEN, NOM_CLIENTE, NOM_PROJECT, CLIENTE, PRES_O_VIRT, PROSPECTO FROM W_MARCASACCESO_LOC_PROY WHERE CODIGO_EXTERNO = @CODE ORDER BY ID DESC;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", user);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.CODIGO = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                data.FECHA_MARCA = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1);
                data.MARCA = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2);
                data.TIPO = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                data.PROJECT = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                data.ORDEN = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                data.NOM_CLIENTE = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                data.NOM_PROJECT = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                data.CLIENTE = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                data.TIPO_V = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                data.PROSPECTO = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
            }
            conn.Close();
            return data;
        }
        // FUNCION QUE OBTIENE DATOS DE USUARIOS EN EMPLEADOS - SAP//
        public List<Models.UserData.OHEM> GetOHEMs(int user)
        {
            var data = new List<Models.UserData.OHEM>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT \"empID\", \"Active\" FROM OHEM WHERE \"userId\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = user;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.UserData.OHEM()
                {
                    empID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    Active = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                });
            }
            conn.Close();
            return data;
        }
        // FUNCION QUE OBTIENE CODIGOS DE EMPELADOS Y PROFESION -- SESION LOGIN ETALENT//
        public List<Models.UserData.UserData> UserDatas(int external_code)
        {
            var data = new List<Models.UserData.UserData>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT EPDO_CODIGO, EPDO_CODIGO_EXTERNO,EDPO_EMPL_PROFESION FROM PAZ_EPDO_EMPLEADO WHERE EPDO_CODIGO_EXTERNO = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", external_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.UserData.UserData()
                {
                    EPDO_CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    EPDO_CODIGO_EXTERNO = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    EDPO_EMPL_PROFESION = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                });
            }
            return data;
        }
    }
}