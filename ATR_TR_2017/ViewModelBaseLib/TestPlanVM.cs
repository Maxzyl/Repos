using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ModelBaseLib;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Windows.Threading;
using Symtant.GeneFunLib;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Mvvm;
using SymtCalc;
using System.Windows.Input;
using DevExpress.Xpf.Core;


namespace ViewModelBaseLib
{
    public class TestPlanVM:NotifyBase
    {
        public TestPlanVM()
        {   
            TestPlan = new TestPlan();
            FilteredManualConnList = new ObservableCollection<TreeNodeVM>();
            GridVMlist = new ObservableCollection<object>();
            listenerlist = new List<IResultListerner>();

            //TestPlan.StepFinish += TestPlan_StepFinish;
            //TestPlan.SeqFinish += TestPlan_SeqFinish;
            //TestPlan.ManualConnStart += TestPlan_ManualConnStart;
            //TestPlan.PointFinish += TestPlan_PointFinish;
            //TestPlan.StepStart += TestPlan_StepStart;
            Cal = new Calculation();
            param = new CommandPara();
        }
        public ObservableCollection<object> GridVMlist { get; set; }
        public List<IResultListerner> listenerlist { get; set; } 
        private string _DisplayName;
        private const string DisplayNamePropertyName = "DisplayName";
        public string DisplayName
        {
            get
            {
                return TestPlan.DisplayName;
            }
            set
            {
                TestPlan.DisplayName = value;
                NotifyPropertyChanged(DisplayNamePropertyName);
            }
        }
        private string _SN;
        private const string SNPropertyName = "SN";
        public string SN
        {
            get
            {
                return TestPlan.SN;
            }
            set
            {
               
                TestPlan.SN = value;
                NotifyPropertyChanged(SNPropertyName);
            }
        }
        private bool _IsMultiDut;
        private const string IsMultiDutPropertyName = "IsMultiDut";
        public bool IsMultiDut
        {
            get
            {
                return TestPlan.IsMultiDut;
            }
            set
            {
                TestPlan.IsMultiDut = value;
                NotifyPropertyChanged(IsMultiDutPropertyName);
            }
        }
        void TestPlan_SeqFinish()
        {   
            if(TestSeqFinish != null)
            {
                TestSeqFinish.Invoke();
            }
            if (TestPlan.ExceptionCollection.Count != 0)
            {
                string expStr = null;
                foreach (var exp in TestPlan.ExceptionCollection)
                {
                    expStr += exp.Message;
                    expStr += "\n";
                }
                System.Windows.MessageBox.Show(expStr);
            }
            foreach(var manul in ManualConnList)
            {
                manul.IsEnabled = true;
            }
        }

        void TestPlan_StepStart(int i)
        { 
            if(StepStart !=null)
            {
                StepStart.Invoke(i);
            }
        }
        void TestPlan_StepFinish(int i)
        {                      
            if(UpdateData !=null)
            {
                UpdateData.Invoke(i);
            }
        }
        void TestPlan_ManualConnStart()
        { 
            if(ManualConnStart != null)
            {
                ManualConnStart.Invoke();
            }
            foreach(var manul in ManualConnList)
            {
                manul.IsEnabled = false;
            }
        }

        void TestPlan_PointFinish(int stepIndex, int traceIndex, int markerIndex)
        { 
            if(PointFinish != null)
            {
                PointFinish.Invoke(stepIndex,traceIndex,markerIndex);
            }
        }

        private TestPlan _TestPlan;
        private const string TestPlanPropertyName = "TestPlan";
        public TestPlan TestPlan
        {
            get
            {
                return _TestPlan;
            }
            set
            { 
                _TestPlan = value;
                if(ManualConnList !=null && ManualConnList.Count > 0)
                {
                    ManualConnList.Clear();
                }
                if(FilteredManualConnList !=null && FilteredManualConnList.Count > 0)
                {
                    FilteredManualConnList.Clear();
                }
                foreach (var manualConn in _TestPlan.ManualConnectionList)
                {   
                    ManualConnList.Add(new TreeNodeVM() { NodeObj = manualConn});
                }
                foreach (var mannConn in ManualConnList)
                {
                    FilteredManualConnList.Add(mannConn);
                }
                TestPlan.StepFinish += TestPlan_StepFinish;
                TestPlan.SeqFinish += TestPlan_SeqFinish;
                TestPlan.StepStart += TestPlan_StepStart;
                TestPlan.ManualConnStart += TestPlan_ManualConnStart;
                TestPlan.PointFinish += TestPlan_PointFinish;
                NotifyPropertyChanged(TestPlanPropertyName);
                NotifyPropertyChanged(TestSpecsPropertyName);
                NotifyPropertyChanged(TestEnvironmPropertyName);
                NotifyPropertyChanged(TestDescriptionListPropertyName);
            }
        }
        public bool Flag { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public Action<int> UpdateData;
        [System.Xml.Serialization.XmlIgnore]
        public Action<int> StepStart ;
        [System.Xml.Serialization.XmlIgnore]
        public Action ManualConnStart;
        [System.Xml.Serialization.XmlIgnore]
        public Action TestSeqFinish;
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public Action<int,int,int> PointFinish;
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<TreeNodeVM> FilteredManualConnList { get; set; }
        
        
        public void Search(string searchStr)
        {
            FilteredManualConnList.Clear();
            foreach(var conn in ManualConnList)
            {
                if(!string.IsNullOrWhiteSpace(searchStr))
                {
                    if (conn.Name.Contains(searchStr))
                    {
                        FilteredManualConnList.Add(conn);
                    }
                    else
                    {  
                        
                        foreach(var step in conn.SubTreeNodeList)
                        {
                            if(step.Name.Contains(searchStr))
                            {

                            }
                        }
                    }
                }
            }
        }
        private ObservableCollection<TreeNodeVM> _ManualConnList = new ObservableCollection<TreeNodeVM>();
        private const string ManualConnListPropertyName = "ManualConnList";
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ObservableCollection<TreeNodeVM> ManualConnList
        {
            get
            {
                return _ManualConnList;

            }
            set
            {
                _ManualConnList = value;
                NotifyPropertyChanged(ManualConnListPropertyName);
            }
        }

        private TreeNodeVM _SelectedItem;
        private const string SelectedItemPropertyName = "SelectedItem";
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TreeNodeVM SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                _SelectedItem = value;
                NotifyPropertyChanged(SelectedItemPropertyName);
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        private int currentConnIndex;
        /// <summary>
        /// test one connection
        /// </summary>
        /// <param name="connIndex">connection index</param>
        public void StartTestSeq(int connIndex)
        {   
            LoggingInfo += "测试开始";
            LoggingInfo += DateTime.Now;
            LoggingInfo += "\n";
           // InitialData();
            foreach (var manul in ManualConnList)
            {
                manul.IsEnabled = false;
            }
            TestPlan.StartTestSeq(connIndex);
        }

        public void StopCurrentTestSeq()
        {
            TestPlan.Stop();
            InitialData();
        }
        public void FinishCurrentTestSeq(string SN)
        {
            string Token = (new ViewModelLocator()).MainWindow.StatusInfo.Token;
            string terminal = (new ViewModelLocator()).MainWindow.StatusInfo.Terminal;
            string material=(new ViewModelLocator()).MainWindow.StatusInfo.Material;
            string processid = (new ViewModelLocator()).MainWindow.StatusInfo.Process;
            if ((new ViewModelLocator()).MainWindow.StatusInfo.IsLocal == false)
            {
                if(DataUtils.StaticInfo.MesMode.ToLower()=="true")
                {
                    string resValue = ATE2MES.ATE2MES.ATESNMOVEOUT(TestPlan.SN, terminal, DataUtils.StaticInfo.LoginUser);
                    if (resValue.Substring(0, 2) != "OK")
                    {
                        DXMessageBox.Show("序号：[" + TestPlan.SN + "]处理异常！异常消息：[" + resValue + "]");
                    }
                }
            }
            InitialData();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResumeCurrentTestSeq()
        {
            TestPlan.ResumeCurrentTestSeq();
        }
        public const string LoggingInfoPropertyName = "LoggingInfo";
        private string _loggingInfo = null;
        [System.Xml.Serialization.XmlIgnore]
        public string LoggingInfo
        {
            get
            {
                return _loggingInfo;
            }

            set
            {
                _loggingInfo = value;
                NotifyPropertyChanged(LoggingInfoPropertyName);
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public object CopyItem { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        private int _SelectedSpecIndex;
        public int SelectedSpecIndex 
        {
            get
            {
                return _SelectedSpecIndex;
            }
            set
            {
                _SelectedSpecIndex = value;
                foreach (var node in ManualConnList)
                {
                    NotifyNodeSpecIndexChanged(node, value);
                }
            }
        }

        public void NotifyNodeSpecIndexChanged(TreeNodeVM node, int specIndex)
        {
            node.SpecIndex = specIndex;

            if (node.SubTreeNodeList.Count > 0)
            {
                foreach (var subnode in node.SubTreeNodeList)
                {
                    NotifyNodeSpecIndexChanged(subnode, specIndex);
                }
            }

        }
        
        public void AddManualConnection()
        {   
            this.TestPlan.AddManualConn();
            ManualConnList.Add(new TreeNodeVM() {NodeObj=TestPlan.ManualConnectionList.Last()});
            Search("");
        }
        /// <summary>
        /// when press Add,remove,copy,paste
        /// </summary>
        public void AddNode()
        {
            if (SelectedItem == null)
            {
                AddManualConnection();
            }
            else
            {
                switch (SelectedItem.Type)
                {   
                   
                    case TreeNodeTypeEnum.ManualConnection:

                        break;
                    case TreeNodeTypeEnum.ParentTestStep:
                        break;
                    case TreeNodeTypeEnum.TestStep:
                        break;
                    case TreeNodeTypeEnum.PointTestItem:
                        break;
                    case TreeNodeTypeEnum.TestTrace:
                        XYTestMarker mk = new XYTestMarker();
                        (SelectedItem.NodeObj as TestTrace).TestSpecList[SelectedSpecIndex].TestMarkerList.Add(mk);
                        int mkCount = (SelectedItem.NodeObj as TestTrace).TestSpecList[SelectedSpecIndex].TestMarkerList.Count - 1;
                        SelectedItem.AddSubNode(new TreeNodeVM() { NodeObj = mk, Name = "Marker" + mkCount,ParentNode = SelectedItem });
                        break;
                    case TreeNodeTypeEnum.TRTestItem:
                        XYTestMarker xyTestMarker = new XYTestMarker();
                        xyTestMarker.IsTRTestItem = true;
                        (SelectedItem.NodeObj as TRTestItem).TestSpecList[SelectedSpecIndex].TestMarkerList.Add(xyTestMarker);
                        int xyTestMarkerCount = (SelectedItem.NodeObj as TRTestItem).TestSpecList[SelectedSpecIndex].TestMarkerList.Count - 1;
                        SelectedItem.AddSubNode(new TreeNodeVM() { NodeObj = xyTestMarker,Name = "Marker" + xyTestMarkerCount,ParentNode = SelectedItem});
                        break;
                    case TreeNodeTypeEnum.TestMarker:
                        break;
                    default:
                        break;
                }
            }
        }
        public void RemoveNode()
        {   
            if(SelectedItem == null)return;
            TreeNodeVM nodeVM = null;
            if (SelectedItem.ParentNode !=null && SelectedItem.ParentNode.SubTreeNodeList.Count() > 0)
            {
                int index = SelectedItem.ParentNode.SubTreeNodeList.IndexOf(SelectedItem);
                if(index != 0)
                {
                    nodeVM = SelectedItem.ParentNode.SubTreeNodeList[index - 1];
                }
            }
            switch (SelectedItem.Type)
            {
                case TreeNodeTypeEnum.ManualConnection:
                    TestPlan.ManualConnectionList.Remove(SelectedItem.NodeObj as ManualConnection);
                    ManualConnList.Remove(SelectedItem);
                    break;
                case TreeNodeTypeEnum.ParentTestStep:
                    ManualConnection manualCon = SelectedItem.ParentNode.NodeObj as ManualConnection;
                    ParentTestStep parentStep = SelectedItem.ParentNode.NodeObj as ParentTestStep;
                    if(manualCon !=null)
                    {
                        manualCon.TestStepList.Remove(SelectedItem.NodeObj as TestStep);
                    }
                    else if(parentStep !=null)
                    {
                        parentStep.ChildTestStepList.Remove(SelectedItem.NodeObj as TestStep);
                    }
                    SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    break;
                case TreeNodeTypeEnum.TestStep:
                    if(SelectedItem.ParentNode.NodeObj as ManualConnection !=null)
                    {
                        (SelectedItem.ParentNode.NodeObj as ManualConnection).TestStepList.Remove(SelectedItem.NodeObj as TestStep);
                        SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    }
                    else if(SelectedItem.ParentNode.NodeObj as LoopTestStep !=null)
                    {
                        (SelectedItem.ParentNode.NodeObj as LoopTestStep).ChildTestStepList.Remove(SelectedItem.NodeObj as TestStep);
                        SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    }
                    break;
                case TreeNodeTypeEnum.PointTestItem:
                    TestStep currstep1 = (SelectedItem.ParentNode.NodeObj as TestStep);
                    if (!TestStepInfoMgr.GetStepInfo(currstep1.GetType().Name).IsFixedItem)
                    {
                        (SelectedItem.ParentNode.NodeObj as TestStep).ItemList.Remove(SelectedItem.NodeObj as TestItem);
                        SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    }
                    break;
                case TreeNodeTypeEnum.TestTrace:
                    TreeNodeVM node = SelectedItem.ParentNode;
                    TestStep currstep = (SelectedItem.ParentNode.NodeObj as TestStep);
                    if (!TestStepInfoMgr.GetStepInfo(currstep.GetType().Name).IsFixedItem)
                    {
                        (SelectedItem.ParentNode.NodeObj as TestStep).ItemList.Remove(SelectedItem.NodeObj as TestItem);
                        SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    }
                    foreach (var item in node.SubTreeNodeList)
                    {   
                        item.Index = node.SubTreeNodeList.IndexOf(item);
                    }
                    break;
                case TreeNodeTypeEnum.TRTestItem:
                    TreeNodeVM stepNode = SelectedItem.ParentNode;
                    TestStep currentStep = (SelectedItem.ParentNode.NodeObj as TestStep);
                    if(!TestStepInfoMgr.GetStepInfo(currentStep.GetType().Name).IsFixedItem)
                    {
                        (SelectedItem.ParentNode.NodeObj as TestStep).ItemList.Remove(SelectedItem.NodeObj as TestItem);
                        SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    }
                    foreach(var item in stepNode.SubTreeNodeList)
                    {
                        item.Index = stepNode.SubTreeNodeList.IndexOf(item);
                    }
                    break;
                case TreeNodeTypeEnum.TestMarker:
                    if (SelectedItem.ParentNode.NodeObj is TestTrace)
                    {
                        (SelectedItem.ParentNode.NodeObj as TestTrace).TestSpecList[SelectedSpecIndex].TestMarkerList.Remove(SelectedItem.NodeObj as XYTestMarker);
                         SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    }
                    else if (SelectedItem.ParentNode.NodeObj is TRTestItem)
                    {
                        (SelectedItem.ParentNode.NodeObj as TRTestItem).TestSpecList[SelectedSpecIndex].TestMarkerList.Remove(SelectedItem.NodeObj as XYTestMarker);
                        SelectedItem.ParentNode.SubTreeNodeList.Remove(SelectedItem);
                    }
                    break;
                default:
                    break;
            }
            if (nodeVM != null)
            {
                nodeVM.IsSelected = true;
            }
           
        }
        public void UpNode()
        {
            if (SelectedItem == null) return;
            if (SelectedItem.NodeObj is ManualConnection)
            {
                int index = ManualConnList.IndexOf(SelectedItem);
                if (index != 0 && ManualConnList.Count() > 1)
                {
                    this.TestPlan.ManualConnectionList.Insert(index-1,(SelectedItem.NodeObj as ManualConnection));
                    ManualConnList.Insert(index - 1, SelectedItem);
                    this.TestPlan.ManualConnectionList.RemoveAt(index+1);
                    ManualConnList.RemoveAt(index + 1);
                }
            }
            else if (SelectedItem.NodeObj is TestStep)
            {
                TreeNodeVM parentNode = SelectedItem.ParentNode;
                int index = parentNode.SubTreeNodeList.IndexOf(SelectedItem);
                if (index != 0 && parentNode.SubTreeNodeList.Count() > 1)
                {   
                    if(parentNode.NodeObj is ManualConnection)
                    {
                        (parentNode.NodeObj as ManualConnection).TestStepList.Insert(index - 1,(SelectedItem.NodeObj as TestStep));
                        (parentNode.NodeObj as ManualConnection).TestStepList.RemoveAt(index + 1);
                    }
                    else if(parentNode.NodeObj is LoopTestStep)
                    {
                        (parentNode.NodeObj as LoopTestStep).Steplist.Insert(index - 1,(SelectedItem.NodeObj as TestStep));
                        (parentNode.NodeObj as LoopTestStep).Steplist.RemoveAt(index + 1);
                    }
                    parentNode.SubTreeNodeList.Insert(index - 1, SelectedItem);
                    parentNode.SubTreeNodeList.RemoveAt(index + 1);
                }
            }
        }
        public void DownNode()
        {
            if (SelectedItem == null) return;
            if(SelectedItem.NodeObj is ManualConnection)
            {
                int index = ManualConnList.IndexOf(SelectedItem);
                if (index != ManualConnList.Count() -1 && ManualConnList.Count() > 1)
                {
                    this.TestPlan.ManualConnectionList.Insert(index + 2, (SelectedItem.NodeObj as ManualConnection));
                    ManualConnList.Insert(index + 2, SelectedItem);
                    this.TestPlan.ManualConnectionList.RemoveAt(index);
                    ManualConnList.RemoveAt(index);
                }
            }
            else if(SelectedItem.NodeObj is TestStep)
            {
                TreeNodeVM parentNode = SelectedItem.ParentNode;
                int index = parentNode.SubTreeNodeList.IndexOf(SelectedItem);
                if (index != parentNode.SubTreeNodeList.Count() - 1 && parentNode.SubTreeNodeList.Count() > 1)
                {
                    if (parentNode.NodeObj is ManualConnection)
                    {
                        (parentNode.NodeObj as ManualConnection).TestStepList.Insert(index + 2, (SelectedItem.NodeObj as TestStep));
                        (parentNode.NodeObj as ManualConnection).TestStepList.RemoveAt(index);
                    }
                    else if (parentNode.NodeObj is LoopTestStep)
                    {
                        (parentNode.NodeObj as LoopTestStep).Steplist.Insert(index + 2, (SelectedItem.NodeObj as TestStep));
                        (parentNode.NodeObj as LoopTestStep).Steplist.RemoveAt(index);
                    }
                    parentNode.SubTreeNodeList.Insert(index + 2, SelectedItem);
                    parentNode.SubTreeNodeList.RemoveAt(index);
                }
            }
        }

        public void CopyNode()
        {
             if(SelectedItem !=null)
             {
                 CopyItem = SelectedItem;
             }
        }
        public void PastNode()
        {    
            if(CopyItem !=null)
            {
                if ((CopyItem as TreeNodeVM).NodeObj as ManualConnection != null)
                {
                    ManualConnection mc = (CopyItem as TreeNodeVM).NodeObj as ManualConnection;
                    byte[] bytes = Interface.SerializerStateModel(mc);
                    object obj = Interface.DeSerializerStateModel(bytes,mc);
                    ManualConnection mcPast = obj as ManualConnection;
                    mcPast.Name = "连接步骤" + ManualConnList.Count();
                    TestPlan.ManualConnectionList.Add(mcPast);
                    ManualConnList.Add(new TreeNodeVM() { NodeObj = TestPlan.ManualConnectionList.Last()});
                }
                else if((CopyItem as TreeNodeVM).NodeObj as TestStep !=null && ((SelectedItem as TreeNodeVM).NodeObj as LoopTestStep !=null || (SelectedItem as TreeNodeVM).NodeObj as ManualConnection !=null))
                {
                    if ((CopyItem as TreeNodeVM).NodeObj as LoopTestStep != null)
                    {
                        LoopTestStep loopTestStep = (CopyItem as TreeNodeVM).NodeObj as LoopTestStep;
                        byte[] bytes = Interface.SerializerStateModel(loopTestStep);
                        object obj = Interface.DeSerializerStateModel(bytes, loopTestStep);
                        LoopTestStep loopTestStepPast = obj as LoopTestStep;
                        NameTestStep(loopTestStepPast);
                        if ((SelectedItem as TreeNodeVM).NodeObj as LoopTestStep != null)
                        {
                            LoopTestStep loopTestStepParent = (SelectedItem as TreeNodeVM).NodeObj as LoopTestStep;
                            loopTestStepParent.ChildTestStepList.Add(loopTestStepPast);
                            (SelectedItem as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = loopTestStepParent.ChildTestStepList.Last(),ParentNode=SelectedItem});
                        }
                        else if ((SelectedItem as TreeNodeVM).NodeObj as ManualConnection != null)
                        {
                            ManualConnection mc = (SelectedItem as TreeNodeVM).NodeObj as ManualConnection;
                            mc.TestStepList.Add(loopTestStepPast);
                            (SelectedItem as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = mc.TestStepList.Last() ,ParentNode=SelectedItem});
                        }
                    }
                    else
                    {
                        TestStep step = (CopyItem as TreeNodeVM).NodeObj as TestStep;
                        byte[] bytes = Interface.SerializerStateModel(step);
                        object obj = Interface.DeSerializerStateModel(bytes, step);
                        TestStep stepPast = obj as TestStep;
                        stepPast.Name = TestStepFactory.getDisplayName(stepPast.GetType().Name) + this.TestPlan.Allcoate(stepPast.GetType().Name);
                        if((SelectedItem as TreeNodeVM).NodeObj as LoopTestStep !=null)
                        {
                            LoopTestStep loopTestStepParent = (SelectedItem as TreeNodeVM).NodeObj as LoopTestStep;
                           // loopTestStepParent.ChildTestStepList.Add(loopTestStepParent);
                          //  (SelectedItem as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() {NodeObj=loopTestStepParent.ChildTestStepList.Last(),ParentNode=SelectedItem});
                            loopTestStepParent.ChildTestStepList.Add(stepPast);
                            (SelectedItem as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() {NodeObj=loopTestStepParent.ChildTestStepList.Last(),ParentNode=SelectedItem });
                        }
                        else if((SelectedItem as TreeNodeVM).NodeObj as ManualConnection !=null)
                        {
                            ManualConnection mc = (SelectedItem as TreeNodeVM).NodeObj as ManualConnection;
                            mc.TestStepList.Add(stepPast);
                            (SelectedItem as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() { NodeObj=mc.TestStepList.Last(),ParentNode=SelectedItem});
                        }
                    }
                }
            }
        }
        public void PastePathNode()
        {
            if ((CopyItem as TreeNodeVM).NodeObj as TestStep != null && ((SelectedItem as TreeNodeVM).NodeObj as LoopTestStep != null || (SelectedItem as TreeNodeVM).NodeObj as ManualConnection != null))
            { 
                TestStep step = (CopyItem as TreeNodeVM).NodeObj as TestStep;
                if (step.PathConfigNameList == null) return;
                foreach(var item in ConfigPathList)
                {   
                    if(item.IsSelected == false)continue;
                    byte[] bytes = Interface.SerializerStateModel(step);
                    object obj = Interface.DeSerializerStateModel(bytes, step);
                    TestStep stepPast = obj as TestStep;
                    stepPast.Name = TestStepFactory.getDisplayName(stepPast.GetType().Name) + this.TestPlan.Allcoate(stepPast.GetType().Name);
                    stepPast.PathConfigName = item.PathStr;
                    if ((SelectedItem as TreeNodeVM).NodeObj as LoopTestStep != null)
                    {
                        LoopTestStep loopTestStepParent = (SelectedItem as TreeNodeVM).NodeObj as LoopTestStep;
                        loopTestStepParent.ChildTestStepList.Add(stepPast);
                        (SelectedItem as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = loopTestStepParent.ChildTestStepList.Last(), ParentNode = SelectedItem });
                    }
                    else if ((SelectedItem as TreeNodeVM).NodeObj as ManualConnection != null)
                    {
                        ManualConnection mc = (SelectedItem as TreeNodeVM).NodeObj as ManualConnection;
                        mc.TestStepList.Add(stepPast);
                        (SelectedItem as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = mc.TestStepList.Last(), ParentNode = SelectedItem });
                    }
                }
            }
        }
        public void AddTestSpecs(string str)
        {             
            TestPlan.AddTestSpecs(str);
        }
        public void DeletSpecs(TotalSpecVM str)
        {   
            TestPlan.DeletSpecs(str);
            TestSpecs.Remove(str);
        }
        private ObservableCollection<string> _TestEnvironm=new ObservableCollection<string>();
        private const string TestEnvironmPropertyName = "TestEnvironm";
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ObservableCollection<string> TestEnvironm
        {
            get
            {
                return TestPlan.TestEnvironm;
            }
            set
            {
                TestPlan.TestEnvironm = value;
                NotifyPropertyChanged(TestEnvironmPropertyName);
            }
        }
        private ObservableCollection<TotalSpecVM> _TestSpecs = new ObservableCollection<TotalSpecVM>();
        private const string TestSpecsPropertyName = "TestSpecs";
        [Newtonsoft.Json.JsonIgnore]
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<TotalSpecVM> TestSpecs
        {
            get
            {
                return TestPlan.TestSpecs;
            }
            set
            {
                TestPlan.TestSpecs = value;
                NotifyPropertyChanged(TestSpecsPropertyName);
            }
        }
        private ObservableCollection<TestStepConfigPath> _ConfigPathList = new ObservableCollection<TestStepConfigPath>();
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<TestStepConfigPath> ConfigPathList
        {
            get
            {
                return _ConfigPathList;
            }
            set
            {
                ConfigPathList = value;
            }
        }
        private ObservableCollection<TestDescription> _TestDescriptionList = new ObservableCollection<TestDescription>();
        private const string TestDescriptionListPropertyName = "TestDescriptionList";
        [System.Xml.Serialization.XmlIgnore]
        public ObservableCollection<TestDescription> TestDescriptionList
        {
            get
            {
                return TestPlan.TestDescriptionList;
            }
            set
            {
                TestPlan.TestDescriptionList = value;
                NotifyPropertyChanged(TestDescriptionListPropertyName);
            }
        }

        //public ObservableCollection<ObservableCollection<XYMarkerDisp>> XYMarkerDispList { get; set; }
        //private  XYMarkerDisp _SelectedXYMarkerDisp=new XYMarkerDisp();
        //private const string SelectedXYMarkerDispPropertyName = "SelectedXYMarkerDisp";
        //public XYMarkerDisp SelectedXYMarkerDisp
        //{
        //    get
        //    {
        //        return _SelectedXYMarkerDisp;
        //    }
        //    set
        //    {
        //        _SelectedXYMarkerDisp = value;
        //        NotifyPropertyChanged(SelectedItemPropertyName);
        //    }
        //}

        public ObservableCollection<CommandViewModel> AddNodeCmdList { get; set; }
        private List<CommandViewModel> _AddTestStepCmdList;
        public List<CommandViewModel> AddTestStepCmdList
        {
            get
            {
                if (_AddTestStepCmdList == null)
                {
                    _AddTestStepCmdList = new List<CommandViewModel>();
                    foreach (string n in TestStepFactory.GetTestStepDisplayNameList())
                    {
                        _AddTestStepCmdList.Add(new CommandViewModel(n, new DelegateCommand<string>(AddTestStep)));
                    }
                }
                return _AddTestStepCmdList;
            }
            
        }

        private void AddTestStep(string stepDisplayTypeName)
        {
            TestStep step = TestStepFactory.CreateTestStep(stepDisplayTypeName);
            
            if (SelectedItem != null)
            {
                ManualConnection mannConn = SelectedItem.NodeObj as ManualConnection;
                ParentTestStep parentStep = SelectedItem.NodeObj as ParentTestStep;
                if (step.GetType() == typeof(LoopTestStep) || step.GetType()==typeof(ManualLoopTestStep))
                {
                    step.Name = TestStepFactory.getDisplayName(step.GetType().Name);
                }
                else
                {
                    step.Name = TestStepFactory.getDisplayName(step.GetType().Name) + this.TestPlan.Allcoate(step.GetType().Name);
                }
                if (mannConn != null)
                {                   
                    mannConn.TestStepList.Add(step);
                    TreeNodeVM nodeVM = new TreeNodeVM() { NodeObj = step,ParentNode=SelectedItem};
                    SelectedItem.AddSubNode(nodeVM);
                    var stepInfo=TestStepInfoMgr.GetStepInfoByDisplayName(stepDisplayTypeName);
                    if(stepInfo!=null)
                    {
                        if (stepInfo.IsFixedItem)
                        {
                            if (stepInfo.TestTraceTypeList != null)
                            {
                                foreach (var tr in stepInfo.TestTraceTypeList)
                                {
                                    AddTestItem(nodeVM, tr);
                                }
                            }
                        }
                    }
                }
                else if(parentStep !=null)
                {
                    parentStep.ChildTestStepList.Add(step);
                    TreeNodeVM nodeVM = new TreeNodeVM() { NodeObj = step,ParentNode=SelectedItem};
                    SelectedItem.AddSubNode(nodeVM);
                    var stepInfo = TestStepInfoMgr.GetStepInfoByDisplayName(stepDisplayTypeName);
                    if (stepInfo != null)
                    {
                        if (stepInfo.IsFixedItem)
                        {
                            if (stepInfo.TestTraceTypeList != null)
                            {
                                foreach (var tr in stepInfo.TestTraceTypeList)
                                {
                                    AddTestItem(nodeVM, tr);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void NameTestStep(TestStep step)
        {
            if (step.GetType() == typeof(LoopTestStep))
            {
                step.Name = TestStepFactory.getDisplayName(step.GetType().Name);
                foreach(var item in (step as LoopTestStep).ChildTestStepList)
                {
                    NameTestStep(item);
                }
            }
            else
            {
                step.Name = TestStepFactory.getDisplayName(step.GetType().Name) + TestPlan.Allcoate(step.GetType().Name);
            }
        }

        private List<CommandViewModel> _AddTestItemCmdList;
        public List<CommandViewModel> AddTestItemCmdList
        {
            get
            {
                _AddTestItemCmdList = new List<CommandViewModel>();
                if (SelectedItem != null&&SelectedItem.Type==TreeNodeTypeEnum.TestStep)
                {
                    //TestStepInfo stepInfo = null;
                    TestStep TestStep = SelectedItem.NodeObj as TestStep;
                    //if (TestStep != null)
                    //{
                    //    stepInfo = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == TestStep.GetType().Name).FirstOrDefault();
                    //}
                    //string[] traceTypeList = null;
                    //if (stepInfo != null)
                    //{
                    //    traceTypeList = stepInfo.TestTraceTypeList;
                    //}
                    //if (traceTypeList == null&&TestStep.ItemTypeNameList!=null)
                    //{
                    //    foreach (var name in TestStep.ItemTypeNameList)
                    //    {
                    //        _AddTestItemCmdList.Add(new CommandViewModel(name, new DelegateCommand<string>(AddTestItemCmd)));
                    //    }
                    //}
                    //else if(traceTypeList !=null)
                    //{
                    //    foreach (string n in traceTypeList)
                    //    {
                    //        _AddTestItemCmdList.Add(new CommandViewModel(n, new DelegateCommand<string>(AddTestItemCmd)));
                    //    }
                    //}
                    if (TestStep.ItemTypeNameList != null)
                    {
                        foreach (var name in TestStep.ItemTypeNameList)
                        {
                            _AddTestItemCmdList.Add(new CommandViewModel(name, new DelegateCommand<string>(AddTestItemCmd)));
                        }
                    }
                }
                return _AddTestItemCmdList;
            }

        }

        private void AddTestItemCmd(string traceTypeName)
        {
            if (!TestStepInfoMgr.GetStepInfo(SelectedItem.NodeObj.GetType().Name).IsFixedItem)
            {
                AddTestItem(SelectedItem, traceTypeName);
            }
            
        }
        public void AddTestItem(TreeNodeVM testStepNode, string traceTypeName)
        {
            if (testStepNode != null)
            {
                TestStep TestStep = testStepNode.NodeObj as TestStep;


                if (TestStep != null)
                {
                    int trCount = TestStep.ItemList.Count;
                    TestStep.CreateTrace(traceTypeName);
                    if (TestStep.ItemList.Count > trCount)
                    {
                        var tr = TestStep.ItemList.Last();
                        tr.UserDefName = tr.TypeName;
                        if (tr is TestTrace)
                        {
                            foreach (var spec in this.TestSpecs)
                            {
                                (tr as TestTrace).TestSpecList.Add(new TestTraceSpec());
                            }
                        }
                        if (tr is PointTestItem)
                        {
                            foreach (var spec in this.TestSpecs)
                            {
                                (tr as PointTestItem).TestSpecList.Add(new PointTestSpec());
                            }
                        }
                        if (tr is TRTestItem)
                        {
                            foreach (var spec in TestSpecs)
                            {
                                (tr as TRTestItem).TestSpecList.Add(new TRTestItemSpec());
                            }
                        }
                        testStepNode.AddSubNode(new TreeNodeVM() { NodeObj = tr, ParentNode = testStepNode, Index = TestStep.ItemList.IndexOf(tr) });
                    }
                }

            }
        }
        /// <summary>
        /// 当测试方案被加载的时候
        /// </summary>
        public void ApplyTestPlan()
        {
            //foreach(ObservableCollection<XYMarkerDisp> xyMarkerdisp in XYMarkerDispList)
            //{
            //    xyMarkerdisp.Clear();
            //}
            TestPlanManager.CurrentTestPlan = this.TestPlan;
            try
            {
                Interface.LoadLocalSettings(this.TestPlan);
                TestPlan.ApplyTestPlan();
            }
            catch(Exception exp)
            {
                System.Windows.MessageBox.Show("无法加载方案\n"+exp.Message+Environment.NewLine+exp.Source+Environment.NewLine+exp.InnerException);
            }
        }

        public void Refresh(int i)
        {
            TestStep ts = TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[i];
            if(ts as LoopTestStep !=null)return;
            for (int k = 0; k < ts.ItemList.Count; k++)
            {
                 if(ts.ItemList[k] as TestTrace !=null)
                 {  
                    TestTrace tc = ts.ItemList[k] as TestTrace;
                    for (int j = 0; j < tc.TestSpecList.Count; j++)
                    {
                        TestTraceSpec tp = tc.TestSpecList[j];
                        for (int p = 0; p < tp.TestMarkerList.Count; p++)
                        {
                            TestMarker tm = tp.TestMarkerList[p];
                            ManualConnList[TestPlan.currentConnIndex].SubTreeNodeList[i].SubTreeNodeList[k].SubTreeNodeList[p].PassFail = tm.PassFail;
                        }

                    }
                    ManualConnList[TestPlan.currentConnIndex].SubTreeNodeList[i].SubTreeNodeList[k].PassFail = tc.PassFail;
                 }
            }
            ManualConnList[TestPlan.currentConnIndex].SubTreeNodeList[i].PassFail = ts.PassFail;
            ManualConnection mc = TestPlan.ManualConnectionList[TestPlan.currentConnIndex];
            ManualConnList[TestPlan.currentConnIndex].PassFail = mc.PassFail;
            RefreshSpec();
        }
        public void RefreshConfigPath(TestStep step)
        {   
            ConfigPathList.Clear();
            if (step.PathConfigNameList == null) return;
            foreach(string str in step.PathConfigNameList)
            {
                ConfigPathList.Add(new TestStepConfigPath() { PathStr = str, IsSelected = false});
            }
        }
        
        public void InitialData()
        {   
            foreach(var mcNode in this.ManualConnList)
            {
                mcNode.PassFail = null;
                foreach(var stepNode in mcNode.SubTreeNodeList)
                {
                    stepNode.PassFail = null;
                    foreach(var traceNode in stepNode.SubTreeNodeList)
                    {
                        traceNode.PassFail = null;
                        foreach(var markerNode in traceNode.SubTreeNodeList)
                        {
                            markerNode.PassFail = null;
                        }
                    }
                }
            }
            
            foreach (TotalSpecVM spec in TestSpecs)
            {
                spec.PassFail = null;
            }

            foreach (ManualConnection mc in TestPlan.ManualConnectionList)
            {
                mc.PassFail = null;
                foreach (TestStep ts in mc.TestStepList)
                {
                    foreach(var realStep in Interface.GetAllSubTestStep(ts))
                    {
                        realStep.PassFail = null;
                        realStep.IsFinish = false;
                        foreach (TestTrace tc in ts.ItemList.Where(x=>x is TestTrace))
                        {
                            tc.PassFail = null;
                            foreach (TestTraceSpec tp in tc.TestSpecList)
                            {
                                tp.PassFail = null;
                                foreach (TestMarker tm in tp.TestMarkerList)
                                {
                                    tm.PassFail = null;
                                }
                            }
                        }
                        if (realStep as FormulaCalcTestStep != null)
                        {
                            FormulaCalcTestStep forCalcStep = realStep as FormulaCalcTestStep;
                            forCalcStep.UpdateCalTestItem = UpdateCalTestItem;
                            forCalcStep.Cal = Cal;
                        }
                        else
                        {

                        }
                    }
                }
            }
            //foreach (var item in XYMarkerDispList)
            //{
            //    item.Clear();
            //}
        }
        public void RefreshSpec()
        {
            for (int i = 0; i < TestSpecs.Count; i++)
            {
                List<bool?> boollist = new List<bool?>();
                foreach (ManualConnection mc in TestPlan.ManualConnectionList)
                {
                    foreach (TestStep ts in mc.TestStepList)
                    {   
                        if(ts as LoopTestStep !=null)
                        {
                            continue;
                        }
                        foreach (var tc in ts.ItemList)
                        {
                            if (tc is TestTrace)
                            {
                                bool? boolSpec = (tc as TestTrace).TestSpecList[i].PassFail;
                                boollist.Add(boolSpec);
                            }
                            else if(tc is TRTestItem)
                            {
                                bool? boolSpec = (tc as TRTestItem).TestSpecList[i].PassFail;
                                boollist.Add(boolSpec);
                            }
                            else if (tc is PointTestItem)
                            {
                                bool? boolSpec = (tc as PointTestItem).TestSpecList[i].PassFail;
                                boollist.Add(boolSpec);
                            }
                        }
                    }
                }
                TestSpecs[i].PassFail = Symtant.GeneFunLib.GeneFun.NullBoolAndList(boollist);
            }
        }
      
        public void UpdateTreeViewAfterDrop(object objContainer,object objSelectedItem,bool isCopy)
        {    
            if(objContainer as TreeNodeVM==null)return;
            //连接步骤移动到连接步骤
            if ((objContainer as TreeNodeVM).NodeObj as ManualConnection != null && (objSelectedItem as TreeNodeVM).NodeObj as ManualConnection !=null)
            {   
                ManualConnection targetMc = (objContainer as TreeNodeVM).NodeObj as ManualConnection;
                object obj = ViewModelBaseLib.Interface.CopyTestPlanVM(((objSelectedItem as TreeNodeVM).NodeObj as ManualConnection));
                if (obj as ManualConnection == null) return;
                ManualConnection copyMc = obj as ManualConnection;
                int index = this.ManualConnList.IndexOf(objContainer as TreeNodeVM);
                TestPlan.ManualConnectionList.Insert(index,copyMc);
                ManualConnList.Insert(index, new TreeNodeVM() {NodeObj=copyMc});
                if (isCopy == false)
                {
                    this.TestPlan.ManualConnectionList.Remove((objSelectedItem as TreeNodeVM).NodeObj as ManualConnection);
                    this.ManualConnList.Remove(objSelectedItem as TreeNodeVM);
                }
                else
                {
                    foreach (var step in copyMc.TestStepList)
                    {
                        NameTestStep(step);
                    }
                }
            }    
            //测试步骤移动到连接步骤
            else if((objContainer as TreeNodeVM).NodeObj as ManualConnection !=null && (objSelectedItem as TreeNodeVM).NodeObj as TestStep !=null)
            {
                ManualConnection targetMc = (objContainer as TreeNodeVM).NodeObj as ManualConnection;
                object obj = ViewModelBaseLib.Interface.CopyTestPlanVM((objSelectedItem as TreeNodeVM).NodeObj as TestStep);
                if (obj as TestStep== null)return;
                TestStep copyStep = obj as TestStep;
                targetMc.TestStepList.Add(copyStep);
                (objContainer as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = copyStep, ParentNode = objContainer as TreeNodeVM });
                if (isCopy == true)
                {
                    NameTestStep(copyStep);
                }
                else
                {
                    LoopTestStep loopTestStep = (objSelectedItem as TreeNodeVM).ParentNode.NodeObj as LoopTestStep;
                    ManualConnection muConn = (objSelectedItem as TreeNodeVM).ParentNode.NodeObj as ManualConnection;
                    if(loopTestStep !=null)
                    {
                        loopTestStep.ChildTestStepList.Remove((objSelectedItem as TreeNodeVM).NodeObj as TestStep);
                    }
                    if(muConn !=null)
                    {
                        muConn.TestStepList.Remove((objSelectedItem as TreeNodeVM).NodeObj as TestStep);
                    }
                    (objSelectedItem as TreeNodeVM).ParentNode.SubTreeNodeList.Remove(objSelectedItem as TreeNodeVM);
                }
            }            
            //测试步骤移动到测试步骤
            else if((objContainer as TreeNodeVM).NodeObj as TestStep !=null && (objSelectedItem as TreeNodeVM).NodeObj as TestStep !=null)
            {
                TestStep targetStep = (objContainer as TreeNodeVM).NodeObj as TestStep;
                object obj = ViewModelBaseLib.Interface.CopyTestPlanVM((objSelectedItem as TreeNodeVM).NodeObj as TestStep);
                if(obj==null)return;
                TestStep copyStep = obj as TestStep;
                if (targetStep as LoopTestStep != null)
                {
                    LoopTestStep loopTestStep = targetStep as LoopTestStep;
                    loopTestStep.ChildTestStepList.Add(copyStep);
                    (objContainer as TreeNodeVM).SubTreeNodeList.Add(new TreeNodeVM() { NodeObj=copyStep,ParentNode=objContainer as TreeNodeVM});
                }
                else
                { 
                     TestStep step = targetStep as TestStep;
                     int index = (objContainer as TreeNodeVM).ParentNode.SubTreeNodeList.IndexOf(objContainer as TreeNodeVM);
                     if ((objContainer as TreeNodeVM).ParentNode.NodeObj as LoopTestStep != null)
                     {
                         LoopTestStep loopTestStep = (objContainer as TreeNodeVM).ParentNode.NodeObj as LoopTestStep;
                         loopTestStep.ChildTestStepList.Insert(index, copyStep);
                     }
                     else if ((objContainer as TreeNodeVM).ParentNode.NodeObj as ManualConnection != null)
                     {
                         ManualConnection muConn=(objContainer as TreeNodeVM).ParentNode.NodeObj as ManualConnection;
                         muConn.TestStepList.Insert(index,copyStep);
                     }
                     (objContainer as TreeNodeVM).ParentNode.SubTreeNodeList.Insert(index, new TreeNodeVM() {NodeObj=copyStep,ParentNode=(objContainer as TreeNodeVM).ParentNode });
                }
                if (isCopy == true)
                {
                    NameTestStep(copyStep);
                }
                else
                {
                    LoopTestStep loopTestStep = (objSelectedItem as TreeNodeVM).ParentNode.NodeObj as LoopTestStep;
                    ManualConnection muConn = (objSelectedItem as TreeNodeVM).ParentNode.NodeObj as ManualConnection;
                    if (loopTestStep != null)
                    {
                        loopTestStep.ChildTestStepList.Remove((objSelectedItem as TreeNodeVM).NodeObj as TestStep);
                    }
                    if (muConn != null)
                    {
                        muConn.TestStepList.Remove((objSelectedItem as TreeNodeVM).NodeObj as TestStep);
                    }
                    (objSelectedItem as TreeNodeVM).ParentNode.SubTreeNodeList.Remove(objSelectedItem as TreeNodeVM);
                }
            }       
        }
        public DataTable getSelectedTraceResult(TreeNodeVM treeNodeVM)
        {
            DataTable result = new DataTable();
            if (treeNodeVM==null || treeNodeVM.NodeObj as TestItem == null) return result;
            if (treeNodeVM.ParentNode.ParentNode.NodeObj as ManualConnection != null) return result; 
            SelectedTreeNode = null;
            FindParentNode(treeNodeVM);
            if (SelectedTreeNode == null) return null;
            int index = treeNodeVM.ParentNode.SubTreeNodeList.IndexOf(treeNodeVM);
            foreach(LoopTestItem item in (SelectedTreeNode.NodeObj as LoopTestStep).ItemList)
            {
                if(item.ItemIndex==index && item.TestStepName==(treeNodeVM.ParentNode.NodeObj as TestStep).Name)
                {
                    result = item.ItemResult;
                    break;
                }
            }

            return result;
        }
        [System.Xml.Serialization.XmlIgnore]
        public TreeNodeVM SelectedTreeNode { get; set; }
        private void FindParentNode(TreeNodeVM node)
        {
            if ((node.ParentNode.NodeObj as ManualConnection) != null && node.NodeObj as LoopTestStep != null)
            {
                SelectedTreeNode = node;
            }
            else
            {
                FindParentNode(node.ParentNode);
            }
        }
        private Calculation _Cal;
        [System.Xml.Serialization.XmlIgnore]
        public Calculation Cal
        {
            get
            {
                return _Cal;
            }
            set
            {
                _Cal = value;
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public List<string> Funclist
        {
            get
            {
                List<string> funs = new List<string>() { "ABS", "SUM", "MAX", "MIN", "SIN", "AVERAGE", "RANDBETWEEN", "SIN", "SQRT", "TAN", 
                    "COS", "EXP", "STDEV","LOG10","PI","RAND","POW"};
                return funs;
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public List<string> Paramlist
        {
            get
            {
                List<string> items = new List<string>();
                Random ran = new Random();
                foreach(ManualConnection mc in TestPlan.ManualConnectionList)
                {
                    foreach(TestStep step in mc.TestStepList)
                    {
                        foreach(var realStep in Interface.GetAllSubTestStep(step))
                        {
                            foreach (TestItem item in realStep.ItemList)
                            {
                                if (item as TestTrace != null)
                                {
                                    TestTrace trace = item as TestTrace; 
                                    if(trace.VarName !=null)
                                    {
                                       items.Add(trace.VarName);
                                       double[] data = new double[10];
                                       for (int j = 0; j < 10; j++)
                                       {
                                           data[j] = ran.NextDouble();
                                       }
                                       Cal.paramDictionary[trace.VarName] = data;                                   
                                    }
                                    foreach (XYTestMarker marker in trace.TestSpecList[SelectedSpecIndex].TestMarkerList)
                                    {
                                        if (marker.VarName != null)
                                        {
                                            items.Add(marker.VarName);
                                            Cal.paramDictionary[marker.VarName] = ran.NextDouble();
                                        }
                                    }
                                }
                                if(item as PointTestItem !=null)
                                {
                                    PointTestItem pointItem = item as PointTestItem;
                                    if(pointItem.VarName !=null)
                                    {
                                        items.Add(pointItem.VarName);
                                        Cal.paramDictionary[pointItem.VarName] = ran.NextDouble();
                                    }
                                }
                            }
                        }
                    }
                }
                return items;
            }
        }
        private DelegateCommand<CommandPara> _PreCalCommand;
        [System.Xml.Serialization.XmlIgnore]
        public ICommand PreCalcCommand
        {
            get
            {
                if (_PreCalCommand == null)
                {
                    _PreCalCommand = new DelegateCommand<CommandPara>(PreCalc);
                }
                return _PreCalCommand;
            }
        }
        public double[] UpdateCalTestItem()
        {
            double[] x = null ;
            double[] x1 = null;
            Cal.paramDictionary.Clear();
            foreach(ManualConnection mc in TestPlan.ManualConnectionList)
            {
                foreach(TestStep step in mc.TestStepList)
                {
                    foreach(var realStep in Interface.GetAllSubTestStep(step))
                    {
                        foreach(TestItem item in realStep.ItemList)
                        {
                            if(item as TestTrace !=null)
                            {
                                TestTrace trace = item as TestTrace;
                                if(trace.ResultData.Count > 0 && trace.VarName !=null)
                                {
                                    double[] data=new double[trace.ResultData.Count()];
                                    for (int i = 0; i < trace.ResultData.Count(); i++)
                                    {
                                        data[i]=trace.ResultData[i].Y;
                                    }
                                    Cal.paramDictionary[trace.VarName] = data;
                                    if (x == null)
                                    {
                                        x = trace.ResultData.Select(m => m.X).ToArray();
                                    }                                 
                                }
                                foreach (XYTestMarker marker in trace.TestSpecList[SelectedSpecIndex].TestMarkerList)
                                {
                                    if (marker.MarkerResult.Count > 0 && marker.VarName != null)
                                    {
                                        Cal.paramDictionary[marker.VarName] = marker.MarkerResult[0].Y;
                                        x1 = new double[] {marker.MarkerResult[0].X };
                                    }
                                }
                            }
                            if(item as PointTestItem !=null)
                            {
                                PointTestItem pointItem = item as PointTestItem;
                                if(pointItem.VarName !=null && pointItem.Y !=null)
                                {
                                    Cal.paramDictionary[pointItem.VarName] = pointItem.Y;
                                    x1 = new double[] { pointItem.X };
                                }
                            }
                        }
                    }
                }
            }
            if (x == null)
            {
                return x1;
            }
            else
            {
                return x;
            }
        }
        [System.Xml.Serialization.XmlIgnore]
        public CommandPara param { get; set; }
        public void PreCalc(CommandPara param)
        {
            try
            {
                object result = Cal.Calucate(param.Formula);
                if (result.GetType() == typeof(double[]))
                {
                    CalcPointTestTrace trace = new CalcPointTestTrace() { Formula = param.Formula};
                    trace.TypeName = param.TypeName;
                    foreach (var spec in TestSpecs)
                    {
                        trace.TestSpecList.Add(new TestTraceSpec());
                    }
                    if((SelectedItem.NodeObj as FormulaCalcTestStep !=null))
                    {
                        (SelectedItem.NodeObj as FormulaCalcTestStep).ItemList.Add(trace);
                        SelectedItem.SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = trace, ParentNode = SelectedItem });
                    }
                }
                else if (result.GetType() == typeof(double))
                {
                    CalcPointTestItem pointItem = new CalcPointTestItem() { Formula=param.Formula};
                    pointItem.TypeName = param.TypeName;
                    foreach (var spec in TestSpecs)
                    {
                        pointItem.TestSpecList.Add(new PointTestSpec());
                    }
                    if((SelectedItem.NodeObj as FormulaCalcTestStep !=null))
                    {
                        (SelectedItem.NodeObj as FormulaCalcTestStep).ItemList.Add(pointItem);
                        SelectedItem.SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = pointItem, ParentNode = SelectedItem });
                    }
                }

            }
            catch (Exception ex)
            {
                DataUtils.LOGINFO.WriteError("计算公式错误信息：" + ex.Message);
            }
        }
    }
    public class CommandPara
    {
        public CommandPara()
        {
            TypeName = "计算公式";
        }
        public string TypeName { get; set; }
        public string Formula { get; set; }
    }
    public class TestStepConfigPath:NotifyBase
    {
        private string _PathStr;
        private const string PathStrPropertyName = "PathStr";
        public string PathStr
        {
            get
            {
                return _PathStr;
            }
            set
            {
                _PathStr = value;
                NotifyPropertyChanged(PathStrPropertyName);
            }
        }
        private bool _IsSelected;
        private const string IsSelectedPropertyName = "IsSelected";
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                NotifyPropertyChanged(IsSelectedPropertyName);
            }
        }
    }


}
