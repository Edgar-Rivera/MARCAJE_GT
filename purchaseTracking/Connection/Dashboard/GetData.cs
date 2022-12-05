using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace purchaseTracking.Connection.Dashboard
{
    public class GetData
    {
        public List<Models.Dashboard.SolicitudesPorEjecutivo> GetSolicitudesPorEjecutivos()
        {
            var data = new List<Models.Dashboard.SolicitudesPorEjecutivo>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"VISTA_EJECUTIVOS\"", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Dashboard.SolicitudesPorEjecutivo()
                {
                    ejecutivo = reader.GetString(0),
                    cantidad = reader.GetInt32(1)
                });
            }
            conn.Close();
            return data;
        }


        public List<Models.Dashboard.TipoOrdenes> GetTipoOrdenes()
        {
            var data = new List<Models.Dashboard.TipoOrdenes>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"VISTA_ORDENES\"", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Dashboard.TipoOrdenes()
                {
                    tipo = reader.GetString(0),
                    cantidad = reader.GetInt32(1)
                });
            }
            conn.Close();
            return data;
        }
    }
}