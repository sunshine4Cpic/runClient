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
        public AppiumDriver Driver;

        public int width { get; set; }
        public int height { get; set; }

        //等待XPATH元素查找
        public IWebElement waitForElementByXPath(TestStep ts)
        {
        
            IWebElement ele = null;
            DateTime eDate = DateTime.Now.AddSeconds(60);
            while (DateTime.Now < eDate)
            {
                ele = null;
                //定时
                try
                {
                    var els = Driver.FindElementsByXPath(ts.Xpath2);
                    
                    if (els.Count > ts.index)
                    {
                        ele = els[ts.index];
                        break;
                    }
                    
                }
                catch (Exception)
                {

                }

                Thread.Sleep(1000);

            }

            int top = height / 4;
            int bottom = top * 3;
            int center = height / 2;

            if (ele.Location.Y > 0 && ele.Location.Y < height)
            {

            }
            else
            {
                int leave = ele.Location.Y - center;

                int oneHeight = bottom - top;
                for (int i = 0; i < leave / oneHeight; i++)
                {
                    Swipe(width / 2, bottom, width / 2, top, 800);

                }

                int last = leave % oneHeight;
                if (last > 100)
                {
                    Swipe(width / 2, bottom, width / 2, bottom - last, 800);
                }
               

            }
            return ele;
        }
    

           //查找元素用文字
        public IWebElement findElementbyName(string name, int index)
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
        public void swipAction(string direction)
        {
            direction = direction.ToLower();//变小写

            int padding = 30;
            
            if (direction == "left")
            {
                Swipe(width - padding, height / 2, padding, height / 2, 800);
            }
            else if (direction == "right")
            {
                Swipe(padding, height / 2, width - padding, height / 2, 800);
            }
            else if (direction == "up")
            {
                Swipe(width / 2, height / 4 * 3, width / 2, padding, 800);
            }
            else if (direction == "down")
            {
                Swipe(width / 2, height / 4, width / 2, height - padding, 800);
            }


        }
        //public static void swip(int x1, int y1, int x2, int y2)
        //{

        //    Swipe(x1, y1, x1, y1-(y2 - y1));
        
        //}

                //坐标点击
        public void Tap(int x, int y)
        {
          
            ITouchAction a2 = new TouchAction(Driver)
                .Press(x, y)
                .Wait(50)
                .Release();
           
            a2.Perform();

        }

        public void Tap(IWebElement ele,int offset_x,int offset_y,int count)
        {
            int x=ele.Location.X+ele.Size.Width/2;
            int y=ele.Location.Y+ele.Size.Height/2;
            if (offset_x > 0)
                x += ele.Size.Width / 2 + offset_x;
            else if (offset_x<0)
                x = x - ele.Size.Width / 2 + offset_x;

            if(offset_y>0)
                //y += ele.Size.Height / 2 + offset_y;
                y +=  offset_y;
            else if (offset_y < 0)
                y = y - ele.Size.Height / 2 + offset_y;
            for (int i = 0; i < count; i++)
            {

                Tap(x, y);
            }
        }

        

        public void Tap(IWebElement ele)
        {
            int x =  ele.Location.X +ele.Size.Width/2;
            int y =  ele.Location.Y +ele.Size.Height/2;
            Tap(x, y);
        }

        //自定义滑动
        public void Swipe(int startX, int startY, int endX, int endY, int duration)
        {

            ITouchAction touchAction = new TouchAction(Driver)
                .Press(startX, startY)
                .Wait(duration)
                .MoveTo(endX, endY)
                .Release();
            touchAction.Perform();
        }

        public void Swipe(int startX, int startY, int endX, int endY)
        {
            Swipe(startX, startY, endX, endY, 800);
        }

        //反射创建类
        public TestStep fanse(string ClassName, XElement step)
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
                test.helper = this;
            }catch(Exception){
                test = new TestStep(step);
            }
            return test;
        }



        
    }
}
