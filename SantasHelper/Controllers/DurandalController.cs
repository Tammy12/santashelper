using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SantasHelper.Controllers {
    public class DurandalController : Controller {
        public ActionResult Index() {
            return View();
        }        
    }
}