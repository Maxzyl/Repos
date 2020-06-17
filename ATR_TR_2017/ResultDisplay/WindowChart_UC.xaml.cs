using ModelBaseLib;
using SymtChartLib;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace TestResultMarkerDip
{
    /// <summary>
    /// Interaction logic for WindowChart_UC.xaml
    /// </summary>
    [UIDisplayPara("Chart显示")]
    public partial class WindowChart_UC : UserControl,IResultListerner
    {
        [System.Xml.Serialization.XmlIgnore]
        public TestPlan TestPlan { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public int SpecIndex { get; set; }
        public WindowChart_UC()
        {
            InitializeComponent();
        }
        public void OnTestManualConnRunStart()
        {
        }
        public void OnTestManualConnRunCompleted()
        {
        }
        public bool OnTestPlanRunStart()
        {
            return true;
        }
        public bool OnTestPlanRunCompleted()
        {
            return true;
        }
        public void OnTestStepRunStart(int stepIndex)
        {
            showChartOnStepStart(stepIndex);
        }
        public void OnTestStepRunCompleted(int stepIndex)
        {
            showChart(stepIndex);
        }

        public void OnChildTestStepRunStart()
        {

        }

        public void OnChildTestStepRunCompleted(string stepName)
        {
            
        }
        public void OnPointFinish(int stepIndex, int traceIndex, int markerIndex)
        {
            showChartOnPointFinish(stepIndex, traceIndex, markerIndex);
        }
        private void showChartOnStepStart(int stepIndex)
        {
            TestStep step = TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[stepIndex];
            windowChart.clearData();
            windowChart.Traces.Clear();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            List<SymtChartLib.XAxis> lstXAxis = new List<SymtChartLib.XAxis>();
            int k = 0;
            if (step as LoopTestStep != null) return;
            foreach (TestTrace tr in step.ItemList.Where(x => x is TestTrace))
            {
                if (lstXAxis.Select(x => x.Start == tr.XAxisInfo.Start && x.Stop == tr.XAxisInfo.Stop && x.Unit == tr.XAxisInfo.Unit).Count() == 0)
                {
                    lstXAxis.Add(new SymtChartLib.XAxis() { Start = tr.XAxisInfo.Start, Stop = tr.XAxisInfo.Stop, Center = tr.XAxisInfo.Center });
                }
                SymtChartLib.Trace trace = new SymtChartLib.Trace();
                
                k++;
                trace.Title = step.Name + "_" + tr.TypeName;
                trace.AxisIndex = lstXAxis.FindIndex(x => x.Start == tr.XAxisInfo.Start && x.Stop == tr.XAxisInfo.Stop && x.Center == tr.XAxisInfo.Center);
                if (tr.ResultData != null)
                {
                    List<XYData> xyData = tr.ResultData;
                    if (xyData.Count > 0)
                    {
                        Point[] point = new Point[xyData.Count];
                        for (int i = 0; i < xyData.Count; i++)
                        {
                            Point p = new Point() { X = xyData[i].X, Y = xyData[i].Y };
                            point[i] = p;
                        }

                        trace.TraceData = point;
                        windowChart.Traces.Add(trace);
                    }
                }
                if (tr.TestSpecList.Count > 0 && SpecIndex >= 0)
                {
                    XYTestLimit testLimit = tr.TestSpecList[SpecIndex].TestLimit;
                    if (testLimit != null)
                    {
                        if (testLimit.Enable)
                        {
                            foreach (var limit in testLimit.LimitLine)
                            {
                                SymtChartLib.LimitLineTypeEnum type = new SymtChartLib.LimitLineTypeEnum();
                                if (limit.Type == ModelBaseLib.LimitLineTypeEnum.Max)
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.Max;
                                }
                                else if (limit.Type == ModelBaseLib.LimitLineTypeEnum.Min)
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.Min;
                                }
                                else
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.None;
                                }
                                trace.LimitLineList.Add(new SymtChartLib.LimitLine() { X1 = limit.X1, Y1 = limit.Y1, X2 = limit.X2, Y2 = limit.Y2, Type = type });
                            }
                        }
                        else
                        {
                            double? yValue = null;
                            if (tr.TestSpecList[SpecIndex].LowLimit == null && tr.TestSpecList[SpecIndex].UpLimit == null) {  }
                            else
                            {
                                if (trace.TraceData != null)
                                {
                                    double x1 = trace.TraceData.Select((d, y) => d.X).Min();
                                    double x2 = trace.TraceData.Select((d, y) => d.X).Max();
                                    yValue = tr.TestSpecList[SpecIndex].LowLimit != null ? tr.TestSpecList[SpecIndex].LowLimit : tr.TestSpecList[SpecIndex].UpLimit;
                                    trace.LimitLineList.Add(new SymtChartLib.LimitLine() { X1 = x1, Y1 = Convert.ToDouble(yValue), X2 = x2, Y2 = Convert.ToDouble(yValue), Type = SymtChartLib.LimitLineTypeEnum.None });
                                }
                            }
                        }
                    }
                }
                trace.Scale = tr.Scale;
                trace.DivisionCount = tr.DivCount;
                trace.ReferenceLevel = tr.RefValue;
                trace.Unit = tr.Unit;
                trace.ReferencePosition = tr.RefPosition;
               
            }
            windowChart.XAxisList = lstXAxis;
            windowChart.UpdateData();
        }

        private void showChartOnPointFinish(int stepIndex, int traceIndex, int markerIndex)
        {
            windowChart.ClearTraceInfo();
            TestStep step = TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[stepIndex];
            foreach (TestTrace tr in step.ItemList.Where(x => x is TestTrace))
            {
                List<XYData> xyData = tr.ResultData;
                SymtChartLib.Trace trace = new SymtChartLib.Trace();
               
                trace.Title = step.Name + "_" + tr.TypeName;
                trace.AxisIndex = windowChart.XAxisList.FindIndex(x => x.Start == tr.XAxisInfo.Start && x.Stop == tr.XAxisInfo.Stop && x.Center == tr.XAxisInfo.Center);
                if (xyData.Count > 0)
                {
                    Point[] point = new Point[xyData.Count];
                    for (int i = 0; i < xyData.Count; i++)
                    {
                        Point p = new Point() { X = xyData[i].X, Y = xyData[i].Y };
                        point[i] = p;
                    }
                    trace.TraceData = point;
                    windowChart.Traces.Add(trace);
                }
                //添加Marker
                //foreach(XYTestMarker marker in tr.TestSpecList[SpecIndex].TestMarkerList)
                //{
                //    int index = tr.ResultData.Select((d, i) => { return new { value = d.X, valueIndex = i }; }).OrderBy(x => Math.Abs(x.value - marker.Start)).First().valueIndex;
                //}
                if (tr.TestSpecList.Count > 0 && SpecIndex >= 0)
                {
                    XYTestLimit testLimit = tr.TestSpecList[SpecIndex].TestLimit;
                    if (testLimit != null)
                    {
                        if (testLimit.Enable)
                        {
                            foreach (var limit in testLimit.LimitLine)
                            {
                                SymtChartLib.LimitLineTypeEnum type = new SymtChartLib.LimitLineTypeEnum();
                                if (limit.Type == ModelBaseLib.LimitLineTypeEnum.Max)
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.Max;
                                }
                                else if (limit.Type == ModelBaseLib.LimitLineTypeEnum.Min)
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.Min;
                                }
                                else
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.None;
                                }
                                trace.LimitLineList.Add(new SymtChartLib.LimitLine() { X1 = limit.X1, Y1 = limit.Y1, X2 = limit.X2, Y2 = limit.Y2, Type = type });
                            }
                        }
                        else
                        {
                            double? yValue = null;
                            if (tr.TestSpecList[SpecIndex].LowLimit == null && tr.TestSpecList[SpecIndex].UpLimit == null) {  }
                            else
                            {
                                if (trace.TraceData != null)
                                {
                                    double x1 = trace.TraceData.Select((d, y) => d.X).Min();
                                    double x2 = trace.TraceData.Select((d, y) => d.X).Max();
                                    yValue = tr.TestSpecList[SpecIndex].LowLimit != null ? tr.TestSpecList[SpecIndex].LowLimit : tr.TestSpecList[SpecIndex].UpLimit;
                                    trace.LimitLineList.Add(new SymtChartLib.LimitLine() { X1 = x1, Y1 = Convert.ToDouble(yValue), X2 = x2, Y2 = Convert.ToDouble(yValue), Type = SymtChartLib.LimitLineTypeEnum.None });
                                }
                            }
                        }
                    }
                }
                trace.Scale = tr.Scale;
                trace.DivisionCount = tr.DivCount;
                trace.ReferenceLevel = tr.RefValue;
                trace.Unit = tr.Unit;
                trace.ReferencePosition = tr.RefPosition;
                
            }
            windowChart.UpdatePointData(traceIndex);
        }

        private void showChart(int stepIndex)
        {             
            TestStep step = TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[stepIndex];
            if(!step.IsPrintTrace)return;
            windowChart.clearData();
            windowChart.Traces.Clear();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            List<SymtChartLib.XAxis> lstXAxis = new List<SymtChartLib.XAxis>();
            int k = 0;
            if (step as LoopTestStep != null) return;
            foreach (TestTrace tr in step.ItemList.Where(x => x is TestTrace))
            {
                if (lstXAxis.Select(x => x.Start == tr.XAxisInfo.Start && x.Stop == tr.XAxisInfo.Stop && x.Unit == tr.XAxisInfo.Unit).Count() == 0)
                {
                    lstXAxis.Add(new SymtChartLib.XAxis() { Start = tr.XAxisInfo.Start, Stop = tr.XAxisInfo.Stop, Center = tr.XAxisInfo.Center });
                }
                SymtChartLib.Trace trace = new SymtChartLib.Trace();
               
                k++;
                //trace.Title = step.Name + tr.TypeName + step.ItemList.IndexOf(tr);
                trace.Title = step.Name + "_" + tr.TypeName;
                trace.AxisIndex = lstXAxis.FindIndex(x => x.Start == tr.XAxisInfo.Start && x.Stop == tr.XAxisInfo.Stop && x.Center == tr.XAxisInfo.Center);
                if (tr.ResultData != null)
                {
                    List<XYData> xyData = tr.ResultData;
                    if (xyData.Count > 0)
                    {
                        Point[] point = new Point[xyData.Count];
                        for (int i = 0; i < xyData.Count; i++)
                        {
                            Point p = new Point() { X = xyData[i].X, Y = xyData[i].Y };
                            point[i] = p;
                        }

                        trace.TraceData = point;
                        windowChart.Traces.Add(trace);
                    }
                }
                //设置Marker
                foreach (XYTestMarker marker in tr.TestSpecList[SpecIndex].TestMarkerList)
                {
                    if(tr.ResultData.Select((d, i) => { return new { value = d.X, valueIndex = i }; }).OrderBy(x => Math.Abs(x.value - marker.MarkerResult[0].X)).FirstOrDefault()!=null)
                    {
                        int index = tr.ResultData.Select((d, i) => { return new { value = d.X, valueIndex = i }; }).OrderBy(x => Math.Abs(x.value - marker.MarkerResult[0].X)).FirstOrDefault().valueIndex;
                        Marker m = new Marker() { X = tr.ResultData[index].X, Y = tr.ResultData[index].Y };
                        trace.Markers.Add(m);
                       
                    }
                }
                if (tr.TestSpecList.Count > 0 && SpecIndex >= 0)
                {
                    XYTestLimit testLimit = tr.TestSpecList[SpecIndex].TestLimit;
                    if (testLimit != null)
                    {
                        if (testLimit.Enable)
                        {
                            foreach (var limit in testLimit.LimitLine)
                            {
                                SymtChartLib.LimitLineTypeEnum type = new SymtChartLib.LimitLineTypeEnum();
                                if (limit.Type == ModelBaseLib.LimitLineTypeEnum.Max)
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.Max;
                                }
                                else if (limit.Type == ModelBaseLib.LimitLineTypeEnum.Min)
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.Min;
                                }
                                else
                                {
                                    type = SymtChartLib.LimitLineTypeEnum.None;
                                }
                                trace.LimitLineList.Add(new SymtChartLib.LimitLine() { X1 = limit.X1, Y1 = limit.Y1, X2 = limit.X2, Y2 = limit.Y2, Type = type });
                            }
                        }
                        else
                        {
                            double? yValue = null;
                            if (tr.TestSpecList[SpecIndex].LowLimit == null && tr.TestSpecList[SpecIndex].UpLimit == null) {  }
                            else
                            {
                                if (trace.TraceData != null)
                                {
                                    double x1 = trace.TraceData.Select((d, y) => d.X).Min();
                                    double x2 = trace.TraceData.Select((d, y) => d.X).Max();
                                    yValue = tr.TestSpecList[SpecIndex].LowLimit != null ? tr.TestSpecList[SpecIndex].LowLimit : tr.TestSpecList[SpecIndex].UpLimit;
                                    trace.LimitLineList.Add(new SymtChartLib.LimitLine() { X1 = x1, Y1 = Convert.ToDouble(yValue), X2 = x2, Y2 = Convert.ToDouble(yValue), Type = SymtChartLib.LimitLineTypeEnum.None });
                                }
                            }
                        }
                    }
                }
                trace.Scale = tr.Scale;
                trace.DivisionCount = tr.DivCount;
                trace.ReferenceLevel = tr.RefValue;
                trace.Unit = tr.Unit;
                trace.ReferencePosition = tr.RefPosition;
            }
            windowChart.XAxisList = lstXAxis;
            windowChart.UpdateData();
            if(step.IsSaveScreen && step.SceenSize != null)
            {
                saveChart(stepIndex, step.SceenSize);
            }
        }

        private void saveChart(int stepIndex,Size size)
        {
            byte[] bytes = null;
            ForceRender(windowChart, size, (Action<RenderTargetBitmap>)delegate(RenderTargetBitmap render)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(render));
                //using(FileStream fs = new FileStream("./21.png",FileMode.Create))
                //{
                //    encoder.Save(fs);
                //}
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    bytes = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(bytes, 0, Convert.ToInt32(ms.Length));
                }
                TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[stepIndex].bImage = bytes;
            });
        }

        private void ForceRender(FrameworkElement FE, Size size, Action<RenderTargetBitmap> action)
        {
            Grid Parent = FE.Parent as Grid;
            Parent.Children.Remove(FE);
            RenderTargetBitmap result;
            if ((size.Height > 0) && (size.Width > 0)) FE.Width = size.Width; FE.Height = size.Height;
            ScrollViewer panel = new ScrollViewer();
            panel.Content = FE;
            panel.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            Window _window = new Window() { Content = panel };
            _window.WindowStyle = WindowStyle.None;
            _window.AllowsTransparency = true;
            _window.Opacity = 0.001;
            _window.ShowInTaskbar = false;
            _window.Width = FE.Width;
            _window.Height = FE.Height;
            _window.Show();
            Dispatcher.BeginInvoke((Action)delegate
            {
                result = new RenderTargetBitmap((int)FE.Width, (int)FE.Height, 96, 96, PixelFormats.Pbgra32);
                result.Render(FE);
                (_window.Content as ScrollViewer).Content = null;
                Parent.Children.Add(FE);
                _window.Close();
                action.Invoke(result);
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle).Wait();
        }
    }
}
