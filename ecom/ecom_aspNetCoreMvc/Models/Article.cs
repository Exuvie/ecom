using ecom_aspNetCoreMvc.Tools;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ecom_aspNetCoreMvc.Models
{
    public class Article
    {
        private int id;
        private string title;
        private string description;
        private decimal? price;
        private DateTime addedDate;
        private string urlImage;
        private int? idCategory;

        private static string request;
        private static MySqlCommand command;
        private static MySqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public decimal? Price { get => price; set => price = value; }
        public DateTime AddedDate { get => addedDate; set => addedDate = value; }
        public string UrlImage { get => urlImage; set => urlImage = value; }
        public int? IdCategory { get => idCategory; set => idCategory = value; }

        public Article()
        {
        }

        public void AddArticle(Article a)
        {
            request = "INSERT INTO Article (title, description, price, addDate, urlImage, idCategory) VALUES (@title, @description, @price, @addDate, @urlImage, @idCategory)";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new MySqlParameter("@title", a.Title));
            command.Parameters.Add(new MySqlParameter("@description", a.Description));
            command.Parameters.Add(new MySqlParameter("@price", a.Price));
            command.Parameters.Add(new MySqlParameter("@addDate", DateTime.Now));
            command.Parameters.Add(new MySqlParameter("@urlImage", a.UrlImage));
            command.Parameters.Add(new MySqlParameter("@idCategory", a.idCategory));
            DataBase.Instance.connection.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            DataBase.Instance.connection.Close();
        }

        public static List<Article> GetArticleByCategory(int? idCategory)
        {
            List<Article> articles = new List<Article>();
            if (idCategory == null)
            {
                request = "SELECT * FROM Article";
                command = new MySqlCommand(request, DataBase.Instance.connection);
            }
            else
            {
                request = "SELECT * FROM Article WHERE idCategory = @idCategory";
                command = new MySqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new MySqlParameter("@idCategory", idCategory));
            }
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Article a = new Article
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    AddedDate = reader.GetDateTime(4),
                    urlImage = reader.GetString(5),
                    IdCategory = reader.GetInt32(6),
                };
                articles.Add(a);
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            return articles;
        }

        public static Article GetArticleById(int id)
        {
            Article a = null;
            request = "SELECT id, title, description, price, addDate, urlImage, idCategory FROM Article WHERE id = @id";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new MySqlParameter("@id", id));
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                a = new Article
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    AddedDate = reader.GetDateTime(4),
                    urlImage = reader.GetString(5),
                    IdCategory = reader.GetInt32(6),
                };
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            return a;
        }

        public static List<Article> GetArticles()
        {
            List<Article> articles = new List<Article>();
            request = "SELECT id, title, description, price, addDate, urlImage, idCategory FROM Article";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Article a = new Article
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    AddedDate = reader.GetDateTime(4),
                    urlImage = reader.GetString(5),
                    IdCategory = reader.GetInt32(6),
                };
                articles.Add(a);
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            return articles;
        }



        public static void DeleteArticle(Article a)
        {
            request = "DELETE FROM Article WHERE id = @id";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new MySqlParameter("@id", a.id));
            DataBase.Instance.connection.Open();
            command.ExecuteNonQuery();
            DataBase.Instance.connection.Close();
        }
    }
}
