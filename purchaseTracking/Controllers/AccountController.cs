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
        public ActionResult PersonalInformation()
        {
            List<Models.eTALENT.EPDO_MASTER_DATA> datos = new List<Models.eTALENT.EPDO_MASTER_DATA>();
            datos = new Connection.UserData.UserData().DatosEmpleados(Convert.ToInt32(Session["code"]));
            return View();
        }
    }
}