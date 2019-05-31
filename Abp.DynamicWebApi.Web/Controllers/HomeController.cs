using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace Abp.DynamicWebApi.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : DynamicWebApiControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}