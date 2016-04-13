using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseHelper;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
namespace robotiumHelper
{
    public class robotiumTestCase : baseTestCase
    {


        public string device { get; set; }

        public string RunApk { get; set; }

        public string package { get; set; }

        public bool? isClear { get; set; }

        private Process DebugP;

        public override void run(string resultPath)
        {
            string casePath = base.outputCaseXml(resultPath);//保存案例文件
            string apkPath = "RunApk\\" + RunApk;
            this.resultXml = null;
            using (Process p = new Process())
            {
                string batName = "RobotiumRunScript.bat";
                if (isClear == true)
                    batName = "RobotiumRunScript2.bat";
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.Arguments = string.Format(@"/c {0}\{6} {1} {2} {3} {4} {5}",
                    System.Environment.CurrentDirectory, casePath, resultPath, device, apkPath, package, batName);
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

        }


        public override void Debug(string resultPath)
        {
            string casePath = base.outputCaseXml(resultPath);//保存案例文件
            string apkPath = "RunApk\\" + RunApk;
            this.resultXml = null;

            DebugPInit();

            DebugP = new Process();

            string batName = "RobotiumRunScript4debug.bat";
            if (isClear == true)
                batName = "RobotiumRunScript24debug.bat";

            DebugP.StartInfo.FileName = "cmd.exe";
            DebugP.StartInfo.UseShellExecute = false;
            DebugP.StartInfo.CreateNoWindow = false;
            DebugP.StartInfo.Arguments = string.Format(@"/c {0}\{6} {1} {2} {3} {4} {5}",
                System.Environment.CurrentDirectory, casePath, resultPath, device, apkPath, package, batName);
            DebugP.Start();
            //DebugP.WaitForExit();

            //案例结果保存路径

        }

        /// <summary>
        /// 初始化DEBUG进程
        /// </summary>
        private void DebugPInit()
        {
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

        }

        public override void CloseAll()
        {
            DebugPInit();
        }


    }
}
