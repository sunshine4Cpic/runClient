using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenQA.Selenium.Remote;
using OpenQA.Selenium;

using System.Reflection;
using System.Xml.Linq;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Xml.XPath;

namespace chromeHelper
{
    public class TestHelper
    {
        public bool isInitClikc =false;

        public  OpenQA.Selenium.Chrome.ChromeDriver ch;
        public  int resolutionWidth;//屏幕分辨率
        public  int resolutionHeight;
        public  int webViewWidth;//浏览器宽度
        public  int webViewHeight;
        public  int spacingTop;//浏览器到顶端距离
        public  int spacingLeft;//浏览器到顶端距离
        //public static string device;
        public  string resultPath;

        public chromeTestCase ctc { get; set; }


        /// <summary>
        /// 0为uiClick 1为标准
        /// </summary>
        public  string clickType
        {
            get;
            set;
        }
        //反射创建类
        public  TestStep fanse(string ClassName, XElement step)
        {
            TestStep test;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Object[] parameters = new Object[2];
                parameters[0] = step;
                parameters[1] = this;
                test = (TestStep)assembly.CreateInstance("chromeHelper." + ClassName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                if (test == null)
                {
                    test = new TestStep(step,this);
                }
            }catch(Exception e){
                test = new TestStep(step,this);
            }
            return test;
        }
    
        public  IWebElement findElement(TestStep ts)
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

            String js = String.Format("window.scroll(0, {0})", y / 2);
            ((IJavaScriptExecutor)ch).ExecuteScript(js);
            return we;
        }
        public  void swipAaction_tmp(String direction)
        {
        
            String js = null;
           
             if (direction == "up")
            {
                js = String.Format("window.scrollBy(0,300)");
            }
            else if (direction == "down")
            {
                js = String.Format("window.scrollBy(0,-300)");
            }

             ((IJavaScriptExecutor)ch).ExecuteScript(js);

        }

        public  void swipAaction(String direction)
        {
            Point p1 = new Point();
            Point p2 = new Point();



          
            if (direction == "up" || direction==null)
            {
                p1.X = resolutionWidth / 2;
                p1.Y = (int)(resolutionHeight * 0.75);

                p2.X = p1.X;
                p2.Y = 50;

               
            }
            else if (direction == "down")
            {

                p1.X = resolutionWidth / 2;
                p1.Y = (int)(resolutionHeight * 0.25);

                p2.X = p1.X;
                p2.Y = resolutionHeight-50;


                
            }
            else if (direction == "right")
            {
                p1.X = 50;
                p1.Y = (int)(resolutionHeight * 0.5);

                p2.X = resolutionWidth-50;
                p2.Y = p1.Y;

             
            }
            else if (direction == "left")
            {
                p1.X = resolutionWidth - 50;
                p1.Y = (int)(resolutionHeight * 0.5);

                p2.X = 50;
                p2.Y = p1.Y;
            }


            swipAaction(p1, p2);

            //JS滑动
            //js = String.Format("window.scrollBy(0,300)");
            //((IJavaScriptExecutor)ch).ExecuteScript(js);
            //ADB滑动
            //ExeCommand(exe);
        }


        public  void swipAaction(Point p1, Point p2)
        {
            String exe = String.Format("adb -s {4} shell input swipe {0} {1} {2} {3}",
                   p1.X, p1.Y, p2.X, p2.Y, ctc.device);

            ExeCommand(exe);
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

        public  void clickElement(IWebElement element)
        {

            clickElement(element, clickType);
        }


        public  void clickElement(IWebElement element,string clickType)
        {

            if (element != null)
            {
                if (clickType == "1")
                {
                    element.Click();
                    return;
                }
                //获取控件的即时位置
                Dictionary<string, object> obj = (Dictionary<string, object>)this.ch.ExecuteScript(
               "var range = document.createRange();" +
               "range.selectNodeContents(arguments[0]);" +
               "return range.getBoundingClientRect();", element);

                int left = (int)Convert.ToSingle(obj["left"].ToString());
                int top = (int)Convert.ToSingle(obj["top"].ToString());
                int width = (int)Convert.ToSingle(obj["width"].ToString());
                int height = (int)Convert.ToSingle(obj["height"].ToString());
                int scale = this.resolutionWidth / this.webViewWidth;
                int webViewX = spacingLeft;
                int webViewY = spacingTop;

                int pointX = webViewX + (left + (width / 2)) * scale;
                int pointY = webViewY + (top + (height / 2)) * scale;

                String exe = String.Format("adb -s {2} shell input tap {0} {1}", pointX, pointY, ctc.device);
                this.ExeCommand(exe);

            }
        }
        



        public  int[] getSpace()
        {


            String path = resultPath + "ui.xml";
            String pullPath = String.Format("adb -s {1} pull /sdcard/test/ui.xml {0}", path, ctc.device);

            XElement WebView = null;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    //截取手机屏幕UI文件传到运行目录

                    this.ExeCommand(String.Format("adb -s {0} shell /system/bin/uiautomator dump sdcard/test/ui.xml", ctc.device));

                    this.ExeCommand(pullPath);

                    XElement xe = XElement.Load(path);
                    WebView =  xe.XPathSelectElement("//*[@class='android.webkit.WebView' or @resource-id='com.android.chrome:id/toolbar_shadow']");
                    /*WebView = (from e in xe.Descendants()
                               where e.Attribute("class") != null
                               && (e.Attribute("class").Value == "android.webkit.WebView" || e.Attribute("resource-id").Value == "com.android.chrome:id/toolbar_shadow") //
                               select e).First();*/
                    break;
                   
                }
                catch (Exception e) { Console.WriteLine("chrome-getSpace错误.......err:" + e.Message); }
                Thread.Sleep(500);
            }
            if(WebView==null)
            {
                Console.WriteLine("chrome-错误.......WebView==null");
                throw new Exception("错误信息123");
            }

            String boundsValue = WebView.Attribute("bounds").Value;

            String left = boundsValue.Split(',')[0].Trim().Split('[')[1];
            String top = boundsValue.Split(',')[1].Trim().Split(']')[0];
            float left1 = float.Parse(left);
            float top1 = float.Parse(top);
            int spacingLeft = Convert.ToInt32(left1);
            int spacingTop = Convert.ToInt32(top1);

            int[] spacing = new int[] { spacingLeft, spacingTop };
            return spacing;
        }


        public  void snapshot(TestStep ts)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                ch.GetScreenshot().SaveAsFile(resultPath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                ts.Step.SetAttributeValue("Photo", fileName);
                
                
            }
            catch { }
            
        }


        public  void snapshot(string fileName)
        {
            try
            {
                
                ch.GetScreenshot().SaveAsFile(resultPath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch { }

        }

        /// <summary>
        /// 初始化click设置,启用屏幕点击必须
        /// </summary>
        public void initClick()
        {
            if (isInitClikc) return;
            //获取浏览器到顶部,侧面的距离
            int[] spacing = this.getSpace();
            this.spacingLeft = spacing[0];
            this.spacingTop = spacing[1];

            /*
            //获取手机分辨率
            String str = this.ExeCommand("adb -s " + ctc.device + " shell dumpsys window windows |findstr Requested").Trim();
            String[] resStr = str.Split(new String[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            String str1 = resStr[0].Trim().Split(new String[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            String resolutionWidth = str1.Split(' ')[0].Trim();
            this.resolutionWidth = Int32.Parse(resolutionWidth);

           
            */


            String str = ExeCommand("adb -s " + ctc.device + " shell dumpsys window | findstr ShownFrame | findstr isReadyForDisplay()=true").Trim();
            String[] resStr = str.Split(new String[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            String str1 = resStr[0].Trim().Split(new String[] { "][" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            //获取分辨率宽度
            String resolutionWidth = str1.Split(',')[0].Trim();
            float f1 = float.Parse(resolutionWidth);
            this.resolutionWidth = Convert.ToInt32(f1); ;
            //获取分辨率高度
            String resolutionHeight = str1.Split(',')[1].Split(']')[0].Trim();
            float f2 = float.Parse(resolutionHeight);
            this.resolutionHeight = Convert.ToInt32(f2);





            //获取浏览器的分辨率
            string webViewWidth = this.ch.ExecuteScript("return window.screen.width").ToString();
            string webViewHeight = this.ch.ExecuteScript("return window.screen.height").ToString();
            this.webViewWidth = Int32.Parse(webViewWidth);
            this.webViewHeight = Int32.Parse(webViewHeight);

            this.isInitClikc = true;

        }


        public void KillPort(int port)
        {

            string BatPath = System.Environment.CurrentDirectory + "\\kill.bat";

            ExeCommand(BatPath + " " + port);

            //return 0;
        }

      

    }
}
