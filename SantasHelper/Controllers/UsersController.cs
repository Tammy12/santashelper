﻿using MySql.Data.MySqlClient;
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

    public class UsersController : Controller
    {
        /// <summary>
        /// Returns all users except for current user
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns>users' ids and names</returns>
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
                    if (reader.HasRows)
                    {
                        currentUserId = reader.GetInt32("id");
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error: Valid login missing id." }, JsonRequestBehavior.AllowGet);
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

                return Json(new { success = true, id = currentUserId }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public JsonResult CreateNewUser(string firstname, string lastname, string email, string password)
        {
            int currentUserId = -1;

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
                    currentUserId = GetNewUserId(mysql, email);
                }
                catch(MySqlException ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    mysql.Close();
                }

                return Json(new { success = true, id = currentUserId, message = "Success Registering New User!"}, JsonRequestBehavior.AllowGet);
            }
        }

        private int GetNewUserId(MySqlConnection mysql, string email)
        {
            int currentUserId = -1;

            MySqlCommand getId = mysql.CreateCommand();
            getId.CommandText = "SELECT id FROM users WHERE email = ?e";
            getId.Parameters.AddWithValue("?e", email);

            MySqlDataReader reader = getId.ExecuteReader();
            try
            {
                //should only have one result max because email is unique in db
                reader.Read();
                if (reader.HasRows)
                {
                    currentUserId = reader.GetInt32("id");
                }
            }
            finally
            {
                reader.Close();
            }

            return currentUserId;
        }
    }
}