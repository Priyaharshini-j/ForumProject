using ForumProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Policy;

namespace ForumProject.Controllers
{
    public class UserController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _connection;

        private readonly IHttpContextAccessor _contxt;

        public UserController (IConfiguration configuration, IHttpContextAccessor context)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("OnlineForum"));
            _contxt = context;
        }
        // GET: UserController
        public ActionResult DiscussionList(int Id)
        {
            
            return View(GetForumById(Id));
            
        }

        public List<ForumModel> GetForumById (int Id)
        {
            _connection.Open();
            List<ForumModel> forums = new List<ForumModel>();
            try
            {
                Console.WriteLine("Inside try block");
                var query = "SELECT * FROM Forum WHERE @Id= Id ";

                Console.WriteLine("Inside query");
                SqlCommand cmd = new SqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("executer query query");
                while (reader.Read())
                {

                    ForumModel model = new ForumModel();
                    model.forumId = (int)reader[0];
                    model.Id = (int)reader[1];
                    model.category = (string)reader[2];
                    model.title = (string)reader[3];
                    model.content = (string)reader[4];
                    model.forumCreated = (DateTime)reader[5];
                    forums.Add(model);
                }
                Console.WriteLine("Outside if");
                if (forums.Count > 0)
                {

                    Console.WriteLine("Inside if");
                    return forums;
                }
                else
                {

                    Console.WriteLine("Inside else");
                    return forums;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Inside catch");
                ModelState.AddModelError(string.Empty, ex.Message);
                return forums;
            }
        }


        [HttpPost]
        public ActionResult<List<ForumModel>> DiscussionList(ForumModel forum, string searchString)
        {
            _connection.Open(); List<ForumModel> forums = new List<ForumModel>();
            try
            {
                //AND Title.Contains(searchString) || Content.Contains(searchString) || Category.Contains(searchString)
                //var query = "SELECT * FROM Forums WHERE @Id= Id ";
                SqlCommand cmd = new SqlCommand("ForumbyId", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", forum.Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    ForumModel model = new ForumModel();
                    model.forumId = (int)reader[0];
                    model.Id = (int)reader[1];
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
                    model.Id = (int)dr1[1];
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

        //GET:UserController/ForumAndReplies
        public ActionResult ForumAndReplies(int id)
        {
            var discussion = GetDiscussionById(id);
            var replies = GetRepliesByDiscussionId(id);

            if (discussion == null)
            {
                return NotFound();
            }

            ViewBag.Discussion = discussion;
            ViewBag.Replies = replies;

            var model = new DiscussionViewModel
            {
                forum = discussion,
                repliesList = replies,
                replies = new RepliesModel { forumId = id }
            };

            return View(model);
        }

        //POST: UserCOntroller/ForumAndRelies
        public ActionResult ForumAndRelies(RepliesModel newReply)
        {
            if (ModelState.IsValid)
            {
                var connection = new SqlConnection(_configuration.GetConnectionString("Users"));
                connection.Open();
                //  Have to change
                /*
                string query = "INSERT INTO dbo.Replies (ForumId, Id, Content, ReplyCreated) VALUES (@DiscussionId, @Email, @Content, @ReplyCreated)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ForumId", newReply.forumId);
                cmd.Parameters.AddWithValue("@Email", newReply.Id);
                cmd.Parameters.AddWithValue("@Content", newReply.replyContent);
                cmd.Parameters.AddWithValue("@ReplyCreated", DateTime.Now);
                */
                cmd.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction("Details", new { id = newReply.forumId });
            }



            // If model state is invalid, return to the same page with the model to show validation errors
            var discussion = GetDiscussionById(newReply.forumId);
            var replies = GetRepliesByDiscussionId(newReply.forumId);

            var model = new DiscussionViewModel
            {
                forum = discussion,
                repliesList = replies,
                replies = newReply
            };

            return View("ForumList");
        }

        private ForumModel GetDiscussionById(int id)
        {
            ForumModel discussion =new();

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
                        Id = (int)dr["Id"],
                        Email = (string)dr["Email"],
                        topic = (string)dr["Title"],
                        description = (string)dr["Description"],
                        forumCreated = (DateTime)dr["DateCreated"]
                    };
                }
                dr.Close();
            }

            return discussion;
        }

        private List<RepliesModel> GetRepliesByDiscussionId(int id)
        {
            List<RepliesModel> replies = new List<RepliesModel>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Users")))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Reply WHERE DiscussionId=@DiscussionId ORDER BY ReplyCreated ASC", connection);
                cmd.Parameters.AddWithValue("@DiscussionId", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    RepliesModel reply = new RepliesModel
                    {
                        Id = (int)dr["Id"],
                        DiscussionId = (int)dr["DiscussionId"],
                        Email = (string)dr["Email"],
                        Content = (string)dr["Content"],
                        ReplyCreated = (DateTime)dr["ReplyCreated"]
                    };
                    replies.Add(reply);
                }
                dr.Close();
            }

            return replies;
        }































        /*









        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        


        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
        }*/
    }
}
