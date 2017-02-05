using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SantasHelper.Controllers {
    public class DurandalController : Controller {
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        public void CreateNewUser(string firstname, string lastname, string email, string password)
        {
            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand createUser = mysql.CreateCommand();
                createUser.CommandText = "INSERT INTO users (id, firstname, lastname, email, password) VALUES (NULL, ?f, ?l, ?e, ?p)";
                createUser.Parameters.AddWithValue("?f", firstname);
                createUser.Parameters.AddWithValue("?l", lastname);
                createUser.Parameters.AddWithValue("?e", email);
                createUser.Parameters.AddWithValue("?p", password);

                mysql.Open();

                try
                {
                    createUser.ExecuteNonQuery();
                }
                finally
                {
                    mysql.Close();
                }

            }
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