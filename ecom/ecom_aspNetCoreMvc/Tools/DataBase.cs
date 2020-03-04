using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ecom_aspNetCoreMvc.Tools
{
    public class DataBase
    {
        private static DataBase instance = null;
        private static object _lock = new object();
        private string connectionString = @"Data Source=(LocalDb)\ecom;Integrated Security=True";
        public SqlConnection connection;

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
            connection = new SqlConnection(connectionString);
        }
    }


}
