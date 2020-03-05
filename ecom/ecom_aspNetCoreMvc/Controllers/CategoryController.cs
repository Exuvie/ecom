using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom_aspNetCoreMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecom_aspNetCoreMvc.Controllers
{
    public class CategoryController : Controller
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

        [Route("Category/Admin")]
        public IActionResult CategoryAdmin()
        {
            UserConnect(ViewBag);
            List<Category> categories = Category.GetCategories();
            return View("CategoryAdmin", categories);
        }

        [Route("Category/Register")]
        [HttpPost]
        public IActionResult CategoryRegister(string title)
        {         
            Category c = new Category { Title = title };
            List<Category> categories = new List<Category>();
            if (title == null)
            {
                ViewBag.errors = "Merci de saisir un nom de catégorie";
                //categories = Category.GetCategories();
                //return View("CategoryAdmin", categories);
                //return RedirectToAction("CategoryAdmin");

            }
            if (Models.Category.CategoryExist(c))
            {
                ViewBag.errors = "Catégorie déjà existante !";
                //categories = Category.GetCategories();
                //return View("CategoryAdmin", categories);
                //return RedirectToAction("CategoryAdmin");
            }
            else
            {
                Models.Category.AddCategory(c);
                ViewBag.validation = "La catégorie " + c.Title + " a été ajoutée";
            }
            categories = Category.GetCategories();
            return View("CategoryAdmin", categories);
        }

        public IActionResult RemoveCategory(Category c)
        {
            if (Models.Category.DeleteCategory(c))
            {
                ViewBag.errors = "La catégorie " + c.Title + "  a été supprimée";
            }
            List <Category> categories = Category.GetCategories();
            return View("CategoryAdmin", categories);
        }
    }
}