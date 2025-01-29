using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using purchaseTracking.Models;
using Sap.Data.Hana;

namespace purchaseTracking.Connection.Orders
{
    public class BusinessSN
    {
        public List<Models.Orders.BusinessPartners> getListNames()
        {
            var data = new List<Models.Orders.BusinessPartners>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_ORDERS_WEA_N\"", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            bool firstRow = true;
            while (reader.Read())
            {
                if (firstRow)
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = "0",
                        CardName = "Seleccione Socio de Negocios",
                    });
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.GetString(0),
                        CardName = reader.GetString(1),
                    });
                    firstRow = false;
                }
                else
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        CardName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    });
                }
            }
            conn.Close();
            return data;
        }


        public int ObtieneNumeroHora()
        {
            string horaActual = DateTime.Now.ToString("HH:mm");
            DateTime dateTime = DateTime.ParseExact(horaActual, "HH:mm", null);
            int horas = dateTime.Hour;
            int minutos = dateTime.Minute;
            int numeroHora = horas * 100 + minutos;
            return numeroHora;
        }



        public List<Models.Orders.BusinessPartners> getListProgramacion(int codigo_usuario)
        {
            var data = new List<Models.Orders.BusinessPartners>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT \"OV\", \"Comments\" FROM TR_VISTA_PROGRAMACION WHERE \"U_InternalID\" = ? AND CURRENT_DATE BETWEEN \"U_FechaAsignacion\" AND \"U_FechaFin\" AND ? BETWEEN \"U_HoraInicio\" AND \"U_HoraFin\";", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            cmd.Parameters.Add(param);

            param = new HanaParameter();
            param.HanaDbType = HanaDbType.Integer;
            cmd.Parameters.Add(param);


            cmd.Parameters[0].Value = codigo_usuario;
            cmd.Parameters[1].Value = ObtieneNumeroHora();
            HanaDataReader reader = cmd.ExecuteReader();
            bool firstRow = true;
            while (reader.Read())
            {
                if (firstRow)
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = "0",
                        CardName = "Seleccione Orden de Venta",
                    });
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        CardName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    });
                    firstRow = false;
                }
                else
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        CardName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    });
                }
            }
            conn.Close();
            return data;
        }


        


            public List<Models.Orders.BusinessPartners> getLista()
        {
            var data = new List<Models.Orders.BusinessPartners>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"W_MARCASACCESO_LOC_PROY\"", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            bool firstRow = true;
            while (reader.Read())
            {
                if (firstRow)
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = "0",
                        CardName = "Seleccione Orden de Venta",
                    });
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.GetString(0),
                        CardName = reader.GetString(1),
                    });
                    firstRow = false;
                }
                else
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        CardName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    });
                }
            }
            conn.Close();
            return data;
        }


        public List<Models.Orders.BusinessPartners> getList()
        {
            var data = new List<Models.Orders.BusinessPartners>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT * FROM \"TR_ORDERS_WEA\"", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            bool firstRow = true;
            while (reader.Read())
            {
                if (firstRow)
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = "0",
                        CardName = "Seleccione Orden de Venta",
                    });
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.GetString(0),
                        CardName = reader.GetString(1),
                    });
                    firstRow = false;
                }
                else
                {
                    data.Add(new Models.Orders.BusinessPartners()
                    {
                        CardCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                        CardName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    });
                }
            }
            conn.Close();
            return data;
        }
        public List<Models.Orders.PedidosCliente> GetPedidosName(string carCode)
        {
            var data = new List<Models.Orders.PedidosCliente>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"CardName\",A.\"CardName\" FROM ORDR A WHERE  A.\"CardCode\" = ? ORDER BY 2 ASC;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = carCode;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                data.Add(new Models.Orders.PedidosCliente()
                {
                    DocEntry = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    DocNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                });


            }
            conn.Close();
            return data;
        }

        public List<Models.Orders.PedidosCliente> GetPedidos(string carCode)
        {
            var data = new List<Models.Orders.PedidosCliente>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT A.\"CardCode\",A.\"CardName\" FROM ORDR A WHERE  A.\"DocNum\" = ? ORDER BY 2 ASC;", conn);
            HanaParameter param = new HanaParameter();
            param.HanaDbType = HanaDbType.NVarChar;
            param.Value = carCode;
            cmd.Parameters.Add(param);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                
                    data.Add(new Models.Orders.PedidosCliente()
                    {
                        DocEntry = reader.IsDBNull(0)? string.Empty : reader.GetString(0),
                        DocNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    });
                
            
            }
            conn.Close();
            return data;
        }

        public List<Models.Orders.Solicitante> GetSolicitantes()
        {
            var data = new List<Models.Orders.Solicitante>();
            HanaConnection conn = new HanaConnection();
            conn = connectionHana.connectionResult();
            HanaCommand cmd = new HanaCommand("SELECT T0.\"SlpName\" FROM OSLP T0;", conn);
            HanaDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new Models.Orders.Solicitante()
                {
                    nombre = reader.GetString(0)
                });
            }

            conn.Close();
            return data;
        }
    }
}