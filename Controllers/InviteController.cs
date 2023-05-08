using Microsoft.AspNetCore.Mvc;
using ForumProject.Models;
using System.Web;
using System.Net.Mail;
using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForumProject.Controllers
{
    [System.Web.Http.Route("api/[controller]")]
    [ApiController]
    public class InviteController : ControllerBase
    {/*
        public IHttpActionResult sendMail(Invite invite)
        {
            string to = invite.To;
            string sub = invite.Subject;
            string body = invite.Body;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("priyadj2001@gmail.com");
            message.To.Add(to);
            message.Subject = sub;
            message.Body = body;
            message.IsBodyHtml = false;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("priyadj2001@gmail.com", "priyadharshini2001");
            smtpClient.Send(message);
            return (IHttpActionResult)Ok();

        }
        */
    }
}
