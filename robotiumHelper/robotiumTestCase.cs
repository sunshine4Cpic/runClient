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

        private Process runProcess;

        public override void run(string resultPath)
        {
            string casePath = base.outputCaseXml(resultPath);//保存案例文件
            string apkPath = "RunApk\\" + RunApk;
            this.resultXml = null;


            runProcess = new Process();
            string batName = "RobotiumRunScript.bat";
            if (isClear == true)
                batName = "RobotiumRunScript2.bat";


            runProcess.StartInfo.FileName = System.Environment.CurrentDirectory + "/" + batName;
            runProcess.StartInfo.UseShellExecute = false;
            runProcess.StartInfo.CreateNoWindow = true;

            runProcess.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4}",
                casePath, resultPath, device, apkPath, package);
            runProcess.Start();
            runProcess.WaitForExit();
            runProcess.Close();


            //案例结果保存路径
            string resultFilePath = base.outputResultXml(resultPath);

            if (File.Exists(resultFilePath))
            {
                this.resultXml = XElement.Load(resultFilePath);
            }

        }




        public override void CloseAll()
        {
            if(runProcess!=null && !runProcess.HasExited)
            {
                try
                {
                    runProcess.Kill();
                    runProcess.Close();
                }
                catch (Exception e)
                {
                    
                    
                }
            }
        }


    }
}
