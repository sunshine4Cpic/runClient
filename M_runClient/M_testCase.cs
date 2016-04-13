using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace M_runClient
{
    public class M_testCase
    {

        /// <summary>
        /// 案例名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 唯一ID,每个执行案例都应该有唯一的ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 案例获取路径,执行机会根据路径获得案例文件
        /// </summary>
        public string caseURL { get; set; }

        /// <summary>
        /// 案例XML,如果没有指定caseURL 可以采用直接发送XML文档过去
        /// 这种调用会对执行机带宽和内存造成影响,所以还是推荐使用caseURL方式
        /// </summary>
        public XElement caseXML { get; set; }

        /// <summary>
        /// 结果回调地址,因为执行是非实时的所以需要指定回调地址将结果文件返回
        /// 如果为空则保存结果在本地
        /// </summary>
        public string CallbackURL { get; set; }

       

    }
}
