using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }
    }
}