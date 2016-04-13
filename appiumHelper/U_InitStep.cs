using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace appiumHelper
{
    class U_InitStep :TestStep
    {

        public string url
        {
            get;
            set;
        }
        public U_InitStep(XElement step)
            : base(step)
        {
            
            
          
            
        }

        public string app
        {
            get;
            set;
        }

        public string udid
        {
            get;
            set;
        }


        public override void Excuo()
        {
            
            Uri serverUri = new Uri(this.url);//连接设备ip地址
            DesiredCapabilities cap = new DesiredCapabilities();
            cap.SetCapability("platformName", "iOS");
            cap.SetCapability("deviceName", "iPhone");//设备名

           
            cap.SetCapability("app", app);//程序
            cap.SetCapability("udid", udid);//设备ID
            //cap.SetCapability("platformVersion", this.Edition);//版本
            AppiumDriver driver = new AppiumDriver(serverUri, cap, TimeSpan.FromSeconds(5000));




            //CancelUpdate(driver);

            TestHelper.Driver = driver;
            base.Excuo();

            

        }


        private void CancelUpdate(AppiumDriver Driver)
        {
            
            Thread.Sleep(5000);
            try
            {
                var up = Driver.FindElementByXPath("//*[@label='更新' or @value='更新']");

                if (up != null)
                {
                    Driver.FindElementByXPath("//*[@label='关闭' or @value='关闭']").Click();
                }
            }
            catch 
            {
                
                
            }
            
        }
    }
}
