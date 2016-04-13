using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeHelper
{
    class C_CheckStep : TestStep
    {
        public C_CheckStep(XElement step,TestHelper th)
            : base(step, th)
        {
        }

        
        public override void Excuo()
        {
            try
            {
                //TestHelper.ch.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));//等到超时操作
                IWebElement element = th.findElement(this);
                if (element == null)
                {
                  
                    this.ResultStatic = "2";
                    this.ResultMsg = "未找到控件";
                    return;
                }
                th.snapshot(this);
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
