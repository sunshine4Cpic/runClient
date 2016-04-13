using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Interfaces;
using System.Xml.Linq;

namespace appiumHelper
{
    class U_ClickStep : TestStep
    {

        /// <summary>
        /// 坐标
        /// </summary>
        public string PointXY
        {
            get;
            set;
        }


        public U_ClickStep(XElement step)
            : base(step)
        {
            XElement xe = (from e in step.Descendants("ParamBinding")
                           where e.Attribute("name").Value == "PointXY"
                           select e).FirstOrDefault();
            if (xe != null)
                this.PointXY = xe.Attribute("value").Value;


        }
        public override void Excuo()
        {
            try
            {
                if (this.PointXY != null && this.PointXY.Trim()!="")
                { //坐标点击
                    string[] arry = this.PointXY.Split(',');
                    int x, y;

                    x = Convert.ToInt32(arry[0]);
                    y = Convert.ToInt32(arry[1]);

                    TestHelper.Tap(x, y);
                }
                else
                {
                    IWebElement ele = TestHelper.waitForElementByXPath(this);
                    if (ele != null)
                    {
                        if (ele.Displayed == true)
                            ele.Click();
                        else
                            TestHelper.Tap(ele);
                    }
                    else
                    {
                        this.ResultStatic = "2";
                        this.ResultMsg = "未找到控件";
                        return;
                    }
                }

            }
            catch (Exception e)
            {
                this.ResultStatic = "3";
                this.ResultMsg = e.Message;
                return;
            }

            base.Excuo();
        }

    }
}
