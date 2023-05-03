using ForumProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;

namespace ForumProject.Controllers
{
    public class UserController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;
                private readonly IHttpContextAccessor Context;

        public UserController (IConfiguration configuration, IHttpContextAccessor context)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("OnlineForum"));
            Context = context;
        }
        // GET: UserController
        public ActionResult DiscussionList(string Id)
        {
            
            return View(GetForumById(Id));
            
        }

        public List<ForumModel> GetForumById (string Email)
        {
            _connection.Open();
            List<ForumModel> forums = new List<ForumModel>();
            try
            {
                SqlCommand cmd = new SqlCommand("ForumById", _connection);
                Console.WriteLine("Connected"); 
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);
                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("Exec");
                while (reader.Read())
                {
                    Console.WriteLine("Reading");
                    ForumModel model = new ForumModel();
                    model.forumId = (int)reader[0];
                    model.Email = (string)reader[1];
                    model.category = (string)reader[2];
                    model.title = (string)reader[3];
                    model.content = (string)reader[4];
                    model.forumCreated = (DateTime)reader[5];
                    forums.Add(model);
                }
                if (forums.Count > 0)
                {
                    Console.WriteLine("Inside the count > 0");
                    return forums;
                }
                else
                {
                    Console.WriteLine("Inside the count = 0");
                    return forums;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Catch");
                ModelState.AddModelError(string.Empty, ex.Message);
                return forums;
            }
        }


        [HttpPost]
        public ActionResult<List<ForumModel>> DiscussionList(ForumModel forum, string searchString)
        {
            _connection.Open(); 
            List<ForumModel> forums = new List<ForumModel>();
            try
            {
                SqlCommand cmd = new SqlCommand("ForumById", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", forum.Email);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    ForumModel model = new ForumModel();
                    model.forumId = (int)reader[0];
                    model.Email = (string)reader[1];
                    model.content = (string)reader[2];
                    model.title = (string)reader[3];
                    model.content = (string)reader[4];
                    model.forumCreated = (DateTime)reader[5];
                    forums.Add(model);
                }
                List<ForumModel> search = new List<ForumModel>();
                foreach(ForumModel model in forums)
                {
                    if(model.title.Contains(searchString) || model.category.Contains(searchString) || model.content.Contains(searchString))
                    {
                        search.Add(model);
                    }
                }
                return View(search);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        public ActionResult<List<ForumModel>> ForumList()
        {
            return View(GetAllForums());
        }
        [HttpPost]
        public ActionResult <List<ForumModel>> ForumList(string searchString, ForumModel forum)
        {
            _connection.Open();
            List<ForumModel> forums = new List<ForumModel>();
            try
            {
                SqlCommand cmd = new SqlCommand("FetchAllForum", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    ForumModel model = new ForumModel();
                    model.forumId = (int)reader[0];
                    model.Email = (string)reader[1];
                    model.content = (string)reader[2];
                    model.title = (string)reader[3];
                    model.content = (string)reader[4];
                    model.forumCreated = (DateTime)reader[5];
                    forums.Add(model);
                }
                List<ForumModel> search = new List<ForumModel>(); 
                string searchLower = searchString.ToLower();
                // convert search string to lowercase
                
                foreach (ForumModel model in forums)
                {
                    Console.WriteLine(model.title + " " + model.content);
                    if (model.title.ToLower().Contains(searchLower))
                    {
                        search.Add(model);
                        Console.WriteLine(search.Count);
                    }
                    
                    else if (model.content.ToLower().Contains(searchLower))
                    {

                        search.Add(model);
                        Console.WriteLine(search.Count);
                    }
                    else 
                    {
                        Console.WriteLine("In else block");
                        continue;
                    }
                }

                return View(search);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }

        public List<ForumModel> GetAllForums()
        {
            List<ForumModel> alldiscussion = new();
            _connection.Open();
            try
            {
                SqlCommand cmd1 = new SqlCommand("FetchAllForum", _connection);
                cmd1.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    ForumModel model = new ForumModel();
                    model.forumId = (int)dr1[0];
                    model.Email = (string)dr1[1];
                    model.category = (string)dr1[2];
                    model.title = (string)(dr1[3]);
                    model.content = dr1.GetString(4);
                    model.forumCreated = (DateTime)dr1[5];
                    alldiscussion.Add(model);
                }
                dr1.Close();
                _connection.Close();
                return alldiscussion;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return alldiscussion;
            }
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ForumCreationModel discuss)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("InsertForum", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", discuss.Email);
                cmd.Parameters.AddWithValue("@Category", discuss.Category);
                cmd.Parameters.AddWithValue("@Title", discuss.Title);
                cmd.Parameters.AddWithValue("@Content", discuss.Content);
                cmd.Parameters.AddWithValue("@CreatedDate", discuss.CreatedDate);
                cmd.ExecuteNonQuery();
                _connection.Close();
                return RedirectToAction("ForumList", "User");
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError (string.Empty, ex.Message);
                return View();
            }
        }


        // GET: DiscussionController/Details/5
        public IActionResult AddReply(int id)
        {

            var discussion = GetDiscussionById(id);
            var replyList = GetRepliesByDiscussionId(id);

            if (discussion == null)
            {
                return NotFound();
            }

            ViewBag.Discussion = discussion;
            ViewBag.Replies = replyList;

            var model = new DiscussionViewModel
            {
                forum = discussion,
                replies = replyList,
                newReply = new RepliesModel { forumId = id }
            };

            return View(model);
        }

        // POST: DiscussionController/Details/5
        [HttpPost]
        public ActionResult AddReply(RepliesModel newReply)
        {
            if (ModelState.IsValid)
            {
                var connection = new SqlConnection(_configuration.GetConnectionString("OnlineForum"));
                connection.Open();
                string query = "INSERT INTO Replies VALUES (@DiscussionId, @Email, @Content, @ReplyCreated)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@DiscussionId", newReply.forumId);
                cmd.Parameters.AddWithValue("@Email", newReply.Email);
                cmd.Parameters.AddWithValue("@Content", newReply.replyContent);
                cmd.Parameters.AddWithValue("@ReplyCreated", DateTime.Now);

                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction("AddReply", new { id = newReply.forumId });
            }



            // If model state is invalid, return to the same page with the model to show validation errors
            var discussion = GetDiscussionById(newReply.forumId);
            var replies_List = GetRepliesByDiscussionId(newReply.forumId);

            var model = new DiscussionViewModel
            {
                forum = discussion,
                replies = replies_List,
                newReply = newReply
            };

            return View("Details", model);
        }

        private ForumModel GetDiscussionById(int id)
        {
            ForumModel discussion = null;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("OnlineForum")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Forum WHERE ForumId=@Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    discussion = new ForumModel
                    {
                        forumId = (int)dr["ForumId"],
                        Email = (string)dr["Email"],
                        category = (string)dr["Category"],
                        title = (string)dr["Title"],
                        content = (string)dr["Content"],
                        forumCreated = (DateTime)dr["CreatedDate"]
                    };
                }
                dr.Close();
            }
            if(discussion != null)
            {
                return discussion;
            }
            return new ForumModel();
        }

        private List<RepliesModel> GetRepliesByDiscussionId(int id)
        {
            List<RepliesModel> replies = new List<RepliesModel>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("OnlineForum")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Replies WHERE ForumId=@ForumId ORDER BY ReplyCreated DESC", connection);
                cmd.Parameters.AddWithValue("@ForumId", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    RepliesModel reply = new RepliesModel
                    {
                        replyId = (int)dr["ReplyId"],
                        forumId = (int)dr["ForumId"],
                        Email = (string)dr["Email"],
                        replyContent = (string)dr["ReplyContent"],
                        replyCreated = (DateTime)dr["ReplyCreated"]
                    };
                    replies.Add(reply);
                }
                dr.Close();
            }

            return replies;
        }
    }
}
