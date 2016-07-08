using BaseHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace chromeWebHelper
{
    public class chromeWebCase:baseTestCase
    {
        TestHelper th;

        private string _device;
      

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


        public chromeWebCase(int port)
        {
            th = new TestHelper();
            th.port = port;
        }
       
        
        private void init()
        {
          

           
            this.StepList = new List<TestStep>();

            this.resultXml = new XElement(this.caseXml);
            if (this.resultXml == null)
            {
                throw new Exception("无xml");
            }
            this.CaseName = resultXml.Attribute("desc").Value;
            List<XElement> steps = (from e in resultXml.Descendants("Step")
                                    select e).ToList();

            foreach (XElement step in steps)
            {
                string name = step.Attribute("name").Value;
                TestStep ts = th.fanse(name, step);

                this.StepList.Add(ts);
            }


        }




        


        public override void run(string resultPath)
        {
            base.outputCaseXml(resultPath);//保存案例文件
            th.resultPath = resultPath;
            this.resultXml = new XElement(caseXml);//结果准备

            try
            {
                init();
                th.KillPort(th.port);
            }
            catch (Exception e) { Console.WriteLine("chrome-init错误.......err:" + e.Message); }

            try
            {
                startRun(resultPath);
            }
            catch (Exception e) { Console.WriteLine("chrome-startRun错误.......err:" + e.Message); }
            finally
            {
                CloseAll();
            }

           

        }

        private void startRun(string resultPath)
        {
      
          
            foreach (TestStep step in this.StepList)
            {
                try
                {

                    step.Excuo();
                    if (!step.ResultStatic.Equals("1"))
                    {
                        th.snapshot("fail.jpg");
                        break;
                    }
                }
                catch (Exception e)
                {
                    step.ResultStatic = "3";
                    step.ResultMsg = e.Message;
                    th.snapshot(step, "fail.jpg");
                    break;

                }
                finally
                {

                    try
                    {
                        step.inputElement();
                        base.outputResultXml(resultPath);
                        
                    }
                    catch 
                    {

                    }
                }
            }
            
        }

        public override void CloseAll()
        {
            //要不干脆 kill
            try
            {

                th.ch.Quit();

            }
            catch { }
        }
       
    }
}
