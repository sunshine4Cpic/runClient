using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace openCaseAPI
{
    public partial class runClient
    {
        //声明DEBUG委托  
        public delegate void DebugEventHandler(Object sender, DebugEventArgs e);

        public event DebugEventHandler DebugEvent; //声明DEBUG事件  


        //声明Scene委托  
        public delegate void SceneEventHandler(Object sender);

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
                    de.BeginInvoke(this, e, null, null);
                }
               
            }

           
            
          
        }

        protected void OnScene()
        {
            if (SceneEvent != null)
            { // 如果有对象注册  
                SceneEvent.BeginInvoke(this, null, null);
            }
        }


        public void startService()
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
                           XElement xe =  XElement.Parse(reader.ReadToEnd());

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
