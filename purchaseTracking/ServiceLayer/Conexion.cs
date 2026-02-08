using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace purchaseTracking.ServiceLayer
{
    public class Conexion
    {
        private static string Server = "https://10.110.240.104:50000/b1s/v1/";
        private static string Schema = "SBO_ISERTEC_GT";
        public HttpWebResponse SesionLogin()
        {
            HttpWebResponse LoginResponse = null;
            ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl) { return true; };
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 2000;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Server + "Login");
            httpWebRequest.ContentType = "application/json;odata=minimalmetadata";
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = new CookieContainer();
            Login ObjectLogin = new Login();
            ObjectLogin.CompanyDB = Schema;
            ObjectLogin.Password = ":s25Zpdp2A";
            ObjectLogin.UserName = "API_SAP_WEB";

            string Parametros = JsonConvert.SerializeObject(ObjectLogin);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(Parametros);
                streamWriter.Flush();
                streamWriter.Close();
            }

            LoginResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            return LoginResponse;

        }
        // SE AGREGA SEGUNDO TIPO DE SESSION 
        public HttpWebResponse SesionInterface()
        {
            HttpWebResponse LoginResponse = null;
            ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Server + "Login");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = new CookieContainer();
            Login ObjectLogin = new Login();
            ObjectLogin.CompanyDB = Schema;
            ObjectLogin.Password = ":s25Zpdp2A";
            ObjectLogin.UserName = "API_SAP_WEB";

            string Parametros = JsonConvert.SerializeObject(ObjectLogin);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(Parametros);
                streamWriter.Flush();
                streamWriter.Close();
            }

            LoginResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            return LoginResponse;

        }
        public HttpWebResponse SesionLogout(CookieCollection Cookies, string Server)
        {
            #region Sesion
            HttpWebResponse LoginResponse = null;
            Uri UrlConecction = new Uri("https://10.110.240.104:50000/b1s/v1/");
            ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Server + "/Logout");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = new CookieContainer();
            foreach (Cookie cookieValue in Cookies)
            {
                Cookie cookie = new Cookie();
                if (cookieValue.Name.Equals("B1SESSION"))
                {
                    cookie.Name = cookieValue.Name;
                    cookie.Value = cookieValue.Value;
                    cookie.Path = "/b1s/v1";
                    cookie.Domain = UrlConecction.Host;
                    httpWebRequest.CookieContainer.Add(cookie);
                }
                else
                {
                    cookie.Name = cookieValue.Name;
                    cookie.Value = cookieValue.Value;
                    cookie.Path = "/b1s";
                    cookie.Domain = UrlConecction.Host;
                    httpWebRequest.CookieContainer.Add(cookie);
                }
            }
            LoginResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            return LoginResponse;
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