using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseHelper;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Threading;

namespace UiautomatorHelper
{
    public class UiautomatorTestCase : baseTestCase
    {
        public string device { get; set; }

        Thread debug;

        private List<caseRun> lcr;





        public override void run(string resultPath)
        {
            //base.outputCaseXml(resultPath);//保存案例文件
            RunInit();
            
          
            foreach (var c in lcr)
            {
                ExeCommand("adb -s " + this.device + " shell uiautomator dump");

                c.run(resultPath);

                if (!c.runOK)
                    break;
                
            }

            if (lcr.Count < 0) return;


            resultXml = new XElement(lcr[0].resultXml);


            for (int i = 1; i < lcr.Count; i++)
            {
                if (lcr[i].resultXml == null) break;
                var sis = lcr[i].resultXml.XPathSelectElements("//Step").ToList();
                foreach (var s in sis)
                {
                    resultXml.Add(s);
                }
            }

            string resultFilePath = base.outputResultXml(resultPath);//保存结果文件




        }

        public override void Debug(string resultPath)
        {
            
            

            debug = new Thread(() => run(resultPath));
            debug.Start();

        }

        /// <summary>
        /// 将案例拆分
        /// </summary>
        private void RunInit()
        {
            this.resultXml = null;

            List<XElement> caseList = new List<XElement>();
           
            var sis = caseXml.XPathSelectElements("//Step").ToList();

            XElement caseTemp = newCaseXElement("单步脚本");
            caseList.Add(caseTemp);
            if (sis.Count == 0) return;
            caseTemp.Add(sis[0]);
            for (int i = 1; i < sis.Count;i++ )
            {
                if (sis[i].Attribute("name").Value == "UI_InitStep")
                {
                    caseTemp = newCaseXElement("单步脚本");
                    caseList.Add(caseTemp);
                }
                caseTemp.Add(sis[i]);
            }
                
            lcr = new List<caseRun>();
            foreach (var e in caseList)
            {
                caseRun cr = new caseRun(device,e);
                lcr.Add(cr);
            }
           
        }

        private XElement newCaseXElement(string desc)
        {
            XElement caseT = new XElement("TestCase");
            caseT.SetAttributeValue("desc", desc);
            caseT.SetAttributeValue("customno", "0");
            return caseT;
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

        public override void CloseAll()
        {
            

            try
            {
                foreach (var s in lcr)
                {
                    s.CloseAll();
                }
                if (debug != null)
                {
                    debug.Abort();
                }

                killUI();
            }
            catch { }
            
            /*
            if (DebugP == null) return;

            try
            {
                if (!DebugP.HasExited)
                {
                    DebugP.CloseMainWindow();
                    DebugP.Kill();
                    DebugP.Close();
                }
            }
            catch { }//什么都不做
            */
        }

        private void killUI()
        {
            //杀掉UI进程
            string ss = ExeCommand("adb  -s " + this.device + " shell ps | findstr uiautomator");
            var rex = Regex.Matches(ss, "[0-9]+");
            if (rex.Count > 0)
            {
                ExeCommand("adb  -s " + this.device + " shell kill " + rex[0].Value);
            }
        }
    }
}
