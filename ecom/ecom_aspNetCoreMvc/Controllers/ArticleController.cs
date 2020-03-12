using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ecom_aspNetCoreMvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecom_aspNetCoreMvc.Controllers
{
    public class ArticleController : Controller
    {

        private IHostingEnvironment _env;

        public ArticleController(IHostingEnvironment env)
        {
            _env = env;
        }

        private void UserConnect(dynamic v)
        {
            bool? logged = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (logged == true)
            {
                v.Logged = logged;
                v.UserName = HttpContext.Session.GetString("UserName");
                v.Total = HttpContext.Session.GetString("Total");
                v.NbArticles = HttpContext.Session.GetString("NbArticles");
            }
        }

        [Route("Article/Admin")]
        [HttpGet]
        public IActionResult ArticleAdmin()
        {
            UserConnect(ViewBag);
            ViewBag.Category = Category.GetCategories();
            List<Article> articles = Article.GetArticles();
            ViewBag.Article = articles;
            articles.ForEach(a =>
            {
                a.UrlImage = $"{Request.Scheme}://{Request.Host.Value}/{a.UrlImage}";
            });
            return View("ArticleAdmin");
        }


        [HttpPost]
        public IActionResult RegisterArticle(string title, string description, decimal? price, int? idCategory, IFormFile imageArticle)
        {
            UserConnect(ViewBag);
            List<string> message = new List<string>();
            Article a = new Article { Title = title, Description = description, Price = price, IdCategory = idCategory };
            if (idCategory == null)
            {
                message.Add("Merci de saisir une catégorie");
            }
            if (title == null)
            {
                message.Add("Merci de saisir le titre de l'article");
            }
            if (description == null)
            {
                message.Add("Merci de saisir une descriptione");
            }
            if (price == null)
            {
                message.Add("Merci de saisir le prix de l'article");
            }
            if (imageArticle == null)
            {
                //a.UrlImage = "images/default.png";
                message.Add("Merci de selectionner une image");
            }
            if (message.Count > 0)
            {
                ViewBag.errors = message;
                ViewBag.Category = Category.GetCategories();
                List<Article> articles = Article.GetArticles();
                ViewBag.Article = articles;
                articles.ForEach(c =>
                {
                    c.UrlImage = $"{Request.Scheme}://{Request.Host.Value}/{c.UrlImage}";
                });
                //return View("ArticleAdmin");
            }
            else
            {
                ViewBag.Category = Category.GetCategories();
                string img = Guid.NewGuid().ToString() + "-" + imageArticle.FileName;
                string pathToUpload = Path.Combine(_env.WebRootPath, "images", img);
                FileStream stream = System.IO.File.Create(pathToUpload);
                imageArticle.CopyTo(stream);
                stream.Close();
                a.UrlImage = "images/" + img;
                a.IdCategory = idCategory;
                a.AddArticle(a);
                ViewBag.validation = "L'article " + a.Title + " a été ajouté";
                List<Article> articles = Article.GetArticles();
                ViewBag.Article = articles;
                articles.ForEach(c =>
                {
                    c.UrlImage = $"{Request.Scheme}://{Request.Host.Value}/{c.UrlImage}";
                });
                
            }
            return View("ArticleAdmin");
        }

        [HttpGet]
        public IActionResult GetArticles(int? idCategory)
        {
            UserConnect(ViewBag);
            ViewBag.Category = Category.GetCategories();
            List<Article> articles = Article.GetArticleByCategory(idCategory);
            ViewBag.Article = articles;
            articles.ForEach(a =>
            {
                a.UrlImage = $"{Request.Scheme}://{Request.Host.Value}/{a.UrlImage}";
            });
            return View("ArticleAdmin");
        }

        public IActionResult RemoveArticle(Article a)
        {
            UserConnect(ViewBag);
            ViewBag.Category = Category.GetCategories();
            Models.Article.DeleteArticle(a);
            ViewBag.validation = "Article supprimé";
            List<Article> articles = Models.Article.GetArticles();
            ViewBag.Article = articles;
            articles.ForEach(c =>
            {
                c.UrlImage = $"{Request.Scheme}://{Request.Host.Value}/{c.UrlImage}";
            });
            return View("ArticleAdmin");
        }
    }
}