using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using purchaseTracking.Services;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using purchaseTracking.Connection;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.EMMA;

namespace purchaseTracking.Controllers
{
    [SessionExpireFilter]
    public class AccountController : Controller
    {
        public ActionResult DigitalForms()
        {
            return View();
        }
        public ActionResult Policies()
        {
            return View();
        }
        public ActionResult DowloadFiles(int id)
        {
            Models.Activities.details requestActivity = new Connection.Activities.DataActivities().getDetailsInvoice(id);
            var tableSigns = GetListSigns(Convert.ToInt32(requestActivity.U_InternalKey), Convert.ToInt32(requestActivity.AttendUser));
            int dias = 0;
            if (!String.IsNullOrEmpty(Convert.ToString(requestActivity.Recontact)) && !string.IsNullOrEmpty(requestActivity.FechaActualizacion))
            {
                DateTime fecha1 = Convert.ToDateTime(requestActivity.Recontact);
                DateTime fecha2 = Convert.ToDateTime(requestActivity.FechaActualizacion);
                TimeSpan diferencia = fecha2 - fecha1;
                for (int i = 0; i <= diferencia.Days; i++)
                {
                    DateTime fechaActual = requestActivity.Recontact.AddDays(i);
                    if (fechaActual.DayOfWeek != DayOfWeek.Saturday && fechaActual.DayOfWeek != DayOfWeek.Sunday && !(fechaActual.Month == 12 && fechaActual.Day == 25) &&
        !(fechaActual.Month == 11 && fechaActual.Day == 1) && !(fechaActual.Month == 9 && fechaActual.Day == 15) && !(fechaActual.Month == 3 && fechaActual.Day == 28) && !(fechaActual.Month == 3 && fechaActual.Day == 29))
                    {
                        dias++;
                    }
                }
            }

            Models.UserData.OHEM data_sap = new Connection.UserData.UserData().GetOHEMs(Convert.ToInt32(requestActivity.U_InternalKey));
            Models.UserData.UserData data_etalent = new Connection.UserData.UserData().UserDatas(data_sap.empID);

            ReportDocument rpt = new ReportDocument();
            rpt = new VACACIONES();
            rpt.SetDatabaseLogon("sa", "M@n4g3rS!st3m$+*");
            rpt.Subreports[0].SetDataSource(tableSigns);

            rpt.SetParameterValue("@FECHA", requestActivity.Recontact);
            rpt.SetParameterValue("@CODEPDO", data_etalent.EPDO_CODIGO);
            rpt.SetParameterValue("MotivoCambio", "");
            rpt.SetParameterValue("FechaFin", requestActivity.FechaActualizacion);
            rpt.SetParameterValue("CantidadDiasVacaciones", "" + dias);
            rpt.SetParameterValue("Observaciones", requestActivity.Details);
            rpt.SetParameterValue("TipoSolicitud", requestActivity.Name);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Solicitud_" + requestActivity.ClgCode + "_.pdf");
            
        }
        




        [HttpGet]
        public ActionResult createSignDigital()
        {
            Models.Images.ImageSign temp = new Models.Images.ImageSign();
            temp = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32(Session["code"]));
            ViewBag.source = temp.U_PathSign;
            ViewBag.nombre = temp.U_Nombre;
            return View();
        }

        [HttpGet]
        public ActionResult EmpleadosUnidad(int? page, string findString)
        {
         
            List<Models.eTALENT.VACACIONES_DISPONIBLES> vacaciones = new List<Models.eTALENT.VACACIONES_DISPONIBLES>();
            vacaciones = new Connection.UserData.UserData().VacacionesUnidades(Convert.ToInt32(Session["internal_code"]));

            List<Models.eTALENT.VACACIONES> vacaciones_periodo_empleados = new List<Models.eTALENT.VACACIONES>();
            // LISTA GENERAL 
            foreach(var vacacion in vacaciones)
            {
                List<Models.eTALENT.VACACIONES> vacaciones_periodo = new List<Models.eTALENT.VACACIONES>();
                vacaciones_periodo = new Connection.UserData.UserData().VacacionesDiaSP(vacacion.CODIGO);
                vacaciones_periodo_empleados.AddRange(vacaciones_periodo);
            }
            


            ViewBag.findString = findString;
            ViewBag.totalItem = vacaciones_periodo_empleados.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = vacaciones_periodo_empleados.Where(s => s.EPDO_NOMBRE_COMPLETO.ToString().Contains(findString));

                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(vacaciones_periodo_empleados.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public ActionResult createSignDigital(string SignatureDataUrl)
        {
            Models.SignDigitalTechnician.SIGN_DIGITAL_OT temp = new Models.SignDigitalTechnician.SIGN_DIGITAL_OT();
            temp.U_InternalKey = Session["code"].ToString();
            temp.U_PathSign = SignatureDataUrl;
            if (new purchaseTracking.ServiceLayer.Schedulings().AddSign(temp))
            {
                return View("Success");
            }
            else
            {
                ViewBag.exception = "No ha sido posible crear la rutina";
                return View("Error");
            }
        }


        // GET: Account
        [HttpGet]
        public ActionResult PersonalInformation()
        {
            List<Models.eTALENT.EPDO_MASTER_DATA> datos = new List<Models.eTALENT.EPDO_MASTER_DATA>();
            datos = new Connection.UserData.UserData().DatosEmpleados(Convert.ToInt32(Session["external_code"]));

            List<Models.eTALENT.VACACIONES> vacaciones = new List<Models.eTALENT.VACACIONES>();
            vacaciones = new Connection.UserData.UserData().VacacionesDiaSP(Convert.ToInt32(Session["internal_code"]));

            List<Models.eTALENT.HISTORICO_VACACIONES> historico = new List<Models.eTALENT.HISTORICO_VACACIONES>();
            historico = new Connection.UserData.UserData().HistoricoEmpleado(Convert.ToInt32(Session["external_code"]));

            ViewBag.historico = historico;
            ViewBag.vacaciones = vacaciones;

            return View(datos);
        }
        [HttpGet]
        public ActionResult Documentos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RequestForms()
        {
            Models.Images.ImageSign temp = new Models.Images.ImageSign();
            temp = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32(Session["code"]));
            ViewBag.source = temp.U_PathSign;
            ViewBag.nombre = temp.U_Nombre;
            return View();
        }

        [HttpGet]
        public ActionResult VacationsRequest()
        {
            return View();
        }

        public byte[] convertStringtoByte(string SignatureDataUrl)
        {
            byte[] jpgArray;
            string temp = string.Empty;
            var base64Signature = SignatureDataUrl.Split(',')[1];
            byte[] imageBytes = System.Convert.FromBase64String(base64Signature);
            using (MemoryStream msIn = new MemoryStream(imageBytes))
            {
                using (Image pic = Image.FromStream(msIn))
                {
                    using (MemoryStream msOut = new MemoryStream())
                    {
                        pic.Save(msOut, System.Drawing.Imaging.ImageFormat.Jpeg);
                        temp = Convert.ToBase64String(msOut.ToArray());
                        jpgArray = System.Convert.FromBase64String(temp);
                    }
                }
            }

            return jpgArray;
        }

        public List<Models.Images.Signs> GetListSigns( int codigo)
        {
            List<Models.Images.Signs> data = new List<Models.Images.Signs>();
            Models.Images.ImageSign dataImage = new Models.Images.ImageSign();
            dataImage = new Connection.Activities.DataActivities().GetSignTechnician(codigo);
            if (dataImage.U_PathSign!="")
            {
                Models.Images.Signs imageData = new Models.Images.Signs();
                imageData.nombre = "Empleado: " + dataImage.U_Nombre;
                imageData.image = convertStringtoByte(dataImage.U_PathSign);
                data.Add(imageData);
            }   
            return data;
        }

        public List<Models.Images.Signs> GetListSigns(int codigo, int jefe_inmediato)
        {
            List<Models.Images.Signs> data = new List<Models.Images.Signs>();
            Models.Images.ImageSign dataImage = new Models.Images.ImageSign();
            dataImage = new Connection.Activities.DataActivities().GetSignTechnician(codigo);
            if (dataImage.U_PathSign != "")
            {
                Models.Images.Signs imageData = new Models.Images.Signs();
                imageData.nombre = "Empleado: " + dataImage.U_Nombre;
                imageData.image = convertStringtoByte(dataImage.U_PathSign);
                data.Add(imageData);
            }

            // OBTENCION FIRMA DIGITAL 
            Models.Images.ImageSign dataImage_j = new Models.Images.ImageSign();
            dataImage_j = new Connection.Activities.DataActivities().GetSignTechnician(jefe_inmediato);
            if (dataImage_j.U_PathSign != "")
            {
                Models.Images.Signs imageData_j = new Models.Images.Signs();
                imageData_j.nombre = "Jefe Inmediato: " + dataImage_j.U_Nombre;
                imageData_j.image = convertStringtoByte(dataImage_j.U_PathSign);
                data.Add(imageData_j);
            }
            return data;
        }


        [HttpPost]
        public ActionResult newRequest(Models.Activities.RequestActivity requestActivity)
        {
            // VARIABLES DEL SERVICE LAYER ESTATICAS    
            requestActivity.DurationType = "du_Seconds";
            requestActivity.U_internalKey = Session["code"].ToString();
            if(requestActivity.ActivityType == 86)
            {
                requestActivity.U_Observaciones = "Solicitud de Vacaciones";
            }
            if(requestActivity.U_retrasoDias == "on")
            {
                requestActivity.U_retrasoDias = "1";
            }
            // METODO QUE RECIBE EL MODULO Y HACE EL POST EN SAP   
            if (new ServiceLayer.Activity.ActivityComponents().addActivity(requestActivity))
            {
                var tableSigns = GetListSigns(Convert.ToInt32(Session["code"].ToString()));

                


                // VALIDA LA CREACION DE LA ACTIVIDAD EN SAP Y ENVIA CORREO ELECTORNICO
                var data = new Connection.Activities.DataActivities().GetEjecutivo(requestActivity.HandledBy);
                ViewBag.email_to = data.correo;
                ViewBag.email_bcc = requestActivity.U_Correo;
                ViewBag.activity = new purchaseTracking.Connection.Activities.DataActivities().getID();
                SendMailer message = new SendMailer();

             
                // RUTINA PARA CREAR PDF APARTIR DE FORMATO CRYSTAL REPORTS
               /* string direct = string.Empty;
                ReportDocument rpt = new ReportDocument();
                rpt = new VACACIONES();             
                rpt.SetDatabaseLogon("sa", "M@n4g3rS!st3m$+*");
                rpt.Subreports[0].SetDataSource(tableSigns);

                // DATA SOURCE FIRMAS
                int dias = 0;
                if(!String.IsNullOrEmpty(requestActivity.StartDate) && !string.IsNullOrEmpty(requestActivity.U_FechaActualizacion))
                {
                    DateTime fecha1 = Convert.ToDateTime(requestActivity.StartDate);
                    DateTime fecha2 = Convert.ToDateTime(requestActivity.U_FechaActualizacion);
                    TimeSpan diferencia = fecha2 - fecha1;
                    for (int i = 0; i <= diferencia.Days; i++)
                    {
                        // Obtener el día actual en la iteración
                        DateTime fechaActual = Convert.ToDateTime(requestActivity.StartDate).AddDays(i);

                        // Verificar si el día actual es sábado o domingo
                        if (fechaActual.DayOfWeek != DayOfWeek.Saturday && fechaActual.DayOfWeek != DayOfWeek.Sunday)
                        {
                            dias++;
                        }
                    }
                }
               */
               

                // DATOS DE FIRMAS
                /*
                rpt.SetParameterValue("@FECHA",requestActivity.StartDate);
                rpt.SetParameterValue("@CODEPDO", Session["internal_code"]);
                rpt.SetParameterValue("MotivoCambio", "");
                rpt.SetParameterValue("FechaFin", requestActivity.U_FechaActualizacion);
                rpt.SetParameterValue("CantidadDiasVacaciones", ""+dias);
                rpt.SetParameterValue("Observaciones", requestActivity.Details);
          
                ExportOptions myoptions;
                DiskFileDestinationOptions path = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions pdf = new PdfRtfWordFormatOptions();
                path.DiskFileName = "C:\\RequestDocuments\\" + requestActivity.U_Solicitante +  '_' + DateTime.Now.ToString("MM-dd-yyyy") + ".pdf";
                direct = path.DiskFileName;
                myoptions = rpt.ExportOptions;
                myoptions.ExportDestinationType = ExportDestinationType.DiskFile;
                myoptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                myoptions.ExportDestinationOptions = path;
                myoptions.ExportFormatOptions = pdf;
                rpt.Export();
                */
                message.sendMail(data.correo, requestActivity.U_Correo, requestActivity.Details, "", requestActivity.U_Solicitante, ViewBag.activity, "");
                return View("Success");
            }
            else
            {
                return View("Error");
            }

        }

        [HttpGet]
        public ActionResult RequestInvoice(int? page, string findString, string filterString)
        {
            List<Models.Activities.List> data = new List<Models.Activities.List>();

            if (!string.IsNullOrEmpty(filterString))
            {
                data = new Connection.Activities.DataActivities().getListRequestInvoiceNonEstatus(Convert.ToInt32(Session["code"]));
                data = (from t in data where t.Name.ToString() == filterString select t).ToList();
            }

            else
            {
                data = new Connection.Activities.DataActivities().getListRequestInvoice(Convert.ToInt32(Session["code"]));
            }
            ViewBag.filterString = filterString;
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
        public ActionResult detailsInvoiceAssign(int id)
        {
           


            Models.Activities.details data = new Connection.Activities.DataActivities().getDetailsInvoice(id);


            Models.Images.ImageSign temp = new Models.Images.ImageSign();
            temp = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32(data.U_InternalKey));
            ViewBag.source = temp.U_PathSign;
            ViewBag.nombre = temp.U_Nombre;

            // SE OBTEIEN FIRMA DE JEFE INMEDIATO
            Models.Images.ImageSign temp_j = new Models.Images.ImageSign();
            temp_j = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32(data.AttendUser));
            ViewBag.source_j = temp_j.U_PathSign;
            ViewBag.nombre_j = temp_j.U_Nombre;


            // SE OBTEIEN FIRMA NOMINA
            Models.Images.ImageSign temp_n = new Models.Images.ImageSign();
            temp_n = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(6);
            ViewBag.source_n = temp_n.U_PathSign;
            ViewBag.nombre_n = temp_n.U_Nombre;

            return View(data);
        }

        [HttpGet]
        public ActionResult detailsInvoice(int id)
        {
            


            Models.Activities.details data = new Connection.Activities.DataActivities().getDetailsInvoice(id);

            Models.Images.ImageSign temp = new Models.Images.ImageSign();
            temp = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32 (data.U_InternalKey));
            ViewBag.source = temp.U_PathSign;
            ViewBag.nombre = temp.U_Nombre;


            // SE OBTEIEN FIRMA DE JEFE INMEDIATO
            Models.Images.ImageSign temp_j = new Models.Images.ImageSign();
            temp_j = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32(data.AttendUser));
            ViewBag.source_j = temp_j.U_PathSign;
            ViewBag.nombre_j = temp_j.U_Nombre;


            // SE OBTEIEN FIRMA NOMINA
            Models.Images.ImageSign temp_n = new Models.Images.ImageSign();
            temp_n = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(6);
            ViewBag.source_n = temp_n.U_PathSign;
            ViewBag.nombre_n = temp_n.U_Nombre;

            return View(data);
        }

        [HttpPost]
        public ActionResult updateMails(int id, string comment)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Tracking.Activities();
            data.U_Correo = comment;

            if (new ServiceLayer.Activity.ActivityComponents().actualizaEmail(id, data))
            {
                return RedirectToAction("detailsInvoice", "Account", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }

        [HttpGet]
        public ActionResult listInvoice(int? page, string findString, string filterString)
        {

            List<Models.Activities.List> data = new List<Models.Activities.List>();
            if (!string.IsNullOrEmpty(filterString))
            {
                
                data = new Connection.Activities.DataActivities().getListAllNonStatusInvoice_A(Convert.ToInt32(Session["code"]));
                data = (from t in data where t.status.ToString() == filterString select t).ToList();
            }
            else
            {
                data = new Connection.Activities.DataActivities().getListAllInvoice_A(Convert.ToInt32(Session["code"]));
            }
            ViewBag.findString = findString;
            ViewBag.totalItem = data.Count();
            ViewBag.filterString = filterString;
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
        public ActionResult listNomina(int? page, string findString, string filterString, string filterJefeInmediato, string filterSolicitante, string filterTipoActividad)
        {
            List<string> JefeInmediato = new List<string>();
            JefeInmediato.Add("Seleccione Jefe Inmediato");

            List<string> Solicitantes = new List<string>();
            Solicitantes.Add("Seleccione Solicitante");

            List<string> TipoActividad = new List<string>();
            TipoActividad.Add("Seleccione Tipo de Actividad"); 


            List<Models.Activities.List> data = new List<Models.Activities.List>();
            if (!string.IsNullOrEmpty(filterString))
            {

                data = new Connection.Activities.DataActivities().getListAllNonStatusInvoice_N();
               
            }
            else
            {
                data = new Connection.Activities.DataActivities().getListAllNonStatusInvoice_N();
               
            }

            var temp_jefe_inmediato = data.Select(x => x.U_NAME).Distinct();
            foreach (var item_fase in temp_jefe_inmediato)
            {
                JefeInmediato.Add(item_fase);
            }

            var temp_solicitante = data.Select(x => x.U_Solicitante).Distinct();
            foreach (var item_fase in temp_solicitante)
            {
                Solicitantes.Add(item_fase);
            }

            var temp_actividad = data.Select(x => x.Name).Distinct();
            foreach (var item_fase in temp_actividad)
            {
                TipoActividad.Add(item_fase);
            }




            if (!string.IsNullOrEmpty(filterJefeInmediato) && filterJefeInmediato != "0" && filterJefeInmediato != "Seleccione Jefe Inmediato")
            {
                data = data.Where(x => x.U_NAME == filterJefeInmediato).ToList();
            }

            if (!string.IsNullOrEmpty(filterSolicitante) && filterSolicitante != "0" && filterSolicitante != "Seleccione Solicitante")
            {
                data = data.Where(x => x.U_Solicitante == filterSolicitante).ToList();
            }

            if (!string.IsNullOrEmpty(filterTipoActividad) && filterTipoActividad != "0" && filterTipoActividad != "Seleccione Tipo de Actividad")
            {
                data = data.Where(x => x.Name == filterTipoActividad).ToList();
            }

            ViewBag.JefeInmediato = JefeInmediato;
            ViewBag.Solicitantes = Solicitantes;
            ViewBag.TipoActividad = TipoActividad;
           


            ViewBag.findString = findString;
            ViewBag.totalItem = data.Count();
            ViewBag.filterString = filterString;
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
                message.sendNotification(ejecutivo, involucrados, "CANCELACION_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                return RedirectToAction("RequestInvoice", "Account", new { id = id });
            }
            else
            {
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult updateComments(int id, string comment, string status, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Activities.Activities();
            data.U_Comentarios = comment;
            data.Status = status;
            if (new ServiceLayer.Activity.ActivityComponents().actualizaComentarios(id, data))
            {
                SendNotification message = new SendNotification();
                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                return RedirectToAction("detailsInvoice", "Account", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }

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
                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                return RedirectToAction("detailsInvoice", "Account", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult updateStatusAssign(int id, string comment, string status, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Activities.Activities();
            data.U_Comentarios = comment;
            data.Status = status;
            if (new ServiceLayer.Activity.ActivityComponents().actualizaComentarios(id, data))
            {
                SendNotification message = new SendNotification();
                if (status == "-3")
                {
                    
                    Models.Activities.details requestActivity = new Connection.Activities.DataActivities().getDetailsInvoice(id);
                    var tableSigns = GetListSigns(Convert.ToInt32(requestActivity.U_InternalKey), Convert.ToInt32(requestActivity.AttendUser));
                    involucrados = involucrados + ",nomina@isertec.com";
                    // RUTINA PARA CREAR PDF APARTIR DE FORMATO CRYSTAL REPORTS
                    int dias = 0;
                    if (!String.IsNullOrEmpty(Convert.ToString(requestActivity.Recontact)) && !string.IsNullOrEmpty(requestActivity.FechaActualizacion))
                    {
                        DateTime fecha1 = Convert.ToDateTime(requestActivity.Recontact);
                        DateTime fecha2 = Convert.ToDateTime(requestActivity.FechaActualizacion);
                        TimeSpan diferencia = fecha2 - fecha1;

                        // Iterar sobre cada día entre las dos fechas
                        for (int i = 0; i <= diferencia.Days; i++)
                        {
                            // Obtener el día actual en la iteración
                            DateTime fechaActual = Convert.ToDateTime(requestActivity.Recontact).AddDays(i);

                            // Verificar si el día actual es sábado o domingo
                            if (fechaActual.DayOfWeek != DayOfWeek.Saturday && fechaActual.DayOfWeek != DayOfWeek.Sunday && !(fechaActual.Month == 12 && fechaActual.Day == 25) &&
       !(fechaActual.Month == 11 && fechaActual.Day == 1) && !(fechaActual.Month == 9 && fechaActual.Day == 15) && !(fechaActual.Month == 3 && fechaActual.Day == 28) && !(fechaActual.Month == 3 && fechaActual.Day == 29))
                            {
                                // Si no es sábado ni domingo, agregar 1 a la cantidad
                                dias++;
                            }
                        }


                    }

                    Models.UserData.OHEM data_sap = new Connection.UserData.UserData().GetOHEMs(Convert.ToInt32(requestActivity.U_InternalKey));
                    Models.UserData.UserData data_etalent = new Connection.UserData.UserData().UserDatas(data_sap.empID);
                    string direct = string.Empty;
                    ReportDocument rpt = new ReportDocument();
                    rpt = new VACACIONES();
                    rpt.SetDatabaseLogon("sa", "M@n4g3rS!st3m$+*");
                    rpt.Subreports[0].SetDataSource(tableSigns);

                    rpt.SetParameterValue("@FECHA", requestActivity.Recontact);
                    rpt.SetParameterValue("@CODEPDO", data_etalent.EPDO_CODIGO);
                    rpt.SetParameterValue("MotivoCambio", "");
                    rpt.SetParameterValue("FechaFin", requestActivity.FechaActualizacion);
                    rpt.SetParameterValue("CantidadDiasVacaciones", ""+ dias);
                    rpt.SetParameterValue("Observaciones", requestActivity.Details);
                    rpt.SetParameterValue("TipoSolicitud", requestActivity.Name);

                    ExportOptions myoptions;
                    DiskFileDestinationOptions path = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions pdf = new PdfRtfWordFormatOptions();
                    path.DiskFileName = "C:\\RequestDocuments\\" + requestActivity.U_Solicitante + '_' + DateTime.Now.ToString("MM-dd-yyyy") + "a_.pdf";
                    direct = path.DiskFileName;
                    myoptions = rpt.ExportOptions;
                    myoptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    myoptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    myoptions.ExportDestinationOptions = path;
                    myoptions.ExportFormatOptions = pdf;
                    rpt.Export();
                    message.sendNotification(ejecutivo, involucrados, "SOLICITUD_APROBADA_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn, direct);
                }
                else
                {
                    if (status == "4")
                    {
                       message.sendNotification(ejecutivo, involucrados, "RECHAZO_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                    }
                }
                
                return RedirectToAction("detailsInvoiceAssign", "Account", new { id = id });
            }
            else
            {
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult updateCommentsAssign(int id, string comment, string status, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Activities.Activities();
            data.U_Comentarios = comment;
            data.Status = status;
            if (new ServiceLayer.Activity.ActivityComponents().actualizaComentarios(id, data))
            {
                SendNotification message = new SendNotification();

                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                return RedirectToAction("detailsInvoiceAssign", "Account", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult updateNotesAssign(int id, string comment, string ejecutivo, string involucrados, string orden_venta, string sn)
        {
            // METODO PARA ACTULIZAR LOS COMENTARIOS
            var data = new purchaseTracking.Models.Orders.Activities();
            data.Notes = comment;
            data.U_FechaActualizacion = DateTime.Now.ToString("yyyy-MM-dd");
            if (new ServiceLayer.Activity.ActivityComponents().actualizaNotas(id, data))
            {
                SendNotification message = new SendNotification();
                message.sendNotification(ejecutivo, involucrados, "ACTUALIZACION_SOLICITUD_" + id + "", Session["nombre"].ToString(), "" + id, comment, orden_venta, sn,"");
                return RedirectToAction("detailsInvoiceAssign", "Account", new { id = id });
            }
            else
            {
                return View("Error");
            }

        }
    }
}