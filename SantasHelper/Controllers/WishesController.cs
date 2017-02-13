using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SantasHelper.Controllers
{
    public class WishesController : Controller
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
            List<int> wishIds = new List<int>();
            List<string> wishNames = new List<string>();

            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand retrieveWishes = mysql.CreateCommand();
                retrieveWishes.CommandText = "SELECT id, itemname FROM wishes WHERE userid = ?u";
                retrieveWishes.Parameters.AddWithValue("?u", currentUserId);

                mysql.Open();

                MySqlDataReader reader = retrieveWishes.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        wishNames.Add(reader.GetString("itemname"));
                        wishIds.Add(reader.GetInt32("id"));
                    }
                }
                catch(MySqlException ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    reader.Close();
                    mysql.Close();
                }

                return Json( new { success = true, ids = wishIds, names = wishNames }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Returns description of wish given wishid
        /// </summary>
        [HttpGet]
        public JsonResult GetWishDescription(int wishid)
        {
            string description;
            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand returnDescription = mysql.CreateCommand();
                returnDescription.CommandText = "SELECT description FROM wishes WHERE id = ?i";
                returnDescription.Parameters.AddWithValue("?i", wishid);

                mysql.Open();

                try
                {
                    //wish ids are unique, so can only return <= 1
                    var result = returnDescription.ExecuteScalar();
                    if (result != null)
                    {
                        description = Convert.ToString(result);
                    }
                    else
                    {
                        description = null;
                    }
                }
                catch (MySqlException ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    mysql.Close();
                }

                return Json(new { success = true, description = description }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Updates selected wish 'claimed' to yes, claimedby current user
        /// </summary>
        /// <param name="wishid"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ClaimWish(int wishid, int currentUserId)
        {
            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand claim = mysql.CreateCommand();
                claim.CommandText = "UPDATE wishes SET claimed = 'y', claimedby = ?cb WHERE id = ?i";
                claim.Parameters.AddWithValue("?i", wishid);
                claim.Parameters.AddWithValue("?cb", currentUserId);

                mysql.Open();

                try
                {
                    claim.ExecuteNonQuery();
                    return Json(new { success = true, message = "Your claim was successful!" }, JsonRequestBehavior.AllowGet);
                }
                catch (MySqlException ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    mysql.Close();
                }
            }
        }
    }
}