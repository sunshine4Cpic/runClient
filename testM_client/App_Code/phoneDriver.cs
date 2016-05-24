
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
    public class phoneDriver : phoneAbs
    {

      

        /// <summary>
        /// 端口(chromeDriver用)
        /// </summary>
        public int port { get; private set; }
        

       

        public ItestCase caseHelper;

        public string listBoxText { get{
            return string.Format("{0}({1})", mark, device);
        }}
      

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
        public override void Debug(Object sender, runClient.DebugEventArgs e)
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
                        logHelper.error(e);
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


                if (name.Contains("R_"))//robotium 初始化
                {

                    var help = new robotiumHelper.robotiumTestCase();
                    caseHelper = help;

                    help.device = this.device;
                    help.caseXml = this.caseXml;
                    help.init();
                }
                else if (name.Contains("UI_"))//uiautomator 初始化
                {
                    var help = new UiautomatorHelper.UiautomatorTestCase();
                    caseHelper = help;

                    help.device = this.device;
                    help.caseXml = this.caseXml;
                }
                else //chrome 初始化
                {
                    var help = new chromeHelper.chromeTestCase(device, port);
                    caseHelper = help;

                    help.caseXml = this.caseXml;
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
            while (true)
            {
                var Scene = testHelper.rc.GetRunScene(this.device);
                if (Scene == null) break;

                if (!string.IsNullOrEmpty(Scene.installApk) && string.IsNullOrEmpty(Scene.installResult))//安装任务未完成,先进行安装
                {

                    string filePath = System.Environment.CurrentDirectory + "\\apkInstall\\" + this.device + "\\";

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    if (testHelper.rc.downApk(Scene.installApk, filePath))
                    {

                        
                        //***************此处上传安装结果******************//
                        SceneInstallResult_req req = new SceneInstallResult_req();

                        req.installResult = this.install(filePath);
                        req.ID = Scene.id;
                      
                       
                        if(!testHelper.rc.SceneInstallResult(req))
                            logHelper.error("上传安装结果失败 场景ID:" + Scene.id);

                    }
                    else
                    {
                        logHelper.error("下载apk失败 场景ID:" + Scene.id);

                    }
                }

                foreach (var rcm in Scene.caseList)
                {
                    if (rcm.state != null && rcm.state > 0)
                    {
                        continue;
                    }

                    logHelper.info(this.listBoxText + "开始执行执行案例 id:" + rcm.id);

                    if(!runCase(rcm.id))
                    {
                        logHelper.error("批量执行案例失败 id:" + rcm.id);
                        continue;//报错继续下一条;
                    }
                }
            }

        }


        private bool runCase(int ID)
        {
            caseResult_req req = new caseResult_req();
            req.ID = ID;

            string runPath = System.Environment.CurrentDirectory + "/run/";


            XElement caseXml = testHelper.rc.GetSceneCase(ID);
            if (caseXml == null) return false;


            string runCasePath = runPath + ID + "/";
            
            this.caseXml = caseXml;
            this.resultXml = null;//置空

            req.startDate = DateTime.Now;
            try
            {
                logHelper.info(string.Format("{0}执行测试,ID:{1}", device, ID));

                this.run(runCasePath);

            }
            catch (Exception e )
            {
                logHelper.error(e);
            }

            req.endDate = DateTime.Now;


            req.state = 1;
            if (this.resultXml == null)
                this.resultXml = this.caseXml;
            req.resultXML = this.resultXml.ToString();
            req.resultPath = "http://" + IP + "/" + ID + "/";

            
            if(testHelper.rc.caseResult(req))
                return true;
            else
                return false;
            
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
