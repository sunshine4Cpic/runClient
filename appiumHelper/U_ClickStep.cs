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

        /// <summary>
        /// 坐标
        /// </summary>
        public string offsetXY
        {
            get;
            set;
        }

        public int clickCnt
        {
            get;
            set;

        }
        public U_ClickStep()
        {

        }

        public U_ClickStep(XElement step)
            : base(step)
        {
            XElement xe = (from e in step.Descendants("ParamBinding")
                           where e.Attribute("name").Value == "PointXY"
                           select e).FirstOrDefault();
            if (xe != null)
                this.PointXY = xe.Attribute("value").Value;

            xe = step.Descendants("ParamBinding").FirstOrDefault(t => t.Attribute("name").Value == "offsetXY");
            if(xe!=null)
                this.offsetXY = xe.Attribute("value").Value;
        
            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "clickCnt"
                  select e).FirstOrDefault();

            if (xe != null)
            {
                
                var tmp = xe.Attribute("value").Value;
                if(!string.IsNullOrEmpty(tmp))
                    this.clickCnt = Int32.Parse(tmp);
                else
                    this.clickCnt = 1;
            }
                
          
            
            
           
        }
        public override void Excuo()
        {
            try
            {
                if (!string.IsNullOrEmpty(PointXY))
                { //坐标点击
                    var xy = getXY(PointXY);
                   
                    
                        helper.Tap(xy[0], xy[1]);
                    
                }
                else
                {
                    IWebElement ele = helper.waitForElementByXPath(this);
                    if (ele != null)
                    {
                        if(!string.IsNullOrEmpty(offsetXY))
                        {

                           // clickCnt = 10;
                            var xy = getXY(offsetXY);
                          
                                helper.Tap(ele, xy[0], xy[1],clickCnt);
                            
                        }
                        else
                        {
                            if (ele.Displayed == true)
                                ele.Click();
                            else
                                helper.Tap(ele);
                        }

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


        private int[] getXY(string XY)
        {
            int[] rt = { 0, 0 };
            try
            {
                string[] arry = XY.Split(',');
                rt[0] = Convert.ToInt32(arry[0]);
                rt[1] = Convert.ToInt32(arry[1]);
            }
            catch (Exception)
            {
                
                
            }
            

            return rt;
        }

    }
}
