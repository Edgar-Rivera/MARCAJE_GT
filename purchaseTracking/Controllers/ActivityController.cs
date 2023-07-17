using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using purchaseTracking.Connection.Activities;
using ClosedXML.Excel;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Data;
using purchaseTracking.Services;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        public Models.Images.ImageSign GetSignTechnician(int internalKey)
        {

            Models.Images.ImageSign dataImage = new Models.Images.ImageSign();
            dataImage = new Connection.Activities.DataActivities().GetSignTechnician(internalKey);
            return dataImage;
        }
        // GET: Activity
        [HttpGet]
        public ActionResult list(int? page, string findString, string filterString)
        {
            if(filterString == null)
            {
                // se setean los valores necesarios al ingresar
                if(Session["code"].ToString() == "13")
                {
                    filterString = "Compras Locales";
                }
                else if(Session["code"].ToString() == "70")
                {
                    filterString = "Compra Internacional";
                }
                else
                {
                    filterString = null;
                }
            }
            List<Models.Activities.List> data = new List<Models.Activities.List>();
            if(filterString != "0" && filterString!=null)
            {
                if(findString != null && findString != "")
                {
                    // OBTIENE CERRADAS Y ABIERATAS
                    data = new DataActivities().getListNonStatus(filterString);
                }
                else
                {
                    data = new DataActivities().getList(filterString);
                }
                // SE FILTRA POR FILTRO DE TIPO DE ACTIVIDAD / OBTIENE TODAS LAS SOLICITUDES, ABIERTAS Y CERRADAS
                
            }
            else
            {
                if (findString != null && findString != "")
                {
                    // OBTIENE CERRADAS Y ABIERATAS
                    data = new DataActivities().getListAllNonStatus();
                }
                else
                {
                    data = new DataActivities().getListAll();
                }
                
            } 
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.ClgCode.ToString().Contains(findString) || s.CntctDate.ToString().Contains(findString) || s.Name.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.AttendUser.ToString().Contains(findString) || s.U_NAME.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.U_Solicitante.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.DocNum.ToString().Contains(findString)
                || s.Details.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.ODC.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.Estado.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }


        // SOLICITUDES DE USUARIOS
        [HttpGet]
        public ActionResult RequestPurchase(int? page, string findString)
        {
            List<Models.Activities.List> data = new DataActivities().getListRequest(Convert.ToInt32(Session["code"]));
            ViewBag.findString = findString;
            ViewBag.totalItem = data.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.ClgCode.ToString().Contains(findString) || s.CntctDate.ToString().Contains(findString) || s.Name.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.AttendUser.ToString().Contains(findString) || s.U_NAME.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.U_Solicitante.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.DocNum.ToString().Contains(findString)
                || s.Details.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.ODC.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.Estado.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult details(int id)
        {
            /* PROCEDIMIENTO QUE MUESTRA LOS DETALLES DE TODAS LAS ACTIVIDADES SELECCIONADAS */
            ViewBag.orders = new Connection.Activities.DataActivities().GetOrderListPurchases(id);
            Models.Activities.details data = new DataActivities().getDetails(id);
            return View(data);
        }

        // ACTION QUE EXPORTA EXCEL PARA LOS EJECTUTIVOS ASIGNADOS
        public FileResult ExportExcelRequest(string findString)
        {
            List<Models.Activities.List> data = new DataActivities().GetListsExcel(Convert.ToInt32(Session["code"]));
            DataTable dt = new DataTable("Mis Solicitudes");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Número Actividad"),
                                            new DataColumn("Fecha Actividad"),
                                            new DataColumn("Solicitante"),
                                            new DataColumn("Asignado"),
                                            new DataColumn("Tipo Actividad"),
                                            new DataColumn("Asunto Actividad"),
                                            new DataColumn("Orden de Venta"),
                                            new DataColumn("Ordenes de Compra Asociadas"),
                                            new DataColumn("Fecha Necesario Proyecto"),
                                            new DataColumn("Estatus")
            
            });
            if (!String.IsNullOrEmpty(findString))
            {
                // AGREGAR LSO MISMOS CAMPOS QUE SE BUSCAN EN LA LISTA TEMPORAL DEL EJECUTIVO ASIGNADO 
                var temp = data.Where(s => s.ClgCode.ToString().Contains(findString) || s.CntctDate.ToString().Contains(findString) || s.Name.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.AttendUser.ToString().Contains(findString) || s.U_NAME.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.U_Solicitante.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.DocNum.ToString().Contains(findString)
                || s.Details.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.ODC.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                foreach (var item in temp)
                {
                    dt.Rows.Add(item.ClgCode, item.Recontact, item.U_Solicitante, item.U_NAME,item.Name, item.Details, item.DocNum,item.ODC,item.endDate,item.Estado);
                }
            } else
            {
                foreach (var item in data)
                {
                    dt.Rows.Add(item.ClgCode, item.Recontact, item.U_Solicitante, item.U_NAME, item.Name, item.Details, item.DocNum, item.ODC, item.endDate, item.Estado);
                }
            }    
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitudes_"+ Session["nombre"] + "_" + DateTime.Now.ToShortDateString() +".xlsx");
                }
            }
        }


        // METODO PARA EXPORTAR EXCEL DATOS DEL SOLICITANDO Y SOLICITUDES QUE TENGAN ASIGNADAS
        public FileResult ExportExcel()
        {
            List<Models.Activities.List> data = new DataActivities().getListExcel(Convert.ToInt32(Session["code"]));
            DataTable dt = new DataTable("Mis Solicitudes");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Número Actividad"),
                                            new DataColumn("Fecha Actividad"),
                                            new DataColumn("Solicitante"),
                                            new DataColumn("Asignado"),
                                            new DataColumn("Tipo Actividad"),
                                            new DataColumn("Asunto Actividad"),
                                            new DataColumn("Orden de Venta"),
                                            new DataColumn("Ordenes de Compra Asociadas"),
                                            new DataColumn("Fecha Necesario Proyecto"),
                                            new DataColumn("Estatus")

            });
           
                foreach (var item in data)
                {
                    dt.Rows.Add(item.ClgCode, item.Recontact, item.U_Solicitante, item.U_NAME, item.Name, item.Details, item.DocNum, item.ODC, item.endDate, item.Estado);
                }
            
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitudes_" + Session["nombre"] + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }

        // CAMBIOS REALIZADOS PARA MOSTRAR OV Y SOCIO DE NEGOCIOS ** EDGAR RIVERA ** 21-12-2021
        [HttpPost]
        public ActionResult updateComments(int id, string comment, string status, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Activities.Activities();
            data.U_Comentarios = comment;
            data.Status = status;
            if(new ServiceLayer.Activity.ActivityComponents().actualizaComentarios(id,data))
            {
                SendNotification message = new SendNotification();
                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_SOLICITUD_"+ id +"", Session["nombre"].ToString(),""+id, comment, orden_venta, sn,"");
                return RedirectToAction("details", "Activity", new { id = id });
            } else
            {
                return View("Error");
            }
            
        }
        // CAMBIOS REALIZADOS PARA MOSTRAR OV Y SOCIO DE NEGOCIOS ** EDGAR RIVERA ** 21-12-2021
        [HttpPost]
        public ActionResult updateStatus(int id, string comment, string status, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Activities.Activities();
            data.U_Comentarios = comment;
            data.Status = status;
            if (new ServiceLayer.Activity.ActivityComponents().actualizaComentarios(id, data))
            {
                SendNotification message = new SendNotification();
                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_ESTADO_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                return RedirectToAction("details", "Activity", new { id = id });
            }
            else
            {
                return View("Error");
            }
        }
        // CAMBIOS REALIZADOS PARA MOSTRAR OV Y SOCIO DE NEGOCIOS ** EDGAR RIVERA ** 21-12-2021
        [HttpPost]
        public ActionResult updateEntrega(int id, string comment, string status, string ejecutivo, string involucrados, string tipoEntrega, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Activities.Activities();
            data.U_Comentarios = comment;
            if (tipoEntrega == "_")
            {
                data.Status = "-3";
            } else
            {
                data.Status = status;
            }
            data.U_dateNotification = DateTime.Now.ToString("yyyy-MM-dd");
            data.U_FechaActualizacion = DateTime.Now.ToString("yyyy-MM-dd");
            if (new ServiceLayer.Activity.ActivityComponents().actualizaComentarios(id, data))
            {
                SendNotification message = new SendNotification();
                message.sendNotification(ejecutivo, involucrados, "DISPONIBLE_ENTREGA"+ tipoEntrega +"SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                return RedirectToAction("details", "Activity", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }
        // CAMBIOS REALIZADOS PARA MOSTRAR OV Y SOCIO DE NEGOCIOS ** EDGAR RIVERA ** 21-12-2021
        [HttpPost]
        public ActionResult updateNotes(int id, string comment, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Orders.Activities();
            data.Notes = comment;
            data.U_FechaActualizacion = DateTime.Now.ToString("yyyy-MM-dd");
            if (new ServiceLayer.Activity.ActivityComponents().actualizaNotas(id, data))
            {
                SendNotification message = new SendNotification();
                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta,sn,"");
                return RedirectToAction("details", "Activity", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }
        [HttpPost]
        public ActionResult updateMails(int id, string comment)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Tracking.Activities();
            data.U_Correo = comment;

            if (new ServiceLayer.Activity.ActivityComponents().actualizaEmail(id, data))
            {         
                return RedirectToAction("details", "Activity", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult updateDate(int id, string correo)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Tracking.Activities();
            data.U_Correo = correo;
            data.U_FechaActualizacion = DateTime.Now.ToString("yyyy-MM-dd");
            if (new ServiceLayer.Activity.ActivityComponents().actualizaEmail(id, data))
            {
                return RedirectToAction("details", "Activity", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }

        // CAMBIOS REALIZADOS PARA MOSTRAR OV Y SOCIO DE NEGOCIOS ** EDGAR RIVERA ** 21-12-2021
        [HttpPost]
        public ActionResult updateClose(int id, string comment, string status, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Activities.Activities();
            string temp = "La solictud" + id + " ha sido cancelada!";
            data.U_Comentarios = comment;
            data.Status = status;
            data.U_FechaActualizacion = DateTime.Now.ToString("yyyy-MM-dd");
            if (new ServiceLayer.Activity.ActivityComponents().actualizaComentarios(id, data))
            {
                SendNotification message = new SendNotification();
                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_ESTADO_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, temp, orden_venta, sn,"");
                return RedirectToAction("RequestPurchase", "Activity");
            }
            else
            {
                return View("Error");
            }

        }
    }
}