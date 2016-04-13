using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeHelper
{
    class C_Control : TestStep
    {
        public String key
        {
            get;
            set;
        }

        public C_Control(XElement step)
            : base(step)
        {
            XElement xe = (from e in step.Descendants("ParamBinding")
                           where e.Attribute("name").Value == "key"
                           select e).FirstOrDefault();
            if (xe != null)
                this.key = xe.Attribute("value").Value;
        }

        public override void Excuo()
        {
            if (key.Equals("back"))
            {
                try
                {
                    TestHelper.ch.Navigate().Back();
                }
                catch (Exception e)
                {
                    this.ResultStatic = "3";
                    this.ResultMsg = e.Message;
                    return;
                }
            }
           
            base.Excuo();
        }
    }
}
