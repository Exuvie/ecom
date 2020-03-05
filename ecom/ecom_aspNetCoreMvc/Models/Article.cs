using ecom_aspNetCoreMvc.Tools;
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
        private decimal price;
        private DateTime addedDate;
        private string urlImage;
        private int idCategory;

        private static string request;
        private static SqlCommand command;
        private static SqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public decimal Price { get => price; set => price = value; }
        public DateTime AddedDate { get => addedDate; set => addedDate = value; }
        public string UrlImage { get => urlImage; set => urlImage = value; }
        public int IdCategory { get => idCategory; set => idCategory = value; }

        public bool AddArticle(Article a)
        {
            request = "INSERT INTO Article (title, description, price, addDate, urlImage, idCategory) OUTPUT INSERTED.ID VALUES (@title, @description, @price, @addDate, @urlImage, @idCategory)";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@title", a.Title));
            command.Parameters.Add(new SqlParameter("@description", a.Description));
            command.Parameters.Add(new SqlParameter("@price", a.Price));
            command.Parameters.Add(new SqlParameter("@addDate", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@urlImage", a.UrlImage));
            command.Parameters.Add(new SqlParameter("@idCategory", a.idCategory));
            DataBase.Instance.connection.Open();
            a.Id = (int)command.ExecuteScalar();
            command.Dispose();
            DataBase.Instance.connection.Close();
            return Id > 0;
        }

        public static List<Article> GetArticles(int? idCategory)
        {
            List<Article> articles = new List<Article>();
            if (idCategory == null)
            {
                request = "SELECT * FROM Article";
                command = new SqlCommand(request, DataBase.Instance.connection);
            }
            else
            {
                request = "SELECT * FROM Article WHERE idCategory = @idCategory";
                command = new SqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new SqlParameter("@idCategory", idCategory));
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
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@id", id));
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

        public static void DeleteArticle(Article a)
        {
            request = "DELETE FROM Article WHERE id = @id";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@id", a.id));
            DataBase.Instance.connection.Open();
            command.ExecuteNonQuery();
            DataBase.Instance.connection.Close();
        }
    }
}
