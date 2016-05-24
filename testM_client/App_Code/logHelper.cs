using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4net.config", Watch = true)]
namespace testM_client
{
     

    public class logHelper
    {
       
        public static void error(Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("");
            log.Error(null, ex);
            Console.WriteLine("*** error:" + ex.StackTrace + " ***");
        }

        public static void error(string ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("");
            log.Error(ex);
            Console.WriteLine("*** error:" + ex + " ***");
        }



        public static void info(string txt)
        {
            Console.WriteLine(txt);
            log4net.ILog log = log4net.LogManager.GetLogger("Info");
            log.Info(txt);

        }

    }
}
