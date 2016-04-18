
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using BaseHelper;
using System.Xml.XPath;
using openCaseAPI;
using System.Configuration;
namespace testM_client
{
    public class phoneDriver : registerDeviceModel
    {

      

       
        

        /// <summary>
        /// 端口(chromeDriver用)
        /// </summary>
        public int port { get; private set; }
        

       

        public ItestCase caseHelper;


        private robotiumHelper.robotiumTestCase robotiumH;

        private UiautomatorHelper.UiautomatorTestCase UiautomatorH;

        private chromeHelper.chromeTestCase chromeH;

        public phoneStatus status { set; get; }

        public string debugPath { set; get; }


        public XElement caseXml { get; set; }

        //结果
        public XElement resultXml { get; set; }

        
        public phoneDriver()
        {
            this.port = testHelper.FindFreePort();
        }



        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="device"></param>
        /// <param name="model"></param>
        public phoneDriver(string device)
            : this()
        {
          
            this.mark = device;
            this.device = device;
            

        }

        

        /// <summary>
        /// Debug执行
        /// </summary>
        /// <returns></returns>
        public void Debug(Object sender, runClient.DebugEventArgs e)
        {
            if (this.status == phoneStatus.Busy) return;
            //DEBUG 目录
            string rPath = System.Environment.CurrentDirectory + "\\runTemp\\" + this.device + "\\";
            this.debugPath = rPath;

            this.caseXml = e.caseXml;


            RunInit(rPath);
           

            caseHelper.run(rPath);

        }



        /// <summary>
        /// 执行测试
        /// </summary>
        /// <param name="rPath"></param>
        /// <returns></returns>
        public virtual bool run(string rPath)
        {
            

            RunInit(rPath);
           
            caseHelper.run(rPath);

            this.resultXml = caseHelper.resultXml;
            

            return true;
        }

       

        private void RunInit(string rPath)
        {
            //创建结果目录
            if (!Directory.Exists(rPath))
            {
                Directory.CreateDirectory(rPath);
            }
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(rPath);
                FileInfo[] files = dirInfo.GetFiles();

                //删除不了会报错
                foreach (FileInfo file in files)
                {
                    try { file.Delete(); }
                    catch (Exception e)
                    {
                        logHelper.error("删除文件失败," + e.Message);
                    }
                }
            }


            if (caseHelper is appiumHelper.appiumTestCase)//如果是appium 初始化
            {
                caseHelper.caseXml = this.caseXml;
                (caseHelper as appiumHelper.appiumTestCase).init();
            }
            else
            {
                if (caseHelper != null) caseHelper.CloseAll();
             
                XElement step =  caseXml.Descendants("Step").FirstOrDefault();

                string name = "R_initStep";//随便赋值的
                if(step!=null)
                    name = step.Attribute("name").Value;
                

                if(name.Contains("R_"))//robotium 初始化
                {
                    
                   
                        caseHelper = new robotiumHelper.robotiumTestCase();
                   
                        robotiumH.device = this.device;
                        robotiumH.caseXml = this.caseXml;
                        robotiumH.init();
                }
                else if (name.Contains("UI_"))//uiautomator 初始化
                {

                    caseHelper = new UiautomatorHelper.UiautomatorTestCase();

                    UiautomatorH.device = this.device;
                    UiautomatorH.caseXml = this.caseXml;
                }
                else //chrome 初始化
                {

                    caseHelper = new chromeHelper.chromeTestCase(device, port);

                    chromeH.caseXml = this.caseXml;

                    this.caseHelper = chromeH;
                }
                
            }
        }

        public string install(string apk)
        {
            
            return testHelper.ExeCommand("adb -s " + this.device + " install -r " + apk);
        }



     
       /// <summary>
       /// 启动执行
       /// </summary>
        public void runScene()
        {

            var Scene = testHelper.rc.GetRunScene(this.device);

            if (string.IsNullOrEmpty(Scene.installApk) && string.IsNullOrEmpty(Scene.installResult))//安装任务未完成,先进行安装
            {

                string filePath = System.Environment.CurrentDirectory + "\\apkInstall\\" + this.device + "\\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                testHelper.rc.downApk(Scene.installApk, filePath);
                Scene.installResult = this.install(filePath);
                //***************此处上传安装结果******************//
                throw new NotImplementedException();
            }

            foreach (var rcm in Scene.caseList)
            {
                runCase(rcm);
            }

            


        }


        private void runCase(runCaseSimpleModel rcm)
        {


        }

        


    }


    public enum phoneStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle =0,

        /// <summary>
        /// 忙
        /// </summary>
        Busy =1
        

    }
}
