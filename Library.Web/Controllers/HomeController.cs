using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
