using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;

namespace openCaseAPI
{
    public partial class runClient
    {

        public runClient(Uri webAddress)
        {
            this.webAddress = webAddress;
        }

        public Uri webAddress { get; set; }



        /// <summary>
        /// 注册执行设备
        /// </summary>
        /// <param name="Req"></param>
        public void registerDevice(phoneAbs Req)
        {

            //请求路径
            Uri apiUri = new Uri(webAddress, "api/runClient/registerDevice");

            //传递body
            string body = JsonConvert.SerializeObject(Req);
            //返回流
            var mark_repo = Postself(apiUri, body);
            
           


            Req.mark = mark_repo;


        }

        public AutoRunSceneModel GetRunScene(string device)
        {

            Uri apiUri = new Uri(webAddress, "api/runClient/AutoRunScene?device=" + device);


            var reposer = Getself(apiUri);
           
            AutoRunSceneModel RunModel = JsonConvert.DeserializeObject<AutoRunSceneModel>(reposer);

            return RunModel;

        }

        public XElement GetSceneCase(int id)
        {

            Uri apiUri = new Uri(webAddress, "api/runClient/RunScript/" + id);
            var reposer = Getself(apiUri);

            if (reposer == null || reposer == "null")
                return null;

            XElement Testxml = XElement.Parse(reposer);

            return Testxml;

        }

        public string caseResult(caseResult_req req)
        {


            Uri apiUri = new Uri(webAddress, "api/runClient/caseResult");
            string body = JsonConvert.SerializeObject(req);
            var reader = Postself(apiUri, body);

            return reader;


        }

        public string SceneInstallResult(SceneInstallResult_req req)
        {


            Uri apiUri = new Uri(webAddress, "api/runClient/SceneInstallResult");
            string body = JsonConvert.SerializeObject(req);
            var reader = Postself(apiUri, body);

            return reader;
        }

        public application_res GetApk(string appID)
        {


            Uri apiUri = new Uri(webAddress, "api/runClient/application/" + appID);
            var reader = Getself(apiUri);

            application_res res = JsonConvert.DeserializeObject<application_res>(reader);

            return res;
        }

        public bool downApk(string apkName, string fileName)
        {
            Uri apkUri = new Uri(webAddress, "apkInstall/" + apkName);
            try
            {
                WebClient wClient = new WebClient();
                wClient.DownloadFile(apkUri, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("下载apk失败");
                Console.WriteLine(e.StackTrace);
                return false;
            }

            return true;
            
        }

        private string Postself(Uri apiUri, string body)
        {
            try
            {
                //创建连接
                HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(apiUri);
                //超时间毫秒为单位
                mHttpRequest.Timeout = 180000;
                //发送请求的方式
                mHttpRequest.Method = "POST";
                //发送的协议

                mHttpRequest.Accept = "application/json, text/json";
                // mHttpRequest.ContentType = "application/x-www-form-urlencoded";
                mHttpRequest.ContentType = "application/json";

                //字节范围
                mHttpRequest.AddRange(100, 10000);
                //是否和请求一起发送
                mHttpRequest.UseDefaultCredentials = true;
                //写数据信息的流对象

                StreamWriter swMessages = new StreamWriter(mHttpRequest.GetRequestStream());
                //写入的流以XMl格式写入
                swMessages.Write(body);
                //关闭写入流
                swMessages.Close();


                //mHttpRequest.Headers.Add("headName", .web.HttpUtility.UrlEncode.UrlEncode("value"));

                //创建一个响应对象
                HttpWebResponse mHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();


                HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));

                return reader.ReadToEnd();
            }
            catch (Exception)
            {

                return null;
            }

        }


        private string Getself(Uri apiUri)
        {
            try
            {
                HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(apiUri);
                //超时间毫秒为单位
                mHttpRequest.Timeout = 180000;
                //发送请求的方式
                mHttpRequest.Method = "GET";
                //发送的协议

                mHttpRequest.Accept = "application/json, text/json";
                // mHttpRequest.ContentType = "application/x-www-form-urlencoded";表格的形式
                mHttpRequest.ContentType = "application/json";

                //字节范围
                mHttpRequest.AddRange(100, 10000);
                //是否和请求一起发送
                mHttpRequest.UseDefaultCredentials = true;
                //创建一个响应对象
                // HttpWebResponse mHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();
                HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));


                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

    }
}
