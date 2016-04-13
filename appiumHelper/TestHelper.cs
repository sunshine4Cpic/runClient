using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Interfaces;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;

namespace appiumHelper
{
    public class TestHelper
    {
        public static AppiumDriver Driver;
        


        //等待XPATH元素查找
        public static IWebElement waitForElementByXPath(TestStep ts)
        {
        
            IWebElement ele = null;
            DateTime eDate = DateTime.Now.AddSeconds(60);
            while (DateTime.Now < eDate)
            {
                ele = null;
                //定时
                
               
                try
                {
                    var els = Driver.FindElementsByXPath(ts.Xpath);

                    if (els.Count >= ts.index)
                    {
                        ele = els[ts.index];
                        break;
                    }
                    else
                    {
                        var els2 = Driver.FindElementsByXPath(ts.Xpath2);
                        if (els2.Count >= ts.index)
                        {
                                //前期等待3s
                            swipAction("UP");
                            
                        }

                        continue;
                    }
                    /*
                    if (ele.Displayed == true && ele.Enabled == true)//判断页面元素是否在页面上显示
                    {
                        break;
                    }
                    else
                    {
                        //前期等待3s
                        swipAction("UP");
                    }*/

                }
                catch (Exception)
                {

                }

                Thread.Sleep(1000);

            }

            return ele;


        }
    

           //查找元素用文字
        public static IWebElement findElementbyName(string name, int index)
        {
            IWebElement ele = null;
            IList<IWebElement> list = null;
            int num = 0;
            while (true)
            {
                Thread.Sleep(1000);
                num++;
                if (num >= 20)
                {
                    //查找20s
                    ele = null;
                    break;
                }

                try
                {
                    list = Driver.FindElementsByXPath("//UIAApplication[1]/UIAWindow[1]/UIAScrollView[1]/UIAWebView[1]/*[@name='" + name + "' or @value='" + name + "']");
                    ele = list[index];
                    if (ele.Displayed == true && ele.Enabled == true)//判断页面元素是否在页面上显示
                    {
                        break;
                    }
                    else if (num >= 3)
                    {
                        swipAction("UP");
                    }

                }
                catch (Exception)
                {

                }

            }
            return ele;
        }
        
               //定向滑动
        public static void swipAction(string direction)
        {
            direction = direction.ToLower();//变小写
            IWebElement myScrean = Driver.FindElementByXPath("//UIAApplication[1]/UIAWindow[1]");
            Size mySize = myScrean.Size;
            int height = mySize.Height;
            int width = mySize.Width;//获得屏幕大小
            if (direction == "left")
            {
                Swipe(width -10, height / 2, 10, height / 2, 800);
            }
            else if (direction == "right")
            {
                Swipe(10, height / 2, width -10, height / 2, 800);
            }
            else if (direction == "up")
            {
                Swipe(width / 2, height / 2, width / 2, 10, 800);
            }
            else if (direction == "down")
            {
                Swipe(width / 2, height / 2, width / 2, height - 10, 800);
            }


        }
        //坐标点击
        public static void Tap(int x, int y)
        {
            ITouchAction a2 = new TouchAction(Driver);
            a2.Tap(x, y);
            a2.Perform();

        }

        public static void Tap(IWebElement ele)
        {
            int x =  ele.Location.X +ele.Size.Width/2;
            int y =  ele.Location.Y +ele.Size.Height/2;
            Tap(x, y);
        }

        //自定义滑动
        public static void Swipe(int startX, int startY, int endX, int endY, int duration)
        {

            ITouchAction touchAction = new TouchAction(Driver)
                .Press(startX, startY)
                .Wait(duration)
                .MoveTo(endX, endY)
                .Release();
            touchAction.Perform();
        }

        public static void Swipe(int startX, int startY, int endX, int endY)
        {
            Swipe(startX, startY, endX, endY, 800);
        }

        //反射创建类
        public static TestStep fanse(string ClassName, XElement step)
        {
            TestStep test;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Object[] parameters = new Object[1];
                parameters[0] = step;
                test = (TestStep)assembly.CreateInstance("appiumHelper." + ClassName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                if (test == null)
                {
                    test = new TestStep(step);
                }
            }catch(Exception){
                test = new TestStep(step);
            }
            return test;
        }



        
    }
}
