using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeWebHelper
{
    class CW_ClickStep : TestStep
    {

        /// <summary>
        /// 0真实模拟 1标准
        /// </summary>
        public string clickType { get; set; }
        public string pointXY { get; set; }
        public CW_ClickStep(XElement step, TestHelper th)
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
                    case "clickType":
                        this.clickType = value;
                        break;
                    case "pointXY":
                        this.pointXY = value;
                        break;
                    default:
                        break;
                }

            }
        }


        public override void Excuo()
        {
            try
            {
                 
               

                //TestHelper.ch.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));//等到超时操作
                IWebElement element = th.findElement(this);

                
                if (element != null)
                {
                    th.snapshot(this);
                    element.Click();

                }
                else
                {
                    this.ResultStatic = "2";
                    this.ResultMsg = "找不到控件";
                    return;
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


        //获取百分比坐标
        public int getPoint(int length, String point)
        {
            if (point.Contains("%"))
            {
                return (int)(length * ((float.Parse(point.Replace("%", "").Trim())) / 100));
            }
            else
            {
                return (int)float.Parse(point.Trim());
            }
        }
    }


}
