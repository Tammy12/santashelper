using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SantasHelper.Controllers {
    public class DurandalController : Controller {
        public ActionResult Index() {
            return View();
        }        

        public JsonResult GetAllUsers()
        {
            List<string> userNames = new List<string>();

            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand getUsersCmd = mysql.CreateCommand();
                getUsersCmd.CommandText = "SELECT * FROM users";

                mysql.Open();

                MySqlDataReader reader = getUsersCmd.ExecuteReader();
                try
                {
                    while(reader.Read())
                    {
                        userNames.Add(reader.GetString("firstname"));
                    }
                }
                finally
                {
                    reader.Close();
                    mysql.Close();
                }

            }

            return Json(userNames, JsonRequestBehavior.AllowGet);
        }
    }
}