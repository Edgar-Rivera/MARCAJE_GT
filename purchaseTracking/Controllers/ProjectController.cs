using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace purchaseTracking.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult list(int? page, string findString, string filterString)
        {      
            // GET HEADER INFORMATION         
            List<Models.Project.FinancialProject> data;
            if (!string.IsNullOrEmpty(filterString))
            {

                data = new Connection.Projet.DataProject().GetProjectsFilter(filterString);
            }
            else
            {
                if (!string.IsNullOrEmpty(findString))
                {
                    data = new Connection.Projet.DataProject().GetProjectsAll();
                }
                else
                {
                    data = new Connection.Projet.DataProject().GetProjects();
                }
            }
            ViewBag.findString = findString;
            ViewBag.filterString = filterString;
            ViewBag.totalItem = data.Count();
            int pageSize =80;
            int pageNumber = (page ?? 1);
            if (!String.IsNullOrEmpty(findString))
            {
                var obj = data.Where(s => s.SlpName.ToString().Contains(findString) || s.DocNum.ToString().Contains(findString) || s.Comments.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                s.PrjCode.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.TipoProyecto.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 || s.CardName.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
                return View(obj.ToPagedList(pageNumber, pageSize));
            }
            return View(data.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult CostingProjects(string PrjCode)     
        {
            // OBTIENE DETALLES DE COSTEO DE PROYECTOS
            ViewBag.headerProject = new Connection.Projet.DataProject().GetProjectsHeader(PrjCode);
            // DATOS DE ORDENES DE VENTA
            ViewBag.ordr = new Connection.Projet.DataProject().GetSalesOrders(PrjCode);
            return View();  
        }
        [HttpGet]
        public ActionResult SalesOrderDetails(string PrjCode)
        {
            ViewBag.Project = PrjCode;
            ViewBag.ordr = new Connection.Projet.DataProject().GetSalesOrders(PrjCode);
            return View();
        }
    }
}