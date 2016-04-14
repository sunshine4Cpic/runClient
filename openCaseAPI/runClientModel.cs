using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace openCaseAPI
{
    public class registerDevice_req
    {
        public string device { set; get; }
        public string model { set; get; }
        public string IP { set; get; }
        public string mark { set; get; }

    }
    public class application_res
    {
        public int id { set; get; }
        public string name { set; get; }
        public string androidPackeg { set; get; }
        public string mainActivity { set; get; }
        public string iosPackage { get; set; }
        public bool clearCache { set; get; }
    }

    public class caseResult_req
    {
        public XmlElement resultXML { set; get; }
        public DateTime startDate { set; get; }
        public DateTime endDate { set; get; }
        public int state { set; get; }
        public string resultPath { set; get; }

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
