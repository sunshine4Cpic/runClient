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
    class U_EditStep : TestStep
    {
        

        public string inputText
        {
            get;
            set;
        }

        /// <summary>
        /// 0 为clear  1为全选后点删除
        /// </summary>
        public int clearMode
        {
            get;
            set;
        }


        /// <summary>
        /// 输入结束后按键
        /// </summary>
        public string InputCompleteClick
        {
            get;
            set;
        }




        public U_EditStep(XElement step)
            : base(step)
        {
            XElement xe;
            xe = (from e in step.Descendants("ParamBinding")
                           where e.Attribute("name").Value == "inputText"
                           select e).FirstOrDefault();
            if (xe != null)
                this.inputText = xe.Attribute("value").Value;


            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "clearMode"
                           select e).FirstOrDefault();

            if (xe != null && xe.Attribute("value").Value.Trim() != "")
            {
               // this.clearMode = Convert.ToInt32(xe.Attribute("value").Value);
                var tmp = xe.Attribute("value").Value;
                if (tmp=="true")
                    this.clearMode =1;//清除
                else
                    this.clearMode = Convert.ToInt32(xe.Attribute("value").Value);


               
            }
   


            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "InputCompleteClick"
                  select e).FirstOrDefault();

            if (xe != null)
                this.InputCompleteClick = xe.Attribute("value").Value;

            InputCompleteClick = "完成";//测试代码
        }
        public override void Excuo()
        {
            
            try
            {

                IWebElement ele = helper.waitForElementByXPath(this);

                if (ele != null)
                {
                    sendKeyToElement(ele, this.inputText);

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



        public Boolean sendKeyToElement(IWebElement sendEle, string things)
        {
            
            if (clearMode == 1)
            {
                //try
                //{
                //    sendEle.Click();
                //    sendEle.Click();
                //    //判断里面是否有内容
                //    IWebElement mElement = TestHelper.Driver.FindElementByName("全选");
                //    if (mElement.Displayed != false)
                //    {
                //        mElement.Click();
                //        TestHelper.Driver.FindElementByName("删除").Click();
                //    }

                //}
                //catch { }
                sendEle.Clear();
            }else
            {
                
            }
            
            
            sendEle.SendKeys(things);

            if (InputCompleteClick != null && InputCompleteClick.Trim() != "")
            {
                try
                {
                    helper.Driver.FindElementByXPath
                        ("//UIAToolbar/*[@label='" + InputCompleteClick + "']").Click();
                }
                catch { }

            }


            return true;
        }
    }
}
