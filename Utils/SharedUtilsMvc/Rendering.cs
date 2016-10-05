using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SharedUtilsMvc
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

        /// <summary>
        /// var html = TempUtils.RenderViewToString("Templates", "FatturaTemplate2", templateModel);
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="viewName"></param>
        /// <param name="viewData"></param>
        /// <returns></returns>
        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                fakeControllerContext.Controller.ControllerContext = fakeControllerContext;
                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }

        private class FakeController : ControllerBase { protected override void ExecuteCore() { } }
    }
}
