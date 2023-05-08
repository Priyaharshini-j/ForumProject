using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;

namespace ForumProject.Controllers
{
    public class EmailController : Controller
    {
        private readonly IConfiguration _config;

        public EmailController(IConfiguration config)
        {
            _config = config;
        }
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(string useremail)
        {
            /*
            string subject = "Invite from Friend";
            string body = "You are invited to join the ConneKt Forum by your friend";
            Console.WriteLine(useremail);
            WebMail.SmtpServer = "smtp.gmail.com";
            WebMail.SmtpPort = 587;
            WebMail.From = "priyadj2001@gmail.com";
            WebMail.UserName = "priyadj2001@gmail.com";
            WebMail.Password = "priyadharshini2001";
            WebMail.Send(useremail, subject, body);
            */
            ViewBag.msg = "Email Successful to"+useremail;
            return View();

        }
    }
}
