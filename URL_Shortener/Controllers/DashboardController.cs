using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace URL_Shortener.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Display()
        {
            return View();
        }
    }
}
