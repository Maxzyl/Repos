
using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using ViewModelBaseLib;

namespace TestResultMarkerDip
{
    public class TestResultMarker : NotifyBase
    {
        public TestResultMarker()
        {
            XYMarkerDisplist = new ObservableCollection<ObservableCollection<XYMarkerDisp>>();
            ManualConnList = new ObservableCollection<TreeNodeVM>();
            XYMarkerDisplist.Add(new ObservableCollection<XYMarkerDisp>());
        }
        
        public ObservableCollection<ObservableCollection<XYMarkerDisp>> XYMarkerDisplist { get; set; }
        public ObservableCollection<TreeNodeVM> ManualConnList { get; set; }
        private int _SelectedSpecIndex;
        private const string SelectedSpecIndexPropertyName = "SelectedSpecIndex";
        public int SelectedSpecIndex
        {
            get
            {
                return _SelectedSpecIndex;
            }
            set
            {
                _SelectedSpecIndex = value;
                NotifyPropertyChanged(SelectedSpecIndexPropertyName);
            }
        }
        private int _SelectedlistViewIndex;
        private const string SelectedlistViewIndexPropertyName = "SelectedlistViewIndex";
        public int SelectedlistViewIndex
        {
            get
            {
                return _SelectedlistViewIndex;
            }
            set
            {
                _SelectedlistViewIndex = value;
                NotifyPropertyChanged(SelectedlistViewIndexPropertyName);
            }
        }
        public void OnSelected(object selectedItem)
        {
            if (selectedItem is XYMarkerDisp)
            {
                XYMarkerDisp disp = selectedItem as XYMarkerDisp;
                if (this.ManualConnList.Count > 0)
                {
                    if (this.ManualConnList[disp.ConnIndex].SubTreeNodeList.Count > 0)
                    {
                        if (this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList.Count > 0)
                        {
                            if (this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].NodeObj is TestTrace && this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].SubTreeNodeList.Count > 0)
                            {
                                this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].SubTreeNodeList[disp.MarkerIndex].IsExpanded = true;
                                this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].SubTreeNodeList[disp.MarkerIndex].IsSelected = true;
                                this.ManualConnList[disp.ConnIndex].IsExpanded = true;
                                this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].IsExpanded = true;
                                this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].IsExpanded = true;

                            }
                            else if (this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].NodeObj is PointTestItem)
                            {
                                this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].IsExpanded = true;
                                this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].SubTreeNodeList[disp.TraceIndex].IsSelected = true;
                                this.ManualConnList[disp.ConnIndex].IsExpanded = true;
                                this.ManualConnList[disp.ConnIndex].SubTreeNodeList[disp.StepIndex].IsExpanded = true;
                            }
                        }
                    }
                }
                
            }
        }
        public int UpdateTestResultIndex(object obj, ObservableCollection<XYMarkerDisp> XYMarkerDispList)
        {
            TreeNodeVM item = obj as TreeNodeVM;
            int indexConn = 0;
            int indexStep = 0;
            int indexTrace = 0;
            int indexMarker = 0;
            int i = 0;
            if (item == null) return 0;
            if (item.Type == TreeNodeTypeEnum.ManualConnection)
            {
                indexConn = this.ManualConnList.IndexOf(item);
                var itemConn = XYMarkerDispList.Where(x => x.ConnIndex == indexConn).FirstOrDefault();
                i = XYMarkerDispList.IndexOf(itemConn);
            }
            else if (item.Type == TreeNodeTypeEnum.TestStep)
            {
                foreach (var mcTreeNode in this.ManualConnList)
                {
                    foreach (var stepTreeNode in mcTreeNode.SubTreeNodeList)
                    {
                        if (stepTreeNode.Equals(item))
                        {
                            indexConn = this.ManualConnList.IndexOf(mcTreeNode);
                            indexStep = mcTreeNode.SubTreeNodeList.IndexOf(stepTreeNode);
                            var itemStep = XYMarkerDispList.Where(x => x.ConnIndex == indexConn && x.StepIndex == indexStep).FirstOrDefault();
                            i = XYMarkerDispList.IndexOf(itemStep);
                            break;
                        }
                    }
                }
            }
            else if (item.Type == TreeNodeTypeEnum.TestTrace)
            {
                foreach (var mcTreeNode in this.ManualConnList)
                {
                    foreach (var stepTreeNode in mcTreeNode.SubTreeNodeList)
                    {
                        foreach (var traceTreeNode in stepTreeNode.SubTreeNodeList)
                        {
                            if (traceTreeNode.Equals(item))
                            {
                                indexConn = this.ManualConnList.IndexOf(mcTreeNode);
                                indexStep = mcTreeNode.SubTreeNodeList.IndexOf(stepTreeNode);
                                indexTrace = stepTreeNode.SubTreeNodeList.IndexOf(traceTreeNode);
                                var itemTrace = XYMarkerDispList.Where(x => x.ConnIndex == indexConn && x.StepIndex == indexStep && x.TraceIndex == indexTrace).FirstOrDefault();
                                i = XYMarkerDispList.IndexOf(itemTrace);
                                break;
                            }
                        }
                    }
                }
            }
            else if (item.Type == TreeNodeTypeEnum.TestMarker)
            {
                foreach (var mcTreeNode in this.ManualConnList)
                {
                    foreach (var stepTreeNode in mcTreeNode.SubTreeNodeList)
                    {
                        foreach (var traceTreeNode in stepTreeNode.SubTreeNodeList)
                        {
                            foreach (var markerTreeNode in traceTreeNode.SubTreeNodeList)
                            {
                                if (markerTreeNode.Equals(item))
                                {
                                    indexConn = this.ManualConnList.IndexOf(mcTreeNode);
                                    indexStep = mcTreeNode.SubTreeNodeList.IndexOf(stepTreeNode);
                                    indexTrace = stepTreeNode.SubTreeNodeList.IndexOf(traceTreeNode);
                                    indexMarker = traceTreeNode.SubTreeNodeList.IndexOf(markerTreeNode);
                                    var itemMarker = XYMarkerDispList.Where(x => x.ConnIndex == indexConn && x.StepIndex == indexStep && x.TraceIndex == indexTrace && x.MarkerIndex == indexMarker).FirstOrDefault();
                                    i = XYMarkerDispList.IndexOf(itemMarker);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return i;
        }
        public void ClearData()
        { 
            foreach(var item in XYMarkerDisplist)
            {
                item.Clear();
            }
        }
    }
}
