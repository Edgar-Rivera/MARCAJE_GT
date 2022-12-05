using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using PagedList;
using purchaseTracking.Connection.CuentaPorPagar;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class DebsController : Controller
    {
        // GET: Debs
        [HttpGet]
        public ActionResult ToPay(int? page, string findString, string filterString)
        {
            List<Models.CuentaPorPagar.CuentaPorPagar> data;
            if (!string.IsNullOrEmpty(filterString))
            {
                data = new DataCuentaPorPagar().getListFiter(filterString);
            } else
            {
                data = new DataCuentaPorPagar().getList();
            }     
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.TipoDocumento.ToString().Contains(findString) || s.origen.ToString().Contains(findString) || s.NombreSN.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.TipoProveedor.ToString().Contains(findString) || s.CodigoSN.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.numeroRef.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.DocNum.ToString().Contains(findString)
                || s.OC.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.origen.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));

        }


        public FileResult ExportDebs(string findString)
        {
            List<Models.CuentaPorPagar.CuentaPorPagar> data= new DataCuentaPorPagar().getListReport();
            DataTable dt = new DataTable("CuentaPorPagar");
            dt.Columns.AddRange(new DataColumn[12] {
                                            new DataColumn("Tipo de Documento"),                                                                               
                                            new DataColumn("Nombre Socio de Negocios"),
                                            new DataColumn("Fecha Emisión"),
                                            new DataColumn("Fecha Vencimiento"),
                                            new DataColumn("Tipo de Proveedor"),
                                            new DataColumn("Días Vencidos"),
                                            new DataColumn("Orden de Compra"),
                                            new DataColumn("Total USD"),
                                            new DataColumn("Saldo USD"),
                                             new DataColumn("Número Factura"),
                                            new DataColumn("Estatus"),
                                            new DataColumn("Origen Datos")

            });           
            foreach (var item in data)
            {
               dt.Rows.Add(item.TipoDocumento,item.NombreSN, item.FechaDocumento.ToString("dd/MM/yyyy"), item.FechaVencimiento.ToString("dd/MM/yyyy"), item.TipoProveedor, item.dias, item.OC, string.Format("{0:#,##0.#0}", item.totalUSD), string.Format("{0:#,##0.#0}", item.saldoUSD), item.DocNum,  item.Estatus, item.origen);
            }          
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CuentaPorPagar" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }


        public ActionResult ToPayBusinessPartners(int? page, string findString, string filterString)
        {
            List<Models.CuentaPorPagar.CuentaPorPagarG> data;
            if (!string.IsNullOrEmpty(filterString) && filterString != "0" && filterString != "Todos")
            {
                data = new DataCuentaPorPagar().getListFilterGroup(filterString);
            }
            else
            {
                data = new DataCuentaPorPagar().getListGroup();
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString) && findString != "")
            {
                var obj = data.Where( s => s.CardCode.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||  s.CardName.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        /* DETALLE DE FACTURAS POR SOCIO DE NEGOCIOS */
        public ActionResult BusinessPartnersDetails (string CardCode, string CardName, string filterString)
        {
            List<Models.CuentaPorPagar.CuentaPorPagar> data;
            ViewBag.CardName = CardName;
            ViewBag.CardCode = CardCode;
            ViewBag.filterString = filterString;
            if(!string.IsNullOrEmpty(filterString) && filterString != "0" && filterString != "Todos")
            {
                data = new DataCuentaPorPagar().getListBusinessPartnerFilter(CardCode,filterString);
            } else
            {
                data = new DataCuentaPorPagar().getListBusinessPartner(CardCode);
            }           
            ViewBag.totalItem = data.Count();
            return View(data);
        }

        /* METODOS PARA LA CREACION DE LOS FORMATOS EN EXCEL */
        public FileResult ExportExcelRequest()
        {
            List<Models.CuentaPorPagar.CuentaPorPagarG> data;
            data = new DataCuentaPorPagar().getListGroup();
            DataTable dt = new DataTable("Cuentas_por_Pagar");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Código Proveedor"),
                                            new DataColumn("Nombre Proveedor"),
                                            new DataColumn("Tipo Proveedor"),
                                            new DataColumn("Saldo Acumulado QTZ"),
                                            new DataColumn("Saldo Acumulado USD")

            });          
             foreach (var item in data)
             {
                dt.Rows.Add(item.CardCode, item.CardName, item.Tipo_Proveedor, item.Saldo_QTZ, item.Saldo_USD);
             }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Cuentas_Por_Pagar" + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }

        public FileResult ExportExcelRequestDetails(string CardCode, string CardName)
        {
            List<Models.CuentaPorPagar.CuentaPorPagar> data;
            data = new DataCuentaPorPagar().getListBusinessPartner(CardCode);
            DataTable dt = new DataTable("Cuentas_por_Pagar");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Nombre Proveedor"),
                                            new DataColumn("Fecha Vencimiento"),
                                            new DataColumn("Orden de Compra"),
                                            new DataColumn("Saldo Acumulado QTZ"),
                                            new DataColumn("Número Factura"),
                                            new DataColumn("Estatus"),
                                            new DataColumn("Estatus Pago")

            });

            foreach (var item in data)
            {
                dt.Rows.Add(item.NombreSN, item.FechaVencimiento.ToString("dd/MM/yyyy"), item.saldoQTZ, item.saldoUSD, item.DocNum, item.Estatus, item.Estatus_Pago);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Cuentas_Por_Pagar" + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }
    }
}