using DataUtils;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public delegate void StepFinishHandler(int i);
    public delegate void StepStartHandler(int i);
    public delegate void SeqFinishHandler();
    public delegate void ManualConnectStartHandler();
    /// <summary>
    /// notify Marker or trace finished
    /// </summary>
    /// <param name="stepIndex"></param>
    /// <param name="traceIndex">-1 if not available</param>
    /// <param name="markerIndex">-1 if not available</param>
    public delegate void PointFinishHandler(int stepIndex,int traceIndex,int markerIndex);

    public class TestPlan
    {
        private const int TestDescriptionCount = 10;
        public TestPlan()
        {
            TestSpecs = new ObservableCollection<TotalSpecVM>();
            ManualConnectionList = new List<ManualConnection>();
            TestStepNameInfoList = new List<TestStepNameInfo>();
            ExceptionCollection = new List<Exception>();
            StepFinish += TestPlan_StepFinish;
            SeqFinish += TestPlan_SeqFinish;
            IsAsync = true;
       //     TestSpecs.Add(new TotalSpecVM() { SpecName="生产指标" });
            TestDescriptionList = new ObservableCollection<TestDescription>();
            
            
            
            GuidStr = Guid.NewGuid().ToString("D");
        }
        private IPathConfigSetter pathConfig
        {
            get
            {
                return TestPlanManager.PathConfigSetter;
            }
        }
        void TestPlan_SeqFinish()
        {
            RunPostSingle();
            if(GeneTestSetup.Instance.IsTestSuccess && IsRun && currentConnIndex < this.ManualConnectionList.Count - 1)
            {
                if (ManualConnStart != null)
                {
                    ManualConnStart.Invoke();
                }
                currentConnIndex++;               
                StartTestSeq(currentConnIndex);
            }
        }
        public int Id { get; set; }
        private string guidStr;
        public string GuidStr { get; set; }
        void TestPlan_StepFinish(int i)
        {
           
        }
        public List<ManualConnection> ManualConnectionList { get; set; }
        public void AddManualConn()
        {
            ManualConnection conn = new ManualConnection();
            conn.Name = "连接步骤" + ManualConnectionList.Count;
            ManualConnectionList.Add(conn);
        }
        private bool _IsMultiDut;
        public bool IsMultiDut
        {
            get
            {
                return _IsMultiDut;
            }
            set
            {
                _IsMultiDut = value;

            }
        }
        private string _DisplayName;
        private const string DisplayNamePropertyName = "DisplayName";
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;               
            }
        }
        private string _SN;
        private const string SNPropertyName = "SN";
        public string SN
        {
            get
            {
                return _SN;
            }
            set
            {
                _SN = value;
            }
        }
        private ObservableCollection<string> _TestEnvironm = new ObservableCollection<string>();
        private const string TestEnvironmPropertyName = "TestEnvironm";
        public ObservableCollection<string> TestEnvironm
        {
            get
            {
                return _TestEnvironm;
            }
            set
            {
                _TestEnvironm = value;
                //NotifyPropertyChanged(TestEnvironmPropertyName);
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<TestDescription> TestDescriptionList { get; set; }
        private ObservableCollection<TotalSpecVM> _TestSpecs;
        private const string TestSpecsPropertyName = "TestSpecs";
        
        public ObservableCollection<TotalSpecVM> TestSpecs
        {
            get
            {
                return _TestSpecs;
            }
            set
            {
                _TestSpecs = value;
                //NotifyPropertyChanged(TestSpecsPropertyName);
            }
        }
        public void AddTestSpecs(string str)
        {
            if(TestSpecs.Where(x=>x.SpecName==str).Count()==0)
            {
                TestSpecs.Add(new TotalSpecVM() {SpecName=str,PassFail=null});
                foreach(var manualConn in ManualConnectionList)
                {
                    foreach(var step in manualConn.TestStepList)
                    {
                        foreach (var realStep in TestStep.GetAllSubTestStep(step))
                        {
                            foreach (var tr in realStep.ItemList)
                            {
                                if (tr is TestTrace)
                                {
                                    (tr as TestTrace).TestSpecList.Add(new TestTraceSpec());
                                }
                                if (tr is PointTestItem)
                                {
                                    (tr as PointTestItem).TestSpecList.Add(new PointTestSpec());
                                }
                                if (tr is TRTestItem)
                                {
                                    (tr as TRTestItem).TestSpecList.Add(new TRTestItemSpec());
                                }
                            }   
                        }
                    }
                }
            }
        }
        public void DeletSpecs(TotalSpecVM str)
        {
            if (TestSpecs.Contains(str))
            {
                int specIndex = TestSpecs.IndexOf(str);
                TestSpecs.Remove(str);            
                foreach (var manualConn in ManualConnectionList)
                {
                    foreach (var step in manualConn.TestStepList)
                    {
                        foreach(var realStep in TestStep.GetAllSubTestStep(step))
                        {
                            foreach (var tr in realStep.ItemList)
                            {
                                if (tr is TestTrace)
                                {
                                    (tr as TestTrace).TestSpecList.RemoveAt(specIndex);
                                }
                                if (tr is PointTestItem)
                                {
                                    (tr as PointTestItem).TestSpecList.RemoveAt(specIndex);
                                }
                            }
                        }
                    }
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public int currentConnIndex;
        /// <summary>
        /// test one connection
        /// </summary>
        /// <param name="connIndex">connection index</param>
        public void StartTestSeq(int connIndex)
        {
            ExceptionCollection.Clear();
            if (IsRuning)
                return;
            LoggingInfo += "测试开始";
            LoggingInfo += DateTime.Now;
            LoggingInfo += "\n";
            currentConnIndex = connIndex;
            ReStart();
            RunPreSingle();
            Run();
        }

        private void RunPreSingle()
        {
            if (TestPlanManager.TestPlanSeq != null)
            {
                TestPlanManager.TestPlanSeq.PreRun();
            }
            var testStepList = ManualConnectionList[currentConnIndex].TestStepList;
            foreach (var step in testStepList)
            {
                step.PreSingle();
            }
        }
        private void RunPostSingle()
        {
            var testStepList = ManualConnectionList[currentConnIndex].TestStepList;
            foreach (var step in testStepList)
            {
                step.PostSingle();
            }
            
            if (TestPlanManager.TestPlanSeq != null)
            {
                TestPlanManager.TestPlanSeq.PostRun();
            }
        }
        public void StopCurrentTestSeq()
        {
            Stop();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResumeCurrentTestSeq()
        {
            Run();
        }
        public void ApplyTestPlan()
        {
            foreach (ManualConnection mc in ManualConnectionList)
            {
                foreach (var step in mc.TestStepList)
                {   
                    if(step is ManualLoopTestStep)
                    {
                        (step as ManualLoopTestStep).InitManulTestItem();
                        continue;
                    }
                    if (step is LoopTestStep)
                    {
                        (step as LoopTestStep).InitTestItem();
                    }
                    foreach(var subStep in TestStepFactory.GetAllSubTestStep(step))
                    {
                        subStep.GenerateDefaultResult();
                        
                    }
                    //else
                    //{
                    //    step.GenerateDefaultResult();
                    //}
                }
            }
            if (!GeneTestSetup.Instance.IsSimulated)
            {
                
                InitMeasInfo(false);
                //foreach (ManualConnection mc in ManualConnectionList)
                //{
                //    foreach (var step in mc.TestStepList)
                //    {

                //        step.PreInitOnce();
                //    }
                //}

                if (TestPlanManager.TestPlanSeq != null)
                {
                    TestPlanManager.TestPlanSeq.PreInitOnce();
                }
                foreach (ManualConnection mc in ManualConnectionList)
                {
                    foreach (var step in mc.TestStepList)
                    {
                        
                        step.InitOnce();
                    }
                }
            }
            if (TestPlanManager.TestPlanStarter != null)
            {
                TestPlanManager.TestPlanStarter.Invoke();
            }
        }
        /// <summary>
        /// call all step's initcal
        /// </summary>
        public void InitAllCal()
        {
            if (TestPlanManager.LocalSettingsStarter != null)
            {
                TestPlanManager.LocalSettingsStarter.PreInit();
            }
            //Symtant.InstruDriver.InstruDriverFactory.ResetAllInstru();
            foreach (ManualConnection mc in ManualConnectionList)
            {
                foreach (var step in mc.TestStepList)
                {
                    foreach (var realStep in step.GetAllSubTestStep())
                    {
                        realStep.InitCal();
                    }
                }
            }

        }
        /// <summary>
        /// connect and init all instrument needed for cal
        /// </summary>
        public void BeginCal()
        {
            if (!GeneTestSetup.Instance.IsSimulated)
            {
                InitMeasInfo(true);
            }
        }
        public void SaveAllCal()
        {
            foreach (ManualConnection mc in ManualConnectionList)
            {
                foreach (var step in mc.TestStepList)
                {
                    foreach (var realStep in step.GetAllSubTestStep())
                    {
                        realStep.SaveCal();
                    }
                }
            }
        }
        private void InitMeasInfo(bool isCal)
        {
            Symtant.InstruDriver.InstruDriverFactory.CloseAllInstru();
            Symtant.InstruDriver.InstruDriverFactory.ClearInstruInfoList();
            foreach (ManualConnection mc in ManualConnectionList)
            {
                foreach (var step in mc.TestStepList)
                {
                    foreach (var realstep in TestStep.GetAllSubTestStep(step))
                    {
                        if (realstep.IsTest && realstep.MeasInfo != null)
                        {
                            foreach (var instruInfo in realstep.MeasInfo.InstruInfoList)
                            {
                                if ((!instruInfo.IsManuallyInit)&&( isCal || !instruInfo.IsCal))
                                {
                                    try
                                    {
                                        instruInfo.InitDriver();
                                    }
                                    catch
                                    {
                                        throw (new Exception(realstep.DisplayName+ " can't get driver from " + instruInfo.DisplayName+","+instruInfo.Address));
                                    }
                                    if (instruInfo.InstruDriver == null)
                                    {
                                        throw (new Exception(realstep.DisplayName+" can't get driver from " + instruInfo.DisplayName + "," + instruInfo.Address));
                                    }
                                }

                                //else
                                //{
                                //    instruInfo.InstruDriver.Open();
                                //}
                            }
                            realstep.InitDriverManually(isCal);
                        }
                    }
                }
            }
            var pathConfigAsStep = pathConfig as TestStep;
            if (pathConfigAsStep != null)
            {
                if (pathConfigAsStep.MeasInfo != null)
                {
                    foreach (var instruInfo in pathConfigAsStep.MeasInfo.InstruInfoList)
                    {
                        try
                        {
                            instruInfo.InitDriver();
                        }
                        catch
                        {
                            throw (new Exception("无法连接路径配置器的开关箱"));
                        }
                    }
                }
            }
            Symtant.InstruDriver.InstruDriverFactory.OpenAllInstru();
            Symtant.InstruDriver.InstruDriverFactory.InitAllInstru();
            Symtant.InstruDriver.InstruDriverFactory.ResetAllInstru();
        }
        public event StepFinishHandler StepFinish;
        public event StepStartHandler StepStart;
        public event SeqFinishHandler SeqFinish;
        public event ManualConnectStartHandler ManualConnStart;
        public event PointFinishHandler PointFinish;
        [System.Xml.Serialization.XmlIgnore]
        public List<Exception> ExceptionCollection { get; set; }
        public bool IsRunWhenException { get; set; }
        public bool IsRuning;
        private bool IsRun;
        [System.Xml.Serialization.XmlIgnore]
        public bool IsAsync;
        public void Stop()
        {
            IsRun = false;

        }
        private int stopIndex = 0;
        private int stopConnectIndex = 0;
        private void ReStart()
        {
            stopIndex = 0;
        }
        private void OnSeqRun()
        {   
            var testStepList = ManualConnectionList[currentConnIndex].TestStepList;
            for (int i = stopIndex; i < testStepList.Count; i++)
            {
                if (IsRun)
                {
                    if (testStepList[i].IsTest)
                    {
                        try
                        {   
                            if(StepStart != null)
                            {
                                StepStart.Invoke(i);
                            }
                            testStepList[i].DetailProgressReport = (traceIndex, markerIndex) =>
                                {
                                    if (PointFinish != null)
                                    {
                                        PointFinish.Invoke(i, traceIndex, markerIndex);
                                    }
                                };
                            testStepList[i].RunState = () => GetRunState();
                            if (!GeneTestSetup.Instance.IsSimulated)
                            {
                                //set path config
                                if (pathConfig == null)
                                {
                                    DataUtils.LOGINFO.WriteLog(testStepList[i].DisplayName + " path is empty");
                                }
                                else
                                {
                                    string configName = testStepList[i].PathConfigName;
                                    if (string.IsNullOrWhiteSpace(configName))
                                    {
                                        DataUtils.LOGINFO.WriteLog(testStepList[i].DisplayName + " path is empty");
                                    }
                                    else
                                    {
                                        var config = testStepList[i].GetPathConfigInfo(configName);
                                        if (config != null)
                                        {
                                            string configValue = config.Path;
                                            string[] pathConfigList = pathConfig.GetPathConfigNameList();
                                            if (pathConfigList != null && pathConfigList.Contains(configValue))
                                            {
                                                pathConfig.SetPathConfig(configValue);
                                            }
                                            else
                                            {
                                                DataUtils.LOGINFO.WriteLog(testStepList[i].DisplayName + " " + configValue + " path is not found");
                                                throw (new Exception(testStepList[i].DisplayName + " " + configValue + " path is not found"));
                                            }
                                        }
                                    }
                                }
                            }
                            DataUtils.LOGINFO.WriteLog(DateTime.Now.ToString() + testStepList[i].DisplayName+" single start");
                            testStepList[i].Single();
                            DataUtils.LOGINFO.WriteLog(DateTime.Now.ToString() + testStepList[i].DisplayName + " single finish");
                            if (!GeneTestSetup.Instance.IsSimulated)
                            {
                                if (pathConfig == null)
                                {
                                    DataUtils.LOGINFO.WriteLog(testStepList[i].DisplayName + " path is empty");
                                }
                                else
                                {
                                    string configName = testStepList[i].PathConfigName;
                                    if (string.IsNullOrWhiteSpace(configName))
                                    {
                                        DataUtils.LOGINFO.WriteLog(testStepList[i].DisplayName + " path is empty");
                                    }
                                    else
                                    {
                                        var config = testStepList[i].GetPathConfigInfo(configName);
                                        if (config != null)
                                        {
                                            string configValue = config.Path;
                                            string[] pathConfigList = pathConfig.GetPathConfigNameList();
                                            if (pathConfigList != null && pathConfigList.Contains(configValue))
                                            {
                                                pathConfig.PostPathConfig(configValue);
                                            }
                                            else
                                            {
                                                DataUtils.LOGINFO.WriteLog(testStepList[i].DisplayName + " " + configValue + " path is not found");
                                                throw (new Exception(testStepList[i].DisplayName + " " + configValue + " path is not found"));
                                            }
                                        }
                                    }
                                }
                            }
                            testStepList[i].CalcStepInfo();
                            //更新连接步骤中的passfail
                            ManualConnectionList[currentConnIndex].PassFail = Symtant.GeneFunLib.GeneFun.NullBoolAndList(testStepList.Select(x => x.PassFail).ToList());
                            if (StepFinish != null)
                            {
                                StepFinish.Invoke(i);
                            }
                        }
                        catch (Exception exp)
                        {
                            testStepList[i].CleanUp();
                            ExceptionCollection.Add(exp);
                            IsRun = IsRunWhenException;
                        }
                        
                    }
                }
                else
                {
                    stopIndex = i;
                    System.Diagnostics.Debug.WriteLine(stopIndex);
                    break;
                }             
            }
            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
            IsRuning = false;
            if (SeqFinish != null)
            {
                SeqFinish.Invoke();
            }
        }

        private bool GetRunState()
        {
            return IsRun;
        }
        
        private void Run()
        {
            if (IsRuning)
            {
                return;
            }
            else
            {   
                IsRun = true;
                IsRuning = true;
                if (IsAsync)
                {
                    //bw.RunWorkerAsync();
                    //System.Threading.Thread.Sleep(1000);
                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(OnSeqRun));
                    t.SetApartmentState(ApartmentState.STA);
                    t.IsBackground = true;
                    t.Start();
                }
                else
                {
                    OnSeqRun();
                }
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public string LoggingInfo { get; set; }
        public List<TestStepNameInfo> TestStepNameInfoList { get; set; }
        public int Allcoate(string stepTypeName)
        {
            var item = TestStepNameInfoList.Find(x => x.StepName == stepTypeName);
            if (item != null)
            {
                int max = item.OccupiedIndexList.Max() + 1;
                item.OccupiedIndexList.Add(max);
                return max;
            }
            else
            {
                TestStepNameInfo nameInfo = new TestStepNameInfo() { StepName=stepTypeName};
                nameInfo.OccupiedIndexList.Add(0);
                TestStepNameInfoList.Add(nameInfo);
                return 0;
            }
        }
        

    }
    public class TotalSpecVM:NotifyBase
    {
        private string _SpecName;
        private const string SpecNamePropertyName = "SpecName";
        public string SpecName
        {
            get
            {
                return _SpecName;
            }
            set
            {
                _SpecName = value;
                NotifyPropertyChanged(SpecNamePropertyName);
            }
        }
        private bool? _PassFail;
        private const string PassFailPropertyName = "PassFail";
        [System.Xml.Serialization.XmlIgnore]
        public bool? PassFail
        {
            get
            {
                return _PassFail;
            }
            set
            {
                _PassFail = value;
                NotifyPropertyChanged(PassFailPropertyName);
            }
        }
    }
    public class TestStepNameInfo
    {
        public TestStepNameInfo()
        {
            OccupiedIndexList = new List<int>();
        }
        public string StepName { get; set; }
        public List<int> OccupiedIndexList { get; set; }
        
    }
    public class TestDescription
    {
        public string DescName { get; set; }
        public object DescValue { get; set; }
        public string DescType { get; set; }
        public object DescSource { get; set; }
    }
}
