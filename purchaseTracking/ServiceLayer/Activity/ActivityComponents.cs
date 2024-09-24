using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.IO;
using log4net.Repository.Hierarchy;

namespace purchaseTracking.ServiceLayer.Activity
{
    public class ActivityComponents
    {
        CookieCollection Cookie;
        Conexion conexion;
        public bool addActivity(Models.Activities.RequestActivity requestActivity)
        {
            conexion = new ServiceLayer.Conexion();
            string server = "";
            try
            {
                HttpWebResponse CreateResponse = null;
                if (requestActivity.Equals(null))
                {
                    return false;
                }
                else
                {
                    Cookie = new CookieCollection();
                    ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
                    string Objecto = JsonConvert.SerializeObject(requestActivity);
                    JObject jObject = JObject.Parse(Objecto);
                    HttpWebResponse session = conexion.SesionLogin();
                    server = string.Empty;
                    server = session.ResponseUri.Authority;
                    server = "https://" + server + "/b1s/v1/";
                    Uri URLSap = new Uri(server + "Activities");
                    ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(URLSap);
                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.CookieContainer = new CookieContainer();
                    httpWebRequest.ServicePoint.Expect100Continue = false;
                    httpWebRequest.Timeout = 600000;
                    Logger.Log("Nueva actividad: " + Objecto);
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
            } catch(WebException e)
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
        public string ObjectSerialize(object objeto)
        {
            string dummy = string.Empty;
            string jsonData = JsonConvert.SerializeObject(objeto, Newtonsoft.Json.Formatting.Indented,
                              new JsonSerializerSettings
                              {
                                  Formatting = Newtonsoft.Json.Formatting.Indented,
                                  ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                                  NullValueHandling = NullValueHandling.Ignore,
                                  DefaultValueHandling = DefaultValueHandling.Ignore
                              });
            JObject Sapobject = JObject.Parse(jsonData);
            List<string> Nombre = new List<string>();
            foreach (var item in Sapobject)
            {
                if (item.Value.Type == JTokenType.Array)
                {
                    if (!item.Value.HasValues)
                    {
                        Nombre.Add(item.Key);
                    }
                }
            }
            for (int i = 0; i < Nombre.Count; i++)
            {
                Sapobject.Remove(Nombre[i]);
            }
            string jsonData2 = JsonConvert.SerializeObject(Sapobject, Newtonsoft.Json.Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                Formatting = Newtonsoft.Json.Formatting.Indented,
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore,
                                DefaultValueHandling = DefaultValueHandling.Ignore
                            });
            return jsonData2;
        }
        public bool actualizaComentarios(int activity, Models.Activities.Activities modelo)
        {
            conexion = new Conexion();
            string server = "";
            try
            {
                HttpWebResponse CreateResponse = null;

                Cookie = new CookieCollection();
                string Objecto = ObjectSerialize(modelo);
                HttpWebResponse session = conexion.SesionLogin(); 
                server = string.Empty;
                server = session.ResponseUri.Authority;
                server = "https://" + server + "/b1s/v1/";
                Uri URLSap = new Uri(server + "Activities(" + activity + ")");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URLSap);
                httpWebRequest.ContentType = "application/json;odata=minimalmetadata;charset=utf8";
                httpWebRequest.Method = "PATCH";
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.Accept = "application/json;odata=minimalmetadata";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Timeout = 10000000;
                Logger.Log("Actualizacion de estado (Marcaje): " + Objecto);
                foreach (Cookie cookieValue in session.Cookies)
                {
                    Cookie cookie = new Cookie();
                    if (cookieValue.Name.Equals("B1SESSION"))
                    {
                        cookie.Name = cookieValue.Name;
                        cookie.Value = cookieValue.Value;
                        cookie.Path = "/b1s/v1";
                        //  cookie.Path = cookieValue.Path;
                        cookie.Domain = URLSap.Host;
                        httpWebRequest.CookieContainer.Add(cookie);
                    }
                    else
                    {
                        cookie.Name = cookieValue.Name;
                        cookie.Value = cookieValue.Value;
                        cookie.Path = "/b1s";
                        //   cookie.Path = cookieValue.Path;
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
                CreateResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(CreateResponse.GetResponseStream()))
                {
                    actual = streamReader.ReadToEnd();
                }
                conexion.SesionLogout(session.Cookies, server);
                return true;
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

        public bool actualizaNotas(int activity, Models.Orders.Activities modelo)
        {
            conexion = new Conexion();
            string server = "";
            try
            {
                HttpWebResponse CreateResponse = null;

                Cookie = new CookieCollection();
                string Objecto = ObjectSerialize(modelo);
                HttpWebResponse session = conexion.SesionLogin();
                server = string.Empty;
                server = session.ResponseUri.Authority;
                server = "https://" + server + "/b1s/v1/";
                Uri URLSap = new Uri(server + "Activities(" + activity + ")");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URLSap);
                httpWebRequest.ContentType = "application/json;odata=minimalmetadata;charset=utf8";
                httpWebRequest.Method = "PATCH";
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.Accept = "application/json;odata=minimalmetadata";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Timeout = 10000000;

                foreach (Cookie cookieValue in session.Cookies)
                {
                    Cookie cookie = new Cookie();
                    if (cookieValue.Name.Equals("B1SESSION"))
                    {
                        cookie.Name = cookieValue.Name;
                        cookie.Value = cookieValue.Value;
                        cookie.Path = "/b1s/v1";
                        //  cookie.Path = cookieValue.Path;
                        cookie.Domain = URLSap.Host;
                        httpWebRequest.CookieContainer.Add(cookie);
                    }
                    else
                    {
                        cookie.Name = cookieValue.Name;
                        cookie.Value = cookieValue.Value;
                        cookie.Path = "/b1s";
                        //   cookie.Path = cookieValue.Path;
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
                CreateResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(CreateResponse.GetResponseStream()))
                {
                    actual = streamReader.ReadToEnd();
                }
                conexion.SesionLogout(session.Cookies, server);
                return true;
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

        public bool actualizaEmail(int activity, Models.Tracking.Activities modelo)
        {
            conexion = new Conexion();
            string server = "";
            try
            {
                HttpWebResponse CreateResponse = null;

                Cookie = new CookieCollection();
                string Objecto = ObjectSerialize(modelo);
                HttpWebResponse session = conexion.SesionLogin();
                server = string.Empty;
                server = session.ResponseUri.Authority;
                server = "https://" + server + "/b1s/v1/";
                Uri URLSap = new Uri(server + "Activities(" + activity + ")");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URLSap);
                httpWebRequest.ContentType = "application/json;odata=minimalmetadata;charset=utf8";
                httpWebRequest.Method = "PATCH";
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.Accept = "application/json;odata=minimalmetadata";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Timeout = 10000000;

                foreach (Cookie cookieValue in session.Cookies)
                {
                    Cookie cookie = new Cookie();
                    if (cookieValue.Name.Equals("B1SESSION"))
                    {
                        cookie.Name = cookieValue.Name;
                        cookie.Value = cookieValue.Value;
                        cookie.Path = "/b1s/v1";
                        //  cookie.Path = cookieValue.Path;
                        cookie.Domain = URLSap.Host;
                        httpWebRequest.CookieContainer.Add(cookie);
                    }
                    else
                    {
                        cookie.Name = cookieValue.Name;
                        cookie.Value = cookieValue.Value;
                        cookie.Path = "/b1s";
                        //   cookie.Path = cookieValue.Path;
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
                CreateResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(CreateResponse.GetResponseStream()))
                {
                    actual = streamReader.ReadToEnd();
                }
                conexion.SesionLogout(session.Cookies, server);
                return true;
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
        #region Certificado
        private static bool RemoteSSLTLSCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors ssl)
        {
            //accept
            return true;
        }
        #endregion
    }
}