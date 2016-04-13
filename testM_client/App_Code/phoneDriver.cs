
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
namespace testM_client
{
    public class phoneDriver
    {

      //base("Data Source=.;Initial Catalog=QCTEST;Integrated Security=True", mappingSource)
        //base(@"Data Source=10.196.37.97\MSSQLSERVER1;Initial Catalog=QCTEST;User ID=sa;Password=1qaz@WSX", mappingSource)
        //base(@"Data Source=10.191.88.187\MSSQLSERVER1;Initial Catalog=QCTEST;User ID=sa;Password=Cpic1234", mappingSource)

        delegate void SceneDelegate();
        
        public string mark
        {
            get {
                return model + "(" + device + ")";
            }
        }

        /// <summary>
        /// 端口(chromeDriver用)
        /// </summary>
        public int port
        {
            get;
            set;
        }

        /// <summary>
        /// 唯一标示
        /// </summary>
        public string device { set; get; }

        /// <summary>
        /// 机型
        /// </summary>
        public string model { set; get; }

       

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
            //this.caseHelper = null;

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
            //robotiumHelper.robotiumTestCase rt = new robotiumHelper.robotiumTestCase();
            //rt.device = device;
            //this.caseHelper = rt;

            this.device = device;
            

        }

        

        /// <summary>
        /// Debug执行
        /// </summary>
        /// <returns></returns>
        public void Debug()
        {
            
            //DEBUG 目录
            string rPath = System.Environment.CurrentDirectory + "\\runTemp\\" + this.device + "\\";
            this.debugPath = rPath;


            RunInit(rPath);
           

            caseHelper.Debug(rPath);

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
               
                XElement step =  caseXml.Descendants("Step").FirstOrDefault();

                string name = "R_initStep";//随便赋值的
                if(step!=null)
                    name = step.Attribute("name").Value;
                

                if(name.Contains("R_"))//robotium 初始化
                {
                    if (this.caseHelper != robotiumH)//清除前一种helper的对象缓存,避免执行影响
                        caseHelper.CloseAll();

                    if (robotiumH == null)
                    {
                        robotiumH = new robotiumHelper.robotiumTestCase();
                    }
                        robotiumH.device = this.device;
                        robotiumH.caseXml = this.caseXml;
                        robotiumH.init();
                  
                    this.caseHelper = robotiumH;
                }
                else if (name.Contains("UI_"))//uiautomator 初始化
                {
                    if (this.caseHelper != UiautomatorH)//清除前一种helper的对象缓存,避免执行影响
                        caseHelper.CloseAll();
                    if (UiautomatorH == null)
                    {
                        UiautomatorH = new UiautomatorHelper.UiautomatorTestCase();
                    }
                    UiautomatorH.device = this.device;
                    UiautomatorH.caseXml = this.caseXml;

                    //DebugP
                    this.caseHelper = UiautomatorH;
                }
                else //chrome 初始化
                {
                    // if (this.caseHelper != chromeH)//清除前一种helper的对象缓存,避免执行影响
                    if (caseHelper != chromeH)
                        caseHelper.CloseAll();
                    if (chromeH == null)
                    {
                        chromeH = new chromeHelper.chromeTestCase(device, port);
                    }
                    //chromeH.device = this.device;
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
            if (this.status == phoneStatus.Idle)//设备未执行
            {
                SceneDelegate sh = new SceneDelegate(startRunScene);
                sh.BeginInvoke(new AsyncCallback(runCallBack),this);
            }

        }

        

        /// <summary>
        /// 停止工作
        /// </summary>
        public void rest()
        {
           
            
        }

        private void startRunScene()
        {
            //执行场景
            Thread.Sleep(10000);
        }

        private void runCallBack(IAsyncResult result)
        {
            this.status = phoneStatus.Idle;
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
