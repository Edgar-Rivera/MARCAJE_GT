using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace purchaseTracking.ServiceLayer.Activity
{
    public class DataTransfer
    {
        CookieCollection Cookie;
        Conexion conexion;
        public void transfer(int DocEntry, Models.DataIntegration.PurchaseOrder purchaseLine, HttpWebResponse SapB1C)
        {
            // METODO PARA LA ACTULAIZACION DE DATOS DESDE SAP 
            conexion = new Conexion();
            string server = "";
            try
            {
                HttpWebResponse CreateResponse = null;
                Cookie = new CookieCollection();
                string Objecto = ObjectSerialize(purchaseLine);
                HttpWebResponse session = SapB1C;
                server = string.Empty;
                server = session.ResponseUri.Authority;
                server = "https://" + server + "/b1s/v1/";
                Uri URLSap = new Uri(server + "PurchaseOrders(" + DocEntry + ")");
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
    }
}