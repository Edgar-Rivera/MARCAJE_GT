using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sap.Data.Hana;

namespace purchaseTracking.Connection.Tracking
{
    public class DataTracking
    {
        public List<Models.Tracking.LocalPurchase> getTrackingData(int id)
        {
            var data = new List<Models.Tracking.LocalPurchase>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT \"ClgCode\", \"CntctDate\", \"U_Solicitante\", \"U_NAME\",\"OC\", \"CardName\", \"FechaOC\", \"U_FechaIngresoCD\", \"EM\",\"FechaEM\",\"Bodega\", COUNT(\"Articulo\") \"cantidad\",\"U_FechaCliente\", \"U_FechaProveedor\", \"U_FechaEnvioProveedor\"  " +
                "FROM(" +
                "SELECT A.\"ClgCode\", A.\"CntctDate\", A.\"U_Solicitante\", F.\"U_NAME\", C.\"DocNum\" \"OC\",C.\"CardCode\", C.\"CardName\", C.\"Series\", C.\"DocDate\" \"FechaOC\", B.\"U_FechaIngresoCD\"," +
                " B.\"U_FechaCliente\", B.\"U_FechaProveedor\",B.\"U_FechaEnvioProveedor\",   " +
                " E.\"DocNum\" \"EM\", E.\"DocDate\" \"FechaEM\", G.\"U_NAME\" \"Bodega\" , B.\"ItemCode\" \"Articulo\"   " +
                "FROM OCLG A " +
                "LEFT JOIN OUSR F ON A.\"AttendUser\" = F.\"INTERNAL_K\"" +
                "LEFT JOIN POR1 B ON A.\"ClgCode\" = B.\"U_NumeroActividad\"" +
                "LEFT JOIN OPOR C ON C.\"DocEntry\" = B.\"DocEntry\"  " +
                "LEFT JOIN PDN1 D ON C.\"DocEntry\" = D.\"BaseEntry\" AND B.\"ItemCode\" = D.\"ItemCode\" AND B.\"LineNum\" = D.\"BaseLine\"" +
                "LEFT JOIN OPDN E ON E.\"DocEntry\" = D.\"DocEntry\"" +
                "LEFT JOIN OUSR G ON E.\"UserSign\" = G.\"INTERNAL_K\"" +
                " WHERE A.\"ClgCode\" = ? AND A.\"CntctType\" = 56 )" +
                "GROUP BY \"ClgCode\", \"CntctDate\",\"U_Solicitante\",\"U_NAME\", \"OC\", \"CardName\", \"FechaOC\", \"U_FechaIngresoCD\",\"U_FechaCliente\", \"U_FechaProveedor\" ,\"U_FechaEnvioProveedor\", \"EM\",\"FechaEM\", \"Bodega\"", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = id;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Tracking.LocalPurchase()
                {
                    ClgCode = reader.GetInt32(0),
                    CntctDate = reader.GetDateTime(1),
                    U_Solicitante = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    U_NAME = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    OC = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    CardName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    FechaOC = reader.IsDBNull(6) ? string.Empty : reader.GetDateTime(6).ToString("dd/MM/yyyy"),
                    U_FechaIngresoCD = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy"),
                    EM = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                    FechaEM = reader.IsDBNull(9) ? string.Empty : reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    Bodega = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    cantidad = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                    U_FechaCliente = reader.IsDBNull(12) ? string.Empty : reader.GetDateTime(12).ToString("dd/MM/yyyy"),
                    U_FechaProveedor = reader.IsDBNull(13) ? string.Empty : reader.GetDateTime(13).ToString("dd/MM/yyyy"),
                    U_FechaEnvioProveedor = reader.IsDBNull(14) ? string.Empty : reader.GetDateTime(14).ToString("dd/MM/yyyy")
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.Tracking.InternationalPurchase> getTrackingDataInternational(int id)
        {
            var data = new List<Models.Tracking.InternationalPurchase>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT \"ClgCode\", \"CntctDate\", \"U_Solicitante\", \"U_NAME\",\"OC\", \"CardName\", \"FechaOC\", \"U_FechaDespacho\",\"U_FechaEmbarcador\",\"U_FechaArribo\", \"U_FechaIngresoCD\", \"EM\",\"FechaEM\",\"Bodega\"," +
                "\"PE\", \"FechaPE\", \"Poliza\", \"U_MedioImportacion\", \"U_Estado\", COUNT(\"Articulo\") \"Cantidad\", \"U_commentsDespacho\", \"U_commentsEmbarcador\", \"U_commentsAduanales\",\"U_FechaCliente\", \"U_FechaProveedor\",\"U_FechaEnvioProveedor\" ,   " +
                "DAYS_BETWEEN(TO_DATE(\"U_FechaProveedor\"), TO_DATE(\"U_FechaCliente\")) \"Dias\" " +
                "FROM(" +
                "SELECT A.\"ClgCode\", A.\"CntctDate\", A.\"U_Solicitante\", F.\"U_NAME\", C.\"DocNum\" \"OC\",C.\"CardCode\", C.\"CardName\", C.\"Series\", C.\"DocDate\" \"FechaOC\",B.\"U_FechaDespacho\", B.\"U_FechaEmbarcador\" ," +
                "B.\"U_FechaArribo\", B.\"U_FechaIngresoCD\",B.\"U_FechaCliente\", B.\"U_FechaProveedor\",B.\"U_FechaEnvioProveedor\", " +
                "E.\"DocNum\" \"EM\", E.\"DocDate\" \"FechaEM\", G.\"U_NAME\" \"Bodega\"," +
                "I.\"DocNum\" \"PE\", I.\"DocDate\" \"FechaPE\", I.\"Ref1\" \"Poliza\",  B.\"U_MedioImportacion\", B.\"U_Estado\", B.\"ItemCode\" \"Articulo\", B.\"U_commentsDespacho\", B.\"U_commentsEmbarcador\", B.\"U_commentsAduanales\"    " +
                "FROM OCLG A " +
                "LEFT JOIN OUSR F ON A.\"AttendUser\" = F.\"INTERNAL_K\"" +
                "LEFT JOIN POR1 B ON A.\"ClgCode\" = B.\"U_NumeroActividad\"" +
                "LEFT JOIN OPOR C ON C.\"DocEntry\" = B.\"DocEntry\"  " +
                "LEFT JOIN PDN1 D ON C.\"DocEntry\" = D.\"BaseEntry\" AND B.\"ItemCode\" = D.\"ItemCode\" AND B.\"LineNum\" = D.\"BaseLine\"               " +
                "LEFT JOIN OPDN E ON E.\"DocEntry\" = D.\"DocEntry\"" +
                "LEFT JOIN OUSR G ON E.\"UserSign\" = G.\"INTERNAL_K\"  " +
                "LEFT JOIN IPF1 H ON H.\"BaseEntry\" = D.\"DocEntry\" AND  D.\"ItemCode\" = H.\"ItemCode\" AND D.\"LineNum\" = H.\"OrigLine\"" +
                "LEFT JOIN OIPF I ON I.\"DocEntry\" =  H.\"DocEntry\"                            " +
                "WHERE A.\"ClgCode\" = ? AND A.\"CntctType\" = 1 AND B.\"ItemCode\" NOT IN('FLETES IMPORT','FLETES IMPORT E&C','FLETES IMPORT EA','FLETES IMPORT MAT'))" +
                "GROUP BY \"ClgCode\", \"CntctDate\",\"U_Solicitante\",\"U_NAME\", \"OC\", \"CardName\", \"FechaOC\", \"U_FechaDespacho\",\"U_FechaEmbarcador\",\"U_FechaArribo\", \"U_FechaIngresoCD\",\"U_FechaCliente\", \"U_FechaProveedor\",\"U_FechaEnvioProveedor\", \"EM\",\"FechaEM\",\"Bodega\"," +
                "\"PE\", \"FechaPE\", \"Poliza\", \"U_MedioImportacion\", \"U_Estado\",\"U_commentsDespacho\", \"U_commentsEmbarcador\", \"U_commentsAduanales\"  ORDER BY 8,9,10,11 ASC", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = id;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Tracking.InternationalPurchase()
                {
                    ClgCode = reader.GetInt32(0),
                    CntctDate = reader.GetDateTime(1),
                    U_Solicitante = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    U_NAME = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    OC = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    CardName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    FechaOC = reader.IsDBNull(6) ? string.Empty : reader.GetDateTime(6).ToString("dd/MM/yyyy"),
                    U_FechaDespacho = reader.IsDBNull(7) ? " " : reader.GetDateTime(7).ToString("dd/MM/yyyy"),
                    U_FechaEmbarcador = reader.IsDBNull(8) ? " " : reader.GetDateTime(8).ToString("dd/MM/yyyy"),
                    U_FechaArribo = reader.IsDBNull(9) ? " ": reader.GetDateTime(9).ToString("dd/MM/yyyy"),
                    U_FechaIngresoCD = reader.IsDBNull(10) ? " " : reader.GetDateTime(10).ToString("dd/MM/yyyy"),
                    EM = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                    FechaEM = reader.IsDBNull(12) ? string.Empty : reader.GetDateTime(12).ToString("dd/MM/yyyy"),
                    Bodega = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    PE = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                    FechaPE = reader.IsDBNull(15) ? string.Empty : reader.GetDateTime(15).ToString("dd/MM/yyyy"),
                    Poliza = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    U_MedioImportacion = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                    U_Estado = reader.IsDBNull(18) ? 0 : reader.GetInt32(18),
                    cantidad = reader.IsDBNull(19) ? "0" : ""+reader.GetInt32(19),
                    U_commentsDespacho = reader.IsDBNull(20) ? "" : reader.GetString(20),
                    U_commentsEmbarcador = reader.IsDBNull(21) ? "" : reader.GetString(21),
                    U_commentsAduanales = reader.IsDBNull(22) ? "" : reader.GetString(22),
                    U_FechaCliente = reader.IsDBNull(23) ? string.Empty : reader.GetDateTime(23).ToString("dd/MM/yyyy"),
                    U_FechaProveedor = reader.IsDBNull(24) ? string.Empty : reader.GetDateTime(24).ToString("dd/MM/yyyy"),
                    U_FechaEnvioProveedor = reader.IsDBNull(25) ? string.Empty : reader.GetDateTime(25).ToString("dd/MM/yyyy"),
                    Dias = reader.IsDBNull(26) ? string.Empty : reader.GetString(26)
                });
            }
            conn.Close();
            return data;
        }
        public Models.Tracking.Details getPurchaseDetails(int id)
        {
            var data = new Models.Tracking.Details();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"DocNum\" , A.\"DocDate\", A.\"CardCode\", A.\"CardName\", A.\"DocRate\", A.\"DocTotal\", A.\"VatSum\", A.\"DocTotalSy\", " +
                "A.\"VatSumSy\", A.\"TotalExpSC\", A.\"TotalExpns\"," +
                "B.\"SlpName\", A.\"Comments\", A.\"DocCur\", A.\"Series\"" +
                "FROM OPOR A " +
                "INNER JOIN OSLP B ON A.\"SlpCode\" = B.\"SlpCode\"" +
                "WHERE A.\"DocNum\" = ?", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = id;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.DocNum = reader.IsDBNull(0)? 0 : reader.GetInt32(0);
                data.DocDate = reader.GetDateTime(1);
                data.CardCode = reader.GetString(2);
                data.CardName = reader.GetString(3);
                data.DocRate = reader.GetDouble(4);
                data.DocTotal = reader.GetDouble(5);
                data.VatSum = reader.GetDouble(6);
                data.DocTotalSy = reader.GetDouble(7);
                data.VatSumSy = reader.GetDouble(8);
                data.TotalExpSC = reader.GetDouble(9);
                data.TotalExpns = reader.GetDouble(10);
                data.SlpName = reader.GetString(11);
                data.Comments = reader.IsDBNull(12)? string.Empty : reader.GetString(12);
                data.DocCur = reader.GetString(13);
                data.Series = reader.GetInt32(14);
            }
            conn.Close();
            return data;
        }


        // OBTIENE EL LISTADO DE LOS ITEMS DE SAP DE LA ORDEN DE COMPRA
        public List<Models.Tracking.ItemList> getPurchaseItems(int id)
        {
            var data = new List<Models.Tracking.ItemList>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand(" SELECT A.\"LineNum\"+1 \"LineNum\", A.\"ItemCode\",   A.\"Quantity\", A.\"Project\", A.\"U_IsOV\", A.\"Price\", A.\"DiscPrcnt\", A.\"LineTotal\", " +
                "A.\"TotalSumSy\", A.\"Currency\", B.\"DocNum\", A.\"U_FechaIngresoCD\",A.\"U_LineaOV\", A.\"U_Estado\", A.\"U_FechaDespacho\",  A.\"U_FechaEmbarcador\" ,A.\"U_FechaArribo\", A.\"U_NumeroActividad\", A.\"Dscription\"  ," +
                "C.\"DocEntry\" , D.\"DocDate\" \"FINGRESO\"" +
                "FROM POR1 A " +
                "INNER JOIN OPOR B ON A.\"DocEntry\" = B.\"DocEntry\"" +
                "LEFT JOIN PDN1 C ON C.\"BaseEntry\" = A.\"DocEntry\" AND A.\"ItemCode\" = C.\"ItemCode\" AND A.\"LineNum\" = C.\"BaseLine\" AND A.\"ObjType\" = C.\"BaseType\"" +
                "LEFT JOIN OPDN D ON D.\"DocEntry\" = C.\"DocEntry\"  AND D.\"CANCELED\" = 'N'" +
                "WHERE B.\"DocNum\" = ? ; ", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = id;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Tracking.ItemList()
                {
                    LineNum = reader.GetInt32(0),
                    ItemCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Quantity = reader.IsDBNull(2) ? 0 : reader.GetDouble(2),
                    Project = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    U_IsOV = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    Price = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    DiscPrcnt = reader.IsDBNull(6) ? 0 : reader.GetDouble(6),
                    LineTotal = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                    TotalSumSy = reader.IsDBNull(8) ? 0 : reader.GetDouble(8),
                    Currency = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    U_FechaIngresoCD = reader.IsDBNull(11) ? " " : reader.GetDateTime(11).ToString("dd/MM/yyyy"),
                    U_LineaOV = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    U_Estado = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                    U_FechaDespacho = reader.IsDBNull(14) ? " " : reader.GetDateTime(14).ToString("dd/MM/yyyy"),
                    U_FechaEmbarcador = reader.IsDBNull(15) ? " " : reader.GetDateTime(15).ToString("dd/MM/yyyy"),
                    U_FechaArribo = reader.IsDBNull(16) ? " " : reader.GetDateTime(16).ToString("dd/MM/yyyy"),
                    U_NumeroActividad = reader.IsDBNull(17) ? "" :reader.GetString(17),
                    Dscription = reader.IsDBNull(18) ? "" :reader.GetString(18),
                    DocEntryOPDN = reader.IsDBNull(19) ? "" :reader.GetString(19),
                    FechaIngreso = reader.IsDBNull(20) ? "" : reader.GetDateTime(20).ToString("dd/MM/yyyy")
                });
            }
            conn.Close();
            return data;
        }
    }
}