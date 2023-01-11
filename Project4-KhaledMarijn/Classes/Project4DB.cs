using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4_KhaledMarijn.Classes
{
    internal class Project4DB
    {
    
        private string connString =
        ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
    }
}
