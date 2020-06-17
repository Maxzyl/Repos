using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
namespace ViewModelBaseLib
{
    //public class ManualConnectionNode:NotifyBase
    //{
    //    public ManualConnectionNode()
    //    {
    //        ManualConn = new ManualConnection();
    //        InitTestStepList();
    //        ManualConnectionVM.Action += Notify;
    //    }

    //    private ManualConnectionVM _ManualConnectionVM=new ManualConnectionVM();
    //    private const string ManualConnectionVMPropertyName = "ManualConnectionVM";
    //    [System.Xml.Serialization.XmlIgnore]
    //    [Newtonsoft.Json.JsonIgnore]
    //    public ManualConnectionVM ManualConnectionVM
    //    {
    //        get
    //        {
    //            return _ManualConnectionVM;
    //        }
    //        set
    //        {
    //            _ManualConnectionVM = value;
    //            NotifyPropertyChanged(ManualConnectionVMPropertyName);
    //        }
    //    }
        
    //    private bool _IsSelected;
    //    private const string IsSelectedPropertyName = "IsSelected";
    //    public bool IsSelected
    //    {
    //        get
    //        {
    //            return _IsSelected;
    //        }
    //        set
    //        {
    //            _IsSelected = value;
    //            NotifyPropertyChanged(IsSelectedPropertyName);
    //        }
    //    }
    //    private bool _IsExpanded;
    //    private const string IsExpandedPropertyName = "IsExpanded";
    //    public bool IsExpanded
    //    {
    //        get
    //        {
    //            return _IsExpanded;
    //        }
    //        set
    //        {
    //            _IsExpanded = value;
    //            NotifyPropertyChanged(IsExpandedPropertyName);
    //        }
    //    }
        
    //    private ManualConnection _ManualConn;
    //    private const string ManualConnPropertyName = "ManualConn";
    //    public ManualConnection ManualConn
    //    {
    //        get
    //        {
    //            return _ManualConn;
    //        }
    //        set
    //        {
    //            _ManualConn = value;
    //            foreach (var step in _ManualConn.TestStepList)
    //            {
    //                object obj = GetStepVM(step.GetType());
    //                TestStepList.Add(new TestStepNode() { TestStep = step, DisplayName = GetStepTypeName(step.GetType()),ObjVM=obj });
    //            }
    //            NotifyPropertyChanged(ManualConnPropertyName);
    //        }
    //    }
        
    //    private string _DisplayName;
    //    private const string DisplayNamePropertyName = "DisplayName";
    //    public string DisplayName
    //    {
    //        get
    //        {
    //            return ManualConn.Name;
    //        }
    //        set
    //        {
    //             ManualConn.Name = value;
    //             NotifyPropertyChanged(DisplayNamePropertyName);
    //        }
    //    }
    //    private void Notify()
    //    { 
    //        NotifyPropertyChanged(DisplayNamePropertyName);
    //    }
    //    private bool ? _PassFail;
    //    private const string PassFailPropertyName = "PassFail";
    //    public bool ? PassFail
    //    {
    //        get
    //        {
    //            return _PassFail;
    //        }
    //        set
    //        {
    //            _PassFail = value;
    //            NotifyPropertyChanged(PassFailPropertyName);
    //        }
    //    }
        
    //    private bool _IsTest;
    //    private const string IsTestPropertyName = "IsTest";
    //    public bool IsTest
    //    {
    //        get
    //        {
    //            return ManualConn.IsTest;
    //        }
    //        set
    //        {
    //            ManualConn.IsTest = value;
    //            NotifyPropertyChanged(IsTestPropertyName);
    //        }
    //    }
    //    private ObservableCollection<TestStepNode> _TestStepList=new ObservableCollection<TestStepNode>();
    //    private const string TestStepListPropertyName = "TestStepList";
    //    public ObservableCollection<TestStepNode> TestStepList
    //    {
    //        get
    //        {
    //            return _TestStepList;
    //        }
    //        set
    //        {
    //            _TestStepList = value;
    //            NotifyPropertyChanged(TestStepListPropertyName);
    //        }
    //    }
        

    //    [System.Xml.Serialization.XmlIgnore]
    //    [Newtonsoft.Json.JsonIgnore]
    //    public ObservableCollection<CommandViewModel> AddTestStepCmdList { get; set; }
    //    private void InitTestStepList()
    //    {
    //        AddTestStepCmdList = new ObservableCollection<CommandViewModel>();
    //        //AddTestStepCmdList.Add(new CommandViewModel(TestStepType.PIM, new DelegateCommand<string>(AddTestStep)));
    //        //AddTestStepCmdList.Add(new CommandViewModel(TestStepType.SParam, new DelegateCommand<string>(AddTestStep)));           
    //        foreach (string n in TestStepFactory.GetTestStepDisplayNameList())
    //        {
    //            AddTestStepCmdList.Add(new CommandViewModel(n, new DelegateCommand<string>(AddTestStep)));
    //        }
    //    }
    //    public void AddTestStep(string stepDisplayTypeName)
    //    {
    //        //TestStep step = null;
    //        //switch (stepTypeName)
    //        //{
    //        //    case TestStepType.PIM:
    //        //        step = new PIMTestStep();
    //        //        break;
    //        //    case TestStepType.SParam:
    //        //        step = new SParamTestStep();
    //        //        break;
    //        //    default:
    //        //        break;
    //        //}
    //        TestStep step = TestStepFactory.CreateTestStep(stepDisplayTypeName);
    //        ManualConn.TestStepList.Add(step);
    //        object obj = GetStepVM(step.GetType());
    //        TestStepList.Add(new TestStepNode() { TestStep = step,DisplayName=stepDisplayTypeName ,ObjVM=obj});          
    //    }
    //    public object GetStepVM(Type stepType)
    //    {
    //        if (stepType.Name == typeof(PIMTestStep).Name)
    //        {
    //            return new PIMTestStepVM();
    //        }
    //        if (stepType.Name == typeof(NFTestStep).Name)
    //        {
    //            return new NFTestStepVM();
    //        }
    //        if (stepType.Name == typeof(IMDTestStep).Name)
    //        {
    //            return new IMDTestStepVM();
    //        }
    //        if(stepType.Name==typeof(DCPSTestStep).Name)
    //        {
    //            return new DCPSTestStepVM();
    //        }
    //        if(stepType.Name==typeof(DUTTestStep).Name)
    //        {
    //            return new DUTTestStepVM();
    //        }
    //        return null;
    //    }

    //    public string GetStepTypeName(Type stepType)
    //    {
    //        //if (stepType.Name == typeof(PIMTestStep).Name)
    //        //{
    //        //    return TestStepType.PIM;
    //        //}
    //        //if (stepType.Name == typeof(SParamTestStep).Name)
    //        //{
    //        //    return TestStepType.SParam;
    //        //}
    //        //if(stepType.Name==typeof(NFTestStep).Name)
    //        //{
    //        //    return TestStepType.NF;
    //        //}
    //        //if(stepType.Name==typeof(IMDTestStep).Name)
    //        //{
    //        //    return TestStepType.IP3;
    //        //}
    //        //if(stepType.Name==typeof(DCPSTestStep).Name)
    //        //{
    //        //    return TestStepType.DCPS;
    //        //}
    //        //if(stepType.Name==typeof(DUTTestStep).Name)
    //        //{
    //        //    return TestStepType.DUT;
    //        //}
    //        //return null;
    //        string typeName = stepType.Name;
    //        var stepInfo = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == typeName).FirstOrDefault();
    //        if (stepInfo != null)
    //        {
    //            return stepInfo.DisplayName;
    //        }
    //        else
    //            return null;
    //    }
    //    public void RemoveTestStep(TestStepNode stepNode)
    //    {
    //        ManualConn.TestStepList.Remove(stepNode.TestStep);
    //        TestStepList.Remove(stepNode);
    //    }

    //}
}
