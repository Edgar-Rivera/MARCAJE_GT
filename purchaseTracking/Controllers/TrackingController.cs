using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using purchaseTracking.Connection.Activities;
using purchaseTracking.Connection.Tracking;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class TrackingController : Controller
    {
        // GET: Tracking
  
        [HttpGet]
        public ActionResult PurchaseList(int? page, string findString)
        {
            // CAMBIOS PARA MOSTRAR RESULTADOS DE BUSQUEDAS
            List<Models.Activities.details> data = new DataActivities().getOpenActivities();
            ViewBag.findString = findString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.ClgCode.ToString().Contains(findString) || s.CntctDate.ToString().Contains(findString) || s.Name.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.AttendUser.ToString().Contains(findString) || s.U_NAME.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.U_Solicitante.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.DocNum.ToString().Contains(findString)
                || s.Details.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.ODC.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.CardName.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.CardCode.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0
                || s.Proveedores.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.Recontact.ToString().Contains(findString) || s.Estado.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 );
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult CompletedRequest(int? page, string findString)
        {
            // CAMBIOS PARA MOSTRAR RESULTADOS DE BUSQUEDAS
            List<Models.Activities.details> data = new DataActivities().getCloseActivities();
            ViewBag.findString = findString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.ClgCode.ToString().Contains(findString) || s.CntctDate.ToString().Contains(findString) || s.Name.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.AttendUser.ToString().Contains(findString) || s.U_NAME.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.U_Solicitante.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.DocNum.ToString().Contains(findString)
                || s.Details.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.ODC.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.CardName.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.CardCode.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0
                || s.Recontact.ToString().Contains(findString) || s.Estado.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult purchase(int id)
        {
            ViewBag.activity = id;
            ViewBag.header = new Connection.Activities.DataActivities().GetOrderDetails(id);
            ViewBag.orders = new Connection.Activities.DataActivities().GetOrderListPurchases(id);
            List<Models.Tracking.LocalPurchase> data = new DataTracking().getTrackingData(id);
            if(data.Count<= 0)
            {
                return RedirectToAction("international", "Tracking", new { id = id });
            } else
            {
                return View(data);
            }
            
        }


        [HttpGet]
        public ActionResult international(int id)
        {
            ViewBag.activity = id;
            ViewBag.header = new Connection.Activities.DataActivities().GetOrderDetails(id);
            ViewBag.orders = new Connection.Activities.DataActivities().GetOrderListPurchases(id);
            List<Models.Tracking.InternationalPurchase> data = new DataTracking().getTrackingDataInternational(id);
            return View(data);
        }

        [HttpGet]
        public ActionResult details(int id, string paramDate, string paramActivity, int status)
        {
            Models.Tracking.Details data = new DataTracking().getPurchaseDetails(id);
            ViewBag.items = new DataTracking().getPurchaseItems(id);
            ViewBag.paramDate = paramDate;
            ViewBag.status = status;
            ViewBag.paramActivity = paramActivity; // numero de actividad
            return View(data);
        }

        public ActionResult detailsInternational(int id, string paramDate, string paramDate_1, string paramDate_2, string paramDate_3, string paramActivity, int status)
        {
            Models.Tracking.Details data = new DataTracking().getPurchaseDetails(id);
            ViewBag.items = new DataTracking().getPurchaseItems(id);
            ViewBag.paramDate = paramDate; // despacho
            ViewBag.paramDate_1 = paramDate_1; // embarcador
            ViewBag.paramDate_2 = paramDate_2; // arribo
            ViewBag.paramDate_3 = paramDate_3; // ingreso cd
            ViewBag.status = status;
            ViewBag.paramActivity = paramActivity; // numero de actividad
            return View(data);
        }
    }
}