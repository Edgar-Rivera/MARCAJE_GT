using purchaseTracking.Models.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using purchaseTracking.Models;
using purchaseTracking.Services;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        // GET: Request
        [HttpGet]
        public ActionResult newRequest()
        {        
            return View();
        }
        [HttpPost]
        public ActionResult newRequest(Models.Activities.RequestActivity requestActivity, string CardName)
        { 
            // VARIABLES DEL SERVICE LAYER ESTATICAS
            requestActivity.StartTime = DateTime.Now.ToString("HH:mm:ss");
            requestActivity.EndTime = DateTime.Now.ToString("HH:mm:ss");
            /** CAMBIOS SOLICITADOS POR DCASTILLO - OPERACIONES - PARA ELIMINAR FECHA NECESARIA POR ANALISTAS **/
            DateTime fechaFinEstandar = DateTime.Now;
            fechaFinEstandar = fechaFinEstandar.AddDays(30);
            requestActivity.EndDueDate = fechaFinEstandar.ToString("yyyy-MM-dd");
            requestActivity.U_retrasoDias = "15";
            requestActivity.DurationType = "du_Seconds";
            requestActivity.U_internalKey = Session["code"].ToString();
            // METODO QUE RECIBE EL MODULO Y HACE EL POST EN SAP   
            if (new ServiceLayer.Activity.ActivityComponents().addActivity(requestActivity))
            {
                // VALIDA LA CREACION DE LA ACTIVIDAD EN SAP Y ENVIA CORREO ELECTORNICO
                var data = new Connection.Activities.DataActivities().GetEjecutivo(requestActivity.HandledBy);
                ViewBag.email_to = data.correo;
                ViewBag.email_bcc = requestActivity.U_Correo;
                ViewBag.activity = new purchaseTracking.Connection.Activities.DataActivities().getID();
                SendMailer message = new SendMailer();
                message.sendMail(data.correo, requestActivity.U_Correo,requestActivity.Details,"",data.nombre,ViewBag.activity, requestActivity.U_Solicitante,
                    requestActivity.Notes, requestActivity.DocNum, CardName);
                return View("Success");
             } 
            else
            {
                return View("Error");
            }

        }
        [HttpGet]
        public ActionResult GetSub(string cardCode)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var data = new purchaseTracking.Connection.Orders.BusinessSN().GetPedidos(cardCode);
            foreach (var item in data)
            {
                items.Add(new SelectListItem() { Text = ""+item.DocNum, Value = ""+item.DocEntry });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSubCard(string cardCode)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var data = new purchaseTracking.Connection.Orders.BusinessSN().GetPedidosName(cardCode);
            foreach (var item in data)
            {
                items.Add(new SelectListItem() { Text = "" + item.DocNum, Value = "" + item.DocEntry });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}