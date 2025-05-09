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
using System.Web.Razor.Parser.SyntaxTree;
using purchaseTracking.Models.eTALENT;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using DocumentFormat.OpenXml.Drawing.Charts;
using purchaseTracking.Models.Orders;
using purchaseTracking.Models.Activities;
using System.Data;
using DocumentFormat.OpenXml.Wordprocessing;

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


        [HttpGet]
        public ActionResult ExportarEmpleadosEstatus(int? page, string findString)
        {
            List<Models.Employees.StatusEmpleados> vacaciones_periodo_empleados = new Connection.UserData.UserData().GetStatusEmpleado();


            if (!String.IsNullOrEmpty(findString))
            {
                vacaciones_periodo_empleados = vacaciones_periodo_empleados
                    .Where(s => s.Name.Contains(findString))
                    .ToList();
            }



            int pageSize = 500;
            int pageNumber = (page ?? 1);
            var paginatedList = vacaciones_periodo_empleados.ToPagedList(pageNumber, pageSize);


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Resumen de Empleados");


                worksheet.Cells[1, 1].Value = "Código";
                worksheet.Cells[1, 2].Value = "Fecha";
                worksheet.Cells[1, 3].Value = "Actividad";
                worksheet.Cells[1, 4].Value = "Tipo";
                worksheet.Cells[1, 5].Value = "Jefe Inmediato";
                worksheet.Cells[1, 6].Value = "Solicitante";
                worksheet.Cells[1, 9].Value = "Detalles";
                worksheet.Cells[1, 7].Value = "Fecha de Inicio";
                worksheet.Cells[1, 8].Value = "Fecha de Fin";
                worksheet.Cells[1, 10].Value = "Medio Dia";
                worksheet.Cells[1, 11].Value = "Status";
                worksheet.Cells[1, 12].Value = "Comentarios Solicitante";
                worksheet.Cells[1, 13].Value = "Comentarios Jefe";
                worksheet.Cells[1, 14].Value = "Dias";

                int row = 2;
                foreach (var item in paginatedList)
                {
                    

                    worksheet.Cells[row, 1].Value = item.ClgCode;
                    worksheet.Cells[row, 2].Value = item.CntctDate;
                    worksheet.Cells[row, 3].Value = item.Actividad;
                    worksheet.Cells[row, 4].Value = item.Name;
                    worksheet.Cells[row, 5].Value = item.JefeInmediato;
                    worksheet.Cells[row, 6].Value = item.Solicitante;
                    worksheet.Cells[row, 7].Value = item.Details;
                    worksheet.Cells[row, 8].Value = item.FechaInicio;
                    worksheet.Cells[row, 9].Value = item.FechaFin;
                    worksheet.Cells[row, 10].Value = item.MedioDia;
                    worksheet.Cells[row, 11].Value = item.Status;
                    worksheet.Cells[row, 12].Value = item.ComentariosSolicitante;
                    worksheet.Cells[row, 13].Value = item.ComentariosJefe;
                    worksheet.Cells[row, 14].Value = item.Dias;

                    row++;
                }


                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;


                string excelName = $"Status_Empleados_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }


        [HttpGet]
        public ActionResult ExportarEmpleados(int? page, string findString)
        {
            List<Models.eTALENT.VACACIONES> vacaciones_periodo_empleados = new Connection.UserData.UserData().VacacionesDiaSP_All();

            
            if (!String.IsNullOrEmpty(findString))
            {
                vacaciones_periodo_empleados = vacaciones_periodo_empleados
                    .Where(s => s.EPDO_NOMBRE_COMPLETO.Contains(findString))
                    .ToList();
            }

            

            int pageSize = 500;
            int pageNumber = (page ?? 1);
            var paginatedList = vacaciones_periodo_empleados.ToPagedList(pageNumber, pageSize);

            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Resumen de Empleados");

                
                worksheet.Cells[1, 1].Value = "Código de Empleado";
                worksheet.Cells[1, 2].Value = "Nombre Completo";
                worksheet.Cells[1, 3].Value = "Fecha de Ingreso";
                worksheet.Cells[1, 4].Value = "Total Días Vacaciones";
                worksheet.Cells[1, 5].Value = "Días Gozados";
                worksheet.Cells[1, 6].Value = "Días Disponibles";

                int row = 2;
                foreach (var item in paginatedList)
                {
                    int temp_total = (int)Math.Round(item.DIAS);
                    int temp_gozados = (int)Math.Round(item.GOZADOS);
                    int temp_dias = (int)Math.Round(item.DIAS - item.GOZADOS);

                    worksheet.Cells[row, 1].Value = item.EPDO_CODIGO;
                    worksheet.Cells[row, 2].Value = item.EPDO_NOMBRE_COMPLETO;
                    worksheet.Cells[row, 3].Value = item.FECHA != null ? item.FECHA.ToString() : string.Empty;
                    worksheet.Cells[row, 4].Value = temp_total;
                    worksheet.Cells[row, 5].Value = temp_gozados;
                    worksheet.Cells[row, 6].Value = temp_dias;

                    row++;
                }

                
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                
                string excelName = $"Resumen_Empleados_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpGet]
        public ActionResult EmpleadosAll(int? page, string findString, string filterEmpleado, string filterUnidad)
        {

            List<string> Empleado = new List<string>();
            Empleado.Add("Seleccione Empleado");

            List<string> Unidad = new List<string>();
            Unidad.Add("Seleccione Unidad");

            List<Models.eTALENT.VACACIONES> vacaciones_periodo_empleados = new List<Models.eTALENT.VACACIONES>();
            vacaciones_periodo_empleados = new Connection.UserData.UserData().VacacionesDiaSP_All();

            var temp_jefe_inmediato = vacaciones_periodo_empleados.Select(x => x.EPDO_NOMBRE_COMPLETO).Distinct();
            foreach (var item_fase in temp_jefe_inmediato)
            {
                Empleado.Add(item_fase);
            }

            var temp_solicitante = vacaciones_periodo_empleados.Select(x => x.UND_NOMBRE).Distinct();
            foreach (var item_fase in temp_solicitante)
            {
                Unidad.Add(item_fase);
            }

            if (!string.IsNullOrEmpty(filterEmpleado) && filterEmpleado != "0" && filterEmpleado != "Seleccione Empleado")
            {
                vacaciones_periodo_empleados = vacaciones_periodo_empleados.Where(x => x.EPDO_NOMBRE_COMPLETO == filterEmpleado).ToList();
            }

            if (!string.IsNullOrEmpty(filterUnidad) && filterUnidad != "0" && filterUnidad != "Seleccione Unidad")
            {
                vacaciones_periodo_empleados = vacaciones_periodo_empleados.Where(x => x.UND_NOMBRE == filterUnidad).ToList();
            }

            ViewBag.Empleado = Empleado;
            ViewBag.Unidad = Unidad;

            ViewBag.findString = findString;
            ViewBag.totalItem = vacaciones_periodo_empleados.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            


            if (!String.IsNullOrEmpty(findString))
            {
                var obj = vacaciones_periodo_empleados.Where(s =>
                s.EPDO_NOMBRE_COMPLETO.ToString().Contains(findString) ||
                s.UND_NOMBRE.ToString().Contains(findString)                 
                );
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
        public ActionResult MailSign()
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

        [HttpGet]
        public ActionResult FlujogramaIsertec()
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
                message.sendMail(data.correo, requestActivity.U_Correo, requestActivity.Details + " - " + requestActivity.U_Solicitante , "", requestActivity.U_Solicitante, ViewBag.activity, "", requestActivity.ActivityType + "-" + requestActivity.U_Observaciones);
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

        [HttpGet]

        public ActionResult detailsHours(int? page, string findString, string filterString, int? Codigo, DateTime? FechaInicio, DateTime? FechaFin, string filterOrden)
        {

            List<string> Orden = new List<string>();
            Orden.Add("Seleccione Orden");

            


            if (Codigo == null)
            {
                return RedirectToAction("Index"); // O cualquier otra vista de error/mensaje
            }

            var data = new Connection.Activities.DataActivities().EmpleadosHoras(Codigo.Value);


            var temp_orden = data.Select(x => x.ORDEN).Distinct();
            foreach (var item_orden in temp_orden)
            {
                Orden.Add(item_orden);
            }

            ViewBag.Orden = Orden;
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            int pageSize = 125;
            int pageNumber = (page ?? 1);


            if (FechaInicio.HasValue && FechaFin.HasValue)
            {
                ViewBag.Inicio = FechaInicio.Value.ToString("yyyy-MM-dd");

                ViewBag.Fin = FechaFin.Value.ToString("yyyy-MM-dd");
            }
            else
            {

                ViewBag.Inicio = null;
                ViewBag.Fin = null;
            }


            if (FechaInicio.HasValue && FechaFin.HasValue)
            {


                var dataFilter = new Connection.Activities.DataActivities().EmpleadosHorasFilter(FechaInicio, FechaFin, Codigo.Value);


            }




            if (!string.IsNullOrEmpty(filterOrden) && filterOrden != "0" && filterOrden != "Seleccione Orden")
            {
                data = data.Where(x => x.ORDEN == filterOrden).ToList();
            }

            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.ORDEN.ToString().Contains(findString));
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult UpdateEmpleadoHoras(int ID, Models.eTALENT.EmpleadosHoras empleadoHoras)
        {
            try
            {
                if (empleadoHoras == null || ID == 0)
                {
                    return Json(new { success = false, mensaje = "Datos inválidos." });
                }

                bool result = new Connection.Activities.DataActivities().UpdateEmpleadosHoras(ID, empleadoHoras);

                if (result)
                {
                    return Json(new { success = true, mensaje = "Registro actualizado correctamente." });
                }
                else
                {
                    return Json(new { success = false, mensaje = "No se encontró el empleado o no se realizó la actualización." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error interno del servidor: " + ex.Message });
            }
        }




        public ActionResult marcajeEmpleados()
        {

            // Crear el DataTable para almacenar los datos
            System.Data.DataTable dtEmpleados = new System.Data.DataTable();

            // Obtener la conexión a la base de datos desde eTalentConnection
            using (SqlConnection sqlCon = eTalentConnection.connectionResult())
            {
                try
                {
                    // Comando SQL para ejecutar el procedimiento almacenado
                    string query = "EXEC AllEmployeeHours2";

                    // Usar SqlDataAdapter para obtener los datos
                    using (SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon))
                    {
                        sqlDa.Fill(dtEmpleados);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error en la base de datos: " + ex.Message;
                }
            }

            // Retornar el DataTable a la vista
            return View(dtEmpleados);

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
        public ActionResult listNomina(int? page, string findString, string filterString, string filterJefeInmediato, string filterSolicitante, string filterTipoActividad, string filterEstatus)
        {
            List<string> JefeInmediato = new List<string>();
            JefeInmediato.Add("Seleccione Jefe Inmediato");

            List<string> Solicitantes = new List<string>();
            Solicitantes.Add("Seleccione Solicitante");

            List<string> TipoActividad = new List<string>();
            TipoActividad.Add("Seleccione Tipo de Actividad");

            List<string> Estatus = new List<string>();
            Estatus.Add("Seleccione Estatus");


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

            var temp_estatus = data.Select(x => x.status).Distinct();

            foreach (var item_fase in temp_estatus)
            {
                Estatus.Add(item_fase);
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

            if (!string.IsNullOrEmpty(filterEstatus) && filterEstatus != "0" && filterEstatus != "Seleccione Estatus")
            {
                
                data = data.Where(x => x.status == filterEstatus).ToList();
            }

            ViewBag.JefeInmediato = JefeInmediato;
            ViewBag.Solicitantes = Solicitantes;
            ViewBag.TipoActividad = TipoActividad;
            ViewBag.Estatus = Estatus;
           


            ViewBag.findString = findString;
            ViewBag.totalItem = data.Count();
            ViewBag.filterString = filterString;
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s =>
                s.ClgCode.ToString().Contains(findString) ||
                s.CntctDate.ToString().Contains(findString) || 
                s.Name.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.AttendUser.ToString().Contains(findString) || 
                s.U_NAME.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || 
                s.U_Solicitante.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.DocNum.ToString().Contains(findString) || 
                s.Details.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || 
                s.ODC.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || 
                
                s.status.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0
                );
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult addInf()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult newAddInf()
        {
            return View();
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
        public ActionResult updateStatusAssign(int id, string comment, string status, string ejecutivo, string involucrados, string orden_venta, string sn, string Solicitante)
        {
            try
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
                                   !(fechaActual.Month == 11 && fechaActual.Day == 1) && !(fechaActual.Month == 9 && fechaActual.Day == 15) && !(fechaActual.Month == 3 && fechaActual.Day == 28) &&
                                   !(fechaActual.Month == 3 && fechaActual.Day == 29))
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
                        rpt.SetParameterValue("CantidadDiasVacaciones", "" + dias);
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
                        
                        message.sendNotification(ejecutivo, involucrados, "SOLICITUD_APROBADA_" + id + "_" + Solicitante, Session["nombre"].ToString(), "" + id, comment, orden_venta, sn, direct);
                    }
                    
                    else
                    {
                        
                        if (status == "4")
                        {
                            message.sendNotification(ejecutivo, involucrados, "RECHAZO_SOLICITUD_" + id + "_" + Solicitante, Session["nombre"].ToString(), "" + id, comment, orden_venta, sn, "");
                        }
                    }

                    return RedirectToAction("detailsInvoiceAssign", "Account", new { id = id });
                }
                else
                {

                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("ErrorActualizarVacaciones");
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