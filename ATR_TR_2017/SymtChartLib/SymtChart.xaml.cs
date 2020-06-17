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

using DevExpress.Xpf.Docking;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;

namespace SymtChartLib
{
    /// <summary>
    /// Interaction logic for SymtChart.xaml
    /// </summary>
    public partial class SymtChart : UserControl
    {
        public SymtChart()
        {
            InitializeComponent();
            //Windows = new List<WindowInfo>();
            Windows = new List<WindowChart>();
            //Traces = new List<Trace>();
        }

        //private List<WindowInfo> Windows { get; set; }
        private List<WindowChart> Windows { get; set; }
        private List<BaseLayoutItem> panels = new List<BaseLayoutItem>();
        //public List<Trace> Traces { get; set; }
        public void AddWindow() 
        {
            //WindowInfo wInfo = new WindowInfo();
            //wInfo.Name = "window" + (Windows.Count + 1).ToString();
            //Windows.Add(wInfo);

            DevExpress.Xpf.Docking.LayoutPanel lpl = new DevExpress.Xpf.Docking.LayoutPanel();
            //lpl.Caption = wInfo.Name;
            WindowChart wChart = new WindowChart();
            wChart.Name = "window" + (panels.Count + 1).ToString();
            lpl.Caption = wChart.Name;
            lpl.Content = wChart;
            lpl.Content = wChart;
            DockPanels.Add(lpl);
            panels.Add(lpl);
            Windows.Add(wChart);
        }

        public void RemoveWindowAt(int WindowIndex) 
        {

            //Windows.RemoveAt(WindowIndex);
            ///have not remove panel
            
            
            DockPanels.Remove(panels[WindowIndex]);
            panels.RemoveAt(WindowIndex);
            Windows.RemoveAt(WindowIndex);
        }
        public WindowChart GetWindow(int index)
        {
            return Windows[index];
        }
        public int Count
        {
            get
            {
                return Windows.Count;
            }
        }
        public void MaxWindow(int windowIndex) { }
        public void Restore() { }

        public void SaveChartImage(string FileName)
        {

            //Width = 1024;
            //Height = 768;
            
            
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(DesiredSize));
            UpdateChart();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            
            //for (int i = 0; i < 1000; i++)
            //{

            //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            //}

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.gridChart.ActualWidth, (int)this.gridChart.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            //RenderTargetBitmap bmp = new RenderTargetBitmap(1024, 768, 96d, 96d, PixelFormats.Pbgra32);
            bmp.Render(this.gridChart);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            System.IO.FileStream fs = new System.IO.FileStream(FileName, System.IO.FileMode.Create);
            encoder.Save(fs);

            fs.Close();

        }

        public byte[] SaveChartImage()
        {
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(DesiredSize));
            //UpdateLayout();
            UpdateChart();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.gridChart.ActualWidth, (int)this.gridChart.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            bmp.Render(this.gridChart);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            MemoryStream ms = new MemoryStream();
            encoder.Save(ms);
            byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
            ms.Close();

            return bytes;


        }


        public List<Measurement> MeasurementList { get; set; }
        public bool IsTracesUpdate { get; set; }
        public event Action<int> TraceAdded;

        public void ClearChart()
        {
            Windows.Clear();
            DockPanels.Clear();
            panels.Clear();
        }
        /// <summary>
        /// 更新所有Window和Trace等的设置到界面
        /// </summary>
        public void UpdateChart() 
        {
            
            foreach (var window in Windows)
            {
                window.UpdateChart();
            }
        }
        /// <summary>
        /// 更新trace中的数据
        /// </summary>
        public void UpdateData() 
        {
            
            foreach (var window in Windows)
            {
                window.UpdateData();
            }
        }

        private void NewWindow_ItemClick(object sender, RoutedEventArgs e)
        {
            AddWindow();
        }

        private void MaxWindowr_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Restore_ItemClick(object sender, RoutedEventArgs e)
        {

        }
    }

}
