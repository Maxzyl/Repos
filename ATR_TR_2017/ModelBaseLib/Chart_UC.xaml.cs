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

namespace ModelBaseLib
{
    /// <summary>
    /// Interaction logic for Chart_UC.xaml
    /// </summary>
    public partial class Chart_UC : UserControl
    {
        TestStep step = new TestStep();
        int specIndex;
        Size size;
        public Chart_UC(TestStep step, int specIndex,Size size)
        {
            InitializeComponent();
            this.step = step;
            this.specIndex = specIndex;
            this.size = size;
        }

        public void ShowChart(TestStep step)
        {
            windowChart.clearData();
            windowChart.Traces.Clear();
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
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
                if (tr.TestSpecList.Count > 0 && specIndex >= 0)
                {
                  
                    XYTestLimit testLimit = tr.TestSpecList[specIndex].TestLimit;
                    if (testLimit != null)
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
        public byte[] saveChart()
        {
            byte[] bytes = null;
            ForceRender(windowChart, size, (Action<RenderTargetBitmap>)delegate(RenderTargetBitmap render)
            {
                 BitmapEncoder encoder = new PngBitmapEncoder();
                 encoder.Frames.Add(BitmapFrame.Create(render));
                //System.IO.FileStream fs = new System.IO.FileStream("./13.jpg", System.IO.FileMode.Create);
                //encoder.Save(fs);
                //fs.Close();
                 using (MemoryStream ms = new MemoryStream())
                 {
                     encoder.Save(ms);
                     bytes = new byte[ms.Length];
                     ms.Seek(0, SeekOrigin.Begin);
                     ms.Read(bytes, 0, Convert.ToInt32(ms.Length));
                 }
            });
            return bytes;
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
            _window.Opacity = 0.0001;
            _window.ShowInTaskbar = false;
            _window.Width = FE.Width;
            _window.Height = FE.Height;
            _window.Show();
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                result = new RenderTargetBitmap((int)FE.Width, (int)FE.Height, 96, 96, PixelFormats.Pbgra32);
                result.Render(FE);
                (_window.Content as ScrollViewer).Content = null;
                Parent.Children.Add(FE);
                _window.Close();
                action.Invoke(result);
            }, DispatcherPriority.ApplicationIdle).Wait();
        }
    }
}
