using BaseHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
namespace appiumHelper
{

    public class appiumTestCase : baseTestCase
    {


        public string url
        {
            get;
            set;
        }

        public string app
        {
            get;
            set;
        }

        public string udid
        {
            get;
            set;
        }


        Thread DebugT;


        //案例ID
        public string CaseID
        {
            get;
            set;
        }
        //案例名
        public string CaseName
        {
            get;
            set;
        }
        //案例列表

        internal List<TestStep> StepList
        {
            get;
            set;
        }

        public appiumTestCase(XElement xe)
        {
            this.caseXml = xe;
            init();
        }

        public appiumTestCase()
        {

        }
       

        TestHelper helper;



        /*
         * 
         * 解析XML生成list列表
         */
        private void init()
        {
            helper = new TestHelper();

            this.StepList = new List<TestStep>();

            this.resultXml = new XElement(this.caseXml);
            if (this.resultXml == null)
            {
                throw new Exception("无xml");
            }
            this.CaseName = resultXml.Attribute("desc").Value;
            //this.CaseID = resultXml.Attribute("customno").Value;
            List<XElement> steps = (from e in resultXml.Descendants("Step")
                                    select e).ToList();

            foreach (XElement step in steps)
            {
                string name = step.Attribute("name").Value;
                TestStep ts = TestHelper.fanse(name, step);

                this.StepList.Add(ts);
            }



        }

        public override void run(string resultPath)
        {
            base.outputCaseXml(resultPath);//保存案例文件

            this.resultXml = new XElement(caseXml);//结果准备

            try
            {
                init();
            }catch{}

            try
            {
                startRun(resultPath);
            }
            catch{}

            try
            {
                TestHelper.Driver.Quit();
            }
            catch{
            }




        }

        private void startRun(string resultPath)
        {
            int i = 0;
            foreach (TestStep step in this.StepList)
            {
                //设置appium地址
                if (step is U_InitStep)
                {
                    (step as U_InitStep).url = this.url;
                    (step as U_InitStep).app = this.app;
                    (step as U_InitStep).udid = this.udid;
                }

                i++;
                try
                {

                    step.Excuo();
                    if (!step.ResultStatic.Equals("1"))
                        break;
                }
                catch (Exception e)
                {
                    step.ResultStatic = "3";
                    step.ResultMsg = e.Message;
                    break;
                }
                finally
                {

                    try
                    {
                        Screenshot screen = TestHelper.Driver.GetScreenshot();
                        screen.SaveAsFile(resultPath + i + ".jpg", ImageFormat.Jpeg);

                        step.Step.SetAttributeValue("Photo", i + ".jpg");
                        step.inputElement();
                        base.outputResultXml(resultPath);
                        //截图
                       
                        
                    }
                    catch (Exception)
                    {

                    }

                }


            }
        }
      

        public override void Debug(string resultPath)
        {
            try
            {
                DebugT.Abort();
            }
            catch { }

            try
            {
                TestHelper.Driver.Quit();
            }
            catch { }

            DebugT = new Thread(() =>
            {
                run(resultPath);
            });
            DebugT.Start();
            
        }
    }
}
