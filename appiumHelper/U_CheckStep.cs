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

        public U_CheckStep(XElement step)
            : base(step)
        {

        }

        public override void Excuo()
        {
            try
            {
                IWebElement ele = helper.waitForElementByXPath(this);
                if (ele == null)
                {
                  
                    this.ResultStatic = "2";
                    this.ResultMsg = "未找到控件";
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
