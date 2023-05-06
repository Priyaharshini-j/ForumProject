using ForumProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Drawing;
using System.Xml.Linq;

namespace ForumProject.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration configuration;
        SqlConnection connection;
        IHttpContextAccessor Context;

        //This part is user  to set the Session
        public LoginController(IConfiguration _configuration, IHttpContextAccessor context)
        {
            configuration = _configuration;
            connection = new SqlConnection(configuration.GetConnectionString("OnlineForum"));
            Context = context;
        }



        //This is get function of Login Page
        // GET: LoginController/Login
        public ActionResult Login()
        {
            return View();
        }


        //This is the POST Function of Login
        // POST: LoginController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersModel user)
        {
            try
            {
                connection.Open();
                //In here we are selecting the record with the the entered email and password
                string query = $"SELECT * FROM Users WHERE Email=@Email AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query,connection);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password",user.Password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int userId = (int) reader["Id"]; 
                    string name = (string)reader["Name"];
                    string email = (string)reader["Email"];
                    string psw = (string)reader["Password"];
                    if(user.Email==email && user.Password == psw)
                    {
                        //Setting the session with username and Id
                        Context.HttpContext.Session.SetString("UserName", name);
                        Context.HttpContext.Session.SetInt32("UserId", userId);
                        Context.HttpContext.Session.SetString("UserEmail", email);
                        return RedirectToAction("DiscussionList", "User", new { Id = user.Email});
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong Password/Email");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                    return View();
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError (string.Empty, exception.Message);
                return View();
            }
        }
        //Get Method of SignUp Page
        //GET: LoginController/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        //POST Method of SignUp Page
        // POST: LoginController1/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UsersModel user)
        {
            try
            {
                connection.Open();
                Console.WriteLine(user.Name);
                Console.WriteLine(user.Email);
                Console.WriteLine(user.Password);
                Console.WriteLine(user.securityQn);
                Console.WriteLine(user.securityQn);
                Console.WriteLine(user.securityAns);
                
                if(connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("InsertUser", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@SecurityQn", user.securityQn);
                    command.Parameters.AddWithValue("@SecurityAns", user.securityAns);

                    command.ExecuteNonQuery();

                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Connection state is not open");
                }
                connection.Close();
                return View();
            }
            catch(Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
                return View();
            }
        }




        //GET: LoginController/ForgotPassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: LoginController/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UsersModel user)
        {
            try
            {
                connection.Open();
                string query = $"SELECT * FROM Users WHERE Email=@Email AND SecurityQn=@securityQn AND SecurityAns=@securityAns";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@SecurityQn", user.securityQn);
                cmd.Parameters.AddWithValue("@securityAns", user.securityAns);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string qn = (string)reader["SecurityQn"];
                    string ans = (string)reader["SecurityAns"];
                    if (user.securityQn == qn && user.securityAns == ans)
                    {

                        Context.HttpContext.Session.SetString("UserEmail", user.Email);
                        int userID = (int)reader["Id"];
                        string userName = (string)reader["Name"];
                        Context.HttpContext.Session.SetString("UserName", userName);
                        Context.HttpContext.Session.SetInt32("UserId", userID);
                        return RedirectToAction("DiscussionList", "User", new {Id= user.Email});
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong Password/Email");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email or Security Question/Answer");
                    return View();
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return View();
            }
        }


        // GET: LoginController/EditProfile
        public ActionResult EditProfile(int id)
        {
            return View();
        }

        // POST: LoginController/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(int id, UsersModel user)
        {
            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("EditUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@SecurityQn", user.securityQn);
                    cmd.Parameters.AddWithValue("@SecurityAns", user.securityAns);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return RedirectToAction("DiscussionList", "User", new { Id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }


        // GET: LoginController/DeleteProfile
        // GET: LoginController1/Delete/5
        public ActionResult DeleteProfile(int id)
        {
            return View(GetUserById(id));
        }

        public UsersModel GetUserById(int id)
        {
            connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("GetUserById", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                UsersModel userDetails = new UsersModel();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();

                    userDetails.Id = (int)reader["Id"];
                    userDetails.Name = (string)reader["Name"];
                    userDetails.Email = (string)reader["Email"];
                    userDetails.Password = (string)reader["Password"];
                    userDetails.securityQn = (string)reader["SecurityQn"];
                    userDetails.securityAns = (string)reader["SecurityAns"];

                }
                return userDetails;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return null;
            }
        }

        // POST: LoginController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProfile(int id, IFormCollection collection)
        {
            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DeleteUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                }
                return RedirectToAction(nameof(Login));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
