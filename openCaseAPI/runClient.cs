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
    public class runClient
    {

        public static Uri webAddress { get; set; }
        AutoRunSceneModel RunModel;
        List<runCaseSimpleModel> caselist;
        registerDevice_req Req;
        caseResult_req caseresult;
        public static application_res res;

        public string registerDevice(string req, string ServerURL)
        {
            try
            {
                Uri uriAddress = new Uri(webAddress, "registerDevice");
                string json = JsonConvert.SerializeObject(req);
                //创建连接
                HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(ServerURL + "api/runClient/registerDevice");
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
                swMessages.Write(json);
                //关闭写入流
                swMessages.Close();


                //mHttpRequest.Headers.Add("headName", .web.HttpUtility.UrlEncode.UrlEncode("value"));

                //创建一个响应对象
                HttpWebResponse mHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();

                if (mHttpResponse.StatusCode == HttpStatusCode.OK)
                {

                    HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                    {
                        string jj = reader.ReadToEnd().ToString();

                        Console.WriteLine(reader.ReadToEnd());
                        Req.mark = jj;
                    }

                }
                return Req.mark;
            }
            catch (Exception)
            {
                string ll = "设备更新失败";
                return ll;
            }
        }
        public AutoRunSceneModel GetRunScene(string device, string ServerURL)
        {
            try
            {
                string json = JsonConvert.SerializeObject(device);
                //创建连接
                HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(ServerURL + "api/runClient/AutoRunScene?device=" + device);
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
                HttpWebResponse mHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();

                if (mHttpResponse.StatusCode == HttpStatusCode.OK)
                {

                    HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                    {

                        //Console.WriteLine(reader.ReadToEnd());
                        string jj = reader.ReadToEnd().ToString();

                        RunModel = JsonConvert.DeserializeObject<AutoRunSceneModel>(jj);


                        Console.WriteLine("deviceId:" + RunModel.id);
                        Console.WriteLine("name:" + RunModel.name);
                        Console.WriteLine("installaApk:" + RunModel.installApk);

                        Console.WriteLine("caseList:" + RunModel.caseList);

                        caselist = RunModel.caseList;
                    }
                }

                return RunModel;

            }

            catch (Exception)
            {


                return null;

            }
        }

        public XElement GetSceneCase(int id, string ServerURL)
        {
            try
            {



                string json = JsonConvert.SerializeObject(id);
                //创建连接
                HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(ServerURL + "api/runClient/RunScript/" + id);
                //超时间毫秒为单位
                mHttpRequest.Timeout = 180000;
                //发送请求的方式
                mHttpRequest.Method = "GET";
                //发送的协议

                mHttpRequest.Accept = "application/xml, text/xml";
                // mHttpRequest.ContentType = "application/x-www-form-urlencoded";表格的形式
                mHttpRequest.ContentType = "application/xml";

                //字节范围
                mHttpRequest.AddRange(100, 10000);
                //是否和请求一起发送
                mHttpRequest.UseDefaultCredentials = true;
                //创建一个响应对象
                HttpWebResponse mHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();

                HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                {

                    Console.WriteLine(reader.ReadToEnd());
                    // string jj = reader.ReadToEnd().ToString();


                    //XElement testxml = XElement.Parse(jj);
                    //return testxml;
                }
                string jj = reader.ReadToEnd().ToString();
                XElement testxml = XElement.Parse(jj);
                return testxml;
            }

            catch (Exception)
            {
                return null;

            }
        }

        public void caseResult(string req, string ServerURL, int ID)
        {

            try
            {
                string Json = JsonConvert.SerializeObject(req);
                HttpWebRequest nHttpRequest = (HttpWebRequest)HttpWebRequest.Create(ServerURL + "api/runClient/caseResult/" + ID);
                //超时间毫秒为单位
                nHttpRequest.Timeout = 180000;
                //发送请求的方式
                nHttpRequest.Method = "POST";
                //发送的协议

                nHttpRequest.Accept = "application/json, text/json";
                // mHttpRequest.ContentType = "application/x-www-form-urlencoded";
                nHttpRequest.ContentType = "application/json";

                //字节范围
                nHttpRequest.AddRange(100, 10000);
                //是否和请求一起发送
                nHttpRequest.UseDefaultCredentials = true;
                //写数据信息的流对象

                StreamWriter wMessages = new StreamWriter(nHttpRequest.GetRequestStream());
                //写入的流以XMl格式写入
                wMessages.Write(Json);
                //关闭写入流
                wMessages.Close();
                //创建一个响应对象
                HttpWebResponse nHttpResponse = (HttpWebResponse)nHttpRequest.GetResponse();

                if (nHttpResponse.StatusCode == HttpStatusCode.OK)
                {

                    HttpWebResponse Response = (HttpWebResponse)nHttpRequest.GetResponse();
                    StreamReader Reader = new StreamReader(Response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                    {

                        Console.WriteLine(Reader.ReadToEnd());
                    }
                }
            }
            catch (Exception)
            {

            }



        }
        public static application_res GetApk(XElement xe, string ServerURL)
        {
            try
            {
                var appID = xe.XPathSelectElement("//ParamBinding[@name='applicationID']");
                string json = JsonConvert.SerializeObject(appID);
                //创建连接
                HttpWebRequest mHttpRequest = (HttpWebRequest)HttpWebRequest.Create(ServerURL + "api/runClient/application/" + appID);
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
                HttpWebResponse mHttpResponse = (HttpWebResponse)mHttpRequest.GetResponse();

                if (mHttpResponse.StatusCode == HttpStatusCode.OK)
                {

                    HttpWebResponse response = (HttpWebResponse)mHttpRequest.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
                    {

                        //Console.WriteLine(reader.ReadToEnd());
                        string jj = reader.ReadToEnd().ToString();
                        res = new application_res();
                        application_res appl = JsonConvert.DeserializeObject<application_res>(jj);



                        Console.WriteLine("appId:" + res.id);
                        Console.WriteLine("appname:" + res.name);
                        Console.WriteLine("androidPackeg:" + res.androidPackeg);
                        Console.WriteLine("mainActivity:" + res.mainActivity);
                        Console.WriteLine("iosPackeg:" + res.iosPackage);
                        Console.WriteLine("clearCache:" + res.clearCache);
                        res.id = appl.id;
                        res.name = appl.name;
                        res.androidPackeg = appl.androidPackeg;
                        res.mainActivity = appl.mainActivity;
                        res.iosPackage = appl.iosPackage;
                        res.clearCache = appl.clearCache;

                    }
                }
                return res;
            }
            catch (Exception)
            {

                return null;
            }



        }

    }
}
