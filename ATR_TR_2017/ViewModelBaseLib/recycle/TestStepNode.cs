using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;

namespace ViewModelBaseLib
{
    //public class TestStepNode:NotifyBase
    //{
    //    private object _ObjVM;
    //    private const string  ObjVMPropertyName = "ObjVM";
    //    [System.Xml.Serialization.XmlIgnore]
    //    [Newtonsoft.Json.JsonIgnore]
    //    public object ObjVM
    //    {
    //        get
    //        {
    //            return _ObjVM;
    //        }
    //        set
    //        {
    //            _ObjVM = value;
    //            NotifyPropertyChanged(ObjVMPropertyName);
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
        
    //    private TestStep _TestStep;
    //    private const string TestStepPropertyName = "TestStep";
    //    public TestStep TestStep
    //    {
    //        get
    //        {
    //            return _TestStep;
    //        }
    //        set
    //        {
    //            _TestStep = value;
    //            foreach (var tr in _TestStep.TraceList)
    //            {
    //                object obj = GetTraceVM(tr.GetType());
    //                TestTraceList.Add(new TestTraceNode() { TestTrace = tr,ObjVM=obj });
    //            }
    //            NotifyPropertyChanged(TestStepPropertyName);
    //        }
    //    }
        
        
    //    private bool _IsTest = true;
    //    private const string IsTestPropertyName = "IsTest";
    //    public bool IsTest
    //    {
    //        get
    //        {
    //            return TestStep.IsTest;
    //        }
    //        set
    //        {
    //            TestStep.IsTest = value;
    //            NotifyPropertyChanged(IsTestPropertyName);
    //        }
    //    }
    //    private bool? _PassFail;
    //    private const string PassFailPropertyName = "PassFail";
    //    public bool? PassFail
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

    //    private string _DisplayName;
    //    private const string DisplayNamePropertyName = "DisplayName";
    //    public string DisplayName
    //    {
    //        get
    //        {
    //            return _DisplayName;
    //        }
    //        set
    //        {
    //            _DisplayName = value;
    //            NotifyPropertyChanged(DisplayNamePropertyName);
    //        }
    //    }
    //    private string _ConfigName;
    //    private const string ConfigNamePropertyName = "ConfigName";
    //    public string ConfigName
    //    {
    //        get
    //        {
    //            if (TestStep != null)
    //            {
    //                return TestStep.ConfigName;
    //            }
    //            else
    //            {
    //                return "";
    //            }
                
    //        }
    //        set
    //        {   
    //            if(TestStep !=null)
    //            {
    //                TestStep.ConfigName = value;
    //                NotifyPropertyChanged(ConfigNamePropertyName);
    //            }
    //        }
    //    }

    //    private ObservableCollection<TestTraceNode> _TestTraceList=new ObservableCollection<TestTraceNode>();
    //    private const string TestTraceListPropertyName = "TestTraceList";
    //    public ObservableCollection<TestTraceNode> TestTraceList
    //    {
    //        get
    //        {
    //            return _TestTraceList;
    //        }
    //        set
    //        {
    //            _TestTraceList = value;               
    //            NotifyPropertyChanged(TestTraceListPropertyName);
    //        }
    //    }

    //    public object GetTraceVM(Type traceType)
    //    { 
    //         if(traceType.Name==typeof(PIMTestTrace).Name)
    //         {
    //             return new PIMTestTraceVM();
    //         }
    //         if(traceType.Name==typeof(NFTestTrace).Name)
    //         {
    //             return new NFTestTraceVM();
    //         }
    //         if(traceType.Name==typeof(IMDTestTrace).Name)
    //         {
    //             return new IMDTestTraceVM();
    //         }
    //         return null;
    //    }

    //    public void AddTestTrace(int currentSpecIndex)
    //    {
    //        if (!TestStep.IsFixedTraces && TestStep.GetType() != typeof(DCPSTestStep) && TestStep.GetType() != typeof(DUTTestStep))
    //        {
    //            TestStep.AddDefaultTestTrace();
    //            object obj = GetTraceVM(TestStep.TraceList.Last().GetType());
    //            TestTraceList.Add(new TestTraceNode() { TestTrace = TestStep.TraceList.Last(),ObjVM=obj });
    //            TestTraceList.Last().Select(currentSpecIndex);
    //        }
    //    }
    //    public void RemoveTestTrace(TestTraceNode traceNode)
    //    {
    //        if (!TestStep.IsFixedTraces)
    //        {
    //            TestStep.TraceList.Remove(traceNode.TestTrace);
    //            TestTraceList.Remove(traceNode);
    //        }
    //    }
    //}
}
