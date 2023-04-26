using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult PersonalInformation()
        {
            List<Models.eTALENT.EPDO_MASTER_DATA> datos = new List<Models.eTALENT.EPDO_MASTER_DATA>();
            datos = new Connection.UserData.UserData().DatosEmpleados(Convert.ToInt32(Session["external_code"]));

            List<Models.eTALENT.VACACIONES_DISPONIBLES> vacaciones = new List<Models.eTALENT.VACACIONES_DISPONIBLES>();
            vacaciones = new Connection.UserData.UserData().VacacionesDia(Convert.ToInt32(Session["external_code"]));

            List<Models.eTALENT.HISTORICO_VACACIONES> historico = new List<Models.eTALENT.HISTORICO_VACACIONES>();
            historico = new Connection.UserData.UserData().HistoricoEmpleado(Convert.ToInt32(Session["external_code"]));

            ViewBag.historico = historico;
            ViewBag.vacaciones = vacaciones;

            return View(datos);
        }
        [HttpGet]
        public ActionResult Documentos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RequestForms()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VacationsRequest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult newRequest(Models.Activities.RequestActivity requestActivity)
        {
            // VARIABLES DEL SERVICE LAYER ESTATICAS
          
            
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
               // SendMailer message = new SendMailer();
               // message.sendMail(data.correo, requestActivity.U_Correo, requestActivity.Details, "", data.nombre, ViewBag.activity, requestActivity.U_Solicitante,
                //    requestActivity.Notes, requestActivity.DocNum, CardName);
                return View("Success");
            }
            else
            {
                return View("Error");
            }

        }

    }
}