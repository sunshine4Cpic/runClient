using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace testM_client
{
    public class testHelper
    {

        public static List<M_application> mapps;

        static int port;

        static testHelper()
        {
            port = 4800;
            try
            {
               testRunDataDataContext trddc = new testRunDataDataContext();
                mapps = trddc.M_application.ToList();
            }
            catch
            {

                Console.WriteLine("M_application初始化失败");
            }
        }
        /// <summary>
        /// 获得robotium执行apk名
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public static M_application getApk(XElement xe)
        {
            M_application app =null;

            var appID = xe.XPathSelectElement("//ParamBinding[@name='applicationID']");
            if (appID != null)
            {
                int ID = Convert.ToInt32(appID.Attribute("value").Value);
                app = mapps.Where(t => t.ID == ID).FirstOrDefault();
               
            }
            if (app == null)
                app = testHelper.mapps.First();
           return app;
        }

        public static string ExeCommand(string commandText)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.LoadUserProfile = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.Arguments = string.Format(@"/c {0}", commandText);

            //string strOutput = p.StandardOutput.ReadToEnd();


            p.Start();

            p.WaitForExit();
            string strOutput = p.StandardOutput.ReadToEnd();
            p.Close();

            return strOutput;
        }

       

        /// <summary>
        /// 获取可用端口
        /// </summary>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            // Locate a free port on the local machine by binding a socket to
            // an IPEndPoint using IPAddress.Any and port 0. The socket will
            // select a free port.

            bool use = false;
            Socket portSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPEndPoint socketEndPoint = new IPEndPoint(IPAddress.Any, port);
                portSocket.Bind(socketEndPoint);
                socketEndPoint = (IPEndPoint)portSocket.LocalEndPoint;
            }
            catch
            {
                use = true;
            }
            finally
            {
                portSocket.Close();
            }

            return use;
        }


        public static int FindFreePort()
        {
            int tmpPort = port;
            do
            {
                if(!PortInUse(port))
                {
                    tmpPort = port++;
                    return tmpPort;
                }
                
            } while (++port<6000);
            throw new Exception("无可用端口");
            //return 0;
        }

      
    }


    /// <summary>
    /// 各种执行层的初始化
    /// </summary>
    public static class initHelp
    {
        
       

        public static void init(this robotiumHelper.robotiumTestCase _tc)
        {

            //需要优化 每次都读
            //testRunDataDataContext trddc = new testRunDataDataContext();


            M_application app =  testHelper.getApk(_tc.caseXml);
           
                _tc.RunApk = app.runApkName;
                _tc.package = app.package;
                _tc.isClear = app.isClear;
          

        }

        

        public static void init(this appiumHelper.appiumTestCase _tc)
        {
            //appium 需求少暂时先不改
            //testRunDataDataContext trddc = new testRunDataDataContext();


            var appID = _tc.caseXml.XPathSelectElement("//ParamBinding[@name='applicationID']");
            if (appID != null)
            {
                int ID = Convert.ToInt32(appID.Attribute("value").Value);
                var app = testHelper.mapps.Where(t => t.ID == ID).FirstOrDefault();
                if (app != null)
                {
                    _tc.app = app.package2;
                }
                else
                {
                    var f = testHelper.mapps.First();
                    _tc.app = f.package2;
                }
            }
            else
            {
                var f = testHelper.mapps.First();
                _tc.app = f.package2;
            }


        }


       


       
    }

    
}
