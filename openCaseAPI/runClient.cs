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
        public runClient()
        {

        }

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
            if (reposer == null || reposer=="null")
            {
                return null;
            }
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

        public bool caseResult(caseResult_req req)
        {
            Uri apiUri = new Uri(webAddress, "api/runClient/caseResult");
            string body = JsonConvert.SerializeObject(req);
            var reader = Postself(apiUri, body);
            if (reader == null)
                return false;
            else
                return true;

        }

        public bool SceneInstallResult(SceneInstallResult_req req)
        {


            Uri apiUri = new Uri(webAddress, "api/runClient/SceneInstallResult");
            string body = JsonConvert.SerializeObject(req);
            var reader = Postself(apiUri, body);
            if (reader == null)
                return false;
            else
                return true;
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
             
                onError(e);
                return false;
            }

            return true;
            
        }

        private string Postself(Uri apiUri, string body)
        {
            for (int i = 0; i < 3; i++)
            {

                try
                {

                    //创建连接
                    HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(apiUri);
                    mHttpRequest.Timeout = 180000;
                    mHttpRequest.Method = "POST";
                    mHttpRequest.Accept = "application/json, text/json";
                    mHttpRequest.ContentType = "application/json";

                    //mHttpRequest.AddRange(100, 10000);//字节范围
                    mHttpRequest.UseDefaultCredentials = true;

                    StreamWriter swMessages = new StreamWriter(mHttpRequest.GetRequestStream());
                    swMessages.Write(body);
                    swMessages.Close();

                   
                    HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));

                

                    return reader.ReadToEnd();


                }
                catch (Exception e)
                {
                    onError(e);
                }

            }
            Exception apierror = new Exception(apiUri.AbsoluteUri + " 报错");
            onError(apierror);
            return null;



        }


        private string Getself(Uri apiUri)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(apiUri);
                    mHttpRequest.Timeout = 180000;
                    mHttpRequest.Method = "GET";

                    mHttpRequest.Accept = "application/json, text/json";
                    mHttpRequest.ContentType = "application/json";

                    mHttpRequest.UseDefaultCredentials = true;

                    HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));

                    
                    return reader.ReadToEnd();
                }
                catch (Exception e)
                {
                    onError(e);
                }
            }
            Exception apierror = new Exception(apiUri.AbsoluteUri+" 报错");
            onError(apierror);
            return null;
        }

    }
}
