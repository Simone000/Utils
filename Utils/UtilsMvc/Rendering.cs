using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UtilsMvc
{
    public static class Rendering
    {
        /// <summary>
        /// var html = MVCUTILS.RenderPartialViewToString(this, "ReportMedicina", ReportModel);
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="ViewName"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static string RenderPartialViewToString(Controller Controller, string ViewName, object Model)
        {
            Controller.ViewData.Model = Model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(Controller.ControllerContext, ViewName);
                ViewContext viewContext = new ViewContext(Controller.ControllerContext, viewResult.View, Controller.ViewData, Controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }
    }
}
