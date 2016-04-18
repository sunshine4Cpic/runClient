using System.Linq;
using openCaseAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace testM_client
{

    public partial class Form1 : Form
    {

        

        //ddd
        string IP;//本机IP

        List<phoneDriver> pdl;//设备列表


        Uri webAddress;

        delegate void phoneSceneRun();

        public Form1()
        {

            webAddress = new Uri(ConfigurationManager.AppSettings["webAddress"].ToString());
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
            
            InitializeComponent();

            //demo
            LogHelper.WriteLog(typeof(Form1), "测试Log4Net已经写入");
           
            Console.WriteLine("获取手机设备信息.......");
            pdl = getPhoneList();
            this.listBox1.DataSource = pdl;
            this.listBox1.DisplayMember = "mark";
            this.listBox1.ValueMember = "device";


           
           
        }
        

        private void button1_Click(object sender, EventArgs e)
        {

            

            runClient rc = new runClient(webAddress);

            testHelper.rc = rc;

            var phone = this.listBox1.SelectedItem as phoneDriver;

       

            rc.DebugEvent += phone.Debug;

            rc.SceneEvent += this.runScene;

            foreach (var p in pdl)
            {
                rc.registerDevice(p);

            }

            this.listBox1.DataSource = null;
            this.listBox1.DataSource = pdl;
            this.listBox1.DisplayMember = "mark";
            this.listBox1.ValueMember = "device";

            (sender as Button).Enabled = false;

            rc.startService();

           
        }

        private void runScene(Object sender)
        {
            foreach (phoneDriver pd in pdl)
            {
                if (pd.status == phoneStatus.Idle)//设备是否空闲
                {
                    phoneSceneRun psr = new phoneSceneRun(pd.runScene);
                    psr.BeginInvoke(new AsyncCallback(runCallBack), pd);
                }

            }
            
        }

        private void runCallBack(IAsyncResult result)
        {
            (result.AsyncState as phoneDriver).status = phoneStatus.Idle;
            //testRun sh = (testRun)((System.Runtime.Remoting.Messaging.AsyncResult)result).AsyncDelegate;

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

