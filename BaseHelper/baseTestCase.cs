using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BaseHelper
{
    public class baseTestCase : ItestCase
    {
        //原案例
        public XElement caseXml { get; set; }

        //结果
        public XElement resultXml { get; set; }

        
        public virtual void run(string resultPath)
        {
           
        }


     

        /// <summary>
        /// 输出结果到文件
        /// </summary>
        /// <param name="resultPath"></param>
        public string outputResultXml(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            string filePath = Path + "test_result.xml";
            if(resultXml!=null)//如果有结果保存成文件
            {
                resultXml.Save(filePath);
            }

            return filePath;
        }

        /// <summary>
        /// 输出案例到文件
        /// </summary>
        /// <param name="resultPath"></param>
        public string outputCaseXml(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            string casePath = Path + "test.xml";
            if (caseXml != null)
            {
                caseXml.Save(casePath);
                
            }

            return casePath;
        }

        public virtual void CloseAll()
        {

        }
    }
}
