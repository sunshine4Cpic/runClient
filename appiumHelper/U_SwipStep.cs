using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace appiumHelper
{
    class U_SwipStep :TestStep
    {
        public string Direction
        {
            get;
            set;
        }

        public U_SwipStep(XElement step)
            : base(step)
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
                TestHelper.swipAction(this.Direction);
               
            }
            catch (Exception e)
            {
                this.ResultStatic = "3";
                this.ResultMsg = e.Message;
            }
            base.Excuo();
        }
    }
}
