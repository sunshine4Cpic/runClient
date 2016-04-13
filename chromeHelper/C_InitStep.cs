using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeHelper
{
    class C_InitStep : TestStep
    {
        public string url { get; set; }

        /// <summary>
        /// 0真实模拟 1标准
        /// </summary>
        public string clickType { get; set; }

        public C_InitStep(XElement step, TestHelper th)
            : base(step, th)
        {

            
            List<XElement> ParamBindings = (from e in step.Descendants("ParamBinding")
                                            select e).ToList();
            foreach (XElement xe in ParamBindings)
            {
                string name = xe.Attribute("name").Value;
                string value = xe.Attribute("value").Value;
                switch (name)
                {

                    case "url":
                        this.url = value;
                        break;
                    case "clickType":
                       
                            this.clickType =  value;
                        break;
                    default:
                        break;
                }

            }

            

        }

        public override void Excuo()
        {

            //System.setProperty("webdriver.chrome.driver", "C:\\Documents and Settings\\Administrator\\Local Settings\\Application Data\\Google\\Chrome\\Application\\chromedriver.exe");

            
            string chromeDir = System.Environment.CurrentDirectory + "\\RunApk\\";
            string chrome = chromeDir+"chromedriver.exe";

            th.ExeCommand("adb start-server");
            OpenQA.Selenium.Chrome.ChromeOptions co;
            OpenQA.Selenium.Chrome.ChromeDriver ch;
            co = new ChromeOptions();
            co.AddAdditionalCapability("androidPackage", "com.android.chrome");
            co.AddAdditionalCapability("androidDeviceSerial", th.ctc.device);//5T2RDQ154U017456

            ChromeDriverService cds = ChromeDriverService.CreateDefaultService(chromeDir);
            cds.Port = th.ctc.port;
           
            if (File.Exists(chrome))
            {
                ch = new ChromeDriver(cds, co);
            }else
            {
                ch = new ChromeDriver(co);
            }

            th.ch = ch;
            
            ch.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));//等待超时

            //if (this.clickType.ToLower().Contains("screen"))
            if (this.clickType == "1")
            {
                th.clickType = "1";
            }
            else
            {
                try
                {
                    th.initClick();
                    th.clickType = "0";
                }
                catch (Exception)
                {
                    Console.WriteLine("chrome UIClick初始化失败 启用标准click.......");
                    th.clickType = "1";
                }
            }

            ch.Navigate().GoToUrl(url);
            th.ch = ch;

            th.snapshot(this);
            base.Excuo();


        }


        
    }
}

