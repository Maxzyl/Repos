using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    /// <summary>
    /// 包含Item相关的设置和Item对应的结果
    /// </summary>
    public class TestItem
    {

        public string TypeName
        {
            get;
            set;
        }
        public double Compensation { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public bool IsTest { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool? PassFail { get; set; }
        public string Unit { get; set; }
        public string UserDefName { get; set; }
        public string VarName { get; set; }
        /// <summary>
        /// 备用属性
        /// </summary>
        public string Info1 { get; set; }
        public virtual void AddSpec() { }
        public virtual void RemoveSpec(int specIndex) { }
        /// <summary>
        /// 端口描述
        /// </summary>
        public string TestConfigDesciption { get; set; }
        /// <summary>
        /// 测试条件
        /// </summary>
        public string XDescription { get; set; }
    }
    
}
