using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using ForumProject.Controllers;
using ForumProject.Models;

namespace ForumProject.Controllers
{
    public class SendMailController : Controller
    {
        public IActionResult Invite()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Invite(Invite invite)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5134/api/Invite");
            var consumeWebApi = client.PostAsJsonAsync<Invite>("Invite", invite);
            consumeWebApi.Wait();
            var sendmail = consumeWebApi.Result;
            if(sendmail.IsSuccessStatusCode)
            {
                ViewBag.message = "This is a Invite mail from ConnekT Forum Community. This Mail has been sent to " + invite.To.ToString();
            }
            return View();
        }
    }
}
