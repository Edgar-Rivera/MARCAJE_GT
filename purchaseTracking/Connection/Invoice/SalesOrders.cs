using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sap.Data.Hana;

namespace purchaseTracking.Connection.Invoice
{
    public class SalesOrders
    {
        public List<Models.Invoice.RecurrentInvoice> GetRecurrentUnidad(string unidad)
        {
            var data = new List<Models.Invoice.RecurrentInvoice>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_VISTA_FACTURACION_RECURRENTE\" WHERE \"UNIDAD_C\" = ?", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = unidad;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Invoice.RecurrentInvoice()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DocTotal = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TipoVenta = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CardName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Unidad_Comercial = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Rate = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    DocCur = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    Duracion = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    Periocidad = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }
        public List<Models.Invoice.RecurrentInvoice> GetRecurrent()
        {
            var data = new List<Models.Invoice.RecurrentInvoice>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_VISTA_FACTURACION_RECURRENTE\";", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Invoice.RecurrentInvoice()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DocTotal = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TipoVenta = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CardName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Unidad_Comercial = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Rate = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    DocCur = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    Duracion = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    Periocidad = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.Invoice.RecurrentInvoice> GetRecurrentBP(int SaleOrder)
        {
            var data = new List<Models.Invoice.RecurrentInvoice>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_VISTA_FACTURACION_RECURRENTE\" WHERE \"DocNum\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = SaleOrder;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Invoice.RecurrentInvoice()
                {
                    SlpName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DocTotal = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TipoVenta = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CardName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    Unidad_Comercial = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    Rate = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    DocCur = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    Duracion = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    Periocidad = reader.IsDBNull(11) ? string.Empty : reader.GetString(11)
                });
            }
            conn.Close();
            return data;
        }
        public List<Models.Invoice.SalesOrders> getSalesOrders()
        {
            var data = new List<Models.Invoice.SalesOrders>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_VSITA_SOCIOS_FACTURADO\";", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Invoice.SalesOrders()
                {
                    SlpName = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DocTotal = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TipoVenta = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CardName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                });
            }
            conn.Close();
            return data;
        }
        public List<Models.Invoice.SalesOrders> getSalesOrdersBP(int SaleOrder)
        {
            var data = new List<Models.Invoice.SalesOrders>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_VSITA_SOCIOS_FACTURADO\" WHERE \"DocNum\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = SaleOrder;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Invoice.SalesOrders()
                {
                    SlpName = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DocDate = reader.IsDBNull(2) ? string.Empty : reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                    Comments = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DocTotal = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    TipoVenta = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    CardName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.Invoice.Invoice> getInvoices(int SaleOrder)
        {
            var data = new List<Models.Invoice.Invoice>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_INVOICE_DETAILS\" WHERE \"OV\" = ? ORDER BY 1 ASC;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            param.Value = SaleOrder;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Invoice.Invoice()
                {
                    FechaFactura = reader.IsDBNull(0) ? "" : reader.GetDateTime(0).ToString("dd/MM/yyyy"),
                    DocNum = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    OrdenVenta = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    FechaOV = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("dd/MM/yyyy"),
                    DTE = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    Cliente = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TotalUSD = reader.IsDBNull(6) ? 0 : reader.GetDouble(6),
                    PagoAplicado = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                    Vendedor = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    TipoFactura = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    EstadoFactura = reader.IsDBNull(10) ? string.Empty : reader.GetString(10)
                });
            }
            conn.Close();
            return data;
        }
    }
}