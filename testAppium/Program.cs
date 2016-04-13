using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using appiumHelper;
using System.Xml.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Diagnostics;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;

namespace testAppium
{
    class Program
    {
        static void Main(string[] args)
        {
           

            /*
            string fPath = System.Environment.CurrentDirectory + "\\点击返回按钮_11682.xml";
            XElement XE = XElement.Load(fPath);
            
            appiumTestCase tc = new appiumTestCase();
            tc.caseXml = XE;
            tc.udid = "30b35355badff668d89a65e3b70e5ad795c9d704";
            tc.app = "ceshikaifa.sunjianping.xiangmuzu";
            //tc.url = @"http://192.168.1.101:4723/wd/hub";
            tc.url = @"http://127.0.0.1:4723/wd/hub";
           


            tc.run(@"C:\Users\Admin\Desktop\新建文件夹\");
             */

            /*
            Uri serverUri = new Uri(@"http://127.0.0.1:4723/wd/hub");//连接设备ip地址
            DesiredCapabilities cap = new DesiredCapabilities();
            cap.SetCapability("platformName", "Android");
            cap.SetCapability("deviceName", "Android Emulator");//设备名


            cap.SetCapability("browserName", "Browser");//程序
            cap.SetCapability("device", "96997872");
            //cap.SetCapability("platformVersion", this.Edition);//版本
            AppiumDriver driver = new AppiumDriver(serverUri, cap, TimeSpan.FromSeconds(5000));
            */


            OpenQA.Selenium.Chrome.ChromeOptions co;
            OpenQA.Selenium.Chrome.ChromeDriver ch;
            co = new ChromeOptions();
            co.AddAdditionalCapability("androidPackage", "com.android.browser");
            //co.AddAdditionalCapability("androidActivity", "com.android.browser.BrowserActivity");
            co.AddAdditionalCapability("androidUseRunningApp", true);

            //co.AddAdditionalCapability("androidDeviceSerial", "5T2RDQ154U017456");//5T2RDQ154U017456
            ch = new ChromeDriver(co);
            Console.WriteLine(ch.PageSource); 
        }
    }
}
