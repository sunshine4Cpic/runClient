using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeWebHelper
{
    class CW_EditStep : TestStep
    {
        public string inputText
        {
            get;
            set;
        }

        public CW_EditStep(XElement step, TestHelper th)
            : base(step, th)
        {
            XElement xe;
            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "inputText"
                  select e).FirstOrDefault();
            if (xe != null)
                this.inputText = xe.Attribute("value").Value;
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
                     element.Clear();
                     element.SendKeys(this.inputText);
                 }else
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
    }
}
