using Newtonsoft.Json;
using purchaseTracking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace purchaseTracking.Controllers
{
    public class LoginController : Controller
    {
        [Authorize]
        public ActionResult logoutSession()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();// Cierra sesión de FormsAuthenticationTicket
            return RedirectToAction("Login");
        }
        public async Task<ActionResult> Login(SiteUser attemp, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    HttpWebResponse LoginResponse = null;
                    LoginResponse = await loginSL(attemp);

                    if (LoginResponse != null)
                    {
                        LoginUserData credentials = new LoginUserData();
                        credentials.UserName = attemp.UserName;
                        credentials.Password = attemp.Password;
                        // obtiene varialbles de nombre de usuario y numero de tecnico para guardar en authorize
                        List<UserNameData> tecnico = new List<UserNameData>();
                        tecnico = await new ServiceLayer.getUserName().Obtener(attemp.UserName, LoginResponse);
                        string fullName = tecnico[0].UserName;
                        string internalKey = tecnico[0].InternalKey;
                        string eMail = tecnico[0].eMail;
                        // Se inicializa el MiddleWare para guardar los datos de los usuarios
                        Session.Add("nombre", fullName);
                        Session.Add("code", internalKey);
                        Session.Add("schema", "SBO_ISERTEC_GT");
                        Session.Add("user", attemp.UserName);
                        Session.Add("eMail", eMail);
                        List<Models.UserData.OHEM> data_sap = new Connection.UserData.UserData().GetOHEMs(Convert.ToInt32(internalKey));
                        if (data_sap.Count() > 0)
                        {
                            List<Models.UserData.UserData> data_etalent = new Connection.UserData.UserData().UserDatas(data_sap[0].empID);
                            if (data_etalent.Count() > 0)
                            {
                                Session.Add("Profession", data_etalent[0].EDPO_EMPL_PROFESION);
                                Session.Add("external_code", data_sap[0].empID);
                                Session.Add("internal_code", data_etalent[0].EPDO_CODIGO);
                                Session.Timeout = 640;  // Tiempo que ASP.NET guardara la cookie de sesión del Usuario, tambien de debe manejar desde IIS o Apache Tomcat
                                DateTime expire = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(Convert.ToInt32(internalKey), fullName, DateTime.Now, expire, false, attemp.UserName); // se crea un nuevo ticket de Sesión para el usuario
                                string hashTicket = FormsAuthentication.Encrypt(ticket); // Encriptación del ticket para no ser expuesta en el navegador
                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
                                cookie.HttpOnly = true;
                                HttpContext.Response.Cookies.Add(cookie);
                                await LogoutSL(LoginResponse);
                                if (!string.IsNullOrEmpty(returnUrl))
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("page", "Home");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "El usuario no existe en eTALENT!");
                                return View();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "El usuario no existe en eTALENT!");
                            return View();
                        }
                    }
                    else
                    {
                        return View(attemp);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña  Incorrecto!.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Pass", "Usuario o Contraseña Incorrectos!");
                return View();
            }

        }

        public async Task<HttpWebResponse> loginSL(SiteUser attempSession)
        {
            string Server = "https://10.93.110.52:50000/b1s/v1/";
            HttpWebResponse LoginResponse = null;
            ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl) { return true; };
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 2000;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Server + "Login");
            httpWebRequest.ContentType = "application/json;odata=minimalmetadata";
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = new CookieContainer();
            LoginUserData ObjectLogin = new LoginUserData();
            ObjectLogin.CompanyDB = "SBO_ISERTEC_GT";
            ObjectLogin.Password = attempSession.Password;
            ObjectLogin.UserName = attempSession.UserName;
            string Parametros = JsonConvert.SerializeObject(ObjectLogin);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(Parametros);
                streamWriter.Flush();
                streamWriter.Close();
            }
            LoginResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            if (LoginResponse != null)
            {
                return LoginResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<HttpWebResponse> LogoutSL(HttpWebResponse session)
        {
            #region Sesion
            string Server = "https://10.93.110.52:50000/b1s/v1/";
            HttpWebResponse LoginResponse;
            Uri UrlConecction = new Uri(Server);
            //ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Server + "/Logout");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = new CookieContainer();
            // Se recorren las cookies de ASP.NET para guardar especificamente la cookie de sesion de SAP B1, las cuales tienen por nombre "B1SESSION"
            foreach (Cookie cookieValue in session.Cookies)
            {
                Cookie cookie = new Cookie();
                if (cookieValue.Name.Equals("B1SESSION"))
                {
                    cookie.Name = cookieValue.Name;
                    cookie.Value = cookieValue.Value;
                    cookie.Path = "/b1s/v1";
                    cookie.Domain = UrlConecction.Host;
                    httpWebRequest.CookieContainer.Add(cookie); // Se agrega las cookies de SAP a el WebRequest 
                }
                else
                {
                    cookie.Name = cookieValue.Name;
                    cookie.Value = cookieValue.Value;
                    cookie.Path = "/b1s";
                    cookie.Domain = UrlConecction.Host;
                    httpWebRequest.CookieContainer.Add(cookie); // Se agrega las cookies de SAP a el WebRequest 
                }
            }
            LoginResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            return LoginResponse;

            //return LoginResponse;
            #endregion
        }
        #region Certificado
        private static bool RemoteSSLTLSCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl)
        {
            return true;
        }
        #endregion
    }
}