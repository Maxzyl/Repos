using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;
namespace ViewModelBaseLib
{
    //public class TestTraceNode:NotifyBase
    //{
    //    public TestTraceNode()
    //    {
    //        MarkerList = new ObservableCollection<TestMarkerNode>();
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
    //    private object _ObjVM;
    //    private const string ObjVMPropertyName = "ObjVM";
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
    //    private TestTrace _TestTrace;
    //    private const string TestTracePropertyName = "TestTrace";
    //    public TestTrace TestTrace
    //    {
    //        get
    //        {
    //            return _TestTrace;
    //        }
    //        set
    //        {
    //            _TestTrace = value;
    //            if (TestTrace.TestSpecList.Count > 0)
    //            {
    //                Select(0);//maybe wrong
    //            }
    //            NotifyPropertyChanged(TestTracePropertyName);
    //        }
    //    }
        
    //    private string _DisplayName;
    //    private const string DisplayNamePropertyName = "DisplayName";
    //    public string DisplayName
    //    {
    //        get
    //        {
    //            if (TestTrace != null)
    //            {  
    //                return TestTrace.TypeName;
    //            }
    //            else
    //                return null;
    //        }
    //        set
    //        {
    //            if (TestTrace != null)
    //            {
    //                TestTrace.TypeName = value;
    //            }
    //            NotifyPropertyChanged(DisplayNamePropertyName);
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
        
    //    private bool _IsTest;
    //    private const string IsTestPropertyName = "IsTest";
    //    public bool IsTest
    //    {
    //        get
    //        {
    //            return TestTrace.IsTest;
    //        }
    //        set
    //        {
    //            TestTrace.IsTest = value;
    //            NotifyPropertyChanged(IsTestPropertyName);
    //        }
    //    }
        
    //    private ObservableCollection<TestMarkerNode> _MarkerList;
    //    private const string MarkerListPropertyName = "MarkerList";
    //    public ObservableCollection<TestMarkerNode> MarkerList
    //    {
    //        get
    //        {
    //            return _MarkerList;
    //        }
    //        set
    //        {
    //            _MarkerList = value;
    //            NotifyPropertyChanged(MarkerListPropertyName);
    //        }
    //    }
    //    public void AddMarker(int specIndex)
    //    {            
    //        if (!TestTrace.IsFixedMarkers)
    //        {
    //            TestTrace.AddDefaultMarker(specIndex);
    //            object obj = GetMarkerTypeName(TestTrace.TestSpecList[specIndex].TestMarkerList.Last().GetType());
    //            TestMarkerNode tm = new TestMarkerNode() { DisplayName = "Marker" + TestTrace.TestSpecList[specIndex].TestMarkerList.Count(), Marker = TestTrace.TestSpecList[specIndex].TestMarkerList.Last(),ObjVM=obj};
    //            MarkerList.Add(tm);
    //        }
    //    }
    //    public void Select(int specIndex)
    //    {
    //        MarkerList.Clear();
    //        while (TestTrace.TestSpecList.Count <= specIndex)
    //        {
    //            TestTrace.TestSpecList.Add(new TestSpec());
    //        }
    //        var markerObjList=TestTrace.TestSpecList[specIndex].TestMarkerList;
    //        for(int i=0;i<markerObjList.Count;i++)
    //        {
    //            object obj = GetMarkerTypeName(markerObjList[i].GetType());
    //            MarkerList.Add(new TestMarkerNode() { DisplayName = "Marker" + i, Marker = markerObjList[i],ObjVM=obj});
    //        }
    //    }
    //    public void Remove(int specIndex, TestMarkerNode markerNode)
    //    {
    //        if (!TestTrace.IsFixedMarkers && specIndex < TestTrace.TestSpecList.Count)
    //        {
    //            TestTrace.TestSpecList[specIndex].TestMarkerList.Remove(markerNode.Marker);
    //            MarkerList.Remove(markerNode);
    //        }
    //    }
    //    public object GetMarkerTypeName(Type markerType)
    //    { 
    //          if(markerType.Name==typeof(XYTestMarker).Name)
    //          {
    //              return new XYTestMarkerVM();
    //          }
    //          return null;
    //    }
    //}
}
