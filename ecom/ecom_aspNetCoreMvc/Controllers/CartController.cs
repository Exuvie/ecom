using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom_aspNetCoreMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ecom_aspNetCoreMvc.Controllers
{
    public class CartController : Controller
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

        public IActionResult UserCart()
        {
            UserConnect(ViewBag);
            string jsonCart = HttpContext.Session.GetString("cart");
            Cart cart = (jsonCart == null) ? new Cart() : JsonConvert.DeserializeObject<Cart>(jsonCart);
            cart.UpdateTotal();
            return View(cart);
        }

        public IActionResult AddArticleToCart(int id)
        {
            UserConnect(ViewBag);
            string jsonCart = HttpContext.Session.GetString("cart");
            Cart cart = (jsonCart == null) ? new Cart() : JsonConvert.DeserializeObject<Cart>(jsonCart);
            cart.AddArticleToCart(Article.GetArticleById(id));
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("UserCart");
        }

        public IActionResult SaveCart()
        {
            HttpContext.Session.SetString("logged", "true");
            User u = new User();
            u.Id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            string jsonCart = HttpContext.Session.GetString("cart");
            Cart cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
            cart.UserId = u.Id;
            cart.SaveCartUser(cart);
            //return View("UserCart");
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }


    }
}