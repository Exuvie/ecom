﻿using ecom_aspNetCoreMvc.Tools;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ecom_aspNetCoreMvc.Models
{
    public class User
    {
        private int id;
        private string lastName;
        private string firstName;
        private string userName;
        private string phone;
        private string email;
        private string address;
        private int? zip;
        private string city;
        private string password;
        private string admin;
        private Cart cart;
        private List<Article> articles;

        private static string request;
        private static MySqlCommand command;
        private static MySqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string Address { get => address; set => address = value; }
        public int? Zip { get => zip; set => zip = value; }
        public string City { get => city; set => city = value; }
        public string Password { get => password; set => password = value; }
        public string Admin { get => admin; set => admin = value; }
        public Cart Cart { get => cart; set => cart = value; }
        public List<Article> Articles { get => articles; set => articles = value; }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public bool UserExist(User u)
        {
            bool result;
            if (u.Email != null)
            {
                request = "SELECT * FROM Users WHERE email = @email";
                command = new MySqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new MySqlParameter("@email", u.Email));
                DataBase.Instance.connection.Open();
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    u.Id = reader.GetInt32(0);
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

        public bool UserLogin(string email, string password, User u)
        {
            bool result = false;
            if (email != null)
            {
                MD5 md5Hash = MD5.Create();
                string passwordHash = GetMd5Hash(md5Hash, password);
                request = "SELECT Id, userName FROM Users WHERE email=@email and password=@password";
                command = new MySqlCommand(request, DataBase.Instance.connection);
                command.Parameters.Add(new MySqlParameter("@email", email));
                command.Parameters.Add(new MySqlParameter("password", passwordHash));
                DataBase.Instance.connection.Open();
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    u.Id = reader.GetInt32(0);
                    u.UserName = reader.GetString(1);
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

        public void AddUser(User u)
        {
            request = "INSERT INTO Users (lastName, firstName, userName, phone, email, address, zip, city, password)" +
                "VALUES (@lastName, @firstName, @userName, @phone, @email, @address, @zip, @city, @password); SELECT LAST_INSERT_ID()";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            MD5 md5Hash = MD5.Create();
            string passwordHash = GetMd5Hash(md5Hash, u.Password);
            command.Parameters.Add(new MySqlParameter("@LastName", u.LastName));
            command.Parameters.Add(new MySqlParameter("@firstName", u.FirstName));
            command.Parameters.Add(new MySqlParameter("@userName", u.UserName));
            command.Parameters.Add(new MySqlParameter("@phone", u.Phone));
            command.Parameters.Add(new MySqlParameter("@email", u.Email));
            command.Parameters.Add(new MySqlParameter("@address", u.Address));
            command.Parameters.Add(new MySqlParameter("@zip", u.Zip));
            command.Parameters.Add(new MySqlParameter("@city", u.City));
            command.Parameters.Add(new MySqlParameter("@password", passwordHash));
            DataBase.Instance.connection.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            DataBase.Instance.connection.Close();
        }

        public static List<User> GetUserList()
        {
            List<User> users = new List<User>();
            command = new MySqlCommand("SELECT * FROM Users", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                User u = new User
                {
                    Id = reader.GetInt32(0),
                    LastName = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    UserName = reader.GetString(3),
                    Phone = reader.GetString(4),
                    Email = reader.GetString(5),
                    Address = reader.GetString(6),
                    Zip = reader.GetInt32(7),
                    City = reader.GetString(8)
                };
                users.Add(u);
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            return users;
        }

        public static List<User> GetUserByLastName(string lastName)
        {
            //User u = null;
            List<User> users = new List<User>();
            command = new MySqlCommand("SELECT * FROM Users WHERE lastName LIKE @lastName", DataBase.Instance.connection);
            command.Parameters.Add(new MySqlParameter("@lastName", "%" + lastName + "%"));
            DataBase.Instance.connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                User u = new User
                {
                    Id = reader.GetInt32(0),
                    LastName = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    UserName = reader.GetString(3),
                    Phone = reader.GetString(4),
                    Email = reader.GetString(5),
                    Address = reader.GetString(6),
                    Zip = reader.GetInt32(7),
                    City = reader.GetString(8)
                };
                users.Add(u);
            }
            reader.Close();
            command.Dispose();
            DataBase.Instance.connection.Close();
            return users;
        }

        public static void DeleteUser(User u)
        {
            request = "DELETE FROM Users WHERE id = @id";
            command = new MySqlCommand(request, DataBase.Instance.connection);
            command.Parameters.Add(new MySqlParameter("@id", u.id));
            DataBase.Instance.connection.Open();
            command.ExecuteNonQuery();
            DataBase.Instance.connection.Close();
        }

    }
}
