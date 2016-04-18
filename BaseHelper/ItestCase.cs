using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BaseHelper
{
    public interface ItestCase
    {
        /// <summary>
        /// 案例xml
        /// </summary>
        XElement caseXml { get; set; }

        /// <summary>
        /// 结果xml
        /// </summary>
        XElement resultXml { get; set; }


        /// <summary>
        /// 执行案例
        /// </summary>
        /// <param name="resultPath"></param>
        void run(string resultPath);



        /// <summary>
        /// 释放资源
        /// </summary>
        void CloseAll();
        
    }
}
