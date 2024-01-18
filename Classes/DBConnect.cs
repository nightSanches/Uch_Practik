using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Airport.Classes
{
    public class DBConnect
    {
        public static SqlConnection Connection()
        {
            SqlConnection sqlConnection = new SqlConnection($@"Data Source=SANCHES\\SQLEXPRESS;Initial Catalog=Airport;User Id = {CurrentUser.login};Password = {CurrentUser.password}\");
            sqlConnection.Open();
            return sqlConnection;
        }

        public static SqlDataReader Query(string Query, SqlConnection Connecton)
        {
            return new SqlCommand(Query, Connecton).ExecuteReader();
        }

        public static void CloseConnection(SqlConnection Connection)
        {
            Connection.Close();
        }
    }
}
