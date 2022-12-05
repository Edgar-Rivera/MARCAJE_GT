using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using purchaseTracking.Models.Invoice;
using PagedList;
using PagedList.Mvc;
using purchaseTracking.Connection.Invoice;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class PaymentReceivedController : Controller
    {

        // GET: PaymentReceived
        public ActionResult PaymentList(int? page, string findString, string filterString, DateTime Inicio, DateTime Fin)
        {
            // GET HEADER INFORMATION         
            List<Models.Bonificaciones.CobrosFecha> data;
            if (!string.IsNullOrEmpty(filterString))
            {
                data = new Connection.Activities.DataActivities().GetCobrosFecha(Inicio, Fin);
            }
            else
            {
                data = new Connection.Activities.DataActivities().GetCobrosFecha(Inicio, Fin);
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            ViewBag.Inicio = Inicio.ToString("dd/MM/yyyy");
            ViewBag.Fin = Fin.ToString("dd/MM/yyyy"); 
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.Vendedor.ToString().Contains(findString) || s.FechaOrden.ToString().Contains(findString) || s.Cliente.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.Medio.ToString().Contains(findString) || Convert.ToString(s.OV).IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult PaymentIntegration(int? page, string findString, string filterString, DateTime Inicio, DateTime Fin)
        {
            // GET HEADER INFORMATION         
            // OBTENER OV PARA DESPLEGAR INFORMACIÓN
            List<Models.Bonificaciones.CobrosFecha> data_ordenes = new Connection.Activities.DataActivities().GetOVFecha(Inicio, Fin);
            List<Models.Bonificaciones.CobrosFecha> integracion_date = new List<Models.Bonificaciones.CobrosFecha>();
            //PROCESO PARA OBTENER EL LISTADO DE CADA UNA DE LAS OV'S
            foreach (var Orden in data_ordenes)
            {            
                if (Orden.OV != "")
                {
                    List<Models.Bonificaciones.CobrosFecha> temp = new Connection.Activities.DataActivities().GetCobrosFechaOV(Convert.ToInt32(Orden.OV));
                    foreach (var elemnto in temp)
                    {
                        integracion_date.Add(elemnto);
                    }
                }              
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = integracion_date.Count();
            ViewBag.Inicio = Inicio.ToString("dd/MM/yyyy");
            ViewBag.Fin = Fin.ToString("dd/MM/yyyy"); ;
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = integracion_date.Where(s => s.Vendedor.ToString().Contains(findString) || s.FechaOrden.ToString().Contains(findString) || s.Cliente.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.Medio.ToString().Contains(findString) || Convert.ToString(s.OV).IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(integracion_date.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult PaymentsDetails(int? page, string findString, string filterString)
        {
            // GET HEADER INFORMATION         
            List<Models.Orders.SalesOrder> data;
            if (!string.IsNullOrEmpty(filterString))
            {
                
                data = new Connection.Activities.DataActivities().GetSalesOrdersFilter(filterString);
            }
            else
            {
                data = new Connection.Activities.DataActivities().GetSalesOrders();
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.SlpName.ToString().Contains(findString) || s.DocDate.ToString().Contains(findString) || s.Comments.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.UnidadComercial.ToString().Contains(findString) || Convert.ToString(s.DocNum).IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult PaymentInvoiceDetails(int SalesOrder)
        {
            // GET HEADER INFORMATION         
            ViewBag.header = new Connection.Activities.DataActivities().GetSalesOrdersFilterSales(SalesOrder);
            List<Models.Bonificaciones.CobrosFecha> temp = new Connection.Activities.DataActivities().GetCobrosFechaOV(SalesOrder);
            ViewBag.totalItem = temp.Count();
            return View(temp);
        }
        /** EXPORTAR DETALLE DE FACTURAS Y PAGOS POR OV **/
        public FileResult ExportExcelPayment(int OV)
        {
            List<Models.Bonificaciones.CobrosFecha> data = new Connection.Activities.DataActivities().GetCobrosFechaOV(OV);
            DataTable dt = new DataTable("Estatus Facturación");
            dt.Columns.AddRange(new DataColumn[15] { new DataColumn("Orde de Venta"),
                                            new DataColumn("Fecha OV"),
                                            new DataColumn("Cliente"),
                                            new DataColumn("Nombre Vendedor"),
                                            new DataColumn("Monto OV"),
                                            new DataColumn("No Factura"),
                                            new DataColumn("DTE"),
                                            new DataColumn("Fecha Factura"),
                                            new DataColumn("Monto Factura"),
                                            new DataColumn("Número de Pago"),
                                            new DataColumn("Número de Recibo"),
                                            new DataColumn("Fecha Pago"),
                                            new DataColumn("Dias Vencimiento"),
                                            new DataColumn("Valor Pago"),
                                            new DataColumn("Medio")

            });

            foreach (var item in data)
            {
                int dias = 0;
                if (item.FechaPago != "" && item.FechaFactura != "")
                {
                    TimeSpan ts = Convert.ToDateTime(item.FechaPago) - Convert.ToDateTime(item.FechaFactura);
                    dias = ts.Days;
                }
                dt.Rows.Add(item.OV, item.FechaOrden, item.Cliente, item.Vendedor, item.ValorOrden, item.No_Factura, item.DTE, item.FechaFactura, item.ValorFactura, item.NumeroPago, item.NumeroRecibo, item.FechaPago, dias, item.ValorCobro, item.Medio);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_" + OV + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }

        public FileResult ExportExcelRequest(DateTime Inicio, DateTime Fin)
        {
            List<Models.Bonificaciones.CobrosFecha> data = new Connection.Activities.DataActivities().GetCobrosFecha(Inicio, Fin);
            DataTable dt = new DataTable("Pagos Recibidos");
            dt.Columns.AddRange(new DataColumn[15] { new DataColumn("Orde de Venta"),
                                            new DataColumn("Fecha OV"),
                                            new DataColumn("Cliente"),
                                            new DataColumn("Nombre Vendedor"),
                                            new DataColumn("Monto OV"),
                                            new DataColumn("No Factura"),
                                            new DataColumn("DTE"),
                                            new DataColumn("Fecha Factura"),
                                            new DataColumn("Monto Factura"),
                                            new DataColumn("Número de Pago"),
                                            new DataColumn("Número de Recibo"),
                                            new DataColumn("Fecha Pago"),
                                            new DataColumn("Dias Vencimiento"),
                                            new DataColumn("Valor Pago"),
                                            new DataColumn("Medio")

            });

            foreach (var item in data)
            {
                int dias = 0;
                if (item.FechaPago != "" && item.FechaFactura != "")
                {
                    TimeSpan ts = Convert.ToDateTime(item.FechaPago) - Convert.ToDateTime(item.FechaFactura);
                    dias = ts.Days;
                }
                dt.Rows.Add(item.OV, item.FechaOrden, item.Cliente, item.Vendedor, item.ValorOrden, item.No_Factura, item.DTE, item.FechaFactura, item.ValorFactura, item.NumeroPago, item.NumeroRecibo,item.FechaPago, dias, item.ValorCobro,item.Medio );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_" + Session["nombre"] + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }

        public FileResult ExportExcelRequestFilter(DateTime Inicio, DateTime Fin)
        {
            List<Models.Bonificaciones.CobrosFecha> data_ordenes = new Connection.Activities.DataActivities().GetOVFecha(Inicio, Fin);
            List<Models.Bonificaciones.CobrosFecha> integracion_date = new List<Models.Bonificaciones.CobrosFecha>();
            //PROCESO PARA OBTENER EL LISTADO DE CADA UNA DE LAS OV'S
            foreach (var Orden in data_ordenes)
            {
                if (Orden.OV != "")
                {
                    List<Models.Bonificaciones.CobrosFecha> temp = new Connection.Activities.DataActivities().GetCobrosFechaOV(Convert.ToInt32(Orden.OV));
                    foreach (var elemnto in temp)
                    {
                        integracion_date.Add(elemnto);
                    }
                }
            }
            DataTable dt = new DataTable("Pagos Recibidos");
            dt.Columns.AddRange(new DataColumn[15] { new DataColumn("Orde de Venta"),
                                            new DataColumn("Fecha OV"),
                                            new DataColumn("Cliente"),
                                            new DataColumn("Nombre Vendedor"),
                                            new DataColumn("Monto OV"),
                                            new DataColumn("No Factura"),
                                            new DataColumn("DTE"),
                                            new DataColumn("Fecha Factura"),
                                            new DataColumn("Monto Factura"),
                                            new DataColumn("Número de Pago"),
                                            new DataColumn("Número de Recibo"),
                                            new DataColumn("Fecha Pago"),
                                            new DataColumn("Días Vencimiento"),
                                            new DataColumn("Valor Pago"),
                                            new DataColumn("Medio")

            });

            foreach (var item in integracion_date)
            {
                int dias = 0;
                if (item.FechaPago != "" && item.FechaFactura != "")
                {
                    TimeSpan ts = Convert.ToDateTime(item.FechaPago) - Convert.ToDateTime(item.FechaFactura);
                    dias = ts.Days;
                }
                dt.Rows.Add(item.OV, item.FechaOrden, item.Cliente, item.Vendedor, item.ValorOrden, item.No_Factura, item.DTE, item.FechaFactura, item.ValorFactura, item.NumeroPago,item.NumeroRecibo ,item.FechaPago, dias, item.ValorCobro, item.Medio);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_" + Session["nombre"] + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }


        /* SE AGREGA VISTA PARA CONTORL DE VISTAS A EA SOLICITADO POR GCATUN */

        [HttpGet]
        public ActionResult PaymentsDetailsUnit(int? page, string findString, string filterString)
        {
            // GET HEADER INFORMATION         
            List<Models.Orders.SalesOrder> data;
            if (!string.IsNullOrEmpty(filterString))
            {

                data = new Connection.Activities.DataActivities().GetSalesOrdersFilterUnit(filterString);
            }
            else
            {
                data = new Connection.Activities.DataActivities().GetSalesOrdersUnit();
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.SlpName.ToString().Contains(findString) || s.DocDate.ToString().Contains(findString) || s.Comments.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.UnidadComercial.ToString().Contains(findString) || Convert.ToString(s.DocNum).IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }


        /* SE AGREGA VISTA PARA CONTORL DE VISTAS A EA SOLICITADO POR NLOPEZ */

        [HttpGet]
        public ActionResult PaymentsDetailsBMA(int? page, string findString, string filterString)
        {
            // GET HEADER INFORMATION         
            List<Models.Orders.SalesOrder> data;
            if (!string.IsNullOrEmpty(filterString))
            {

                data = new Connection.Activities.DataActivities().GetSalesOrdersFilterUnitBMA(filterString);
            }
            else
            {
                data = new Connection.Activities.DataActivities().GetSalesOrdersUnitBMA();
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.SlpName.ToString().Contains(findString) || s.DocDate.ToString().Contains(findString) || s.Comments.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.UnidadComercial.ToString().Contains(findString) || Convert.ToString(s.DocNum).IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }
    }
}