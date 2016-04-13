using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace M_runClient
{
    public class M_RunScene
    {
        public M_RunScene()
        {
            Port = 8500;
        }
        List<M_testCase> testCaseList { get; set; }

        public string ClientIP { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// 手机名
        /// </summary>
        public string device { get; set; }

        private string ClientURL
        {
            get
            {
                return string.Format("http://{0}:{1}/testM/?runType=Scene", ClientIP, Port);
            }
        }


        

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public bool startRun()
        {
            try
            {
                //创建连接
                HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(ClientURL);
                //超时间毫秒为单位
                mHttpRequest.Timeout = 60 * 1000;
                //发送请求的方式
                mHttpRequest.Method = "POST";
                //发送的协议
                mHttpRequest.Accept = "HTTP";
                //字节范围
                mHttpRequest.AddRange(100, 10000);
                //是否和请求一起发送
                mHttpRequest.UseDefaultCredentials = true;
                //写数据信息的流对象
                //C:\Users\Admin\Desktop\test2.xml
                StreamWriter swMessages = new StreamWriter(mHttpRequest.GetRequestStream());
                //写入的流以XMl格式写入
                swMessages.Write(PostBody());
                //关闭写入流
                swMessages.Close();
                //创建一个响应对象
                HttpWebResponse mHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();
                if (mHttpResponse.StatusDescription == "OK")
                {

                }
                mHttpResponse.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }

        /// <summary>
        /// 请求body
        /// </summary>
        /// <returns></returns>
        private string PostBody()
        {
            XElement Scene = new XElement("Scene");
            foreach(var tc in testCaseList)
            {
                XElement step = new XElement("Step");
                step.SetAttributeValue("name", tc.name);
                step.SetAttributeValue("id", tc.id);
                step.SetAttributeValue("caseURL", tc.caseURL);
                //step.SetAttributeValue("CallbackURL", CallbackURL);
                Scene.Add(step);
            }

            return Scene.ToString();
        }


    }

}
