
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using openCaseAPI;

namespace testM_client
{

    public partial class Form1 : Form
    {

        


        string IP;//本机IP

        List<phoneDriver> pdl;//设备列表


        public Form1()
        {
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
           
            foreach (IPAddress add in addressList)
            {
                if (add.AddressFamily.ToString() == "InterNetwork")
                {
                    IP = add.ToString();
                    break;
                }
            }

            Console.OutputEncoding = Encoding.GetEncoding(936);
            
            
            Console.ForegroundColor = ConsoleColor.White;
            InitializeComponent();
           
            //screenth = null;
            Console.WriteLine("获取手机设备信息.......");
            pdl = getPhoneList();
            this.listBox1.DataSource = pdl;
            this.listBox1.DisplayMember = "mark";
            this.listBox1.ValueMember = "device";

           
           
        }
        

        private void button1_Click(object sender, EventArgs e)
        {

            Uri webAddress = new Uri("http://localhost:20349");

            runClient rc = new runClient(webAddress);

            testHelper.rc = rc;

            var phone = this.listBox1.SelectedItem as phoneDriver;

            rc.DebugEvent += phone.Debug;

            foreach (var p in pdl)
            {
                rc.registerDevice(p);
            }

            this.listBox1.DataSource = pdl;
            this.listBox1.DisplayMember = "mark";
            this.listBox1.ValueMember = "device";

            (sender as Button).Enabled = false;

            rc.startService();

           


            
           
          
           
        }

        

        private List<phoneDriver> getPhoneList()
        {
            List<phoneDriver> pdl = new List<phoneDriver>();

            string devices = testHelper.ExeCommand("adb devices");

            

            foreach (Match mch in Regex.Matches(devices, "\\r\\n.*\\tdevice"))
            {
                String x = mch.Value;
                x = x.Substring(0, x.LastIndexOf("device"));
                phoneDriver pd = new phoneDriver(x.Trim());
                pd.IP = this.IP;
                
                pdl.Add(pd);
                //this.listBox1.Items.Add(x.Trim());
                //dList.Add(x.Trim());  
            }  

            foreach(phoneDriver pd in pdl)
            {
                string cmd = string.Format("adb -s {0} shell getprop ro.product.model", pd.device);
                pd.model = testHelper.ExeCommand(cmd).Trim();
            }
            
            try
            {
                XElement config = XElement.Load("config.xml");
                foreach (XElement x in config.Elements("appium"))
                {
                    phoneDriver ad = new phoneDriver();
                    ad.model = x.Attribute("model").Value;
                    ad.device = x.Attribute("device").Value;
                    appiumHelper.appiumTestCase caseHelper = new appiumHelper.appiumTestCase();
                    caseHelper.url = x.Attribute("url").Value;
                    caseHelper.udid = ad.device;//udid
                    ad.caseHelper = caseHelper;
                    pdl.Add(ad);
                }
            }
            catch { }
            
            return pdl;
        }

        

       

     

        /// <summary>
        /// 不只是执行场景,其实是激活设备进行执行
        /// </summary>
        /// <param name="pd"></param>
        private void runScene(phoneDriver pd)
        {
        

             testRunDataDataContext trddc = new testRunDataDataContext();

            //属于这台手机并已经开始的场景
            var Scene = trddc.M_runScene.Where(t => t.M_deviceConfig.device == pd.device && t.M_testDemand.isRun == true);

            //有安装任务 或未执行案例>0
            var rs = from t in Scene
                     where (t.M_testDemand.apkName != null && t.installResult == null)
                     || t.M_runTestCase.Where(c => c.state == null).Count() > 0
                     select t;


            
            //遍历场景
            while (rs.Count() > 0)
            {
                var r = rs.OrderBy(T=>T.ID).First();//取得第一个场景 *************以后改成时间排序

                if(r.M_testDemand.apkName!=null && r.installResult==null)
                {

                    if (pd.caseHelper is appiumHelper.appiumTestCase)
                    {
                        //是appium 怎么安装?
                    }
                    else
                    {
                        Console.WriteLine(pd.device + "安装apk");

                        string apkPath = ConfigurationManager.AppSettings["apkPath"].ToString() 
                            + r.M_testDemand.apkName;

                        string filePath = System.Environment.CurrentDirectory + "\\apkInstall\\" + pd.device + "\\";
                        string rPath = filePath+ r.M_testDemand.apkName;
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        try
                        {
                            WebClient wClient = new WebClient();
                            wClient.DownloadFile(apkPath, rPath);
                            r.installResult = pd.install(rPath);
                        }
                        catch (Exception e)
                        {
                            r.installResult = "安装失败:" + e.Message;
                        }
                        
                        //*********安装代码
                        
                        trddc.SubmitChanges();
                    }
                }
                //执行场景
                runSceneCase(pd, r.ID);
                

            }

        }

        /// <summary>
        /// 手机执行某个场景
        /// </summary>
        /// <param name="pd">手机</param>
        /// <param name="ID">场景ID</param>
        private void runSceneCase(phoneDriver pd,int ID)
        {
            //if (pd.status == phoneStatus.Busy) return;
            pd.status = phoneStatus.Busy;
            string runPath = System.Environment.CurrentDirectory + "\\run\\";
         
            testRunDataDataContext trddc = new testRunDataDataContext();

            //原执行逻辑
            var ts = from t in trddc.M_runTestCase
                     where t.M_runScene.M_testDemand.isRun == true && t.M_runScene.M_testDemand.visable ==null && t.sceneID == ID && t.state == null
                     select t;

            while (ts.Count() > 0)
            {
                M_runTestCase t = ts.First();
                string runCasePath = runPath + t.ID + "\\";
                DateTime sd = DateTime.Now;
                pd.caseXml = t.testXML;
                pd.resultXml = null;//置空
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(pd.device + "执行测试,ID: {0}......", t.ID);
                    pd.run(runCasePath);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(pd.device + "执行测试失败,ID: {0}......", t.ID);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                t.endDate = DateTime.Now;
                
               

                t.startDate = sd;

                t.state = 1;

                t.resultXML = pd.resultXml;
                t.resultPath = "http://" + IP + "/" + t.ID + "/";
                try
                {

                    trddc.SubmitChanges();
                }
                catch
                {
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("案例"+t.ID+"更新失败!");
                    Console.ForegroundColor = ConsoleColor.White;
                    trddc.Refresh(RefreshMode.OverwriteCurrentValues,t);
                    trddc.SubmitChanges();
                }
            }
        }

        


      




        #region 关闭事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;  //不显示在系统任务栏

            //this.Visible = false;
            this.notifyIcon1.Visible = true;
        }

        private void quit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void open_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;  //显示在系统任务栏

            //this.Visible = false;
            this.notifyIcon1.Visible = false;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            open_Click(null,null);
        }

        #endregion 关闭事件




    }
}

