using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    [Serializable]
    public class ManualConnection
    {
        public ManualConnection()
        {
            TestStepList = new List<TestStep>();
            IsTest = true;
        }
        [System.Xml.Serialization.XmlIgnore]
        public Action UpdateName { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public bool IsTest { get; set; }
        public string Name { get; set; }
        public string SN { get; set; }
        public bool? PassFail { get; set; }
        public string ConnectionGuideMsg { get; set; }
        public List<TestStep> TestStepList { get; set; }

        //前置延时
        public double PreDelay { get; set; }
        //前置延时使能
        public bool IsPreDelayEnable { get; set; }
        //后置延时
        public double PostDelay { get; set; }
        //后置延时使能
        public bool IsPostDelayEnable { get; set; }

        //步骤向导图片
        public string ImageFileName { get; set; }
        //向导图片描述
        public string ImageDescription { get; set; }

        //接续上步
        public bool IsFollowEnable { get; set; }
        //接续时长最小值
        public double FollowTimeMin { get; set; }
        //接续时长最大值
        public double FollowTimeMax { get; set; }
        //接续规则
        public FollowRuleEnum FollowRule { get; set; }

    }

    public enum FollowRuleEnum { 超时自动测试, 超时禁止测试 };
}
