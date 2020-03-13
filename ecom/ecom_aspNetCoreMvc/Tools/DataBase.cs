using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom_aspNetCoreMvc.Tools
{
    public class DataBase
    {
        private static DataBase instance = null;
        private static object _lock = new object();
        private string connectionString = "Rentre ici la chaine";
        public MySqlConnection connection;

        public static DataBase Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new DataBase();
                        return instance;
                }
            }
        }

        private DataBase()
        {
            connection = new MySqlConnection(connectionString);
        }
    }


}
