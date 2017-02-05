using System.Web.Mvc;

namespace SantasHelper.Controllers {
    public class DurandalController : Controller {
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        public void CreateNewUser(string firstname, string lastname, string email, string password)
        {

        }
    }
}