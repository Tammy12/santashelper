using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SantasHelper.Controllers
{
    public class MyListController : Controller
    {
        //// GET: MyList
        //public ActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// Updates wishes table with new item from current user
        /// </summary>
        /// <param name="item">Name of the wish item</param>
        [HttpGet]
        public void AddNewWish(string item, int currentUserId)
        {
            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand addwish = mysql.CreateCommand();
                addwish.CommandText = "INSERT INTO wishes (userid, itemname, claimed) VALUES (?u, ?i, 'n')";
                addwish.Parameters.AddWithValue("?u", currentUserId);
                addwish.Parameters.AddWithValue("?i", item);

                mysql.Open();

                try
                {
                    addwish.ExecuteNonQuery();
                }
                finally
                {
                    mysql.Close();
                }

            }
        }

        /// <summary>
        /// Returns all items associated with current user from wishes table
        /// </summary>
        [HttpGet]
        public JsonResult GetWishlist(int currentUserId)
        {
            List<string> wishes = new List<string>();

            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand retrieveWishes = mysql.CreateCommand();
                retrieveWishes.CommandText = "SELECT itemname FROM wishes WHERE userid = ?u";
                retrieveWishes.Parameters.AddWithValue("?u", currentUserId);

                mysql.Open();

                MySqlDataReader reader = retrieveWishes.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        wishes.Add(reader.GetString(0));
                    }
                }
                finally
                {
                    reader.Close();
                    mysql.Close();
                }

                return Json(wishes, JsonRequestBehavior.AllowGet);
            }
        }
    }
}