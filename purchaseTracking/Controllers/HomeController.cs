using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace purchaseTracking.Controllers
{
    [SessionExpireFilter]


    public class HomeController : Controller
    {
        public ActionResult ObtenerHora()
        {
            // Obtener la hora del servidor
            DateTime horaServidor = DateTime.Now;
            string horaServidorStr = horaServidor.ToString("HH:mm:ss");

            return Json(horaServidorStr, JsonRequestBehavior.AllowGet);
        }
        private string TYPE_M(string temp)
        {
            if(temp == "INICIO")
            {
                return "1";
            } else if (temp == "INICIO_TRASLADO")
            {
                return "9";
            }
            else if (temp == "INICIO_COMIDA")
            {
                return "2";
            }
            else if (temp == "FIN_COMIDA")
            {
                return "3";
            }
            else if (temp == "SALIDA")
            {
                return "4";
            }
            else if (temp == "FIN_TRASLADO")
            {
                return "10";
            }
            else
            {
                return "";
            }

        }
        
        [HttpGet]
        public ActionResult page()
        {
            return View();
        }

        [HttpPost]
        public ActionResult page(string ORDR, string CardName, string lt, string lg, string uuid, string type_p, string TYPE_V)
        {
            if (!string.IsNullOrEmpty(Session["nombre"] as string))
            {
                var data = new Connection.UserData.UserData().GetSAPData(Convert.ToInt32(ORDR));
                // METODO PARA REALIZAR MARCAS, SE REALIZA METODO POST DEPENDIENDO DE BOTON
                if (!string.IsNullOrEmpty(lt) && !string.IsNullOrEmpty(lg))
                {
                    if (ORDR != "0")
                    {
                        if (type_p == "INICIO" || type_p == "INICIO_TRASLADO" || type_p == "INICIO_COMIDA" || type_p == "SALIDA" || type_p == "FIN_TRASLADO" || type_p == "FIN_COMIDA")
                        {

                            if (new Connection.UserData.UserData().InsertWEA(Convert.ToInt32(Session["internal_code"]), TYPE_M(type_p), lt, lg, data.Project, uuid, Convert.ToString(Session["external_code"]), ORDR, data.CardCode, "", TYPE_V, data.CardName, data.Comments))
                            {
                                return RedirectToAction("page", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "HA OCURRIDO UN ERROR AL INGRESAR LA MARCA!");
                                return View();

                            }
                        }
                        else
                        {
                            return RedirectToAction("page", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "DEBE DE SELECCIONAR UNA OV VÁLIDA!");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "NO CUENTA CON UNA LOCALIZACIÓN VÁLIDA!");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        [HttpPost]
        public ActionResult page_c(string CardCode, string lt_c, string lg_c, string uuid_c, string TYPE_V, string prospect, string type_p)
        {
            if (!string.IsNullOrEmpty(Session["nombre"] as string))
            {

                var data = new Connection.UserData.UserData().GetSAPDataName(CardCode);
                // METODO PARA REALIZAR MARCAS, SE REALIZA METODO POST DEPENDIENDO DE BOTON
                if (!string.IsNullOrEmpty(lt_c) && !string.IsNullOrEmpty(lg_c))
                {
                    if ((!string.IsNullOrEmpty(CardCode) && CardCode != "0") || (!string.IsNullOrEmpty(prospect)))
                    {
                        if (type_p == "INICIO" || type_p == "INICIO_TRASLADO" || type_p == "INICIO_COMIDA" || type_p == "SALIDA" || type_p == "FIN_TRASLADO" || type_p == "FIN_COMIDA")
                        {
                            if (!string.IsNullOrEmpty(prospect))
                            {
                                if (new Connection.UserData.UserData().InsertWEA_CP(Convert.ToInt32(Session["internal_code"]), TYPE_M(type_p), lt_c, lg_c, "", uuid_c, Convert.ToString(Session["external_code"]), "1", "", prospect, TYPE_V, "", ""))
                                {
                                    return RedirectToAction("page", "Home");
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "HA OCURRIDO UN ERROR AL INGRESAR LA MARCA!.");
                                    return View("page");

                                }
                            }
                            else
                            {
                                if (new Connection.UserData.UserData().InsertWEA_C(Convert.ToInt32(Session["internal_code"]), TYPE_M(type_p), lt_c, lg_c, "", uuid_c, Convert.ToString(Session["external_code"]), "1", CardCode, "", TYPE_V, data.CardName, data.Comments))
                                {
                                    return RedirectToAction("page", "Home");
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "HA OCURRIDO UN ERROR AL INGRESAR LA MARCA!.");
                                    return View("page");

                                }
                            }


                        }
                        else
                        {
                            return RedirectToAction("page", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "DEBE DE SELECCIONAR UN SOCIO DE NEGOCIO VÁLIDO!.");
                        return View("page");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "NO CUENTA CON UNA LOCALIZACIÓN VÁLIDA!.");
                    return View("page");
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public JsonResult GetSolicitudes()
        {
            var data = new List<Models.Dashboard.SolicitudesPorEjecutivo>();
            try
            {

                data = new Connection.Dashboard.GetData().GetSolicitudesPorEjecutivos();

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return Json(new { JSONList = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult OrdenesCompraTipo()
        {
            var data = new List<Models.Dashboard.TipoOrdenes>();
            try
            {

                data = new Connection.Dashboard.GetData().GetTipoOrdenes();

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return Json(new { JSONList = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}