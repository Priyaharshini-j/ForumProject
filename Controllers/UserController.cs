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

        public UserController(IConfiguration configuration, IHttpContextAccessor context)
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

        public List<ForumModel> GetForumById(string Email)
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
                    model.category = (string)reader[2];
                    model.title = (string)reader[3];
                    model.content = (string)reader[4];
                    model.forumCreated = (DateTime)reader[5];
                    forums.Add(model);
                }
                List<ForumModel> search = new List<ForumModel>();
                foreach (ForumModel model in forums)
                {
                    if (model.title.Contains(searchString) || model.category.Contains(searchString) || model.content.Contains(searchString))
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
        public ActionResult<List<ForumModel>> ForumList(string searchString, ForumModel forum)
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
                Console.WriteLine(discuss.Email);
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
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }


        // GET: DiscussionController/Details/5
        public IActionResult AddReply(int id)
        {
            try
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        // POST: DiscussionController/Details/5
        [HttpPost]
        public ActionResult AddReply(RepliesModel newReply)
        {
            try
            {
                Console.WriteLine(newReply.Email);
                if (ModelState.IsValid && newReply.Email != null)
                {
                    var connection = new SqlConnection(_configuration.GetConnectionString("OnlineForum"));
                    connection.Open();
                    string query = "INSERT INTO Replies VALUES (@DiscussionId, @Email, @Content, @ReplyCreated)";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@DiscussionId", newReply.forumId);
                    cmd.Parameters.AddWithValue("@Email", Context.HttpContext.Session.GetString("UserEmail"));
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

                return View("DiscussionList", model);
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError(string.Empty,ex.Message);
                return View("AddReply", new { id = newReply.forumId });
            }
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
            if (discussion != null)
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
        //To creating a poll
        [HttpGet]
        public ActionResult CreatePoll()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePoll(UserPoll poll)
        {
            try
            {
                Console.WriteLine(poll.Email);
                int pollId = 0;
                using (SqlCommand command = new SqlCommand("CreatePoll", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Email", poll.Email);
                    command.Parameters.AddWithValue("@Title", poll.Title);
                    command.Parameters.AddWithValue("@Category", poll.Category);
                    command.Parameters.AddWithValue("@Question", poll.Question);
                    command.Parameters.AddWithValue("@Created", poll.Created);

                    SqlParameter pollIdParam = command.Parameters.Add("@PollId", SqlDbType.Int);
                    pollIdParam.Direction = ParameterDirection.Output;

                    _connection.Open();
                    command.ExecuteNonQuery();

                    pollId = Convert.ToInt32(pollIdParam.Value);
                    Console.WriteLine("New poll created with PollId = " + pollId);
                    _connection.Close();
                }
                using (SqlCommand cmd = new SqlCommand("CreateOptions", _connection))
                {
                    int Index = 0;
                    foreach (var item in poll.Options)
                    {
                        Console.WriteLine(poll.Options.Count);
                        Console.WriteLine(item.OptionText);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear(); // Clear previous parameters
                        cmd.Parameters.AddWithValue("PollId", pollId);
                        cmd.Parameters.AddWithValue("OptionId", Index);
                        Index++;
                        cmd.Parameters.AddWithValue("Option", item.OptionText);
                        _connection.Open();
                        cmd.ExecuteNonQuery();
                        _connection.Close();
                    }
                }

                return RedirectToAction("Polls");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }
        [HttpGet]
        public ActionResult<List<UserPoll>> Polls()
        {
            List<UserPoll> pollList = new List<UserPoll>();
            _connection.Open();
            using (SqlCommand cmd = new SqlCommand("FetchPollsAndOptions", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int pollId = (int)reader["PollId"];
                    UserPoll poll = pollList.FirstOrDefault(p => p.Id == pollId);
                    if (poll == null)
                    {
                        poll = new UserPoll()
                        {
                            Id = pollId,
                            Email = (string)reader["Email"],
                            Title = (string)reader["Title"],
                            Category = (string)reader["Category"],
                            Question = (string)reader["Question"],
                            Created = (DateTime)reader["Created"],
                            Options = new List<PollOption>()
                        };
                        pollList.Add(poll);
                    }
                    int optionId = (int)reader["OptionId"];
                    string optionText = reader["OptionText"] as string;
                    double optionPrecentage = (double)reader["VotePercentage"];
                    if (!string.IsNullOrEmpty(optionText))
                    {
                        PollOption option = new PollOption()
                        {
                            Id = pollId,
                            OptionId=optionId,
                            OptionText = optionText,
                            VotePercentage = optionPrecentage

                        };
                        poll.Options.Add(option);
                    }
                }
                reader.Close();
            }
            _connection.Close();
            return View(pollList);
        }
        
        [HttpPost]
        public IActionResult Polls(UserPoll poll)
        {
            try
            {

                var pollId = int.Parse(Request.Form["PollId"]);
                var optionId = int.Parse(Request.Form["OptionId"]);
                Console.WriteLine("OptionId is "+optionId);
                Console.WriteLine("Try block");
                _connection.Open();
                int opt_Id = 0;
                using (SqlCommand comm = new SqlCommand("SELECT OptionId FROM UserVoted WHERE PollId = @PollId AND Email = @Email", _connection))
                {
                    Console.WriteLine("inside the select using");
                    comm.Parameters.AddWithValue("@PollId", pollId);
                    comm.Parameters.AddWithValue("@Email", Context.HttpContext.Session.GetString("UserEmail"));

                    Console.WriteLine("bfr reader");
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("insid reader");
                        opt_Id++;
                    }
                    reader.Close();
                    Console.WriteLine(opt_Id);
                    Console.WriteLine("close reader");
                }

                Console.WriteLine(opt_Id);
                if (opt_Id == 0)
                {
                    Console.WriteLine("in if");

                    Console.WriteLine("OptionId is " + optionId);
                    using (SqlCommand cmd = new SqlCommand("UpdateVote", _connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        Console.WriteLine("bfr reader");
                        Console.WriteLine("using block");
                        Console.WriteLine(optionId);
                        cmd.Parameters.AddWithValue("@PollId", pollId);
                        cmd.Parameters.AddWithValue("@OptionId", optionId);
                        cmd.ExecuteNonQuery();
                    }

                    Console.Write("SqlDataReader dr = cmd.ExecuteReader() executed");
                    using (SqlCommand command = new SqlCommand("INSERT INTO UserVoted VALUES (@email,@PollId, @OptionId) ", _connection))
                    {

                        Console.WriteLine("OptionId is " + optionId);
                        command.Parameters.AddWithValue("@PollId", pollId);
                        command.Parameters.AddWithValue("@OptionId", optionId);
                        Console.WriteLine(Context.HttpContext.Session.GetString("UserEmail"));
                        command.Parameters.AddWithValue("@email", Context.HttpContext.Session.GetString("UserEmail"));
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("in the last stage");
                    return RedirectToAction("Polls");
                }
                else
                {
                    return RedirectToAction("Polls");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("In catch");
                ViewBag.Message = ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new List<UserPoll>());
            }
        }
        [HttpGet]
        public IActionResult DeleteForum(int id)
        {
            return View(GetForumById(id));
        }

        ForumModel GetForumById(int id)
        {
            try
            {
                _connection.Open();
                ForumModel model = new ForumModel();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Forum WHERE ForumId = @ForumId", _connection);
                cmd.Parameters.AddWithValue("@ForumId", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    model.forumId = (int)reader[0];
                    model.Email = (string)reader[1];
                    model.category = (string)reader[2];
                    model.title = (string)reader[3];
                    model.content = (string)reader[4];
                    model.forumCreated = (DateTime)reader[5];
                }
                return model;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return new ForumModel();
            }
        }

        [HttpPost]
        public ActionResult DeleteForum(ForumModel model, int Id, int id)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd2 = new SqlCommand("DELETE FROM Replies WHERE ForumId = @ForumId", _connection);
                cmd2.Parameters.AddWithValue("@ForumId", Id);
                cmd2.ExecuteNonQuery();
                SqlCommand cmd = new SqlCommand("DELETE FROM Forum WHERE ForumId = @ForumId", _connection);
                cmd.Parameters.AddWithValue("@ForumId", Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction(nameof(ForumList));
        }
        [HttpGet]
        public IActionResult EditForum(int id)
        {
            return View(GetForumById(id));
        }
         [HttpPost]
        public ActionResult EditForum(ForumModel model)
        {
            _connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("EditForum", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Category", model.category);
                cmd.Parameters.AddWithValue("@Content", model.content);
                cmd.Parameters.AddWithValue("@Title", model.title);
                cmd.Parameters.AddWithValue("@ForumId", model.forumId);
                cmd.Parameters.AddWithValue("@ForumCreated", model.forumCreated);
                cmd.ExecuteNonQuery();
                return RedirectToAction(nameof(ForumList));
            }
            catch
            {

                return RedirectToAction(nameof(ForumList));
            }
        }
        [HttpGet]
        public ActionResult<List<UserPoll>> PollList(string Email)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("FetchPoll", _connection);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.CommandType = CommandType.StoredProcedure;
                List<UserPoll> pollList = new List<UserPoll>();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    int pollId = (int)reader["PollId"];
                    UserPoll poll = pollList.FirstOrDefault(p => p.Id == pollId);
                    if (poll == null)
                    {
                        poll = new UserPoll()
                        {
                            Id = pollId,
                            Email = (string)reader["Email"],
                            Title = (string)reader["Title"],
                            Category = (string)reader["Category"],
                            Question = (string)reader["Question"],
                            Created = (DateTime)reader["Created"],
                            Options = new List<PollOption>()
                        };
                        pollList.Add(poll);
                    }
                    string optionText = reader["OptionText"] as string;
                    if (!string.IsNullOrEmpty(optionText))
                    {
                        PollOption option = new PollOption()
                        {
                            Id = pollId,
                            OptionText = optionText
                        };
                        poll.Options.Add(option);
                    }
                }
                return View(pollList);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Console.WriteLine(ex.Message);
                return View(new List<UserPoll>());
            }
        }
        [HttpGet]
        public ActionResult<UserPoll> DeletePoll(int id)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand("GetPoll", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PollId", id);
                SqlDataReader reader =cmd.ExecuteReader();
                UserPoll poll = null;
                while(reader.Read())
                {
                    poll = new UserPoll()
                    {
                        Id = (int)reader[0],
                        Email = (string)reader["Email"],
                        Title = (string)reader["Title"],
                        Category = (string)reader["Category"],
                        Question = (string)reader["Question"],
                        Created = (DateTime)reader["Created"],
                        Options = new List<PollOption>()
                    };
                    int pollId = poll.Id;   
                    string optionText = reader["OptionText"] as string;
                    if (!string.IsNullOrEmpty(optionText))
                    {
                        PollOption option = new PollOption()
                        {
                            Id = pollId,
                            OptionText = optionText
                        };
                        poll.Options.Add(option);
                    }
                }
                _connection.Close();
                return View(poll);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
        [HttpPost]
        public ActionResult DeletePoll(UserPoll poll)
        {
            try
            {
                Console.WriteLine(poll.Id);
                _connection.Open();
                SqlCommand cmd = new SqlCommand("DeletePoll", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PollId", poll.Id);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Polls", User);
             }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Console.WriteLine(ex.Message);
                return View();
            }
        }
    }
}
