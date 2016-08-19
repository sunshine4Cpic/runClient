using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Interfaces;

namespace appiumHelper
{


    class U_selectSwip : TestStep
    {


        public string moveText
        {
            get;
            set;

        }
        public string startText
        {
            get;
            set;

        }


        public U_selectSwip(XElement step)
            : base(step)
        {


            XElement xe;
            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "moveText"
                  select e).FirstOrDefault();

            if (xe != null)
                this.moveText = xe.Attribute("value").Value;


            xe = (from e in step.Descendants("ParamBinding")
                  where e.Attribute("name").Value == "startText"
                  select e).FirstOrDefault();

            if (xe != null)
                this.startText = xe.Attribute("value").Value;
            var ts = ("//*[@name='" + this.startText + "']");
        }


        public override void Excuo()
        {
            swip2();
        }



        private void swip(int Startheight)
        {
            //var Start = TestHelper.Driver.FindElementByXPath("//*[@name='" + this.startText + "']");
            //int Startheight = Start.Location.Y;
            //int Startwidth = Start.Location.X;

            var Move = helper.Driver.FindElementByXPath("//*[@name='" + this.moveText + "']");
            int Movewidth = Move.Location.X;
            int Moveheight = Move.Location.Y;

           
                // TestHelper.swip(Startwidth, Startheight, Movewidth, Moveheight);
            helper.Swipe(Movewidth, Startheight, Movewidth, Startheight - (Moveheight - Startheight), 800);


            base.Excuo();
       

                

        }


        private void swip2()
        {
            var Start = helper.Driver.FindElementByXPath("//*[@name='" + this.startText + "']");
            int Startheight = Start.Location.Y;
            int Startwidth = Start.Location.X;

            var Move = helper.Driver.FindElementByXPath("//*[@name='" + this.moveText + "']");
            int Movewidth = Move.Location.X;
            int Moveheight = Move.Location.Y;



           
                var tt = (Moveheight - Startheight) / 300;

                for (int i = 0; i < tt; i++)
                {
                    helper.Swipe(Startwidth, Startheight, Movewidth, Startheight - 300);
                }


            
         
           
            



            swip(Startheight);





        }

    }
}
