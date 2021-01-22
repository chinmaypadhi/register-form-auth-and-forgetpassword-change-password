using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WebApplication76
{
    public class globalConnection
    {
       public static string str;
         static globalConnection()
        {
            str = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
        }
    }
}