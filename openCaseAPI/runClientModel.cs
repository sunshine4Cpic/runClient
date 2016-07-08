using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Runtime.Serialization;
using System.Net;

namespace openCaseAPI
{

    [DataContract]
    public abstract class phoneAbs
    {
        [DataMember]
        public string device { set; get; }

        [DataMember]
        public string model { set; get; }

        [DataMember]
        public string IP { set; get; }

        [DataMember]
        public string mark { set; get; }

        public abstract void Debug(Object sender, runClient.DebugEventArgs e);
        
    }

    /// <summary>
    /// web自动化抽象类
    /// </summary>
    public abstract class webClientAbs
    {
        public string IP { set; get; }
        public string mark { set; get; }
        public abstract void Debug(Object sender, runClient.DebugEventArgs e);
    }


    public class application_res
    {
        public int id { set; get; }
        public string name { set; get; }
        public string androidPackeg { set; get; }
        public string mainActivity { set; get; }
        public string iosPackage { get; set; }
        public bool clearCache { set; get; }
        public string robotiumApk { get; set; }
    }

    public class caseResult_req
    {
        public int ID { get; set; }
        public string resultXML { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }
        public int state { set; get; }
        public string resultPath { set; get; }

    }
    public class SceneInstallResult_req
    {
        
        public int ID { get; set; }

      
        public string installResult { get; set; }
    }


    public class AutoRunSceneModel
    {

        public int id { get; set; }

        public string name { get; set; }

        /// <summary>
        /// 安装apk
        /// </summary>
        public string installApk { set; get; }

        /// <summary>
        /// 安装结果
        /// </summary>
        public string installResult { set; get; }

       

        public List<runCaseSimpleModel> caseList { get; set; }

    }

    public class runCaseSimpleModel
    {
        public int id { get; set; }

        public string name { get; set; }

        /// <summary>
        /// 1执行 2已处理
        /// </summary>
        public int? state { get; set; }
    }

}
