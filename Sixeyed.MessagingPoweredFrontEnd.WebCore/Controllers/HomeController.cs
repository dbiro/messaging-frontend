using Microsoft.AspNetCore.Mvc;
using Sixeyed.MessagingPoweredFrontEnd.WebCore.Models;
using System.Diagnostics;

namespace Sixeyed.MessagingPoweredFrontEnd.WebCore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string user)
        {
            var model = new MailModel
            {
                Sender = user
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
