using ecom_aspNetCoreMvc.Tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ecom_aspNetCoreMvc.Models
{
    public class Category
    {
        private int id;
        private string title;

        private static string request;
        private static SqlCommand command;
        private static SqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }

        public static void AddCategory(Category c)
        {
            request = "INSERT INTO Category (title) OUTPUT INSERTED.ID VALUES (@title)";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@title", c.Title));
            DataBase.Instance.connection.Open();
            c.Id = (int)command.ExecuteScalar();
            command.Dispose();
            DataBase.Instance.connection.Close();
        }

        public static bool DeleteCategory(Category c)
        {
            bool result = false;
            request = "DELETE FROM Category WHERE id = @id";
            command = new SqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new SqlParameter("@id", c.Id));
            DataBase.Instance.connection.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                request = "DELETE FROM Article WHERE idCategory = @id";
                command = new SqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new SqlParameter("@id", c.Id));
                while (command.ExecuteNonQuery() > 0)
                {
                    result = true;
                }
                //result = true;
            }
            DataBase.Instance.connection.Close();
            return result;
        }

        public static List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            request = "SELECT * FROM Category";
            command = new SqlCommand(request, DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            while(reader.Read())
            {
                Category c = new Category
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1)
                };
                categories.Add(c);
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            return categories;
        }

        public static bool CategoryExist(Category c)
        {
            bool result = false;
            if (c.Title != null)
            {
                request = "SELECT * FROM Category WHERE title = @title";
                command = new SqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new SqlParameter("@title", c.Title));
                DataBase.Instance.connection.Open();
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    c.Id = reader.GetInt32(0);
                    result = true;
                }
                else
                {
                    result = false;
                }
                reader.Close();
                command.Dispose();
                DataBase.Instance.connection.Close();
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
