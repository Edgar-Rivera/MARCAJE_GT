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
        private static string Server = "https://192.168.1.221:50000/b1s/v1/";
        public HttpWebResponse SesionLogin()
        {
            HttpWebResponse LoginResponse = null;
            ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Server + "Login");
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = new CookieContainer();
            Login ObjectLogin = new Login();
            ObjectLogin.CompanyDB = "SBO_ISERTEC_GT";
            ObjectLogin.Password = "Sup2021/.";
            ObjectLogin.UserName = "Support";

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
            ObjectLogin.CompanyDB = "SBO_ISERTEC_GT";
            ObjectLogin.Password = "S3gur!d4d/";
            ObjectLogin.UserName = "applications";

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
            Uri UrlConecction = new Uri(Server);
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