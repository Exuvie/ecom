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
                v.Total = HttpContext.Session.GetString("Total");
                v.NbArticles = HttpContext.Session.GetString("NbArticles");
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
            string jsonCart = HttpContext.Session.GetString("cart");
            Cart cart = (jsonCart == null) ? new Cart() : JsonConvert.DeserializeObject<Cart>(jsonCart);
            cart.AddArticleToCart(Article.GetArticleById(id));
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            string total = Convert.ToString(cart.Total);
            HttpContext.Session.SetString("Total", total);
            string nbArticles = Convert.ToString(cart.NbArticles);
            HttpContext.Session.SetString("NbArticles", nbArticles);
            return RedirectToRoute(new { controller = "Cart", action = "UserCart" });
        }

        public IActionResult RemoveArticleToCart(int id)
        {
            string jsonCart = HttpContext.Session.GetString("cart");
            Cart cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
            cart.RemoveArticleToCart(Article.GetArticleById(id));
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            //foreach (CartArticle ca in cart.Articles)
            //{
            //    if (ca.Quantity == 0)
            //    {
            //        //cart = new Cart();
            //        cart.Articles.Remove(ca);
            //        //HttpContext.Session.GetString("cart");


            //    }
            //}
            string total = Convert.ToString(cart.Total);
            HttpContext.Session.SetString("Total", total);
            string nbArticles = Convert.ToString(cart.NbArticles);
            HttpContext.Session.SetString("NbArticles", nbArticles);
            return RedirectToRoute(new { controller = "Cart", action = "UserCart" });
        }

        public IActionResult SaveCart()
        {
            UserConnect(ViewBag);
            User u = new User();
            u.Id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            Cart cart = new Cart();
            string nbA = HttpContext.Session.GetString("NbArticles");
            
            if (nbA == null || nbA == "0")
            {
                ViewBag.validation = "Aucun article sélectionné";
            }
            else
            {
                string jsonCart = HttpContext.Session.GetString("cart");
                cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
                cart.User.Id = u.Id;
                cart.SaveCartUser(cart);
                ViewBag.validation = "La commande a été enregistrée";

                cart = new Cart();
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                //jsonCart = HttpContext.Session.GetString("cart");
                string total = Convert.ToString(cart.Total);
                HttpContext.Session.SetString("Total", total);
                string nbArticles = Convert.ToString(cart.NbArticles);
                HttpContext.Session.SetString("NbArticles", nbArticles);
            }
            return View("UserCart", cart);
        }

        public IActionResult CartList()
        {
            UserConnect(ViewBag);
            return View(Cart.GetAllCarts());
        }

        public IActionResult CartDetail(int id)
        {
            UserConnect(ViewBag);
            return View(Cart.GetCartArticleById(id));
        }
    }
}