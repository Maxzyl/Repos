using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symtant.InstruDriver;
using System.Collections.ObjectModel;
using System.Reflection;
namespace ModelBaseLib
{
    /// <summary>
    /// 测试步骤的信息包含支持的测量类信息，测量类信息中包含支持的仪表的信息
    /// 测试步骤被创建后，当前配置的测试类信息被传递到测试步骤中
    /// </summary>
    public class MeasClsInfo
    {
        
        public string TypeName { get; set; }
        public InstruInfo[] InstruInfoList { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }
        public MeasClsInfo()
        {
            IsSelected = true;
            Settings = new Dictionary<string, string>();
        } 
        public bool IsNeedCal { get; set; }
        public PathConfigInfo[] PathConfigInfoList { get; set; }
        //
        /// <summary>
        /// 从配置文件中读取的测量类对应的Info，比如用来指定网络分析仪端口个数
        /// </summary>
        public string Info { get; set; }
        
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> Settings { get; set; }
    }
    
    public class TestStepInfo
    {
        public TestStepInfo()
        {
            MeasClsInfoList = new List<MeasClsInfo>();
            Settings = new Dictionary<string, string>();
        }
        //public MeasClsInfo[] SupportedMeasClsInfoList { get; set; }
        public string TypeName { get; set; }
        public string DisplayName { get; set; }
        public List<MeasClsInfo> MeasClsInfoList { get; set; }
        public string[] TestTraceTypeList { get; set; }
        public bool IsFixedItem { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public Type StepType { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public Assembly Assembly { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public string TestStepViewModelStr { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public string AdvUserControlStr { get; set; }
        public bool IsNeedCal { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> Settings { get; set; }
        /// <summary>
        /// 本地设置和校准界面全类型名 xxx.dll;xxx.xxx
        /// </summary>
        public string LocalSettingViewStr { get; set; }

    }
    public class TestTraceInfo
    {
        public string TestTraceViewModelStr { get; set; }
        public string AdvUserControlStr { get; set; }
    }
    public class TestStepInfoMgr
    {
        private TestStepInfoMgr()
        {
            
        }
        public TestTraceInfo[] TestTraceInfoList { get; set; }
        public TestStepInfo[] TestStepInfoList { get; set; }
        private static TestStepInfoMgr _Instance = new TestStepInfoMgr();
        public static TestStepInfoMgr Instance
        {
            get
            {
                return _Instance;
            }
        }
        public void LoadInstruInfoFromFile()
        {
            string currentFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = currentFilePath + "/configfiles/DutDescSet.xml";
            object obj = DataUtils.CommUtils.DeserializerData(typeof(ObservableCollection<InstruInfo>), fileName);
            if(obj is ObservableCollection<InstruInfo>)
            {
                ObservableCollection<InstruInfo> InstruInfoList=obj as ObservableCollection<InstruInfo>;
                int i = 0;
                foreach(TestStepInfo testStepInfo in TestStepInfoList)
                {
                    foreach(MeasClsInfo measClsInfo in testStepInfo.MeasClsInfoList)
                    {
                        if (measClsInfo.IsSelected)
                        {
                            foreach (InstruInfo instruInfo in measClsInfo.InstruInfoList)
                            {
                                //for (int i = 0; i < InstruInfoList.Count;i++ )
                                //{
                                if (i < InstruInfoList.Count)
                                {
                                    instruInfo.InstruDriverType = InstruInfoList[i].InstruDriverType;
                                    instruInfo.Model = InstruInfoList[i].Model;
                                    instruInfo.Vendor = InstruInfoList[i].Vendor;
                                    instruInfo.SerialNum = InstruInfoList[i].SerialNum;
                                    instruInfo.FirmwareVersion = InstruInfoList[i].FirmwareVersion;
                                    instruInfo.RemoteInterface = InstruInfoList[i].RemoteInterface;
                                    instruInfo.Address = InstruInfoList[i].Address;
                                    //}
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }
        public static TestStepInfo GetStepInfo(string stepTypeName)
        {
            return TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == stepTypeName).FirstOrDefault();
        }
        public static TestStepInfo GetStepInfoByDisplayName(string displayName)
        {
            return TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.DisplayName == displayName).FirstOrDefault();
        }
    }
    public class PathConfigInfo
    {
        public PathConfigInfo()
        {
            Settings = new Dictionary<string, string>();
        }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Info { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
    //public class ConfigMeasClsInfo : MeasClsInfo
    //{
    //    public ConfigInfo[] ConfigInfoList { get; set; }
    //}
}
