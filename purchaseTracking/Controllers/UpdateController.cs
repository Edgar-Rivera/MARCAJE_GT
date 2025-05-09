using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using purchaseTracking.Models;
using FileHelpers;
using PagedList;
using PagedList.Mvc;
using System.Net;
using purchaseTracking.ServiceLayer;
using System.Globalization;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class UpdateController : Controller
    {
        // GET: Update
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult updateTracking(int? page, string findString)
        {
            ViewBag.Status = "Datos Obtenidos desde archivo";
            ViewBag.findString = findString;
            int pageSize = 45;
            int pageNumber = (page ?? 1);
            string path = @"C:\Importaciones\DataIntegration.csv";
            var data = new List<purchaseTracking.Models.DataIntegration.PurchaseOrders>();
            var engine = new FileHelperEngine<purchaseTracking.Models.DataIntegration.PurchaseOrders>();
            var records = engine.ReadFile(path);
            bool first = true;
            foreach(var item in records)
            {
                if (!first) { 
                    data.Add(new Models.DataIntegration.PurchaseOrders()
                    {
                        DocEntry = item.DocEntry,
                        DocNum = item.DocNum,
                        LineNum = item.LineNum,
                        ItemCode = item.ItemCode,
                        U_NumeroActividad = item.U_NumeroActividad,
                        U_LineaOV = item.U_LineaOV,
                        U_FechaDespacho = item.U_FechaDespacho,
                        U_FechaEmbarcador = item.U_FechaEmbarcador,
                        U_FechaArribo = item.U_FechaArribo,
                        U_FechaIngresoCD = item.U_FechaIngresoCD,
                        U_Estado = item.U_Estado,
                        U_MedioImportacion = item.U_MedioImportacion,
                        U_commentsDespacho = item.U_commentsDespacho,
                        U_commentsEmbarcador = item.U_commentsEmbarcador,
                        U_commentsAduanales = item.U_commentsAduanales,
                        U_commentsPrecioEntrega = item.U_commentsPrecioEntrega
                    });
                } else
                {
                    first = false;
                }
            }           
            return View(data.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult TransferData()
        {
            Conexion conexion;
            conexion = new ServiceLayer.Conexion();
            HttpWebResponse session = conexion.SesionInterface();
            var count = 0;
            try
            {
                // LOGIN PARA OBTENER UNA SESSION VALIDA DE SAP
               
                string path = @"C:\Importaciones\DataIntegration.csv";
                var engine = new FileHelperEngine<purchaseTracking.Models.DataIntegration.PurchaseOrders>();
                var records = engine.ReadFile(path);
                bool first = true;
                count = records.Count();
                foreach (var item in records)
                {
                    if (!first)
                    {
                        // CREA CADA UNO DE LOS MODELOS DE DATOS A ACTUALIZAR EN SAP
                        Models.DataIntegration.PurchaseOrder data = new Models.DataIntegration.PurchaseOrder();
                        List<Models.DataIntegration.DocumentLines> line = new List<Models.DataIntegration.DocumentLines>();
                        Models.DataIntegration.DocumentLines linePurchase = new Models.DataIntegration.DocumentLines();
                        linePurchase.LineNum = Convert.ToInt32(item.LineNum);
                        linePurchase.U_NumeroActividad = item.U_NumeroActividad;                       
                        linePurchase.U_FechaDespacho = convertoToDateTimeString(item.U_FechaDespacho);
                        linePurchase.U_FechaEmbarcador = convertoToDateTimeString(item.U_FechaEmbarcador); 
                        linePurchase.U_FechaArribo = convertoToDateTimeString(item.U_FechaArribo); 
                        linePurchase.U_FechaIngresoCD = convertoToDateTimeString(item.U_FechaIngresoCD); 
                        linePurchase.U_Estado = item.U_Estado;
                        linePurchase.U_MedioImportacion = item.U_MedioImportacion;
                        linePurchase.U_commentsDespacho = item.U_commentsDespacho;
                        linePurchase.U_commentsEmbarcador = item.U_commentsEmbarcador;
                        linePurchase.U_commentsAduanales = item.U_commentsAduanales;
                        linePurchase.U_commentsPrecioEntrega = item.U_commentsPrecioEntrega;
                        line.Add(linePurchase);
                        data.DocumentLines = line;
                        // METODO DE ENVIO Y CARGA DE DATOS
                        if (item.DocEntry != "" && item.DocEntry != null)
                        {
                            new ServiceLayer.Activity.DataTransfer().transfer(Convert.ToInt32(item.DocEntry), data, session);
                        }
                    }
                    else
                    {
                        first = false;
                    }
                }
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message.ToString();
                return View("Error");
            }
            conexion.SesionLogout(session.Cookies, "https://192.168.1.221:50000/b1s/v1/");
            ViewBag.Message = count;
            return View();
        }

        private string convertoToDateTimeString(string date_received)
        {
            if (date_received != "") 
            {
                DateTime enteredDate = DateTime.ParseExact(date_received, "dd/MM/yyyy", null);
                return enteredDate.ToString("yyyy-MM-dd");
            } else
            {
                return "";
            }               
        }
    }
}