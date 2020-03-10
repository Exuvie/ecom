using ecom_aspNetCoreMvc.Tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ecom_aspNetCoreMvc.Models
{
    public class Cart
    {
        private int id;
        private int userId;
        private List<CartArticle> articles;
        private decimal total;

        private static string request;
        private static SqlCommand command;
        private static SqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; set => userId = value; }
        public List<CartArticle> Articles { get => articles; set => articles = value; }
        public decimal Total { get => total; set => total = value; }

        public Cart()
        {
            Articles = new List<CartArticle>();
        }

        public void AddArticleToCart(Article article)
        {
            bool found = false;
            foreach(CartArticle c in Articles)
            {
                if (c.Article.Id == article.Id)
                {
                    found = true;
                    c.Quantity++;
                    break;
                }
            }
            if (!found)
            {
                Articles.Add(new CartArticle { Article = article, Quantity = 1 });
            }
            //System.Diagnostics.Debug.WriteLine(Articles);
            UpdateTotal();
        }

        public void UpdateTotal()
        {
            total = 0;
            foreach(CartArticle c in Articles)
            {
                total += Convert.ToDecimal(c.Article.Price) * c.Quantity;
            }
        }

        public void SaveCartUser(Cart c)
        {
            request = "INSERT INTO Cart (userId, total) OUTPUT INSERTED.ID VALUES (@userId, @total)";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@total", c.Total));
            command.Parameters.Add(new SqlParameter("@userId", c.UserId));
            DataBase.Instance.connection.Open();
            c.Id = (int)command.ExecuteScalar();
            command.Dispose();
            foreach (CartArticle ca in Articles)
            {
                request = "INSERT INTO CartArticle (cartId, articleId, quantity) VALUES (@cartId, @articleId, @quantity)";
                command = new SqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new SqlParameter("@cartId", Id));
                command.Parameters.Add(new SqlParameter("@articleId", ca.Article.Id));
                command.Parameters.Add(new SqlParameter("@quantity", ca.Quantity));
                command.ExecuteNonQuery();
                command.Dispose();
            }
            DataBase.Instance.connection.Close();
        }
    }
}
