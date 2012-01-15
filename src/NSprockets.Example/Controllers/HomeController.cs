using System.Web.Mvc;

namespace NSprockets.Example.Controllers
{
    public class HomeController: Controller
    {
         public ActionResult Index()
         {
             return View();
         }
    }
}