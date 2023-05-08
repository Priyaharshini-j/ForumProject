using ForumProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ForumProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        IConfiguration configuration;
        SqlConnection connection;
        IHttpContextAccessor Context;
        public AdminController(IConfiguration _configuration, IHttpContextAccessor context)
        {
            configuration = _configuration;
            connection = new SqlConnection(configuration.GetConnectionString("OnlineForum"));
            Context = context;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<List<UsersModel>> Get()
        {
            connection.Open();
            try
            {
                List<UsersModel> users = new List<UsersModel>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users", connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UsersModel usersModel = new UsersModel();
                    usersModel.Id = (int)reader[0];
                    usersModel.Name = (string)reader[1];
                    usersModel.Email = (string)reader[2];
                    usersModel.securityQn = (string)reader[4];
                    usersModel.securityAns = (string)reader[5];
                    users.Add(usersModel);
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return new List<UsersModel>();
        }

        [HttpGet("forums")]
        public ActionResult<List<ForumModel>> GetForums()
        {
            connection.Open();
            try
            {
                List<ForumModel> forumModels = new List<ForumModel>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Forum", connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ForumModel forum = new ForumModel();
                    forum.forumId =(int) reader[0];
                    forum.Email = (string)reader[1];
                    forum.category = (string)reader[2];
                    forum.title = (string)reader[3];
                    forum.content = (string)reader[4];
                    forumModels.Add(forum);
                }
                return forumModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<ForumModel>();
            }
        }
        [HttpGet("polls")]
        public ActionResult<List<UserPoll>> GetPolls()
        {
            connection.Open();
            try
            {
                List<UserPoll> userPolls = new List<UserPoll>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserPoll", connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserPoll poll = new UserPoll();
                    poll.Id = (int)reader[0];
                    poll.Email = (string)reader[1];
                    poll.Title = (string)reader[2];
                    poll.Category = (string)reader[3];
                    poll.Question = (string)reader[4];
                    userPolls.Add(poll);
                }
                return userPolls;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<UserPoll>();
            }
        }

    }
}
