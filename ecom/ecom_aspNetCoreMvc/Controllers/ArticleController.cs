using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom_aspNetCoreMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecom_aspNetCoreMvc.Controllers
{
    public class ArticleController : Controller
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

        [Route("Article/Admin")]
        public IActionResult ArticleAdmin()
        {
            UserConnect(ViewBag);
            List<Category> categories = Category.GetCategories();
            return View("CategoryAdmin", categories);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}