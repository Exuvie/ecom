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
        private int? id;
        private User user;
        private List<CartArticle> articles;
        private int nbArticles;
        private decimal total;
        private DateTime registerDate;

        private static string request;
        private static SqlCommand command;
        private static SqlDataReader reader;

        public int? Id { get => id; set => id = value; }
        public List<CartArticle> Articles { get => articles; set => articles = value; }
        public decimal Total { get => total; set => total = value; }
        public User User { get => user; set => user = value; }
        public int NbArticles { get => nbArticles; set => nbArticles = value; }
        public DateTime RegisterDate { get => registerDate; set => registerDate = value; }

        public Cart()
        {
            Articles = new List<CartArticle>();
            User = new User();
            Total = 0;
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
                    NbArticles += 1;
                    break;
                }
            }
            if (!found)
            {
                Articles.Add(new CartArticle { Article = article, Quantity = 1 });
                NbArticles += 1;
            }
            //System.Diagnostics.Debug.WriteLine(Articles);
            UpdateTotal();
        }

        public void RemoveArticleToCart(Article article)
        {
            foreach (CartArticle c in Articles)
            {
                if (c.Article.Id == article.Id)
                {
                    if (c.Quantity == 1)
                    {
                        Articles.Remove(c);
                        NbArticles -= 1;
                        break;
                    }
                    else
                    {
                        c.Quantity--;
                        NbArticles -= 1;
                        break;
                    }
                }
            }
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
            request = "INSERT INTO Cart (userId, total, registerDate) OUTPUT INSERTED.ID VALUES (@userId, @total, @registerDate)";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@total", c.Total));
            command.Parameters.Add(new SqlParameter("@userId", c.User.Id));
            command.Parameters.Add(new SqlParameter("@registerDate", DateTime.Now));
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

        public void GetCartArticles()
        {
            request = "SELECT ca.quantity, a.id, a.title, a.price FROM CartArticle as ca " +
                        "INNER JOIN Article as a " +
                        "ON a.id = ca.articleId " +
                        "WHERE ca.cartId = @cartId";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@cartId", Id));
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Articles.Add(new CartArticle
                {
                    Quantity = reader.GetInt32(0),
                    Article = new Article
                    {
                        Id = reader.GetInt32(1),
                        Title = reader.GetString(2),
                        Price = reader.GetDecimal(3)
                    }
                });
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
        }

        public static List<Cart> GetAllCarts()
        {
            List<Cart> liste = new List<Cart>();
            request = "SELECT c.id as cartId, c.total, c.registerDate, u.id as userId, u.lastName, u.firstName FROM Cart as c INNER JOIN Users as u ON c.userId = u.id";
            command = new SqlCommand(request, DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Cart c = new Cart
                {
                    Id = reader.GetInt32(0),
                    total = reader.GetDecimal(1),
                    RegisterDate = reader.GetDateTime(2),
                    User = new User
                    {
                        Id = reader.GetInt32(3),
                        LastName = reader.GetString(4),
                        FirstName = reader.GetString(5)
                        
                    }
                };
                liste.Add(c);
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            foreach (Cart c in liste)
            {
                c.GetCartArticles();
            }
            return liste;
        }

        public static Cart GetCartArticleById(int id)
        {
            Cart cart = null;
            request = "SELECT c.id as cartId, c.total, c.registerDate, u.id as userId, u.lastName, u.firstName FROM Cart as c " +
                        "INNER JOIN Users as u " +
                        "ON c.userId = u.id " +
                        "WHERE c.id = @id";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@id", id));
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                cart = new Cart
                {
                    Id = reader.GetInt32(0),
                    total = reader.GetDecimal(1),
                    RegisterDate = reader.GetDateTime(2),
                    User = new User
                    {
                        Id = reader.GetInt32(3),
                        LastName = reader.GetString(4),
                        FirstName = reader.GetString(5)
                        
                    }
                };
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            cart?.GetCartArticles();
            return cart;
        }

        
    }
}
