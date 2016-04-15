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
      //123


        public void registerDevice(registerDeviceModel Req)
        {
            try
            {
                //请求路径
                Uri apiUri = new Uri(webAddress, "api/runClient/registerDevice");

                //传递body
                string body = JsonConvert.SerializeObject(Req);
                //返回流
                var reader = Postself(apiUri,body);
                //流转字符串
                string mark_repo = reader.ReadToEnd();


                Req.mark = mark_repo;
                
            }
            catch (Exception)
            {
                throw new Exception("error");
            }
        }

        public  AutoRunSceneModel GetRunScene(string device)
        {
            try
            {
                Uri apiUri = new Uri(webAddress, "api/runClient/AutoRunScene?device=" + device);


                var reader = Getself(apiUri);
                string reposer = reader.ReadToEnd().ToString();
                AutoRunSceneModel RunModel = JsonConvert.DeserializeObject<AutoRunSceneModel>(reposer);
              
                return RunModel;
            }

            catch (Exception)
            {


                return null;

            }
        }

        public  XElement GetSceneCase(int id)
        {
            try
            {
                Uri apiUri = new Uri(webAddress, "api/runClient/RunScript/" + id);
                var reader = Getself(apiUri);
                string reposer = reader.ReadToEnd();
                XElement Testxml = XElement.Parse(reposer);
                //return testxml;
                return Testxml;
            }

            catch (Exception)
            {
                return null;

            }
        }

        public void caseResult(caseResult_req caseresult, int ID)
        {

            try
            {
                Uri apiUri = new Uri(webAddress, "api/runClient/caseResult/" + ID);
                string body = JsonConvert.SerializeObject(caseresult);
                var reader = Postself(apiUri,body);


                Console.WriteLine(reader.ReadToEnd());
            }
            catch (Exception)
            {

            }



        }

        public application_res GetApk(string appID)
        {
            try
            {
               
                Uri apiUri = new Uri(webAddress, "api/runClient/application/" + appID);
                var reader = Getself(apiUri);

                application_res res = JsonConvert.DeserializeObject<application_res>(reader.ReadToEnd());
                
                return res;
           

            }
            catch (Exception)
            {

                return null;
            }



        }

        private StreamReader Postself(  Uri apiUri,string body)
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
              
                return reader;

            }


        private StreamReader Getself(Uri apiUri)

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
          

          return reader;
      }

    }
}
