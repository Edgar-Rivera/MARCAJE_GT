using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Connection.Projet
{
    public class DataProject
    {
        public List<Models.Project.FinancialProject> GetProjectsHeader(string PrjCode)
        {
            var data = new List<Models.Project.FinancialProject>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_COSTEO_PROYECTO WHERE \"PrjCode\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = PrjCode;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Project.FinancialProject()
                {
                    TipoProyecto = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    EstatusProyecto = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PrjCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DocNum = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    CardName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    SlpName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Comments = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    FechaAutorizacion = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy")
                });
            }
            conn.Close();
            return data;
        }
        public List<Models.Project.FinancialProject> GetProjects()
        {
            var data = new List<Models.Project.FinancialProject>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_COSTEO_PROYECTO WHERE \"EstatusProyecto\" = 'Abierto' ORDER BY 8 DESC;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Project.FinancialProject()
                {
                    TipoProyecto = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    EstatusProyecto = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PrjCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DocNum = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    CardName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    SlpName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Comments = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    FechaAutorizacion = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy")
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.Project.FinancialProject> GetProjectsAll()
        {
            var data = new List<Models.Project.FinancialProject>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_COSTEO_PROYECTO ORDER BY 8 DESC;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Project.FinancialProject()
                {
                    TipoProyecto = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    EstatusProyecto = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PrjCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DocNum = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    CardName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    SlpName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Comments = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    FechaAutorizacion = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy")
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.Project.FinancialProject> GetProjectsFilter(string filter)
        {
            var data = new List<Models.Project.FinancialProject>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_COSTEO_PROYECTO WHERE \"U_TipoProyecto\" = ? ORDER BY 8 DESC;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Project.FinancialProject()
                {
                    TipoProyecto = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    EstatusProyecto = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    PrjCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    DocNum = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    CardName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    SlpName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    Comments = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    FechaAutorizacion = reader.IsDBNull(7) ? string.Empty : reader.GetDateTime(7).ToString("dd/MM/yyyy")
                });
            }
            conn.Close();
            return data;
        }

        /** ORDENES DE VENTA **/
        public List<Models.Project.SalesOrders> GetSalesOrders(string filter)
        {
            var data = new List<Models.Project.SalesOrders>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("CALL \"TR_SBO_CP_ORDR\"(?);", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filter;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Project.SalesOrders()
                {
                    DocNum = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    FechaOrden = reader.IsDBNull(1) ? string.Empty : reader.GetDateTime(1).ToString("dd/MM/yyyy"),
                    QTZ = reader.IsDBNull(2) ? 0 : reader.GetDouble(2),
                    USD = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                    CANCELED = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    SlpCode = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CardCode = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    CardName = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Project = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    Series = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                    Indicator = reader.IsDBNull(10) ? string.Empty : reader.GetString(10)
                });
            }
            conn.Close();
            return data;
        }
    }
}