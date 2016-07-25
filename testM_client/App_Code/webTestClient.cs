using BaseHelper;
using chromeWebHelper;
using openCaseAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace testM_client
{
       

    public class webTestClient : webClientAbs
    {
        /// <summary>
        /// 端口(chromeDriver用)
        /// </summary>
        public int port { get; private set; }
        public string debugPath { set; get; }
        public XElement caseXml { get; set; }

        //结果
        public XElement resultXml { get; set; }

        public ItestCase webHelper { get; set; }

        public webTestClient()
        {
            this.port = testHelper.FindFreePort();
            webHelper = new chromeWebCase(this.port);
        }

        public override void Debug(Object sender, runClient.DebugEventArgs e)
        {

          
            //DEBUG 目录
            string rPath = System.Environment.CurrentDirectory + "\\runTemp\\webClient_" + port + "\\";
            this.debugPath = rPath;

            webHelper.caseXml = this.caseXml = e.caseXml;


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
                    catch (Exception ex)
                    {
                        logHelper.error(ex);
                    }
                }
            }

            webHelper.run(rPath);
        }
    }
}
