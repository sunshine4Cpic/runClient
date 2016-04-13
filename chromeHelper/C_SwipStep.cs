using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeHelper
{
    class C_SwipStep : TestStep
    {
        public string Direction
        {
            get;
            set;
        }
        public C_SwipStep(XElement step, TestHelper th)
            : base(step, th)
        {
            XElement xe = (from e in step.Descendants("ParamBinding")
                           where e.Attribute("name").Value == "Direction"
                           select e).FirstOrDefault();
            if (xe != null)
                this.Direction = xe.Attribute("value").Value;
        }

         public override void Excuo()
         {
            
             try
             {
                 th.swipAaction(this.Direction);
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
