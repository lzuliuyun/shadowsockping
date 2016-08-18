using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WindowsFormsApplication1.Model
{
    class XMLOperate
    {
        private string xmlName;

        public string XmlName
        {
            get
            {
                return xmlName;
            }

            set
            {
                xmlName = value;
            }
        }

        /// <summary>  
        /// 返回XMl文件指定元素的指定属性值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <param name="xmlAttribute">指定属性</param>  
        /// <returns></returns>  
        public string getXmlValue(string xmlElement, string xmlAttribute)
        {
            XDocument xmlDoc = XDocument.Load(this.XmlName);
            var results = from c in xmlDoc.Descendants(xmlElement)
                          select c;
            string s = "";
            foreach (var result in results)
            {
                s = result.Attribute(xmlAttribute).Value.ToString();
            }
            return s;
        }

        /// <summary>  
        /// 返回XMl文件指定元素的指定属性值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <returns></returns>  
        public string getXmlValue(string xmlElement)
        {
            XDocument xmlDoc = XDocument.Load(this.XmlName);
            object obj = xmlDoc.Element(xmlElement);
            string value = xmlDoc.Element(xmlElement).Value.ToString();           
            return value;
        }

        /// <summary>  
        /// 返回XMl文件指定元素的指定属性值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <returns></returns>  
        public void setXmlValue(string xmlElement, string xmlValue)
        {
            XDocument xmlDoc = XDocument.Load(this.XmlName);
            xmlDoc.Element(xmlElement).SetValue(xmlValue);
            xmlDoc.Save(this.xmlName);
        }

        /// <summary>  
        /// 设置XMl文件指定元素的指定属性的值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <param name="xmlAttribute">指定属性</param>  
        /// <param name="xmlValue">指定值</param>  
        public void setXmlValue(string xmlElement, string xmlAttribute, string xmlValue)
        {
            XDocument xmlDoc = XDocument.Load(this.XmlName);
            xmlDoc.Element("config").Element(xmlElement).Attribute(xmlAttribute).SetValue(xmlValue);
            xmlDoc.Save(this.XmlName);
        }
    }
}

