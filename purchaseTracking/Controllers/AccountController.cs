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


    }
}