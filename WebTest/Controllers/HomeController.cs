using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WebTest.Models;
using Microsoft.AspNet.Identity;

namespace WebTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult BecomeAdmin()
        {
            if (!Roles.IsUserInRole(User.Identity.GetUserName(), "Admin"))
                Roles.AddUserToRole(User.Identity.GetUserName(), "Admin");
            return RedirectToAction("Index");
        }
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
            if (!Roles.RoleExists("Admin"))
                Roles.CreateRole("Admin");
            Random rnd = new Random();
            ViewBag.Message = msg[rnd.Next(0, msg.Length)];
            return View();
        }

        readonly CelebrationContext CelContext = new CelebrationContext();
        [Authorize]
        public ActionResult Celebrations()
        {
            IEnumerable<Celebration> cel = CelContext.Celebrations;
            return View(cel);
        }

        [HttpGet]
        public ActionResult CreateCel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCel(Celebration cel)
        {
            CelContext.Celebrations.Add(cel);
            CelContext.SaveChanges();
            return RedirectToAction("Celebrations");
        }
        [HttpGet]
        public ActionResult DeleteCel(int id)
        {
            Celebration cel = CelContext.Celebrations.Find(id);
            if (cel == null)
            {
                return HttpNotFound();
            }
            return View(cel);
        }
        [HttpPost, ActionName("DeleteCel")]
        public ActionResult DeleteConfirmedCel(int id)
        {
            Celebration cel = CelContext.Celebrations.Find(id);
            if (cel == null)
            {
                return HttpNotFound();
            }
            CelContext.Celebrations.Remove(cel);
            CelContext.SaveChanges();
            return RedirectToAction("Celebrations");
        }
        [HttpGet]
        public ActionResult EditCel(int id)
        {
            Celebration cel = CelContext.Celebrations.Find(id);
            if (cel == null)
            {
                return HttpNotFound();
            }
            return View(cel);
        }
        [HttpPost, ActionName("EditCel")]
        public ActionResult EditConfirmedCel(int id)
        {
            Celebration cel = CelContext.Celebrations.Find(id);
            if (cel == null)
            {
                return HttpNotFound();
            }
            context.Entry(cel).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Celebrations");
        }

        readonly GuestContext context = new GuestContext();
        [Authorize(Roles = "Admin")]
        public  ActionResult Guests()
        {
            IEnumerable<Guest> guests = context.Guests;
            return View(guests);
        }

        public ActionResult SendReminder(string email)
        {
            SendReminderEmail(email);
            Response.Write("<script language=javascript>alert('Message sended.')</script>");
            return View("Index");
        }

        [HttpGet, Authorize]
        public ViewResult Interview()
        {
            //var user = Membership.GetUser(HttpContext.User.Identity.Name);
            ViewBag.Email = "lev.petryakov@gmail.com";
            ViewBag.Name = "Lev";
            return View();
        }

        [HttpPost]
        public ViewResult Interview(Guest guest)
        {
            SendEmail(guest);
            ViewBag.Email = guest.Email;
            if (ModelState.IsValid)
            {
                context.Guests.Add(guest);
                context.SaveChanges();
                return View("Gratitude", guest);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Guest guest)
        {
            context.Guests.Add(guest);
            context.SaveChanges();
            return RedirectToAction("Guests");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Guest guest = context.Guests.Find(id);
            if (guest==null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Guest guest = context.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            context.Guests.Remove(guest);
            context.SaveChanges();
            return RedirectToAction("Guests");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Guest guest = context.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult EditConfirmed(int id)
        {
            Guest guest = context.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            context.Entry(guest).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Guests");
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

        public void SendEmail(Guest guest)
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