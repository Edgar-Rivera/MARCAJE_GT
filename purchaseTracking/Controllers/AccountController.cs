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

namespace purchaseTracking.Controllers
{
    [SessionExpireFilter]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult createSignDigital()
        {
            Models.Images.ImageSign temp = new Models.Images.ImageSign();
            temp = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32(Session["code"]));
            ViewBag.source = temp.U_PathSign;
            ViewBag.nombre = temp.U_Nombre;
            return View();
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

            List<Models.eTALENT.VACACIONES_DISPONIBLES> vacaciones = new List<Models.eTALENT.VACACIONES_DISPONIBLES>();
            vacaciones = new Connection.UserData.UserData().VacacionesDia(Convert.ToInt32(Session["external_code"]));

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
            return View();
        }

        [HttpGet]
        public ActionResult VacationsRequest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult newRequest(Models.Activities.RequestActivity requestActivity)
        {
            // VARIABLES DEL SERVICE LAYER ESTATICAS    
            requestActivity.DurationType = "du_Seconds";
            requestActivity.U_internalKey = Session["code"].ToString();
            // METODO QUE RECIBE EL MODULO Y HACE EL POST EN SAP   
            if (new ServiceLayer.Activity.ActivityComponents().addActivity(requestActivity))
            {
                // VALIDA LA CREACION DE LA ACTIVIDAD EN SAP Y ENVIA CORREO ELECTORNICO
                var data = new Connection.Activities.DataActivities().GetEjecutivo(requestActivity.HandledBy);
                ViewBag.email_to = data.correo;
                ViewBag.email_bcc = requestActivity.U_Correo;
                ViewBag.activity = new purchaseTracking.Connection.Activities.DataActivities().getID();
                SendMailer message = new SendMailer();

             
                // RUTINA PARA CREAR PDF APARTIR DE FORMATO CRYSTAL REPORTS
                string direct = string.Empty;
                ReportDocument rpt = new ReportDocument();
                rpt = new VACACIONES();
                rpt.SetDatabaseLogon("sa","manag3RS");

                // DATA SOURCE 
                
                rpt.SetParameterValue("@FECHA",requestActivity.StartDate);
                rpt.SetParameterValue("@CODEPDO", Session["internal_code"]);
                rpt.SetParameterValue("MotivoCambio", "");
                rpt.SetParameterValue("FechaFin", requestActivity.U_FechaActualizacion);
                rpt.SetParameterValue("CantidadDiasVacaciones", 15);
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
                message.sendMail(data.correo, requestActivity.U_Correo, requestActivity.Details, "", requestActivity.U_Solicitante, ViewBag.activity, direct);
                return View("Success");
            }
            else
            {
                return View("Error");
            }

        }


        /* SOLICITUDES PUESTAS POR USUARIO */
        [HttpGet]
        public ActionResult RequestInvoice(int? page, string findString, string filterString)
        {
            List<Models.Activities.List> data = new List<Models.Activities.List>();

            if (!string.IsNullOrEmpty(filterString))
            {
                data = new Connection.Activities.DataActivities().getListRequestInvoiceNonEstatus(Convert.ToInt32(Session["code"]));
                data = (from t in data where t.Estado.ToString() == filterString select t).ToList();
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
        public ActionResult detailsInvoice(int id)
        {
            Models.Images.ImageSign temp = new Models.Images.ImageSign();
            temp = new purchaseTracking.Connection.Activities.DataActivities().GetSignTechnician(Convert.ToInt32(Session["code"]));
            ViewBag.source = temp.U_PathSign;
            ViewBag.nombre = temp.U_Nombre;
            Models.Activities.details data = new Connection.Activities.DataActivities().getDetailsInvoice(id);
            return View(data);
        }

        [HttpGet]
        public ActionResult listInvoice(int? page, string findString, string filterString)
        {

            List<Models.Activities.List> data = new List<Models.Activities.List>();
            if (!string.IsNullOrEmpty(filterString))
            {
                data = new Connection.Activities.DataActivities().getListAllNonStatusInvoice_A(Convert.ToInt32(Session["code"]));
                data = (from t in data where t.Estado.ToString() == filterString select t).ToList();
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
    }
}