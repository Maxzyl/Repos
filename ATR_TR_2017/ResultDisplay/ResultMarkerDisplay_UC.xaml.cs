using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModelBaseLib;
using System.Collections.ObjectModel;

namespace TestResultMarkerDip
{
    /// <summary>
    /// Interaction logic for ResultMarkerDisplay_UC.xaml
    /// </summary>
    [UIDisplayPara("Marker点显示")]
    public partial class ResultMarkerDisplay_UC : UserControl,IResultListerner
    {   
        TestResultMarker vm = new TestResultMarker();
        [System.Xml.Serialization.XmlIgnore]
        public TestPlan TestPlan { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public int SpecIndex { get; set; }
        public ResultMarkerDisplay_UC()
        {
            InitializeComponent();
            resultMarkerDisplay.DataContext = vm;
        }
        public void OnTestManualConnRunStart()
        {
            vm.ClearData();
        }
        public void OnTestManualConnRunCompleted()
        { 
        }
        public bool OnTestPlanRunStart()
        {
            vm.ClearData();
            return true;
        }
        public bool OnTestPlanRunCompleted()
        {
            vm.ClearData();
            return true;
        }
        public void OnTestStepRunStart(int stepIndex)
        {

        }
        public void OnTestStepRunCompleted(int stepIndex)
        {   
            TestStep step=TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[stepIndex];
            if (step is LoopTestStep) return;
            for (int k = 0; k < step.ItemList.Count; k++)
            {
                if (step.ItemList[k] as TestTrace != null)
                {
                    TestTrace tc = step.ItemList[k] as TestTrace;
                    for (int j = 0; j < tc.TestSpecList.Count; j++)
                    {
                        TestTraceSpec tp = tc.TestSpecList[j];
                        for (int p = 0; p < tp.TestMarkerList.Count; p++)
                        {
                            TestMarker tm = tp.TestMarkerList[p];
                            if (tm as XYTestMarker != null)
                            {
                                XYTestMarker xyM = tm as XYTestMarker;
                                foreach (XYData xyData in xyM.MarkerResult)
                                {
                                    if (vm.XYMarkerDisplist.Count <= j)
                                    {
                                        ObservableCollection<XYMarkerDisp> XYMarkerDisps = new ObservableCollection<XYMarkerDisp>();
                                        vm.XYMarkerDisplist.Add(XYMarkerDisps);
                                    }
                                    if (vm.XYMarkerDisplist[j].Where(x => x.ConnIndex == TestPlan.currentConnIndex && x.StepIndex == stepIndex && x.TraceIndex == k && x.MarkerIndex == p).Count() == 0)
                                    {
                                        vm.XYMarkerDisplist[j].Add(new XYMarkerDisp() { ConnIndex = TestPlan.currentConnIndex, StepIndex = stepIndex, TraceIndex = k, MarkerIndex = p, PortName = xyM.TestConfigDesciption, UserDefName = xyM.UserDefName, Freq = xyData.X, TestResult = xyData.Y, PassFail = xyM.PassFail, LimitDescription=xyM.LimitDescription,XDescription=xyM.XDescription });
                                    }
                                    else
                                    {
                                        var item = vm.XYMarkerDisplist[j].Where(x => x.ConnIndex == TestPlan.currentConnIndex && x.StepIndex == stepIndex && x.TraceIndex == k && x.MarkerIndex == p).First();
                                        item.Freq = xyData.X;
                                        item.TestResult = xyData.Y;
                                        item.PassFail = xyM.PassFail;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (step.ItemList[k] as TRTestItem != null)
                {
                    TRTestItem tt = step.ItemList[k] as TRTestItem;
                    for (int j = 0; j < tt.TestSpecList.Count; j++)
                    {
                        TRTestItemSpec tp = tt.TestSpecList[j];
                        for (int p = 0; p < tp.TestMarkerList.Count; p++)
                        {
                            TestMarker tm = tp.TestMarkerList[p];
                            if (tm as XYTestMarker != null)
                            {
                                XYTestMarker xyM = tm as XYTestMarker;
                                foreach (XYData xyData in xyM.MarkerResult)
                                {
                                    if (vm.XYMarkerDisplist.Count <= j)
                                    {
                                        ObservableCollection<XYMarkerDisp> XYMarkerDisps = new ObservableCollection<XYMarkerDisp>();
                                        vm.XYMarkerDisplist.Add(XYMarkerDisps);
                                    }
                                    if (vm.XYMarkerDisplist[j].Where(x => x.ConnIndex == TestPlan.currentConnIndex && x.StepIndex == stepIndex && x.TraceIndex == k && x.MarkerIndex == p).Count() == 0)
                                    {
                                        vm.XYMarkerDisplist[j].Add(new XYMarkerDisp() { ConnIndex = TestPlan.currentConnIndex, StepIndex = stepIndex, TraceIndex = k, MarkerIndex = p, PortName = xyM.TestConfigDesciption, UserDefName = xyM.UserDefName, Freq = xyData.X, TestResult = xyData.Y, PassFail = xyM.PassFail, LimitDescription = xyM.LimitDescription, XDescription = xyM.XDescription });
                                    }
                                    else
                                    {
                                        var item = vm.XYMarkerDisplist[j].Where(x => x.ConnIndex == TestPlan.currentConnIndex && x.StepIndex == stepIndex && x.TraceIndex == k && x.MarkerIndex == p).First();
                                        item.Freq = xyData.X;
                                        item.TestResult = xyData.Y;
                                        item.PassFail = xyM.PassFail;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (step.ItemList[k] is PointTestItem)
                {
                    PointTestItem pTestItem = step.ItemList[k] as PointTestItem;
                    for (int j = 0; j < pTestItem.TestSpecList.Count; j++)
                    {
                        if (vm.XYMarkerDisplist.Count <= j)
                        {
                            ObservableCollection<XYMarkerDisp> XYMarkerDisps = new ObservableCollection<XYMarkerDisp>();
                            vm.XYMarkerDisplist.Add(XYMarkerDisps);
                        }
                        if (vm.XYMarkerDisplist[j].Where(x => x.ConnIndex == TestPlan.currentConnIndex && x.StepIndex == stepIndex && x.TraceIndex == k && x.MarkerIndex == 0).Count() == 0)
                        {
                            vm.XYMarkerDisplist[j].Add(new XYMarkerDisp()
                            {
                                ConnIndex = TestPlan.currentConnIndex,
                                StepIndex = stepIndex,
                                TraceIndex = k,
                                MarkerIndex = 0,
                                PortName = pTestItem.TestConfigDesciption,
                                UserDefName = pTestItem.UserDefName,
                                Freq = pTestItem.X,
                                TestResult = pTestItem.Y,
                                PassFail = pTestItem.PassFail,
                                LimitDescription = pTestItem.TestSpecList[j].LimitDescription,
                                XDescription = pTestItem.XDescription
                            });
                        }
                        else
                        {
                            var item = vm.XYMarkerDisplist[j].Where(x => x.ConnIndex == TestPlan.currentConnIndex && x.StepIndex == stepIndex && x.TraceIndex == k && x.MarkerIndex == 0).First();
                            item.Freq = pTestItem.X;
                            item.TestResult = pTestItem.Y;
                            item.PassFail = pTestItem.PassFail;
                        }
                    }
                }
                else if (step.ItemList[k] is BoolTestItem)
                {
                    BoolTestItem bTestItem = step.ItemList[k] as BoolTestItem;
                    int resultData = bTestItem.ResultData == true ? 1 : 0;
                    vm.XYMarkerDisplist[0].Add(new XYMarkerDisp() { TestResult = Convert.ToDouble(resultData), UserDefName = bTestItem.UserDefName });
                }
            }
        }
        public void OnChildTestStepRunStart()
        { 
        }

        public void OnChildTestStepRunCompleted(string stepName)
        { 
        }
        public void OnPointFinish(int stepIndex, int traceIndex, int markerIndex)
        {
        }
    }
}
