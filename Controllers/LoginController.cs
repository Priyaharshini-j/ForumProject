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

namespace ForumProject.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration configuration;
        SqlConnection connection;
        IHttpContextAccessor Context;

        int userId;
        public LoginController(IConfiguration _configuration, IHttpContextAccessor context)
        {
            configuration = _configuration;
            connection = new SqlConnection(configuration.GetConnectionString("OnlineForum"));
            Context = context;
        }

        // GET: LoginController1/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: LoginController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsersModel user)
        {
            try
            {
                
                connection.Open();
                string query = $"SELECT * FROM Users WHERE Email=@Email AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query,connection);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password",user.Password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string name = (string)reader["Name"];
                    string email = (string)reader["Email"];
                    string psw = (string)reader["Password"];
                    if(user.Email==email && user.Password == psw)
                    {
                        Context.HttpContext.Session.SetString("UserName", name);
                        return RedirectToAction("DiscussionList", "User", new {Id = user.Id});
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
        //GET: LoginController/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: LoginController1/Create
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

        //GET: LoginController/SignUp
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: LoginController1/Create
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
                        int userId = reader.GetInt32(reader.GetOrdinal("Id"));
                        return RedirectToAction("DiscussionList", "User", new {Id=user.Id});
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
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return View();
            }
        }


        // GET: LoginController1/Edit/5
        public ActionResult EditProfile(int id)
        {
            return View();
        }

        // POST: LoginController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(int id, UsersModel user)
        {
            try
            {
                return RedirectToAction("DiscussionList","Login");
            }
            catch
            {
                return View();
            }
        }
        

        // GET: LoginController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
