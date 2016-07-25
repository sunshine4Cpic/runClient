using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
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

        public bool pressEnter { get; set; }

        public CW_EditStep(XElement step, TestHelper th)
            : base(step, th)
        {
            XElement xe;
            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "inputText"
                  select e).FirstOrDefault();
            if (xe != null)
                this.inputText = xe.Attribute("value").Value;

            //输入后回车
            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "pressEnter"
                  select e).FirstOrDefault();
            if (xe != null)
            {
                if (xe.Attribute("value").Value.ToLower() == "true")
                    pressEnter = true;
                else
                    pressEnter = false;
                
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
                     element.Clear();
                     element.SendKeys(this.inputText);
                     if(pressEnter)
                     {
                         Actions builder = new Actions(th.ch);
                         builder.SendKeys(Keys.Enter).Perform();
                     }
                     
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
