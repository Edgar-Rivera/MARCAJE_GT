using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using purchaseTracking.Models.Invoice;
using PagedList;
using PagedList.Mvc;
using purchaseTracking.Connection.Invoice;


namespace purchaseTracking.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        // CONTROLADOR PARA PODER OBTENER EL LISTADO DE TODAS LAS ORDENES DE VENTA DE RA //
        [HttpGet]
        public ActionResult BusinessPartnersSalesOrders(int? page, string findString)
        {
            List<Models.Invoice.SalesOrders> data = new Connection.Invoice.SalesOrders().getSalesOrders();
            ViewBag.findString = findString;
            ViewBag.totalItem = data.Count();
            int pageSize = 45;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.SlpName.ToString().Contains(findString) || s.DocDate.ToString().Contains(findString) || s.Comments.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.TipoVenta.ToString().Contains(findString) || Convert.ToString(s.DocNum).IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult InvoiceDetails(int SaleOrder)
        {
            // GET HEADER INFORMATION         
            ViewBag.header = new Connection.Invoice.SalesOrders().getSalesOrdersBP(SaleOrder);
            List<Models.Invoice.Invoice> data = new Connection.Invoice.SalesOrders().getInvoices(SaleOrder);
            ViewBag.totalItem = data.Count();
            return View(data);
        }

        [HttpGet]
        public ActionResult RecurrentList(int? page, string findString, string filterString)
        {
            // GET HEADER INFORMATION         
            List<Models.Invoice.RecurrentInvoice> data;
            if (!string.IsNullOrEmpty(filterString))
            {
                data = new Connection.Invoice.SalesOrders().GetRecurrentUnidad(filterString);
            }
            else
            {
                data = new Connection.Invoice.SalesOrders().GetRecurrent();
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize = 45;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.SlpName.ToString().Contains(findString) || s.DocDate.ToString().Contains(findString) || s.Comments.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.TipoVenta.ToString().Contains(findString) || Convert.ToString(s.DocNum).IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult RecurrentDatails(int SaleOrder)
        {
            // GET HEADER INFORMATION         
            ViewBag.header = new Connection.Invoice.SalesOrders().GetRecurrentBP(SaleOrder);
            List<Models.Invoice.Invoice> data = new Connection.Invoice.SalesOrders().getInvoices(SaleOrder);
            ViewBag.totalItem = data.Count();
            return View(data);
        }
    }
}