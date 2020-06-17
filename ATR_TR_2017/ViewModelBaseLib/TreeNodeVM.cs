using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;

namespace ViewModelBaseLib
{
    public class TreeNodeVM:NotifyBase
    {
        public TreeNodeVM()
        {
            SpecIndex = 0;
            _IsEnabled = true;
        }
        public TreeNodeVM ParentNode { get; set; }
        private string _Name;
        private const string NamePropertyName = "Name";
        public string Name
        {
            get
            {
                if (NodeObj is ManualConnection)
                {
                    _Name = (NodeObj as ManualConnection).Name;
                    (NodeObj as ManualConnection).UpdateName = new Action(() => { NotifyPropertyChanged(NamePropertyName); });
                }
                if(NodeObj is TestStep)
                {
                    _Name = (NodeObj as TestStep).Name;
                }
                if (NodeObj is TestTrace)
                {
                    _Name = (NodeObj as TestTrace).TypeName;
                }
                if (NodeObj is PointTestItem)
                {
                    _Name = (NodeObj as PointTestItem).TypeName;
                }
                if(NodeObj is BoolTestItem)
                {
                    _Name = (NodeObj as BoolTestItem).TypeName;
                }

                if (NodeObj is TRTestItem)
                {
                    _Name = (NodeObj as TRTestItem).TypeName;
                }
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged(NamePropertyName);
            }
        }
        private int _Index;
        private const string IndexPropertyName = "Index";
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
                NotifyPropertyChanged(IndexPropertyName);
            }
        }
        private string _DisplayName;
        private const string DisplayNamePropertyName = "DisplayName";
        public string DisplayName
        {
            get
            {
                if (NodeObj is ManualConnection)
                {
                    _DisplayName = (NodeObj as ManualConnection).Name;
                }
                if (NodeObj is TestTrace)
                {
                    _DisplayName = (NodeObj as TestTrace).TypeName;
                }
                if (NodeObj is TestStep)
                {
                    _DisplayName = Interface.GetStepTypeName(NodeObj.GetType());
                }
                if (NodeObj is TRTestItem)
                {
                    _DisplayName = (NodeObj as TRTestItem).TypeName;
                }
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                NotifyPropertyChanged(DisplayNamePropertyName);
            }
        }
        private string _SN;
        private const string SNPropertyName = "SN";
        public string SN
        {
            get
            {
                if (NodeObj is ManualConnection)
                {
                    _SN = (NodeObj as ManualConnection).SN;
                }
                else
                {
                    _SN = "";
                }
                return _SN;
            }
            set
            {
                if(NodeObj is ManualConnection)
                {
                    (NodeObj as ManualConnection).SN = value;
                }
                NotifyPropertyChanged(SNPropertyName);
            }
        }
        private bool? _PassFail;
        private const string PassFailPropertyName = "PassFail";
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
        private bool _IsExpanded;
        private const string IsExpandedPropertyName = "IsExpanded";
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                _IsExpanded = value;
                NotifyPropertyChanged(IsExpandedPropertyName);
            }
        }
        private bool _IsTest;
        private const string IsTestPropertyName = "IsTest";
        public bool IsTest
        {
            get
            {
                return _IsTest;
            }
            set
            {
                _IsTest = value;
                NotifyPropertyChanged(IsTestPropertyName);
            }
        }
        private object _NodeObj;
        public object NodeObj
        {
            get
            {
                return _NodeObj;

            }
            set
            {
                _NodeObj = value;
                if (NodeObj is ManualConnection)
                {
                    var mann = NodeObj as ManualConnection;
                    SubTreeNodeList.Clear();
                    foreach (var step in mann.TestStepList)
                    {
                        SubTreeNodeList.Add(new TreeNodeVM() { NodeObj=step,ParentNode=this});
                    }
                }
                
                else if (NodeObj is TestStep)
                {
                    if (NodeObj is LoopTestStep)
                    {
                        var loopStep = NodeObj as LoopTestStep;
                        SubTreeNodeList.Clear();
                        foreach (var step in loopStep.ChildTestStepList)
                        {   
                            SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = step,ParentNode=this});
                        }
                    }
                    else
                    {
                        var step = NodeObj as TestStep;
                        SubTreeNodeList.Clear();
                        foreach (var tr in step.ItemList)
                        {
                            SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = tr, Name = tr.TypeName, ParentNode = this,Index=step.ItemList.IndexOf(tr) });
                        }
                    }
                }
                else if(NodeObj is TestTrace)
                {
                    var trace = NodeObj as TestTrace;
                    SubTreeNodeList.Clear();
                    if(trace.TestSpecList.Count==0)return;
                    var markerObjList = trace.TestSpecList[_SpecIndex].TestMarkerList;
                    for (int i = 0; i < markerObjList.Count; i++)
                    {
                        SubTreeNodeList.Add(new TreeNodeVM() {NodeObj=markerObjList[i],Name="Marker"+ i,ParentNode=this});
                    }
                }
                else if(NodeObj is TRTestItem)
                {
                    var trTestItem = NodeObj as TRTestItem;
                    SubTreeNodeList.Clear();
                    if (trTestItem.TestSpecList.Count == 0) return;
                    var markerObjList = trTestItem.TestSpecList[_SpecIndex].TestMarkerList;
                    for (int i = 0; i < markerObjList.Count; i++)
                    {
                        SubTreeNodeList.Add(new TreeNodeVM() { NodeObj = markerObjList[i], Name = "Marker" + i, ParentNode = this });
                    }
                }
                
            }
        }
        private ObservableCollection<TreeNodeVM> _SubTreeNodeList=new ObservableCollection<TreeNodeVM>();
        private const string SubTreeNodeListPropertyName = "SubTreeNodeList";
        public ObservableCollection<TreeNodeVM> SubTreeNodeList
        {
            get
            {
                return _SubTreeNodeList;
            }
            set
            {
                _SubTreeNodeList = value;
                NotifyPropertyChanged(SubTreeNodeListPropertyName);
            }
        }
        private int _SpecIndex;

        public int SpecIndex
        {
            get 
            { 
                return _SpecIndex; 
            }
            set 
            { 
                _SpecIndex = value;
                if (NodeObj is TestTrace)
                {
                    var tr = NodeObj as TestTrace;
                    SubTreeNodeList.Clear();
                    if (tr.TestSpecList.Count == 0 || _SpecIndex<0) return;
                    var markerObjList = tr.TestSpecList[_SpecIndex].TestMarkerList;
                    for(int i=0;i<markerObjList.Count;i++)
                    {
                        SubTreeNodeList.Add(new TreeNodeVM() { Name = "Marker" + i, NodeObj = markerObjList[i],ParentNode=this});
                    }
                }
                else if (NodeObj is TRTestItem)
                {
                    var trTestItem = NodeObj as TRTestItem;
                    SubTreeNodeList.Clear();
                    if (trTestItem.TestSpecList.Count == 0 || _SpecIndex < 0) return;
                    var markerObjList = trTestItem.TestSpecList[_SpecIndex].TestMarkerList;
                    for (int i = 0; i < markerObjList.Count; i++)
                    {
                        SubTreeNodeList.Add(new TreeNodeVM() { Name = "Marker" + i, NodeObj = markerObjList[i], ParentNode = this });
                    }
                }
            }
        }
        public TreeNodeTypeEnum Type
        {
            get
            {
                if (NodeObj is ManualConnection)
                    return TreeNodeTypeEnum.ManualConnection;
                if (NodeObj is TestStep)
                {
                    if (NodeObj is ParentTestStep)
                        return TreeNodeTypeEnum.ParentTestStep;
                    else
                        return TreeNodeTypeEnum.TestStep;
                }
                if (NodeObj is TestTrace)
                    return TreeNodeTypeEnum.TestTrace;
                if (NodeObj is TestMarker)
                    return TreeNodeTypeEnum.TestMarker;
                if(NodeObj is PointTestItem)
                    return TreeNodeTypeEnum.PointTestItem;
                if (NodeObj is TRTestItem)
                    return TreeNodeTypeEnum.TRTestItem;
                return TreeNodeTypeEnum.BoolTestItem;
            }
        }
        //测试步骤的按钮Enbale
        private bool _IsEnabled;
        private const string IsEnabledPropertyName = "IsEnabled";
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                NotifyPropertyChanged(IsEnabledPropertyName);
            }
        }
        public void AddSubNode(TreeNodeVM subNode)
        {
            SubTreeNodeList.Add(subNode);
            subNode.ParentNode = this;
        }
        
    }
    public enum TreeNodeTypeEnum { ManualConnection, ParentTestStep, TestStep, PointTestItem, TestTrace, BoolTestItem, TestMarker,TRTestItem }
}
