using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ecom_aspNetCoreMvc.Models;

namespace ecom_aspNetCoreMvc.Controllers
{
    public class UserController : Controller
    {
        private void UserConnect(dynamic v)
        {
            bool? logged = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (logged == true)
            {
                v.Logged = logged;
                v.UserName = HttpContext.Session.GetString("UserName");
                v.Id = HttpContext.Session.GetInt32("Id");
            }
        }

        [Route("[Controller]/Login")]
        public IActionResult Login()
        {
            UserConnect(ViewBag);
            User u = new User();
            return View(u);
        }

        [HttpPost]
        public IActionResult LoginPost(string email, string password)
        {
            User u = new User { Email = email, Password = password };
            List<string> message = new List<string>();
            if (email == null)
            {
                message.Add("Merci de saisir une adresse mail");
            }
            if (password == null)
            {
                message.Add("Merci de saisir un mot de passe");
            }
            else if (!u.UserLogin(email, password, u))
            {
                message.Add("Adresse mail et/ou mot de passe incorrect(s)");
            }
            if (message.Count > 0)
            {
                ViewBag.errors = message;
                return View("Login", u);
            }
            else
            {
                HttpContext.Session.SetString("logged", "true");
                HttpContext.Session.SetString("UserName", u.UserName);
                HttpContext.Session.SetInt32("Id", u.Id);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
        }

        [HttpPost]
        public IActionResult Register(string lastName, string firstName, string userName, string phone, string email, string address, int? zip, string city, string password, string cPassword)
        {
            List<string> message = new List<string>();
            User u = new User
            {
                LastName = lastName,
                FirstName = firstName,
                UserName = userName,
                Phone = phone,
                Email = email,
                Address = address,
                Zip = zip,
                City = city,
                Password = password
            };
            if (lastName == null)
            {
                message.Add("Veuillez saisir un nom");
            }
            if (firstName == null)
            {
                message.Add("Veuillez saisir un prénom");
            }
            if (userName == null)
            {
                message.Add("Veuillez saisir un pseudo");
            }
            if (phone == null)
            {
                message.Add("Veuillez saisir un numéro de téléphone");
            }
            if (email == null)
            {
                message.Add("Veuillez saisir une adresse mail");
            }
            if (address == null)
            {
                message.Add("Veuillez saisir une adresse");
            }
            if (zip == null)
            {
                message.Add("Veuillez saisir un code postal");
            }
            if (city == null)
            {
                message.Add("Veuillez saisir une ville");
            }
            if (password == null)
            {
                message.Add("Veuillez saisir un mot de passe");
            }
            if (password != cPassword)
            {
                message.Add("Confirmation incorrect");
            }
            if (u.UserExist(u))
            {
                message.Add("Adresse mail déjà utilisée");
            }
            if (message.Count > 0)
            {
                ViewBag.errors = message;
                return View("Login", u);
            }
            else
            {
                u.AddUser(u);
                ViewBag.inscription = true;
                ViewBag.validation = "Merci pour votre inscription, vous pouvez à présent vous connecter !";
            }
            return View("Login", u);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [Route("UserList")]
        [HttpGet]
        public IActionResult UserList()
        {
            UserConnect(ViewBag);
            List<User> users = Models.User.GetUserList();
            ViewBag.users = users;
            return View("UserList");
        }

        [HttpGet]
        public IActionResult GetUserByLastName(string lastName)
        {
            User u = new User();
            List<User> users = new List<User>();
            if (lastName == null)
            {
                users = Models.User.GetUserList();
                ViewBag.users = users;
            }
            else
            {
                users = new List<User>();
                users = Models.User.GetUserByLastName(lastName);
                //users.Add(u);
                ViewBag.users = users;
            }
            return View("UserList");
        }

        public IActionResult UserDelete(User u)
        {
            UserConnect(ViewBag);
            Models.User.DeleteUser(u);
            List<User> users = Models.User.GetUserList();
            ViewBag.users = users;
            return RedirectToAction("UserList");
        }
    }
}