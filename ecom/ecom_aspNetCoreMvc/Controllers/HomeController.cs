using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecom_aspNetCoreMvc.Models;
using Microsoft.AspNetCore.Http;

namespace ecom_aspNetCoreMvc.Controllers
{
    public class HomeController : Controller
    {
        private void UserConnect(dynamic v)
        {
            bool? logged = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (logged == true)
            {
                v.Logged = logged;
                v.UserName = HttpContext.Session.GetString("UserName");
            }
        }

        public IActionResult Index()
        {
            UserConnect(ViewBag);
            return View();
        }

        public IActionResult Admin()
        {
            UserConnect(ViewBag);
            return View();
        }
    }
}
