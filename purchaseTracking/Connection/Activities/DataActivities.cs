using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sap.Data.Hana;

namespace purchaseTracking.Connection.Activities
{
    public class DataActivities
    {
        // obtiene firma de tecnicos / empleados
        public Models.Images.ImageSign GetSignTechnician(int codigo)
        {
            var data = new Models.Images.ImageSign();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"U_InternalKey\", A.\"U_PathSign\" , B.\"U_NAME\"" +
                " FROM \"@SIGN_DIGITAL_OT\" A" +
                " INNER JOIN OUSR B ON B.\"USERID\" = A.\"U_InternalKey\"" +
                " WHERE A.\"U_InternalKey\" = ?; ", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = codigo;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.U_InternalKey = reader.GetInt32(0);
                data.U_PathSign = reader.GetString(1);
                data.U_Nombre = reader.GetString(2);
            }
            conn.Close();
            return data;
        }
        public List<Models.Activities.HandledUsers> getUsersAssign()
        {
            var data = new List<Models.Activities.HandledUsers>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT \"USERID\", \"U_NAME\" FROM OUSR WHERE \"USERID\" IN(13,70,191)", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            data.Add(new Models.Activities.HandledUsers()
            {
                USERID = 0,
                U_NAME = "Seleccione Valor",
            });
            while (reader.Read())
            {
                data.Add(new Models.Activities.HandledUsers()
                {
                    USERID = reader.GetInt32(0),
                    U_NAME = reader.GetString(1),
                });
            }

            conn.Close();
            return data;
        }

        public List<Models.Activities.List> getListAllInvoice_A(int idUser)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" IN(?) AND A.\"CntctType\" IN (86)   ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = idUser;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = string.Empty,
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }


        public List<Models.Activities.List> getListAllNonStatusInvoice_N()
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"CntctType\" IN (86) AND A.\"Recontact\" >= '20230101'  ORDER BY 2 DESC", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = string.Empty,
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }
        // DATOS DE ETALENT
        public List<Models.Activities.List> getListAllNonStatusInvoice_A(int idUser)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" IN(?)  AND A.\"CntctType\" IN (86) AND A.\"Recontact\" >= '20230101'  ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = idUser;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = string.Empty,
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }
        public Models.Activities.details getDetailsInvoice(int id)
        {
            var data = new Models.Activities.details();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                "A.\"DocNum\", A.\"Details\", A.\"CardCode\",  D.\"CardName\",A.\"status\", A.\"Priority\"," +
                "(CASE WHEN A.\"DocType\" = 17 THEN 'Pedidos de Cliente'" +
                "WHEN A.\"DocType\" = 22 THEN 'Orden de Compra'" +
                "WHEN A.\"DocType\" = 20 THEN 'Entrada de Mercancias'" +
                "WHEN A.\"DocType\" = 21 THEN 'Devolución de Mercancias'" +
                "WHEN A.\"DocType\" = 18 THEN 'Factura de Proveedores' " +
                "WHEN A.\"DocType\" = 69 THEN 'Precio de Entrega'" +
                "END) \"ClaseDocumento\", A.\"DocType\", A.\"Notes\", A.\"Recontact\", A.\"endDate\",A.\"U_Comentarios\", A.\"U_Correo\", B.\"E_Mail\", A.\"U_FechaActualizacion\", A.\"U_DireccionFacturacion\", A.\"U_MontoFacturar\", A.\"U_OrdenCompra\", A.\"U_Entregas\", A.\"U_Contacto\", A.\"U_Observaciones\", A.\"U_TipoEnvio\", A.\"U_Moneda\", A.\"U_Refacturacion\",  A.\"U_Concepto\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "LEFT JOIN OCRD D ON D.\"CardCode\" = A.\"CardCode\"" +
                "WHERE  A.\"ClgCode\" = ?", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = id;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.ClgCode = reader.GetInt32(0);
                data.CntctDate = reader.GetDateTime(1);
                data.Actividad = reader.GetString(2);
                data.Name = reader.GetString(3);
                data.AttendUser = reader.GetString(4);
                data.U_NAME = reader.GetString(5);
                data.U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                data.DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                data.Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                data.CardCode = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                data.CardName = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                data.status = reader.GetInt32(11);
                data.Priority = reader.GetInt32(12);
                data.ClaseDocumento = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                data.DocType = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                data.Notes = reader.IsDBNull(15) ? string.Empty : reader.GetString(15);
                data.Recontact = reader.GetDateTime(16);
                data.endDate = reader.GetDateTime(17);
                data.U_Comentarios = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                data.U_Correo = reader.IsDBNull(19) ? string.Empty : reader.GetString(19);
                data.E_Mail = reader.IsDBNull(20) ? string.Empty : reader.GetString(20);
                data.FechaActualizacion = reader.IsDBNull(21) ? string.Empty : reader.GetString(21);
                data.DireccionFacturacion = reader.IsDBNull(22) ? string.Empty : reader.GetString(22);
                data.MontoFacturar = reader.IsDBNull(23) ? 0 : reader.GetDouble(23);
                data.OC = reader.IsDBNull(24) ? 0 : reader.GetInt32(24);
                data.Entregas = reader.IsDBNull(25) ? string.Empty : reader.GetString(25);
                data.NombreContacto = reader.IsDBNull(26) ? string.Empty : reader.GetString(26);
                data.observaciones = reader.IsDBNull(27) ? string.Empty : reader.GetString(27);
                data.TipoEnvio = reader.IsDBNull(28) ? string.Empty : reader.GetString(28);
                data.Moneda = reader.IsDBNull(29) ? string.Empty : reader.GetString(29);
                data.Refacturacion = reader.IsDBNull(30) ? 0 : reader.GetInt32(30);
                data.U_Concepto = reader.IsDBNull(31) ? "" : reader.GetString(31);

            }
            conn.Close();
            return data;
        }

        // OBTIENE LISTAOD DE ACTIVIDADES DEL SOLICITANTE PARA SOLICITUDES DE ETALENT
        public List<Models.Activities.List> getListRequestInvoice(int idUser)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"U_internalKey\" = ? AND A.\"CntctType\" IN (86)   ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = idUser;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = string.Empty,
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? DateTime.Now.ToString("dd/MM/yyyy") : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? "0" : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                }); ;
            }
            conn.Close();
            return data;
        }
        /* RUTINA PRA SOLICITUDES DE ETALENT */
        public List<Models.Activities.List> getListRequestInvoiceNonEstatus(int idUser)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"U_internalKey\" = ?  AND A.\"CntctType\" IN (86)   ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = idUser;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = string.Empty,
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? DateTime.Now.ToString("dd/MM/yyyy") : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? "0" : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                }); ;
            }
            conn.Close();
            return data;
        }


        /** OBTEIEN LOS DATOS DE LAS OV PARA LUEGO FILTRAR LAS FACTURAS Y PAGOS SIN FILTRO -- SOLICITADO NLOPEZ  **/
        public List<Models.Orders.SalesOrder> GetSalesOrdersUnitBMA()
        {
            var data = new List<Models.Orders.SalesOrder>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_VISTA_SALES_ORDER WHERE \"UNIDAD_C\" = 'BIOMA';", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.SalesOrder()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    TotalUSD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TotalQ = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Cur = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Tipo_Venta = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Cliente = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Rate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    GerenteComercial = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    UnidadComercial = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }
        //** OBTIENE LOS DATOS DE LLAS OV FILTRADOS SOLICITUD NLPEZ * //
        public List<Models.Orders.SalesOrder> GetSalesOrdersFilterUnitBMA(string filter)
        {
            var data = new List<Models.Orders.SalesOrder>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_VISTA_SALES_ORDER WHERE \"UNIDAD_C\" = 'BIOMA' AND \"TIPO_VENTA\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.SalesOrder()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    TotalUSD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TotalQ = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Cur = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Tipo_Venta = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Cliente = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Rate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    GerenteComercial = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    UnidadComercial = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }


        /** OBTEIEN LOS DATOS DE LAS OV PARA LUEGO FILTRAR LAS FACTURAS Y PAGOS SIN FILTRO -- SOLICITADO GCATUN  **/
        public List<Models.Orders.SalesOrder> GetSalesOrdersUnit()
        {
            var data = new List<Models.Orders.SalesOrder>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_VISTA_SALES_ORDER WHERE \"UNIDAD_C\" = 'EA';", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.SalesOrder()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    TotalUSD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TotalQ = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Cur = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Tipo_Venta = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Cliente = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Rate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    GerenteComercial = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    UnidadComercial = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }
        //** OBTIENE LOS DATOS DE LLAS OV FILTRADOS SOLICITUD GCATUN * //
        public List<Models.Orders.SalesOrder> GetSalesOrdersFilterUnit(string filter)
        {
            var data = new List<Models.Orders.SalesOrder>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_VISTA_SALES_ORDER WHERE \"UNIDAD_C\" = 'EA' AND \"TIPO_VENTA\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.SalesOrder()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    TotalUSD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TotalQ = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Cur = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Tipo_Venta = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Cliente = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Rate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    GerenteComercial = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    UnidadComercial = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }

        //** OBTIENE LOS DATOS DE LLAS OV FILTRADOS * //
        public List<Models.Orders.SalesOrder> GetSalesOrdersFilterSales(int filter)
        {
            var data = new List<Models.Orders.SalesOrder>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_VISTA_SALES_ORDER WHERE \"DocNum\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.SalesOrder()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    TotalUSD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TotalQ = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Cur = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Tipo_Venta = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Cliente = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Rate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    GerenteComercial = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    UnidadComercial = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }
            //** OBTIENE LOS DATOS DE LLAS OV FILTRADOS * //
            public List<Models.Orders.SalesOrder> GetSalesOrdersFilter(string filter)
        {
            var data = new List<Models.Orders.SalesOrder>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_VISTA_SALES_ORDER WHERE \"UNIDAD_C\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.SalesOrder()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    TotalUSD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TotalQ = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Cur = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Tipo_Venta = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Cliente = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Rate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    GerenteComercial = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    UnidadComercial = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }
        /** OBTEIEN LOS DATOS DE LAS OV PARA LUEGO FILTRAR LAS FACTURAS Y PAGOS SIN FILTRO **/
        public List<Models.Orders.SalesOrder> GetSalesOrders()
        {
            var data = new List<Models.Orders.SalesOrder>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_VISTA_SALES_ORDER;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.SalesOrder()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    TotalUSD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TotalQ = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Cur = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Tipo_Venta = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Cliente = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Rate = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    GerenteComercial = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    UnidadComercial = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }
        /** OBTEIEN TODOS LOS DATOS DE COBROS POR OV **/
        public List<Models.Bonificaciones.CobrosFecha> GetCobrosFechaOV(int OV)
        {
            var data = new List<Models.Bonificaciones.CobrosFecha>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_BONIFICACIONES_PAGOS WHERE \"OV\" = ? ORDER BY 12 ASC;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = OV;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Bonificaciones.CobrosFecha()
                {
                    Vendedor = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    OV = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FechaOrden = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    No_Factura = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DTE = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    FechaFactura = reader.IsDBNull(5) ? string.Empty : reader.GetDateTime(5).ToString("dd/MM/yyyy"),
                    Cliente = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    ValorOrden = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                    ValorFactura = reader.IsDBNull(8) ? 0 : reader.GetDouble(8),
                    ValorCobro = reader.IsDBNull(9) ? 0 : reader.GetDouble(9),
                    NumeroRecibo = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    NumeroPago = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                    FechaPago = reader.IsDBNull(12) ? string.Empty : reader.GetDateTime(12).ToString("dd/MM/yyyy"),
                    PorcentajeFacturacion = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    Medio = reader.IsDBNull(14) ? string.Empty : reader.GetString(14)
                });
            }
            conn.Close();
            return data;
        }
        /** OBTIENE DISTINCION DE LAS OV QUE SE REGISTRARON EN LOS PAGOS RECIBIDOS **/
        public List<Models.Bonificaciones.CobrosFecha> GetOVFecha(DateTime inicio, DateTime fin)
        {
            var data = new List<Models.Bonificaciones.CobrosFecha>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT DISTINCT \"OV\" FROM TR_BONIFICACIONES_PAGOS WHERE \"Fecha Pago\" BETWEEN ? AND ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Date;
            cmd.Parameters.Add(param);
            param = new HanaParameter();
            param.HanaDbType = HanaDbType.Date;
            cmd.Parameters.Add(param);
            cmd.Parameters[0].Value = inicio;
            cmd.Parameters[1].Value = fin;
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Bonificaciones.CobrosFecha()
                {
                    OV = reader.IsDBNull(0) ? string.Empty : reader.GetString(0)
                });
            }
            conn.Close();
            return data;
        }
        /** OBTIENE DATOS DE LOS PAGOS RECIBIDOS DE ACORDE A FECHAS INGRESADAS **/
        public List<Models.Bonificaciones.CobrosFecha> GetCobrosFecha(DateTime inicio, DateTime fin)
        {
            var data = new List<Models.Bonificaciones.CobrosFecha>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_BONIFICACIONES_PAGOS WHERE \"Fecha Pago\" BETWEEN ? AND ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Date;
            cmd.Parameters.Add(param);
            param = new HanaParameter();
            param.HanaDbType = HanaDbType.Date;
            cmd.Parameters.Add(param);
            cmd.Parameters[0].Value = inicio;
            cmd.Parameters[1].Value = fin;
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Bonificaciones.CobrosFecha()
                {
                    Vendedor = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    OV = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FechaOrden = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    No_Factura = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DTE = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    FechaFactura = reader.IsDBNull(5) ? string.Empty : reader.GetDateTime(5).ToString("dd/MM/yyyy"),
                    Cliente = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    ValorOrden = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                    ValorFactura = reader.IsDBNull(8) ? 0 : reader.GetDouble(8),
                    ValorCobro = reader.IsDBNull(9) ? 0 : reader.GetDouble(9),
                    NumeroRecibo = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    NumeroPago = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                    FechaPago = reader.IsDBNull(12) ? string.Empty : reader.GetDateTime(12).ToString("dd/MM/yyyy"),
                    PorcentajeFacturacion = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    Medio = reader.IsDBNull(14) ? string.Empty : reader.GetString(14)
                });
            }
            conn.Close();
            return data;
        }
        // para solicitante
        public List<Models.Activities.List> getListExcel(int filter)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"U_internalKey\" = ? AND A.\"status\" <> -3 AND A.\"CntctType\" IN (56,1)   ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }
        /* PRUEBA CONCATENACION DATOS DE ORDENES DE COMPRA*/
        public List<Models.Activities.List> getList(string filter)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" IN(13,70) AND A.\"status\" <> -3 AND A.\"CntctType\" IN (56,1,64) AND C.\"Name\" = ?  ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.Activities.List> getListNonStatus(string filter)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" IN(13,70) AND A.\"CntctType\" IN (56,1,64) AND C.\"Name\" = ? AND A.\"Recontact\" >= '20210101' ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.Activities.List> getListAll()
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" IN(13,70) AND A.\"status\" <> -3 AND A.\"CntctType\" IN (56,1,64)   ORDER BY 2 DESC", conn);
 
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }
        public List<Models.Activities.List> getListAllNonStatus()
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'  " +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" IN(13,70)  AND A.\"CntctType\" IN (56,1,64) AND A.\"Recontact\" >= '20210101'  ORDER BY 2 DESC", conn);

            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }
        // OBTIENE ESTATUS DE LLAMADAS ABIERTAS Y CERRADAS -- CAMBIOS ALOPEZ
        public List<Models.Activities.List> GetListsExcel(int idUser)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" = ?  AND A.\"CntctType\" IN (56,1) AND A.\"Recontact\" >= '20210101'   ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = idUser;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }
        // OBTIENE ESTATUS DE LLAMADAS ABIERTAS Y CERRADAS -- CAMBIOS ALOPEZ
        public List<Models.Activities.List> GetLists(int idUser)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"AttendUser\" = ?  AND A.\"CntctType\" IN (56,1) AND A.\"Recontact\" >= '20210101'   ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = idUser;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? "" : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }
        // OBTIENE LISTAOD DE ACTIVIDADES DEL SOLICITANTE
        public List<Models.Activities.List> getListRequest(int idUser)
        {
            var data = new List<Models.Activities.List>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                " A.\"DocNum\", A.\"Details\", A.\"Recontact\", A.\"endDate\" ,  A.\"U_retrasoDias\", A.\"status\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "WHERE A.\"U_internalKey\" = ? AND A.\"status\" <> -3 AND A.\"CntctType\" IN (56,1)   ORDER BY 2 DESC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = idUser;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.List()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    ODC = getODC(temp),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    endDate = reader.IsDBNull(10) ? DateTime.Now.ToString("dd/MM/yyyy") : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    U_retrasoDias = reader.IsDBNull(11) ? "0" : reader.GetString(11),
                    status = reader.IsDBNull(12) ? "0" : reader.GetString(12),
                    Estado = getStatus(Convert.ToInt32(reader.GetString(12)))
                });
            }
            conn.Close();
            return data;
        }

        /* METODO QUE RETORNA DATOS DE ODC */
        public string getODC(int actividad)
        {
            string data = string.Empty;
            int i = 0;
            string temp = string.Empty;
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT DISTINCT A.\"DocNum\" FROM OPOR A" +
                " INNER JOIN POR1 B ON A.\"DocEntry\" = B.\"DocEntry\"" +
                " WHERE \"U_NumeroActividad\" = ?; ", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = actividad;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data = reader.GetString(0);
                if(i == 0)
                {
                    temp = "GT-" + data;
                }else
                {
                    temp = temp + ", " + string.Concat("GT-", data) ;
                }
                i++;
            }

            conn.Close();
            return temp;
        }

        public string getProveedores(int actividad)
        {
            string data = string.Empty;
            int i = 0;
            string temp = string.Empty;
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT DISTINCT A.\"CardName\" FROM OPOR A" +
                " INNER JOIN POR1 B ON A.\"DocEntry\" = B.\"DocEntry\"" +
                " WHERE \"U_NumeroActividad\" = ?; ", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = actividad;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data = reader.GetString(0);
                if (i == 0)
                {
                    temp =  data;
                }
                else
                {
                    temp = temp + ", " +  data;
                }
                i++;
            }

            conn.Close();
            return temp;
        }

        public Models.Activities.details getDetails(int id)
        {
            var data = new Models.Activities.details();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                "A.\"DocNum\", A.\"Details\", A.\"CardCode\",  D.\"CardName\",A.\"status\", A.\"Priority\"," +
                "(CASE WHEN A.\"DocType\" = 17 THEN 'Pedidos de Cliente'" +
                "WHEN A.\"DocType\" = 22 THEN 'Orden de Compra'" +
                "WHEN A.\"DocType\" = 20 THEN 'Entrada de Mercancias'" +
                "WHEN A.\"DocType\" = 21 THEN 'Devolución de Mercancias'" +
                "WHEN A.\"DocType\" = 18 THEN 'Factura de Proveedores' " +
                "WHEN A.\"DocType\" = 69 THEN 'Precio de Entrega'" +
                "END) \"ClaseDocumento\", A.\"DocType\", A.\"Notes\", A.\"Recontact\", A.\"endDate\",A.\"U_Comentarios\", A.\"U_Correo\", B.\"E_Mail\", A.\"U_FechaActualizacion\" " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "LEFT JOIN OCRD D ON D.\"CardCode\" = A.\"CardCode\"" +
                "WHERE  A.\"ClgCode\" = ?", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = id;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.ClgCode = reader.GetInt32(0);
                data.CntctDate = reader.GetDateTime(1);
                data.Actividad = reader.GetString(2);
                data.Name = reader.GetString(3);
                data.AttendUser = reader.GetString(4);
                data.U_NAME = reader.GetString(5);
                data.U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                data.DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                data.Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                data.CardCode = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                data.CardName = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                data.status = reader.GetInt32(11);
                data.Priority = reader.GetInt32(12);
                data.ClaseDocumento = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                data.DocType = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                data.Notes = reader.IsDBNull(15) ? string.Empty : reader.GetString(15);
                data.Recontact = reader.GetDateTime(16);
                data.endDate = reader.GetDateTime(17);
                data.U_Comentarios = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                data.U_Correo = reader.IsDBNull(19) ? string.Empty : reader.GetString(19);
                data.E_Mail = reader.IsDBNull(20) ? string.Empty : reader.GetString(20);
                data.FechaActualizacion = reader.IsDBNull(21) ? string.Empty : reader.GetString(21);
            }
            conn.Close();
            return data;
        }
        public List<Models.Activities.details> getOpenActivities()
        {
            var data = new List<Models.Activities.details>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                "A.\"DocNum\", A.\"Details\", A.\"CardCode\",  D.\"CardName\",A.\"status\", A.\"Priority\"," +
                "(CASE WHEN A.\"DocType\" = 17 THEN 'Pedidos de Cliente'" +
                "WHEN A.\"DocType\" = 22 THEN 'Orden de Compra'" +
                "WHEN A.\"DocType\" = 20 THEN 'Entrada de Mercancias'" +
                "WHEN A.\"DocType\" = 21 THEN 'Devolución de Mercancias'" +
                "WHEN A.\"DocType\" = 18 THEN 'Factura de Proveedores' " +
                "WHEN A.\"DocType\" = 69 THEN 'Precio de Entrega'" +
                "END) \"ClaseDocumento\", A.\"DocType\", A.\"Notes\", A.\"Recontact\", A.\"endDate\",A.\"U_Comentarios\",A.\"U_retrasoDias\"  " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "LEFT JOIN OCRD D ON D.\"CardCode\" = A.\"CardCode\"" +
                "WHERE  A.\"CntctType\" IN (56,1) AND A.\"status\" <> -3", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.details()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    CardCode = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    CardName = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    status = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                    Estado = getStatus(reader.GetInt32(11)),
                    Priority = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                    ClaseDocumento = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    DocType = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                    Notes = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    Recontact = reader.IsDBNull(16) ? DateTime.Now : reader.GetDateTime(16),
                    endDate = reader.IsDBNull(17) ? DateTime.Now : reader.GetDateTime(17),
                    U_Comentarios = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    ODC = getODC(temp),
                    U_retrasoDias = reader.IsDBNull(19) ? "0" : reader.GetString(19),
                    Proveedores = "" /*getProveedores(temp)*/,
                    
                });; 
            }
            conn.Close();
            return data;
        }
        // OBTIENE TIPOS DE ESTADO DE SOLICITUDES
        private string getStatus(int status)
        {
            if(status == -2)
            {
                return "No iniciado";
            } else if (status ==  -3)
            {
                return "Concluido";
            } else if(status ==  1)
            {
                return "En proceso";
            }
            else if (status == 4)
            {
                return "Tarea en espera";
            }
            else
            {
                return "No iniciado";
            }
           
        }
        public List<Models.Activities.details> getCloseActivities()
        {
            var data = new List<Models.Activities.details>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\",A.\"CntctDate\"," +
                "(CASE WHEN  A.\"Action\" = 'C' THEN 'Llamada Teléfonica'" +
                "WHEN  A.\"Action\" = 'M' THEN 'Reunión'	" +
                "WHEN  A.\"Action\" = 'T' THEN 'Tarea'	" +
                "WHEN  A.\"Action\" = 'E' THEN 'Nota'	" +
                "WHEN  A.\"Action\" = 'P' THEN 'Campaña'" +
                "WHEN  A.\"Action\" = 'P' THEN 'Otros'	" +
                "END)\"Actividad\", C.\"Name\", A.\"AttendUser\", B.\"U_NAME\", A.\"U_Solicitante\"," +
                "A.\"DocNum\", A.\"Details\", A.\"CardCode\",  D.\"CardName\",A.\"status\", A.\"Priority\"," +
                "(CASE WHEN A.\"DocType\" = 17 THEN 'Pedidos de Cliente'" +
                "WHEN A.\"DocType\" = 22 THEN 'Orden de Compra'" +
                "WHEN A.\"DocType\" = 20 THEN 'Entrada de Mercancias'" +
                "WHEN A.\"DocType\" = 21 THEN 'Devolución de Mercancias'" +
                "WHEN A.\"DocType\" = 18 THEN 'Factura de Proveedores' " +
                "WHEN A.\"DocType\" = 69 THEN 'Precio de Entrega'" +
                "END) \"ClaseDocumento\", A.\"DocType\", A.\"Notes\", A.\"Recontact\", A.\"endDate\",A.\"U_Comentarios\",A.\"U_retrasoDias\"  " +
                "FROM OCLG A " +
                "INNER JOIN OUSR B ON A.\"AttendUser\" = B.\"INTERNAL_K\"" +
                "INNER JOIN OCLT C ON C.\"Code\" = A.\"CntctType\" " +
                "LEFT JOIN OCRD D ON D.\"CardCode\" = A.\"CardCode\"" +
                "WHERE  A.\"CntctType\" IN (56,1) AND A.\"status\" = -3 AND A.\"CntctDate\" > '20210101'", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int temp = reader.GetInt32(0);
                data.Add(new Models.Activities.details()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    CntctDate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1),
                    Actividad = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    AttendUser = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_NAME = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    U_Solicitante = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    DocNum = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    Details = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    CardCode = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    CardName = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    status = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                    Estado = getStatus(reader.GetInt32(11)),
                    Priority = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                    ClaseDocumento = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    DocType = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                    Notes = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    Recontact = reader.IsDBNull(16) ? DateTime.Now : reader.GetDateTime(16),
                    endDate = reader.IsDBNull(17) ? DateTime.Now : reader.GetDateTime(17),
                    U_Comentarios = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    ODC = getODC(temp),
                    U_retrasoDias = reader.IsDBNull(19) ? "0" : reader.GetString(19),
                });
            }
            conn.Close();
            return data;
        }

        // OBTIENE NOMBRE DE 
        public List<Models.Activities.HandledUsers> getUsers()
        {
            var data = new List<Models.Activities.HandledUsers>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT*  FROM TR_JEFE_ETALENT ORDER BY 2 ASC;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            data.Add(new Models.Activities.HandledUsers()
            {
                USERID = 0,
                U_NAME = "Seleccione Valor",
            });
            while (reader.Read())
            {
                data.Add(new Models.Activities.HandledUsers()
                {
                    USERID = reader.IsDBNull(0)? 0 : reader.GetInt32(0),
                    U_NAME = reader.IsDBNull(1)? string.Empty : reader.GetString(1),
                });
            }

            conn.Close();
            return data;
        }
        public string getID()
        {
            string temp = string.Empty;
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT \"ClgCode\" FROM OCLG ORDER BY 1 DESC LIMIT 1;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                temp = "" + reader.GetInt32(0);
            }
            conn.Close();
            return temp;
        }

        public Models.Activities.Ejecutivo GetEjecutivo(int id )
        {
            var data = new Models.Activities.Ejecutivo();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"U_NAME\", A.\"E_Mail\" FROM OUSR A WHERE A.\"USERID\" = ?", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = id;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.nombre = reader.IsDBNull(0)? string.Empty : reader.GetString(0);
                data.correo = reader.IsDBNull(1) ? "notificaciones@isertec.com" : reader.GetString(1);
            }
            conn.Close();
            return data;
        }
        public List<Models.Activities.OrdenesCompra> GetOrdenes(int actividad)
        {
            var data = new List<Models.Activities.OrdenesCompra>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT DISTINCT A.\"DocNum\" FROM OPOR A" +
                " INNER JOIN POR1 B ON A.\"DocEntry\" = B.\"DocEntry\"" +
                " WHERE \"U_NumeroActividad\" = ?; ", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = actividad;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Activities.OrdenesCompra()
                {
                    DocNum = reader.IsDBNull(0)? "" : ""+reader.GetInt32(0)
                });
            }

            conn.Close();
            return data;
        }

        public List<Models.Activities.OrderDetails> GetOrderDetails(int actividad)
        {
            var data = new List<Models.Activities.OrderDetails>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"ClgCode\", B.\"DocNum\", B.\"CardCode\", A.\"endDate\", B.\"CardName\", A.\"U_retrasoDias\", A.\"Notes\" , A.\"U_Comentarios\", A.\"U_Correo\" , A.\"Recontact\" ,A.\"U_FechaActualizacion\", A.\"status\"" +
                " FROM OCLG A " +
                "LEFT JOIN ORDR B ON A.\"DocNum\" = B.\"DocNum\"" +
                "WHERE A.\"ClgCode\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = actividad;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Activities.OrderDetails()
                {
                    ClgCode = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    DocNum = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    CardCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    endDate = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("dd/MM/yyyy"),
                    CardName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    U_retrasoDias = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),    
                    U_Comentarios = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),    
                    U_Correo = reader.IsDBNull(8)? string.Empty : reader.GetString(8),
                    Recontact = reader.IsDBNull(9) ? "" : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    FechaActualizacion = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    Status = reader.IsDBNull(11) ? 0 : reader.GetInt32(11)
                }); ;
            }
            conn.Close();
            return data;
        }

        public List<Models.Activities.OrderListPurchase> GetOrderListPurchases(int actividad)
        {
            var data = new List<Models.Activities.OrderListPurchase>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT DISTINCT A.\"DocNum\", A.\"DocDate\", A.\"CardCode\", A.\"CardName\"" +
                " FROM OPOR A " +
                "INNER JOIN POR1 B ON A.\"DocEntry\" = B.\"DocEntry\" " +
                "WHERE B.\"U_NumeroActividad\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = actividad;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Activities.OrderListPurchase()
                {
                    DocNum = reader.IsDBNull(0) ? "" : ""+reader.GetInt32(0),
                    DocDate = reader.IsDBNull(1) ? "" : reader.GetDateTime(1).ToString("dd/MM/yyyy"),
                    CardCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    CardName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                }); ;
            }
            conn.Close();
            return data;
        }
    }
}