using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeWebHelper
{
    class CW_InitStep : TestStep
    {
        public string url { get; set; }



        public CW_InitStep(XElement step, TestHelper th)
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
                    default:
                        break;
                }

            }

            

        }

        public override void Excuo()
        {


            string chromeDir = System.Environment.CurrentDirectory + "\\RunApk\\";
            string chrome = chromeDir + "chromedriver.exe";
            ChromeDriverService cds = ChromeDriverService.CreateDefaultService(chromeDir);
            cds.Port = th.port;

              ChromeOptions co =new ChromeOptions();
              co.AddArgument("test-type");
              co.AddArgument("start-maximized");

            ChromeDriver ch;
            if (File.Exists(chrome))
            {
                ch = new ChromeDriver(cds, co);
            }
            else
            {
                ch = new ChromeDriver(co);
            }

            

            th.ch = ch;


            ch.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));//等待超时

   

            ch.Navigate().GoToUrl(url);
            th.ch = ch;

            th.snapshot(this);
            base.Excuo();


        }


        
    }
}

