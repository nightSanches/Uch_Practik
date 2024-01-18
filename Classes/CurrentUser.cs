using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Classes
{
    public class CurrentUser
    {
        public static string login;
        public static string password;
        public static string connectString(string l, string p)
        {
            return $@"Data Source=SANCHES\SQLEXPRESS;Initial Catalog=Airport;User Id = {l};Password = {p}";
        }
    }
}
