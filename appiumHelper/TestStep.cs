using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Configuration;
using System.Collections;


namespace appiumHelper
{


    public class TestStep
    {
        #region 属性


        public string userXPath
        {
            get;
            set;
        }

        public string Xpath
        {
            get
            {
                if (userXPath != null && userXPath.Trim() != "")
                    return userXPath;
                return Xpath2 + "[@visible='true' and @enabled='true']";
            }
        }

        public string Xpath2
        {
            get
            {
                if (userXPath != null && userXPath.Trim() != "")
                    return userXPath;
                StringBuilder sb = new StringBuilder();
                if (tagName == null || tagName.Trim() == "")
                {
                    sb.Append("//*");
                }
                else
                {
                    sb.Append("//" + this.tagName);
                }
                if (id != null && id.Trim() != "")
                {
                    sb.Append(string.Format("[@id='{0}']", id));
                }

                if (textContent != null && textContent.Trim() != "")
                {
                    sb.Append(string.Format("[@label='{0}' or @value='{0}']", textContent));
                }

                

               

                return sb.ToString();
            }
        }


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

        public string tagName
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


        public XElement Step
        {
            get;
            set;
        }

        public TestHelper helper { get; set; }

        #endregion 属性

        public TestStep()
        {
        }
        public TestStep(XElement step)
        {
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

                    case "id":
                        this.id = value;
                        break;
                    case "textContent":
                        this.textContent = value;
                        break;
                    case "tagName":
                        this.tagName = value;
                        break;
                    case "userXPath":
                        this.userXPath = value;
                        break;
                    case "index":
                        if (value.Trim() != "")
                            this.index = Convert.ToInt32(value) - 1;
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
            
        }
    }
}
