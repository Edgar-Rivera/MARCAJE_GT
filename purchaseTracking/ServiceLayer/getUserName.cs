using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using purchaseTracking.Models;

namespace purchaseTracking.ServiceLayer
{
    public class getUserName
    {
        CookieCollection Cookie;
        public async Task<List<UserNameData>> Obtener(string Codigo, HttpWebResponse sessionSAP)
        {
            List<UserNameData> tecnico = new List<UserNameData>();
            string server = "";

            try
            {
                HttpWebResponse CreateResponse = null;
                Cookie = new CookieCollection();
                server = sessionSAP.ResponseUri.Authority;
                server = "https://" + server + "/b1s/v1/";
                Uri URLSap = new Uri(server + "Users?$select = UserName, InternalKey, eMail &$filter = UserCode eq ' " + Codigo + "'");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URLSap);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "GET";
                httpWebRequest.CookieContainer = new CookieContainer();
                foreach (Cookie cookieValue in sessionSAP.Cookies)
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
                string actual = string.Empty;
                CreateResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(CreateResponse.GetResponseStream()))
                {
                    actual = streamReader.ReadToEnd();
                }

                JObject jObject = JObject.Parse(actual);
                tecnico = jObject.SelectToken("value").Select(jt => jt.ToObject<UserNameData>()).ToList();
                return tecnico;
            }
            catch (WebException e)
            {
                string text;
                HttpWebResponse httpResponse;
                WebResponse response;
                using (response = e.Response)
                {
                    httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        text = new StreamReader(data).ReadToEnd();
                    }
                }
                return tecnico;
            }

        }
    }
}