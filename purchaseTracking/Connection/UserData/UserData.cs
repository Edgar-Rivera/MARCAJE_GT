using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Sap.Data.Hana;
using DocumentFormat.OpenXml.Wordprocessing;
using Org.BouncyCastle.Ocsp;
//using static ClosedXML.Excel.XLPredefinedFormat;

namespace purchaseTracking.Connection.UserData
{
    public class UserData
    {

        // CAMBIOS FINALES REPORTE DIAS VACACIONES
        // RETORNA VACACIONES DISPONIBLES
        public List<Models.eTALENT.VACACIONES> VacacionesDiaSP(int internal_code)
        {
            var data = new List<Models.eTALENT.VACACIONES>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "EXEC [dbo].[SP_ReporteEstandar_25_ReporteVacDia_Prod] @codepdo = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", internal_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.VACACIONES()
                {
                    TEMP_CODCIA = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    GRL_DESC = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    EPDO_CODIGO = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    EPDO_NOMBRE_COMPLETO = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    FECHA = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    UND = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    UND_NOMBRE = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    UND_CODIGO = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    UND_DESCRIPCION = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    PERIODOS = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                    DIAS = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10),
                    GOZADOS = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11),
                    JEFE = reader.IsDBNull(15) ? 0 : reader.GetInt32(15),
                });
            }
            return data;
        }


        public List<Models.eTALENT.VACACIONES> VacacionesDiaSP_All()
        {
            var data = new List<Models.eTALENT.VACACIONES>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "EXEC [dbo].[SP_ReporteEstandar_25_ReporteVacDia_Prod_All];";
            cmd = new SqlCommand(commandText, conn);
            //cmd.Parameters.AddWithValue("@CODE", internal_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.VACACIONES()
                {
                    TEMP_CODCIA = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    GRL_DESC = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    EPDO_CODIGO = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    EPDO_NOMBRE_COMPLETO = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    FECHA = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    UND = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    UND_NOMBRE = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    UND_CODIGO = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    UND_DESCRIPCION = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    PERIODOS = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                    DIAS = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10),
                    GOZADOS = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11),

                    JEFE = reader.IsDBNull(15) ? 0 : reader.GetInt32(15),
                });
            }
            return data;
        }
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

        public bool INS_CALC_VACS(int Codigo, int CodigoInterno, int CodigoExterno, string NombreCompleto, DateTime FechaIngreso, float Saldo, string Estado)
        {
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            string commandText = "INSERT INTO PROC_INS_CALC_VACS VALUES(@Codigo, @CodigoInterno, @CodigoExterno, @NombreCompleto, @FechaIngreso, @Saldo, @Estado;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@Codigo", Codigo);
            cmd.Parameters.AddWithValue("@CodigoInterno", CodigoInterno);
            cmd.Parameters.AddWithValue("@CodigoExterno", CodigoExterno);
            cmd.Parameters.AddWithValue("@NombreCompleto", NombreCompleto);
            cmd.Parameters.AddWithValue("@FechaIngreso", FechaIngreso);
            cmd.Parameters.AddWithValue("@Saldo", Saldo);
            cmd.Parameters.AddWithValue("@Estado", Estado);            
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
            HanaCommand cmd = new HanaCommand("SELECT A.\"DocNum\", A.\"Project\", A.\"Comments\", A.\"CardName\", A.\"CardCode\" FROM ORDR A WHERE A.\"DocNum\" = ?;", conn);
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
                data.CardCode = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);            
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
                data.FECHA_MARCA = reader.IsDBNull(1) ? System.DateTime.Now : reader.GetDateTime(1);
                data.MARCA = reader.IsDBNull(2) ? System.DateTime.Now : reader.GetDateTime(2);
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
        public Models.UserData.OHEM GetOHEMs(int user)
        {
            var data = new Models.UserData.OHEM();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT \"Code\", \"Active\" FROM OHEM WHERE \"userId\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = user;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.empID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                data.Active = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            }
            conn.Close();
            return data;
        }
        // FUNCION QUE OBTIENE CODIGOS DE EMPELADOS Y PROFESION -- SESION LOGIN ETALENT//
        public Models.UserData.UserData UserDatas(int external_code)
        {
            var data = new Models.UserData.UserData();
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
                data.EPDO_CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                data.EPDO_CODIGO_EXTERNO = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                data.EDPO_EMPL_PROFESION = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            }
            return data;
        }

        //// DATOS OBTENDIOS DESDE ETLAENT SEGUN JEFE INMEDIATO
        ///
        public List<Models.eTALENT.EPDO_MASTER_DATA> EmpleadosUnidad(int external_code)
        {
            var data = new List<Models.eTALENT.EPDO_MASTER_DATA>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT * FROM TR_DATOS_MAESTROS_EMPLEADO_ISERTEC WHERE COD_JEFE_INMEDIATO = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", external_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.EPDO_MASTER_DATA()
                {
                    EPDO_CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    P_APELLIDO = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    S_APELLIDO = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    P_NOMBRE = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    S_NOMBRE = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    DIRECCION = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    NACIONALIDAD = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    FECHA_NACIMIENTO = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy"),
                    EPDO_SEXO = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    EPDO_ESTADO = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    ESTADO_CIVIL = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    FECHA_INGRESO = reader.IsDBNull(11) ? string.Empty : reader.GetDateTime(11).ToString("dd/MM/yyyy"),
                    SALARIO = reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                    BONIFICACION = reader.IsDBNull(13) ? 0 : reader.GetDecimal(13),
                    CUENTA = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                    FORMA_PAGO = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    TIPO_CIUENTA = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    CONTRATO_INICIO = reader.IsDBNull(17) ? string.Empty : reader.GetDateTime(17).ToString("dd/MM/yyyy"),
                    CONTRATO_FIN = reader.IsDBNull(18) ? string.Empty : reader.GetDateTime(18).ToString("dd/MM/yyyy"),
                    EDAD = reader.IsDBNull(19) ? 0 : reader.GetInt32(19),
                    FECHA_INGRESO_COPR = reader.IsDBNull(20) ? string.Empty : reader.GetDateTime(20).ToString("dd/MM/yyyy"),
                    BONO_NO_AFECTO = reader.IsDBNull(21) ? 0 : reader.GetDecimal(21),
                    MAIL = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                    MAIL_INTERNO = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                    CELULAR = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                    EPDO_MOTIVO_RETIRO = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                    CAUSA_RETIRO = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                    EPDO_PUESTO_RETIRO = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                    NOMBRE_COMPLETO = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                    EPDO_CASADA = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                    NUMERO_HIJOS = reader.IsDBNull(30) ? 0 : reader.GetInt32(30),
                    PROFESION = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                    CODIGO_EXTERNO = reader.IsDBNull(32) ? string.Empty : reader.GetString(32),
                    NUMERO_EMERGENCIA = reader.IsDBNull(33) ? string.Empty : reader.GetString(33),
                    UNIDAD = reader.IsDBNull(34) ? string.Empty : reader.GetString(34),
                    JEFE_INMEDIATO = reader.IsDBNull(35) ? string.Empty : reader.GetString(35),
                    PLAZA = reader.IsDBNull(36) ? string.Empty : reader.GetString(36),
                    CODIGO_JEFE = reader.IsDBNull(37) ? 0 : reader.GetInt32(37),
                });
            }
            return data;
        }

        //// DATOS OBTENDIOS DESDE ETLAENT PARA DATOS GENERALES DE EMPLEADOS
        ///
        public List<Models.eTALENT.EPDO_MASTER_DATA> DatosEmpleados(int external_code)
        {
            var data = new List<Models.eTALENT.EPDO_MASTER_DATA>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT * FROM TR_DATOS_MAESTROS_EMPLEADO_ISERTEC WHERE EPDO_CODIGO_EXTERNO = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", external_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.EPDO_MASTER_DATA()
                {
                    EPDO_CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    P_APELLIDO = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    S_APELLIDO = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    P_NOMBRE = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    S_NOMBRE = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    DIRECCION = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    NACIONALIDAD = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    FECHA_NACIMIENTO = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy"),
                    EPDO_SEXO = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    EPDO_ESTADO = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    ESTADO_CIVIL = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    FECHA_INGRESO = reader.IsDBNull(11) ? string.Empty : reader.GetDateTime(11).ToString("dd/MM/yyyy"),
                    SALARIO = reader.IsDBNull(12) ? 0 : reader.GetDecimal(12),
                    BONIFICACION = reader.IsDBNull(13) ? 0 : reader.GetDecimal(13),
                    CUENTA = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                    FORMA_PAGO = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    TIPO_CIUENTA = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    CONTRATO_INICIO = reader.IsDBNull(17) ? string.Empty : reader.GetDateTime(17).ToString("dd/MM/yyyy"),
                    CONTRATO_FIN = reader.IsDBNull(18) ? string.Empty : reader.GetDateTime(18).ToString("dd/MM/yyyy"),
                    EDAD = reader.IsDBNull(19) ? 0 : reader.GetInt32(19),
                    FECHA_INGRESO_COPR = reader.IsDBNull(20) ? string.Empty : reader.GetDateTime(20).ToString("dd/MM/yyyy"),
                    BONO_NO_AFECTO = reader.IsDBNull(21) ? 0 : reader.GetDecimal(21),
                    MAIL = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                    MAIL_INTERNO = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                    CELULAR = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                    EPDO_MOTIVO_RETIRO = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                    CAUSA_RETIRO = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                    EPDO_PUESTO_RETIRO = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                    NOMBRE_COMPLETO = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                    EPDO_CASADA = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                    NUMERO_HIJOS = reader.IsDBNull(30) ? 0 : reader.GetInt32(30),
                    PROFESION = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                    CODIGO_EXTERNO = reader.IsDBNull(32) ? string.Empty : reader.GetString(32),
                    NUMERO_EMERGENCIA = reader.IsDBNull(33) ? string.Empty : reader.GetString(33),
                    UNIDAD = reader.IsDBNull(34) ? string.Empty : reader.GetString(34),
                    JEFE_INMEDIATO = reader.IsDBNull(35) ? string.Empty : reader.GetString(35),
                    PLAZA = reader.IsDBNull(36) ? string.Empty : reader.GetString(36),
                    CODIGO_JEFE = reader.IsDBNull(37) ? 0 : reader.GetInt32(37),
                });
            }
            return data;
        }

        // RETORNA VACACIONES DISPONIBLES
        public List<Models.eTALENT.VACACIONES_DISPONIBLES> VacacionesUnidades(int external_code)
        {
            var data = new List<Models.eTALENT.VACACIONES_DISPONIBLES>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT * FROM TR_VACACIONES_DISPONIBLES_ISERTEC WHERE COD_JEFE_INMEDIATO = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", external_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.VACACIONES_DISPONIBLES()
                {
                    CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    NOMBRE = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FECHA_INGRESO = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    TOTAL_VACACIONES = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    DIAS_TOMADOS = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    CODIGO_EXTERNO = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CODIGO_J = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    DIAS_PENDIENTES = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                });
            }
            conn.Close();
            return data;
        }

        // RETORNA VACACIONES DISPONIBLES
        public List<Models.eTALENT.VACACIONES_DISPONIBLES> VacacionesAll()
        {
            var data = new List<Models.eTALENT.VACACIONES_DISPONIBLES>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT * FROM TR_VACACIONES_DISPONIBLES_ISERTEC;";
            cmd = new SqlCommand(commandText, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.VACACIONES_DISPONIBLES()
                {
                    CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    NOMBRE = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FECHA_INGRESO = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    TOTAL_VACACIONES = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    DIAS_TOMADOS = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    CODIGO_EXTERNO = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CODIGO_J = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    DIAS_PENDIENTES = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                });
            }
            conn.Close();
            return data;
        }


        // RETORNA VACACIONES DISPONIBLES
        public List<Models.eTALENT.VACACIONES_DISPONIBLES> VacacionesDia(int external_code)
        {
            var data = new List<Models.eTALENT.VACACIONES_DISPONIBLES>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT * FROM TR_VACACIONES_DISPONIBLES_ISERTEC WHERE EPDO_CODIGO_EXTERNO = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", external_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.VACACIONES_DISPONIBLES()
                {
                    CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    NOMBRE = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FECHA_INGRESO = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    TOTAL_VACACIONES = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    DIAS_TOMADOS = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    CODIGO_EXTERNO = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CODIGO_J = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                    DIAS_PENDIENTES = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                });
            }
            return data;
        }

        public List<Models.eTALENT.HISTORICO_VACACIONES> HistoricoVacaciones(int external_code)
        {
            var data = new List<Models.eTALENT.HISTORICO_VACACIONES>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT * FROM TR_VACACIONES_DISPONIBLES_ISERTEC WHERE EPDO_CODIGO_EXTERNO = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", external_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.HISTORICO_VACACIONES()
                {
                    CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    NOMBRE_COMPLETO = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                });
            }
            return data;
        }

        public List<Models.eTALENT.HISTORICO_VACACIONES> HistoricoEmpleado(int external_code)
        {
            var data = new List<Models.eTALENT.HISTORICO_VACACIONES>();
            SqlConnection conn = new SqlConnection();
            conn = eTalentConnection.connectionResult();
            SqlCommand cmd;
            SqlDataReader reader;
            string commandText = "SELECT * FROM TR_VACACIONES_EMPLEADOS_ISERTEC WHERE EPDO_CODIGO_EXTERNO = @CODE;";
            cmd = new SqlCommand(commandText, conn);
            cmd.Parameters.AddWithValue("@CODE", external_code);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.eTALENT.HISTORICO_VACACIONES()
                {
                    CODIGO = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    NOMBRE_COMPLETO = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FECHA_INGRESO = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    PERIODO = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DIAS = reader.IsDBNull(4) ? 0: reader.GetFloat(4),
                    GOZADOS = reader.IsDBNull(5) ? 0 : reader.GetFloat(5),
                    DVCON_DIAS = reader.IsDBNull(6) ? 0 : reader.GetFloat(6),
                    DESDE = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy"),
                    HASTA = reader.IsDBNull(8) ? string.Empty : reader.GetDateTime(8).ToString("dd/MM/yyyy"),
                    CODIGO_EXTERNO = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
                });
            }
            return data;
        }

    }
}