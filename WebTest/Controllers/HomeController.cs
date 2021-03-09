using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int hour = DateTime.Now.Hour;
            string[] msg = new string[]
            {
                "We were waiting You",
                "Are You shure about this ?",
                "Welcome to the club, buddy !",
                "Hello World",
                "I am alone",
                "All around me are familiar faces",
                "How are You ?"
            };
            if (hour <= 6 && hour > 0)
            {
                ViewBag.GetDayTime = "night";
            }
            else if(hour <= 12 && hour > 6)
            {
                ViewBag.GetDayTime = "morning";
            }
            else if(hour <= 18 && hour > 12)
            {
                ViewBag.GetDayTime = "afternoon";
            }
            else if (hour <= 24 && hour > 18)
            {
                ViewBag.GetDayTime = "evening";
            }
            Random rnd = new Random();
            ViewBag.Message = msg[rnd.Next(0, msg.Length)];
            return View();
        }

        public ActionResult SendReminder(string email)
        {
            SendReminderEmail(email);
            Response.Write("<script language=javascript>alert('Message sended.')</script>");
            return View("Index");
        }

        [HttpGet]
        public ViewResult Interview()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Interview(GuestModels guest)
        {
            SendEmail(guest);
            ViewBag.Email = guest.Email;
            if (ModelState.IsValid)
            {
                return View("Gratitude", guest);
            }
            else
            {
                return View();
            }
        }

        public void SendReminderEmail(string email)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "jef11037@gmail.com";
                WebMail.Password = "";
                WebMail.From = "jef11037@gmail.com";
                WebMail.Send(email, "Interview result", "Your pass", filesToAttach: new String[] 
                { 
                    Path.Combine(Server.MapPath("~/Images/"), 
                    Path.GetFileName("yes.jpg")) 
                });
                ViewBag.Message = "Mail was sended";
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong !";
            }
        }

        public void SendEmail(GuestModels guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "jef11037@gmail.com";
                WebMail.Password = "";
                WebMail.From = "jef11037@gmail.com";
                WebMail.Send(guest.Email + ", jef11037@gmail.com", "Interview result", guest.Name + " decide to " + ((guest.WillAttend ?? false) ? "come" : "leave"));
                ViewBag.Message = "Mail was sended";
            }
            catch (Exception)
            {
                ViewBag.Message = "Something went wrong !";
            }
        }
    }
}