using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public delegate void DetailProgressReportHandler(int traceIndex, int markerIndex);
    
    public class TestStep
    {
        public TestStep()
        {
            
            ItemList = new List<TestItem>();
            IsTest = true;
            //CalStepList = new List<CalStepInfo>();
            if (MeasInfoDisplayNameList!=null)
            {
                MeasInfoDisplayName = MeasInfoDisplayNameList.FirstOrDefault();
            }
            if (PathConfigNameList != null)
            {
                PathConfigName = PathConfigNameList.FirstOrDefault();
            }

            IsPrintTrace = true;
        }
        public string Name { get; set; }
        public string DisplayName
        {
            get
            {
                return Name;
            }
        }
        public byte[] bImage { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public MeasClsInfo MeasInfo
        {
            get
            {
                if (TestStepInfoMgr.Instance.TestStepInfoList == null) return null;
                var t = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == this.GetType().Name).FirstOrDefault();
                if (t != null)
                {
                    return t.MeasClsInfoList.Where(x => x.DisplayName == MeasInfoDisplayName).FirstOrDefault();
                }
                else return null;
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public string[] MeasInfoDisplayNameList
        {
            get
            {
                if (TestStepInfoMgr.Instance.TestStepInfoList == null) return null;
                var t = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == this.GetType().Name).FirstOrDefault();
                if (t != null)
                {
                    return t.MeasClsInfoList.Select(x => x.DisplayName).ToArray();
                }
                else return null;
            }
        }
        
        public string MeasInfoDisplayName { get; set; }

        public virtual MeasClsInfo[] SupportedMeasClsInfoList
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// used for Init Driver manually, dynmic init driver
        /// </summary>
        public virtual void InitDriverManually(bool isCal) { }
        /// <summary>
        /// call when testplan load
        /// </summary>
        public virtual void InitOnce() { }
        public virtual void PreSingle() { }
        public virtual void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {

            }
        }
        public virtual void PostSingle() { }
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool IsFinish { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool? PassFail { get; set; }
        public string PathConfigName { get; set; }
        public string[] PathConfigNameList
        {
            get
            {
                if (MeasInfo != null&&MeasInfo.PathConfigInfoList!=null)
                {
                    return MeasInfo.PathConfigInfoList.Select(x => x.Name).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
        public PathConfigInfo GetPathConfigInfo(string pathConfigName)
        {
            if (MeasInfo != null && MeasInfo.PathConfigInfoList != null)
            {
                return MeasInfo.PathConfigInfoList.Where(x => x.Name == pathConfigName).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public PathConfigInfo GetCurrentPathConfigInfo()
        {
            return GetPathConfigInfo(PathConfigName);
        }
        public void SetCurrentPathConfig()
        {
            PathConfigInfo pathConfig = GetCurrentPathConfigInfo();
            
        }
        [System.Xml.Serialization.XmlIgnore]
        public Dictionary<string, string> StepSettings
        {
            get
            {
                if (TestStepInfoMgr.Instance.TestStepInfoList == null) return null;
                var t = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == this.GetType().Name).FirstOrDefault();
                if (t != null)
                {
                    return t.Settings;
                }
                else return null;
            }
        }
        public List<TestItem> ItemList { get; set; }
        /// <summary>
        /// call after testplan finish
        /// </summary>
        public virtual void CleanUp() { }
        [System.Xml.Serialization.XmlIgnore]
        public bool IsTest { get; set; }
        public bool IsSavePic { get; set; }
        /// <summary>
        /// 是否画图
        /// </summary>
        public bool IsPrintTrace { get; set; }
        public bool IsSaveScreen { get; set; }
        public System.Windows.Size SceenSize { get; set; }
        public virtual void GeneSimulatedData()
        {

        }
        /// <summary>
        /// call when testplan applied
        /// </summary>
        public virtual void GenerateDefaultResult()
        {
        }
        ///// <summary>
        ///// teststep 
        ///// </summary>
        //public virtual TestTrace GetDefaultTestTrace()
        //{

        //    return new TestTrace() { TypeName = "S12" };

        //}
        //public void AddDefaultTestTrace()
        //{
        //    TestTrace tr = GetDefaultTestTrace();
        //    if (tr != null)
        //    {
        //        ItemList.Add(GetDefaultTestTrace());
        //    }
        //}
        //public virtual void GenerateItemResultTemplate()
        //{

        //}
        [System.Xml.Serialization.XmlIgnore]
        public DetailProgressReportHandler DetailProgressReport;
        /// <summary>
        /// 支持在TestStep中以Trace为粒度通知界面
        /// </summary>
        /// <param name="traceIndex"></param>
        /// <param name="markerIndex"></param>
        protected void RaiseDetailProgressReport(int traceIndex, int markerIndex)
        {
            if (DetailProgressReport != null)
            {
                DetailProgressReport.Invoke(traceIndex, markerIndex);
            }
        }
        public void CalcStepInfo()
        {  
            foreach (var tr in ItemList)
            {   
                if(tr is TestTrace)
                {
                    (tr as TestTrace).CalcInfo();
                }
                else if (tr is PointTestItem)
                {
                    (tr as PointTestItem).CalcInfo();
                }
                else if(tr is TRTestItem)
                {
                    (tr as TRTestItem).CalcInfo();
                }
            }
            PassFail = Symtant.GeneFunLib.GeneFun.NullBoolAndList(ItemList.Select(x => x.PassFail).ToList());
            IsFinish = true;
        }
        //public bool IsFixedTraces { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public Func<bool> RunState;
        /// <summary>
        ///通常测试流程的停止是以最上层的TestStep为粒度的
        ///如果想在TestStep中及时响应停止消息，那么就在TestStep中查询GetRunState
        /// </summary>
        /// <returns></returns>
        protected bool GetRunState()
        {
            if (RunState == null)
            {
                return true;
            }
            else
            {
                return RunState.Invoke();
            }
        }
        //public virtual string[] GetTableRow()
        //{
        //    return null;
        //}

        
        /// <summary>
        /// 本地设置和数据的接口，比如校准数据
        /// </summary>
        public virtual object GetLocalSetting() {return null; }
        public virtual void SetLocalSetting(object v) { }

        #region cal
        public CalInfo CalInfo { get; set; }
        
        public virtual void InitCal() { }
        public virtual void AcquireStep(string guideMsg) { }
        public virtual void FinishCal() { }
        public virtual void SaveCal() { }
        //[System.Xml.Serialization.XmlIgnore]
        //public List<CalStepInfo> CalStepList { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public virtual string[] CalStepMsgList
        {
            get
            {
                return null;
            }
        }
        public bool CorrectionEnable { get; set; }
        public int CalInterval { get; set; }
        public CalWarningTypeEnum CalWarning { get; set; }
        public bool IsCalEachTest { get; set; }

        
        /// <summary>
        /// 测试步骤对应的本地校准文件是否合法
        /// </summary> 
        [System.Xml.Serialization.XmlIgnore]
        public bool IsCalValid { get; set; }
        
        #endregion

        //[System.Xml.Serialization.XmlIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        //public int IndexInSeq { get; set; }
        public virtual string[] ItemTypeNameList
        {
            get
            {
                return null;
            }
        }
        public virtual Type[] LocalSettingTypeList
        {
            get
            {
                return null;
            }
        }
        public virtual void CreateTrace(string traceTypeName)
        {

        }
        public List<TestStep> GetAllSubTestStep()
        {
            return TestStep.GetAllSubTestStep(this);
        }
        public static List<TestStep> GetAllSubTestStep(TestStep step)
        {
            if (step is ParentTestStep)
            {
                List<TestStep> stepList = new List<TestStep>();
                foreach (var subStep in (step as ParentTestStep).ChildTestStepList)
                {
                    var subSteps = GetAllSubTestStep(subStep);
                    stepList.AddRange(subSteps);
                }
                return stepList;
            }
            else
            {
                List<TestStep> stepList = new List<TestStep>();
                stepList.Add(step);
                return stepList;
            }
        }
        public static List<TestStep> GetAllTestStep(TestPlan testPlan)
        {
            List<TestStep> stepList = new List<TestStep>();
            foreach(var manual in testPlan.ManualConnectionList)
            {
                foreach(var step in manual.TestStepList)
                {
                    if (step is ParentTestStep)
                    {
                        foreach (var subStep in (step as ParentTestStep).ChildTestStepList)
                        {
                            var subSteps = GetAllSubTestStep(subStep);
                            stepList.AddRange(subSteps);
                        }
                    }
                    else
                    {
                        stepList.Add(step);
                    }
                }
            }
            return stepList;
        }

        public List<T> GetTestItem<T>(string itemTypeName) where T:TestItem
        {
            List<T> items = new List<T>();
            var res=ItemList.Where(x=>x.TypeName==itemTypeName);
            if (res != null)
            {
                foreach (T item in res)
                {
                    items.Add(item);
                }
            }
            return items;
        }
        protected string StepConfigFileName
        {
            get
            {
                return string.Format(@"./configfiles/MeasClsInfo_{0}.xml", this.GetType().Name);
            }
        }
        
        protected List<T> LoadMeasClsInfoList<T>(List<T> defaultInfoList)
        {
            if (System.IO.File.Exists(StepConfigFileName))
            {
                List<T> res = Symtant.GeneFunLib.GeneFun.DeserializeList<T>(StepConfigFileName);
                return res;
            }
            else
            {
                Symtant.GeneFunLib.GeneFun.SerializeList<T>(StepConfigFileName, defaultInfoList);
                return defaultInfoList;
            }
        }
    }
    
}
