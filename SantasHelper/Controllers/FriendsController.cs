using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SantasHelper.Controllers
{
    public class Friend
    {
        public string firstname { get; set; }
        public int id { get; set; }
    }

    public class FriendsController : Controller
    {
        public JsonResult GetFriendUsers(int currentUserId)
        {
            List<Friend> users = new List<Friend>();

            string connectionString = "Server=localhost;Database=santashelper;Uid=root;Pwd=Ghmar01!;";
            using (MySqlConnection mysql = new MySqlConnection(connectionString))
            {
                MySqlCommand getUsersCmd = mysql.CreateCommand();
                getUsersCmd.CommandText = "SELECT * FROM users WHERE id <> ?u";
                getUsersCmd.Parameters.AddWithValue("?u", currentUserId);

                mysql.Open();

                MySqlDataReader reader = getUsersCmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Friend friend = new Friend();
                        friend.firstname = reader.GetString("firstname");
                        friend.id = reader.GetInt32("id");
                        users.Add(friend);
                    }
                }
                finally
                {
                    reader.Close();
                    mysql.Close();
                }

            }

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}