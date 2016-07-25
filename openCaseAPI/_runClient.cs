﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Threading;

namespace openCaseAPI
{
    public partial class runClient
    {

        public delegate void errorHandler(Exception e);


        public event errorHandler errorEvent; //声明eror事件  

        private void onError(Exception e)
        {
            if(errorEvent!=null)
            {
                errorEvent(e);
            }
        }
        
       
        public delegate void DebugEventHandler(Object sender, DebugEventArgs e);


        /// <summary>
        /// 注册debug事件(异步),debug调用开始后将立刻反馈给平台信息.
        /// </summary>
        public event DebugEventHandler DebugEvent; //声明DEBUG事件  


        //声明Scene委托  
        public delegate void SceneEventHandler(Object sender);

        /// <summary>
        /// 注册Scene事件(同步),Scene事件在开始后不会立刻反馈给平台.
        /// </summary>
        public event SceneEventHandler SceneEvent; //声明Scene事件  



        public class DebugEventArgs : EventArgs
        {
            public readonly XElement caseXml;//  

            
            public DebugEventArgs(XElement caseXml)
            {
                this.caseXml = caseXml;
            }
        }




      

        protected void OnDebug(DebugEventArgs e)
        {
            
            if (DebugEvent != null)
            { // 如果有对象注册  
                foreach (DebugEventHandler de in DebugEvent.GetInvocationList())
                {
                   //Console.WriteLine("DebugEvent begin...");
                    de.BeginInvoke(this, e, new AsyncCallback(DebugCallBack), de);
                }
            }

        }

        private void DebugCallBack(IAsyncResult result)
        {
            //调用结束后干些什么
            //Console.WriteLine("DebugEvent end...");
        }

        protected void OnScene()
        {
            
            if (SceneEvent != null)
            { // 如果有对象注册  
                SceneEvent(this);
            }

        }

        


        public void startService()
        {

            Thread httpThread = new Thread(new ThreadStart(ServiceListerner));
            httpThread.IsBackground = true;
            httpThread.Start();

        }


        private void ServiceListerner()
        {

            using (HttpListener listerner = new HttpListener())
            {
                listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                listerner.Prefixes.Add("http://+:8500/testM/");

                // listerner.Prefixes.Add("http://localhost/web/");
                listerner.Start();
                Console.WriteLine("ClientServer:8500 Start Successed.......");
                //listerner.BeginGetContext();//异步调用(并发处理,暂时不需要)
                while (true)
                {
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    HttpListenerContext ctx = listerner.GetContext();
                    ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
                    ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    string runType = ctx.Request.QueryString["runType"];


                    StreamReader reader = new StreamReader(ctx.Request.InputStream);
                    if (runType == "Debug")//没有就是调试模式
                    {
                        try
                        {
                            XElement xe = XElement.Parse(reader.ReadToEnd());

                            DebugEventArgs e = new DebugEventArgs(xe);
                            OnDebug(e); // 调试
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("调试模式错误.......err:" + e.Message);
                        }
                    }
                    else if (runType == "Scene")//场景处理
                    {
                        OnScene();
                    }
                    ctx.Response.Close();

                }
            }
        }
    }
}
