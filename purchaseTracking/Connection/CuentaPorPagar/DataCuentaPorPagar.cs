using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Connection.CuentaPorPagar
{
    public class DataCuentaPorPagar
    {
        public List<Models.CuentaPorPagar.CuentaPorPagar> getListReport()
        {
            var data = new List<Models.CuentaPorPagar.CuentaPorPagar>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM TR_CUENTA_POR_PAGAR_RPT;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.CuentaPorPagar.CuentaPorPagar()
                {
                    TipoDocumento = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FechaDocumento = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                    FechaVencimiento = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                    CodigoSN = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    NombreSN = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TipoProveedor = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    dias = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    OC = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    numeroRef = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    moneda = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    totalQTZ = reader.IsDBNull(11) ? 0 : reader.GetDouble(11),
                    totalUSD = reader.IsDBNull(12) ? 0 : reader.GetDouble(12),
                    saldoQTZ = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                    saldoUSD = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                    corriente = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    D0_30 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    D30_60 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                    D60_90 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    D90_120 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                    D_120 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                    Estatus = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                    origen = reader.IsDBNull(23) ? string.Empty : reader.GetString(23)
                });
            }
            conn.Close();
            return data;
        }
        public List<Models.CuentaPorPagar.CuentaPorPagar> getList()
        {
            var data = new List<Models.CuentaPorPagar.CuentaPorPagar>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM CUENTA_POR_PAGAR;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.CuentaPorPagar.CuentaPorPagar()
                {
                    TipoDocumento = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FechaDocumento = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                    FechaVencimiento = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                    CodigoSN = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    NombreSN = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TipoProveedor = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    dias = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    OC = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    numeroRef = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    moneda = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    totalQTZ = reader.IsDBNull(11) ? 0 : reader.GetDouble(11),
                    totalUSD = reader.IsDBNull(12) ? 0 : reader.GetDouble(12),
                    saldoQTZ = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                    saldoUSD = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                    corriente = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    D0_30 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    D30_60 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                    D60_90 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    D90_120 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                    D_120 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                    Estatus = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                    origen = reader.IsDBNull(23) ? string.Empty : reader.GetString(23)
                });
             }   
            conn.Close();
            return data;
        }

        public List<Models.CuentaPorPagar.CuentaPorPagar> getListFiter(string filterString)
        {
            var data = new List<Models.CuentaPorPagar.CuentaPorPagar>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM CUENTA_POR_PAGAR WHERE \"Origen\" = ? ;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filterString;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.CuentaPorPagar.CuentaPorPagar()
                {
                    TipoDocumento = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FechaDocumento = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                    FechaVencimiento = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                    CodigoSN = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    NombreSN = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TipoProveedor = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    dias = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    OC = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    numeroRef = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    moneda = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    totalQTZ = reader.IsDBNull(11) ? 0 : reader.GetDouble(11),
                    totalUSD = reader.IsDBNull(12) ? 0 : reader.GetDouble(12),
                    saldoQTZ = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                    saldoUSD = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                    corriente = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    D0_30 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    D30_60 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                    D60_90 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    D90_120 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                    D_120 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                    Estatus = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                    origen = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                    TT = reader.IsDBNull(24) ? 0 : reader.GetInt32(24),
                    EP = reader.IsDBNull(25) ? string.Empty : reader.GetString(25)
                    
                });
            }
            conn.Close();
            return data;
        }

        /* FILTRO PARA BUSCAR FACTURAS POR DETERMINADO SN */
        public List<Models.CuentaPorPagar.CuentaPorPagar> getListBusinessPartner(string filterString)
        {
            var data = new List<Models.CuentaPorPagar.CuentaPorPagar>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM CUENTA_POR_PAGAR WHERE \"CardCode\" = ? ;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filterString;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.CuentaPorPagar.CuentaPorPagar()
                {
                    TipoDocumento = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FechaDocumento = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                    FechaVencimiento = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                    CodigoSN = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    NombreSN = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TipoProveedor = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    dias = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    OC = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    numeroRef = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    moneda = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    totalQTZ = reader.IsDBNull(11) ? 0 : reader.GetDouble(11),
                    totalUSD = reader.IsDBNull(12) ? 0 : reader.GetDouble(12),
                    saldoQTZ = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                    saldoUSD = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                    corriente = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    D0_30 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    D30_60 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                    D60_90 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    D90_120 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                    D_120 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                    Estatus = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                    Estatus_Pago = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                    origen = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                    TT = reader.IsDBNull(24) ? 0 : reader.GetInt32(24),
                    EP = reader.IsDBNull(25) ? string.Empty : reader.GetString(25)
                });
            }
            conn.Close();
            return data;
        }


        /* FILTRO PARA BUSCAR FACTURAS POR DETERMINADO SN */
        public List<Models.CuentaPorPagar.CuentaPorPagar> getListBusinessPartnerFilter(string CardCodeString, string filterString)
        {
            var data = new List<Models.CuentaPorPagar.CuentaPorPagar>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM CUENTA_POR_PAGAR WHERE \"CardCode\" = ? AND \"Origen\" = ?;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            cmd.Parameters.Add(param);
            param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            cmd.Parameters.Add(param);
            cmd.Parameters[0].Value = CardCodeString;
            cmd.Parameters[1].Value = filterString;
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.CuentaPorPagar.CuentaPorPagar()
                {
                    TipoDocumento = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    FechaDocumento = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                    FechaVencimiento = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                    CodigoSN = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    NombreSN = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    TipoProveedor = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    dias = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                    OC = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    numeroRef = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    moneda = reader.IsDBNull(10) ? "" : reader.GetString(10),
                    totalQTZ = reader.IsDBNull(11) ? 0 : reader.GetDouble(11),
                    totalUSD = reader.IsDBNull(12) ? 0 : reader.GetDouble(12),
                    saldoQTZ = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                    saldoUSD = reader.IsDBNull(14) ? 0 : reader.GetDouble(14),
                    corriente = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                    D0_30 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                    D30_60 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                    D60_90 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                    D90_120 = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                    D_120 = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                    Estatus = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                    Estatus_Pago = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                    origen = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                    TT = reader.IsDBNull(24) ? 0 : reader.GetInt32(24),
                    EP = reader.IsDBNull(25) ? string.Empty : reader.GetString(25)
                });
            }
            conn.Close();
            return data;
        }


        /* CAMBIOS PARA AGRUPACION DE CUENTA POR PAGAR POR SOCIO DE NEGOCIOS
         * CAMBIOS REALIZADOS 28-02-2022 POR EDGAR RIVERA */
        public List<Models.CuentaPorPagar.CuentaPorPagarG> getListGroup()
        {
            var data = new List<Models.CuentaPorPagar.CuentaPorPagarG>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM CUENTA_POR_PAGAR_G;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.CuentaPorPagar.CuentaPorPagarG()
                {
                    CardCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    CardName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Tipo_Proveedor = reader.IsDBNull(2) ? string.Empty: reader.GetString(2),
                    Saldo_QTZ = reader.IsDBNull(3) ? 0: reader.GetDouble(3),
                    Saldo_USD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4)
                });
            }
            conn.Close();
            return data;
        }

        public List<Models.CuentaPorPagar.CuentaPorPagarG> getListFilterGroup(string filterString)
        {
            var data = new List<Models.CuentaPorPagar.CuentaPorPagarG>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM CUENTA_POR_PAGAR_G WHERE \"Tipo Proveedor\" = ? ;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = filterString;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.CuentaPorPagar.CuentaPorPagarG()
                {
                    CardCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    CardName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Tipo_Proveedor = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Saldo_QTZ = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                    Saldo_USD = reader.IsDBNull(4) ? 0 : reader.GetDouble(4)
                });
            }
            conn.Close();
            return data;
        }
    }
}