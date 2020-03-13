using ecom_aspNetCoreMvc.Tools;
using MySql.Data.MySqlClient;
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
        //private int userId;
        private User user;
        private List<CartArticle> articles;
        private int nbArticles;
        private decimal total;

        private static string request;
        private static MySqlCommand command;
        private static MySqlDataReader reader;

        public int Id { get => id; set => id = value; }
        //public int UserId { get => userId; set => userId = value; }
        public List<CartArticle> Articles { get => articles; set => articles = value; }
        public decimal Total { get => total; set => total = value; }
        public User User { get => user; set => user = value; }
        public int NbArticles { get => nbArticles; set => nbArticles = value; }

        public Cart()
        {
            Articles = new List<CartArticle>();
            User = new User();
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
            request = "INSERT INTO Cart (userId, total) VALUES (@userId, @total)";
            command = new MySqlCommand(request, DataBase.Instance.connection);            
            command.Parameters.Add(new MySqlParameter("@total", c.Total));
            command.Parameters.Add(new MySqlParameter("@userId", c.User.Id));
            DataBase.Instance.connection.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            request = "SELECT * FROM Cart";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            MySqlDataReader reader = command.ExecuteReader();
            int tmpId=0;
            int cId=0;
            while (reader.Read())
            {
                tmpId = reader.GetInt32(0);
                if(cId<tmpId)
                {
                    cId = tmpId;
                }
            }
            c.Id = cId;
            command.Dispose();
            foreach (CartArticle ca in Articles)
            {
                request = "INSERT INTO CartArticle (cartId, articleId, quantity) VALUES (@cartId, @articleId, @quantity)";
                command = new MySqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new MySqlParameter("@cartId", Id));
                command.Parameters.Add(new MySqlParameter("@articleId", ca.Article.Id));
                command.Parameters.Add(new MySqlParameter("@quantity", ca.Quantity));
                command.ExecuteNonQuery();
                command.Dispose();
            }
            command.Dispose();
            DataBase.Instance.connection.Close();
        }

        public void GetCartArticles()
        {
            request = "SELECT ca.quantity, a.id, a.title, a.price FROM CartArticle as ca " +
                        "INNER JOIN Article as a " +
                        "ON a.id = ca.articleId " +
                        "WHERE ca.cartId = @cartId";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new MySqlParameter("@cartId", Id));
            DataBase.Instance.connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
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
            request = "SELECT c.id as cartId, c.total, u.id as userId, u.lastName, u.firstName FROM Cart as c INNER JOIN Users as u ON c.userId = u.id";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Cart c = new Cart
                {
                    Id = reader.GetInt32(0),
                    total = reader.GetDecimal(1),
                    User = new User
                    {
                        Id = reader.GetInt32(2),
                        LastName = reader.GetString(3),
                        FirstName = reader.GetString(4)
                        
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
            request = "SELECT c.id as cartId, c.total, u.id as userId, u.lastName, u.firstName FROM Cart as c " +
                        "INNER JOIN Users as u " +
                        "ON c.userId = u.id " +
                        "WHERE c.id = @id";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new MySqlParameter("@id", id));
            DataBase.Instance.connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                cart = new Cart
                {
                    Id = reader.GetInt32(0),
                    total = reader.GetDecimal(1),
                    User = new User
                    {
                        Id = reader.GetInt32(2),
                        LastName = reader.GetString(3),
                        FirstName = reader.GetString(4)
                        
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
