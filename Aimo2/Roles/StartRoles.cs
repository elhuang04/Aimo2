using Microsoft.AspNetCore.Mvc;

namespace Aimo2.Roles
{
    public class StartRoles : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
