using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SantasHelper.Controllers
{
    public class RegisterController : Controller
    {
        /// <summary>
        /// Checks to see if user's login info matches database.
        /// If yes, set currentID. If no, return error message.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        [HttpGet]
        public JsonResult LoginUser(string email, string password)
        {
            int currentUserId = -1;

            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand getUser = mysql.CreateCommand();
                getUser.CommandText = "SELECT * FROM users WHERE email = ?e and password = ?p";
                getUser.Parameters.AddWithValue("?e", email);
                getUser.Parameters.AddWithValue("?p", password);

                mysql.Open();

                MySqlDataReader reader = getUser.ExecuteReader();
                try
                {
                    //should only have one result max because email is unique in db
                    reader.Read();
                    if(reader.HasRows)
                    {
                        currentUserId = reader.GetInt32("id");
                    }
                }
                finally
                {
                    reader.Close();
                    mysql.Close();
                }

                return Json(currentUserId, JsonRequestBehavior.AllowGet);

            }
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
    }
}