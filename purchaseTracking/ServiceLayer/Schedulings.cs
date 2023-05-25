using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using purchaseTracking.Connection.Activities;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using purchaseTracking.Services;
using System.Drawing;
using System.IO.Compression;

namespace purchaseTracking.ServiceLayer
{
    public class Schedulings
    {
        CookieCollection Cookie;
        Conexion conexion;
        // PARAMETRIZACION DE INGRESO DE FIRMA DIGITAL DE TECNICO EN TURNO
        public bool AddSign(Models.SignDigitalTechnician.SIGN_DIGITAL_OT sign_digital)
        {
            conexion = new ServiceLayer.Conexion();
            string server = "";
            try
            {
                HttpWebResponse CreateResponse = null;
                if (sign_digital.Equals(null))
                {
                    return false;
                }
                else
                {
                    Cookie = new CookieCollection();
                    ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
                    string Objecto = JsonConvert.SerializeObject(sign_digital);
                    JObject jObject = JObject.Parse(Objecto);
                    HttpWebResponse session = conexion.SesionLogin();
                    server = string.Empty;
                    server = session.ResponseUri.Authority;
                    server = "https://" + server + "/b1s/v1/";
                    Uri URLSap = new Uri(server + "SIGN_DIGITAL_OT");
                    ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(URLSap);
                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.CookieContainer = new CookieContainer();
                    httpWebRequest.ServicePoint.Expect100Continue = false;
                    httpWebRequest.Timeout = 600000;
                    foreach (Cookie cookieValue in session.Cookies)
                    {
                        Cookie cookie = new Cookie();
                        if (cookieValue.Name.Equals("B1SESSION"))
                        {
                            cookie.Name = cookieValue.Name;
                            cookie.Value = cookieValue.Value;
                            cookie.Path = "/b1s/v1";
                            cookie.Domain = URLSap.Host;
                            httpWebRequest.CookieContainer.Add(cookie);
                        }
                        else
                        {
                            cookie.Name = cookieValue.Name;
                            cookie.Value = cookieValue.Value;
                            cookie.Path = "/b1s";
                            cookie.Domain = URLSap.Host;
                            httpWebRequest.CookieContainer.Add(cookie);
                        }
                        Cookie.Add(cookie);
                    }
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(Objecto);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    string actual = string.Empty;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                    CreateResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(CreateResponse.GetResponseStream()))
                    {
                        actual = streamReader.ReadToEnd();
                    }
                    conexion.SesionLogout(session.Cookies, server);
                    return true;

                }

            }
            catch (WebException e)
            {
                string text;
                WebResponse response;
                using (response = e.Response)
                {


                    using (Stream data = response.GetResponseStream())
                    {
                        text = new StreamReader(data).ReadToEnd();
                    }
                }

                conexion.SesionLogout(Cookie, server);

                return false;
            }
        }

        private static bool RemoteSSLTLSCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl)
        {
            //accept
            return true;
        }
    }
}