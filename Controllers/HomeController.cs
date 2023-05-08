using ForumProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace ForumProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HttpClientHandler _httpClientHandler = new HttpClientHandler();
        public HomeController(ILogger<HomeController> logger)

        {
            _logger = logger;
            _httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

        }

        public List<UsersModel> Users = new List<UsersModel>();

        [HttpGet]
        public async Task<List<UsersModel>> GetUsers()
        {
            Users = new List<UsersModel>();
            using (var httpClient = new HttpClient(_httpClientHandler))
            {
                using (var response = await httpClient.GetAsync("http://localhost:5134/api/Admin"))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    Users = JsonConvert.DeserializeObject<List<UsersModel>>(apiresponse);
                }
            }
            return Users;
        }
        public List<ForumModel> forums = new List<ForumModel>();
        [HttpGet]
        public async Task<List<ForumModel>> GetForums()
        {
            forums = new List<ForumModel>();
            using (var httpClient = new HttpClient(_httpClientHandler))
            {
                using (var response = await httpClient.GetAsync("http://localhost:5134/api/Admin/forums"))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    forums = JsonConvert.DeserializeObject<List<ForumModel>>(apiresponse);
                }
            }
            return forums;
        }
        public List<UserPoll> polls = new List<UserPoll>();
        [HttpGet]
        public async Task<List<UserPoll>> GetPolls()
        {
            polls = new List<UserPoll>();
            using (var httpClient = new HttpClient(_httpClientHandler))
            {
                using (var response = await httpClient.GetAsync("http://localhost:5134/api/Admin/polls"))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    polls = JsonConvert.DeserializeObject<List<UserPoll>>(apiresponse);
                }
            }
            return polls;
        }

        public IActionResult Index()
            {
                return View();
            }

            public IActionResult Privacy()
            {
                return View();
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
    }
}