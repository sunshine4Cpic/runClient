using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace chromeWebHelper
{
    public class TestHelper
    {
        public OpenQA.Selenium.Chrome.ChromeDriver ch { get; set; }
        public  string resultPath { get; set; }
        public int port { get; set; }

        //反射创建类
        public TestStep fanse(string ClassName, XElement step)
        {
            TestStep test;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Object[] parameters = new Object[2];
                parameters[0] = step;
                parameters[1] = this;
                test = (TestStep)assembly.CreateInstance("chromeWebHelper." + ClassName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                if (test == null)
                {
                    test = new TestStep(step, this);
                }
            }
            catch 
            {
                test = new TestStep(step, this);
            }
            return test;
        }

        public void snapshot(TestStep ts)
        {

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            snapshot(ts, fileName);
        }

        public void snapshot(TestStep ts, string fileName)
        {
            try
            {
                ch.GetScreenshot().SaveAsFile(resultPath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                if(ts.Photo==null)//没有快照时添加
                    ts.Photo = fileName;
            }
            catch { }

        }


        public void snapshot(string fileName)
        {
            try
            {

                ch.GetScreenshot().SaveAsFile(resultPath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch { }

        }

        public void KillPort(int port)
        {

            string BatPath = System.Environment.CurrentDirectory + "\\kill.bat";

            ExeCommand(BatPath + " " + port);

            //return 0;
        }

        public String ExeCommand(String commandText)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = String.Format(@"/c {0}", commandText);
            string strOutput = null;

            try
            {
                p.Start();
                p.StandardInput.WriteLine(commandText);
                p.StandardInput.WriteLine("exit");
                p.WaitForExit();
                strOutput = p.StandardOutput.ReadToEnd();
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }

            return strOutput;
        }

        public IWebElement findElement(TestStep ts)
        {
            IWebElement we = null;
            String xpath = ts.Xpath();
            if (ts.userXPath != null)
            {
                xpath = ts.userXPath;
            }
            if (ts.index > 0)
                we = ch.FindElementsByXPath(xpath)[ts.index - 1];
            else
                we = ch.FindElementByXPath(xpath);

            int y = we.Location.Y;

            ((IJavaScriptExecutor)ch).ExecuteScript("arguments[0].scrollIntoView();", we);

            //String js = String.Format("window.scroll(0, {0})", y / 2);
            //((IJavaScriptExecutor)ch).ExecuteScript(js);
            return we;
        }
    }



}
