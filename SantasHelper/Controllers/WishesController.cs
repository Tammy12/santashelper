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
        public void AddNewWish(string itemName, string itemDesc, int itemCount, int currentUserId)
        {
            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand addwish = mysql.CreateCommand();
                addwish.CommandText = "INSERT INTO wishes (userid, itemname, description, claimed, count) VALUES (?u, ?i, ?d, 'n', ?c)";
                addwish.Parameters.AddWithValue("?u", currentUserId);
                addwish.Parameters.AddWithValue("?i", itemName);
                addwish.Parameters.AddWithValue("?d", (itemDesc != null && itemDesc != "") ? itemDesc : null);
                addwish.Parameters.AddWithValue("?c", itemCount);


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

        [HttpGet]
        public void EditExistingWish(int itemId, string itemName, string itemDesc, int itemCount, int currentUserId)
        {
            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand addwish = mysql.CreateCommand();
                addwish.CommandText = "UPDATE wishes SET userid = ?u, itemname = ?i, description = ?d, count = ?c WHERE id = ?id;";
                addwish.Parameters.AddWithValue("?id", itemId);
                addwish.Parameters.AddWithValue("?u", currentUserId);
                addwish.Parameters.AddWithValue("?i", itemName);
                addwish.Parameters.AddWithValue("?d", (itemDesc != null && itemDesc != "") ? itemDesc : null);
                addwish.Parameters.AddWithValue("?c", itemCount);


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
        /// Deletes wish item from wishes SQL table using wishid
        /// </summary>
        /// <param name="itemId"></param>
        [HttpGet] 
        public JsonResult DeleteWish(int itemId)
        {
            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand checkClaimStatus = mysql.CreateCommand();
                MySqlCommand deletewish = mysql.CreateCommand();

                checkClaimStatus.CommandText = "SELECT claimed FROM wishes WHERE id = ?id;";
                deletewish.CommandText = "DELETE FROM wishes WHERE id = ?id;";

                checkClaimStatus.Parameters.AddWithValue("?id", itemId);
                deletewish.Parameters.AddWithValue("?id", itemId);
                
                mysql.Open();

                try
                {
                    var result = checkClaimStatus.ExecuteScalar();
                    if (result != null && Convert.ToString(result) == "n")
                    {
                        deletewish.ExecuteNonQuery();
                        return Json(new { success = true}, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { success = false, message = "Can't delete. Someone already claimed this item!" }, JsonRequestBehavior.AllowGet);

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
            List<string> wishDescriptions = new List<string>();
            List<bool> wishClaimed = new List<bool>();
            List<int> wishCount = new List<int>();


            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand retrieveWishes = mysql.CreateCommand();
                retrieveWishes.CommandText = "SELECT id, itemname, description, claimed, count FROM wishes WHERE userid = ?u";
                retrieveWishes.Parameters.AddWithValue("?u", currentUserId);

                mysql.Open();

                MySqlDataReader reader = retrieveWishes.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        wishIds.Add(reader.GetInt32("id"));
                        if (!reader.IsDBNull(reader.GetOrdinal("itemname")))
                            wishNames.Add(reader.GetString("itemname"));
                        else
                            wishNames.Add("");

                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            wishDescriptions.Add(reader.GetString("description"));
                        else
                            wishDescriptions.Add("");

                        wishCount.Add(reader.GetInt32("count"));
                        if (reader.GetChar("claimed") == 'y')
                            wishClaimed.Add(true);
                        else
                            wishClaimed.Add(false);
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

                return Json( new { success = true, ids = wishIds, names = wishNames, descriptions = wishDescriptions, counts = wishCount, claimed = wishClaimed }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets all wishes claimed by the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetClaimedList(int currentUserId)
        {
            List<string> friendNames = new List<string>();
            List<string> wishNames = new List<string>();
            List<string> wishDescriptions = new List<string>();
            List<int> wishCounts = new List<int>();

            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand getclaims = mysql.CreateCommand();
                getclaims.CommandText = @"SELECT users.firstname, users.lastname, wishes.itemname, wishes.description, wishes.count
                                        FROM wishes
                                        LEFT JOIN users
                                        ON wishes.userid = users.id
                                        WHERE wishes.claimedby = ?u";
                getclaims.Parameters.AddWithValue("?u", currentUserId);

                mysql.Open();
                MySqlDataReader reader = getclaims.ExecuteReader();


                try
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("firstname")) && !reader.IsDBNull(reader.GetOrdinal("lastname")))
                            friendNames.Add(reader.GetString("firstname") + " " + reader.GetString("lastname"));
                        else
                            friendNames.Add("");

                        if (!reader.IsDBNull(reader.GetOrdinal("itemname")))
                            wishNames.Add(reader.GetString("itemname"));
                        else
                            wishNames.Add("");

                        if (!reader.IsDBNull(reader.GetOrdinal("description")))
                            wishDescriptions.Add(reader.GetString("description"));
                        else
                            wishDescriptions.Add("");

                        wishCounts.Add(reader.GetInt32("count"));
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

                return Json(new { success = true, friends = friendNames, wishes = wishNames, descriptions = wishDescriptions, counts = wishCounts}, JsonRequestBehavior.AllowGet);
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