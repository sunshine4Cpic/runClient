using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using BaseHelper;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace UiautomatorHelper
{

    public class caseRun : baseTestCase
    {
        public caseRun(string device,XElement xe)
        {
            this.device = device;
            this.caseXml = xe;
            runOK = false;
        }

        public string device { get; set; }




        public bool runOK { get; private set; }

        public override void run(string resultPath)
        {


            using (Process p = newProcess(resultPath))
            {
                p.Start();
                p.WaitForExit();
                p.Close();
            }
            //案例结果保存路径
            string resultFilePath = base.outputResultXml(resultPath);

            if (File.Exists(resultFilePath))
            {
                this.resultXml = XElement.Load(resultFilePath);
            }
            int rsc = this.resultXml.XPathSelectElements("//Step").Count();
            int rs1 = this.resultXml.XPathSelectElements("//Step[@ResultStatic='1']").Count();
            if(rsc==rs1)
            {
                runOK = true;
            }

        }



        /// <summary>
        /// 执行准备
        /// </summary>
        /// <param name="resultPath"></param>
        /// <returns></returns>
        private Process newProcess(string resultPath)
        {



            this.resultXml = null;
            string casePath = base.outputCaseXml(resultPath);//保存案例文件

            Process p = new Process();
            string batName = "UiautomatorRunScript.bat";
            string UiautomatorScrpit = string.Format(@"{0}\RunApk\{1}",
                System.Environment.CurrentDirectory, "CalculatorAutoTest.jar");
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = false;
            string runBat = string.Format(@"/c {0}\{1}",
                System.Environment.CurrentDirectory, batName);
            p.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4}",
               runBat, device, casePath, UiautomatorScrpit, resultPath);

            return p;

        }




        private string ExeCommand(string commandText)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.LoadUserProfile = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.Arguments = string.Format(@"/c {0}", commandText);

            //string strOutput = p.StandardOutput.ReadToEnd();


            p.Start();

            p.WaitForExit();
            string strOutput = p.StandardOutput.ReadToEnd();
            p.Close();

            return strOutput;
        }


    }
}
