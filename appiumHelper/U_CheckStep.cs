using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace appiumHelper
{
    public class U_CheckStep : TestStep
    {

        public string checkMode
        {
            get;
            set;

        }


        public U_CheckStep(XElement step)
            : base(step)
        {
            XElement xe = (from e in step.Descendants("ParamBinding")
                           where e.Attribute("name").Value == "checkMode"
                           select e).FirstOrDefault();
            if (xe != null)

                this.checkMode = xe.Attribute("value").Value;
           



        }

        public override void Excuo()
        {
          
           
                if (checkMode != "notfound")
                {
                    try
                    {
                        var ele = helper.Driver.FindElementByXPath(this.Xpath2);

                        IWebElement ele2 = helper.waitForElementByXPath(this);

                    }
                    catch {
                        this.ResultStatic = "2";
                        this.ResultMsg = "未找到控件";
                        return;
                    }

                  

                  
                   
                }
                else
                {
                    try
                    {
                        var ele = helper.Driver.FindElementByXPath(this.Xpath2);
                        this.ResultStatic = "2";
                        this.ResultMsg = "控件未消失";
                        return;
                    }
                    catch{
                        
                       
                    }

                    

                   
                }
            base.Excuo();
        }
    }
}
