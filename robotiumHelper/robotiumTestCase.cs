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

        public bool isClear { get; set; }


        public bool install { get; set; }




        public override void run(string resultPath)
        {
         
            string casePath = base.outputCaseXml(resultPath);//保存案例文件
            string apkPath = "RunApk\\" + RunApk;
            this.resultXml = null;


            Process runProcess = new Process();
            string batName = "RobotiumRunScript.bat";


            runProcess.StartInfo.FileName = System.Environment.CurrentDirectory + "/" + batName;
            runProcess.StartInfo.UseShellExecute = false;
            runProcess.StartInfo.LoadUserProfile = true;
            runProcess.StartInfo.CreateNoWindow = true;
            runProcess.StartInfo.RedirectStandardOutput = true;

            runProcess.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4}",
                casePath, resultPath, device, install ? apkPath : "no", isClear ? package : "no");
            runProcess.Start();
            runProcess.WaitForExit();

            var result = runProcess.StandardOutput.ReadToEnd();
            Console.WriteLine(result);

            using (FileStream fs = new FileStream(resultPath+"log.txt", FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(result);
                sw.Close();

            }
            

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
           
            
        }


    }
}
