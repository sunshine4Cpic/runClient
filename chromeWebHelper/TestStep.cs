using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace chromeWebHelper
{
    public class TestStep
    {

        #region 属性

        public TestHelper th;

        public string userXPath
        {
            get;
            set;
        }

        public string Photo { get; set; }


        //用文字查找
        public string textContent
        {
            get;
            set;
        }

        public string id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string className
        {
            get;
            set;
        }


        public int index
        {
            get;
            set;
        }

        public int WaitTime
        {
            get;
            set;
        }

        private string _resultStatic = "0";//执行状态 0未执行 1执行成功 2执行失败 3报错
        public string ResultStatic
        {
            get
            {
                return _resultStatic;
            }
            set
            {
                _resultStatic = value;
            }
        }


        public string ResultMsg
        {
            get;
            set;
        }

        private string desc
        {
            get;
            set;
        }
        private String tagName
        {
            get;
            set;
        }

        public XElement Step
        {
            get;
            set;
        }

        #endregion 属性

        public TestStep(XElement step,TestHelper th)
        {
            this.th = th;
            this.Step = step;
            desc = step.Attribute("desc").Value;

            List<XElement> ParamBindings = (from e in step.Descendants("ParamBinding")
                                            select e).ToList();
            foreach (XElement xe in ParamBindings)
            {
                string name = xe.Attribute("name").Value;
                string value = xe.Attribute("value").Value;
                switch (name)
                {
 
                    case "tagName":
                        this.tagName = value;
                        if (this.tagName == "")
                        {
                            this.tagName = null;
                        }
                        break;
                    case "id":
                        this.id = value;
                        if (this.id == "")
                        {
                            this.id = null;
                        }
                        break;
                    case "Name":
                        this.Name = value;
                        if (this.Name == "")
                        {
                            this.Name = null;
                        }
                        break;
                    case "textContent":
                        this.textContent = value;
                        if (this.textContent == "")
                        {
                            this.textContent = null;
                        }
                        break;
                    case "className":
                        this.className = value;
                        if (this.className == "")
                        {
                            this.className = null;
                        }
                        break;
                    case "userXPath":
                        this.userXPath = value;
                        if (this.userXPath == "")
                        {
                            this.userXPath = null;
                        }
                        break;
                    case "index":
                        if (value.Trim() != "")
                            this.index = Convert.ToInt32(value);
                        break;
                    case "waitTime":
                        if (value.Trim() != "")
                            this.WaitTime = Convert.ToInt32(value);
                        else
                            this.WaitTime = 0;
                        break;
                    default:
                        break;
                }

            }
        }
        public String Xpath()
        {
            StringBuilder xp = new StringBuilder();
            if (this.tagName == null)
            {
                xp.Append("//*");
            }
            else
            {
                xp.Append("//" + this.tagName);
            }
            

            if (this.className != null)
            { 
                xp.Append("[@class='" + this.className + "']");
            }
            if (this.id != null)
            {
                xp.Append("[@id='" + this.id + "']");
            }
            if (this.Name != null)
            {
                xp.Append("[@Name='" + this.Name + "']");
            }
            

            if (this.textContent != null)
            {
               
                    xp.Append("[text()='" + this.textContent + "']");
            }

          

            return xp.ToString();

        }

        public virtual void Excuo()
        {
            Console.WriteLine("步骤:" + desc + " 执行成功");
            Thread.Sleep(this.WaitTime * 1000);
            this.ResultStatic = "1";
            this.ResultMsg = "执行成功";

        }

        public void inputElement()
        {
            this.Step.SetAttributeValue("ResultMsg", this.ResultMsg);
            this.Step.SetAttributeValue("ResultStatic", this.ResultStatic);
            this.Step.SetAttributeValue("Photo", this.Photo);

        }

    }
}
