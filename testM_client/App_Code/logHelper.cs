using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testM_client
{
    public class logHelper
    {
        public static void error(string txt)
        {

            Console.WriteLine("****** error:" + txt + " ******");
  
        }

        public static void log(string txt)
        {
            Console.WriteLine(txt);
        }
    }
}
