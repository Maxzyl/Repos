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

using System.Collections.ObjectModel;
using NationalInstruments.Controls;
using NationalInstruments.Controls.Primitives;
using NationalInstruments.Controls.Rendering;
using System.Windows.Threading;
using System.Collections;
using System.IO;

namespace SymtChartLib
{
    /// <summary>
    /// Interaction logic for WindowChart.xaml
    /// </summary>
    public partial class WindowChart : UserControl
    {

        //private List<object> portlist = new List<object>();
        Point oldPoint = new Point();
        ContextMenu cursorMenu = new ContextMenu();
        bool isMove = false;
        List<Point[]> pointlist = new List<Point[]>();
        List<Point[]> _limitPointlist = new List<Point[]>();
        public List<Trace> Traces { get; set; }
        public SolidColorBrush[] Brushes { get; set; }
        public string Name { get; set; }
        public List<XAxis> XAxisList { get; set; }
        Trace _LastSelectedTrace = null;
        Plot _SelectedPlot = null;
        NationalInstruments.Controls.Cursor _SelectedCursor = new NationalInstruments.Controls.Cursor();
        int SelectedIndex;
        bool isSingleTrace;
        int coupleIndex;
        public WindowChart()
        {
            InitializeComponent();
            Brushes =new SolidColorBrush[]
            { 
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(255, 0, 128))),
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(0, 155, 255))),
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(255,144,0))),
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(5,255,0))),
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(255, 0, 229))),
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(0,0,255))),
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(255,245,0))),
                new SolidColorBrush(GetMediaColorFromDrawingColor(System.Drawing.Color.FromArgb(128, 255, 182))),
            };
            MenuItem item1 = new MenuItem() { Header = "Marker Off" };
            MenuItem item2 = new MenuItem() {Header="Search Max"};
            MenuItem item3 = new MenuItem() {Header="Search Min"};
            item1.Click += MenuItemClick;
            item2.Click += MenuItemClick;
            item3.Click += MenuItemClick;
            cursorMenu.Items.Add(item1);
            cursorMenu.Items.Add(item2);
            cursorMenu.Items.Add(item3);
            Traces = new List<Trace>();
            XAxisList = new List<XAxis>();
            XAxisList.Add(new XAxis() {Start="1G", Stop="100G",Center="0dbm"});
            XAxisList.Add(new XAxis() { Start = "1.2G", Stop = "1200G", Center = "0dbm" });
            this.lstAxis.ItemsSource = XAxisList;
          
        }

        

        private System.Windows.Media.Color GetMediaColorFromDrawingColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 更新所有Trace等的设置到界面
        /// </summary>
        public void UpdateChart()
        {
            clearData();

            graph.RenderMode = RenderMode.Hardware;
            this.lstYxis.ItemsSource = Traces;
        }
        private NationalInstruments.Controls.Cursor GetCursor()
        {    
            NationalInstruments.Controls.Cursor cursor = new NationalInstruments.Controls.Cursor();
            cursor.SnapToData = true;  
            
            cursor.AllowablePlots = NationalInstruments.Controls.PlotsToSearch.Single;
            cursor.LabelVisibility = Visibility.Visible;
            cursor.ValueVisibility = Visibility.Hidden;
            cursor.HorizontalCrosshairLength = 0;
            cursor.VerticalCrosshairLength = 0;
            
            cursor.TargetShape = PointShape.OutwardTriangle;
            cursor.TargetSize = new Size(20, 20);
            cursor.PositionChanged += Cursor_PositionChanged;
            cursor.PreviewMouseRightButtonDown += Cursor_PreViewMouseRightButtonDown;
            cursor.PreviewMouseLeftButtonDown += Cursor_PreViewMouseLeftButtonDown;

            cursor.ContextMenu = cursorMenu;
            return cursor;
        }
        private void MenuItemClick(object sender,EventArgs e)
        {
            Plot plot = _SelectedCursor.Plot as Plot;
            int pIndex = graph.Plots.IndexOf(plot);
            Trace tr = Traces[pIndex];
            int markerIndex=0;
            if ((sender as MenuItem).Header.ToString() == "Marker Off")
            {       
                   foreach(var traceCursor in tr.Cursors)
                   {
                       if(_SelectedCursor.Equals(traceCursor))
                       {
                           markerIndex = tr.Cursors.IndexOf(traceCursor);
                       }
                   }
                   RemoveMarker(pIndex, markerIndex);
            }
            else if ((sender as MenuItem).Header.ToString() == "Search Max")
            {
                SearchMarker(pIndex,MarkerSearchTypeEnum.Max);
            }
            else if ((sender as MenuItem).Header.ToString() == "Search Min")
            {
                SearchMarker(pIndex, MarkerSearchTypeEnum.Min);
            }
        }
        private void Cursor_PositionChanged(object sender, EventArgs e)
        {
            var v = (sender as NationalInstruments.Controls.Cursor).Value;
            var cursor = sender as NationalInstruments.Controls.Cursor;
      
            if(cursor.Index !=null)
            {
                int index = Convert.ToInt32(cursor.Index);
                Plot p = cursor.Plot as Plot;
                int pIndex = graph.Plots.IndexOf(p);
                Trace tr = Traces[pIndex];
                cursor.Tag = tr.TraceData[index].X + "~" + tr.TraceData[index].Y;
                int markerIndex = Traces[pIndex].Cursors.IndexOf(cursor);
                if(markerIndex>=0)
                {
                   Traces[pIndex].Markers[markerIndex].X = tr.TraceData[index].X;
                   Traces[pIndex].Markers[markerIndex].Y=tr.TraceData[index].Y;
                }
            }
        }
        private void Cursor_PreViewMouseRightButtonDown(object sender,MouseButtonEventArgs e)
        {
            _SelectedCursor = sender as NationalInstruments.Controls.Cursor;
            cursorMenu.IsOpen = true;
        }
        private void Cursor_PreViewMouseLeftButtonDown(object sender,MouseButtonEventArgs e)
        {
            _SelectedCursor = sender as NationalInstruments.Controls.Cursor;
            foreach(var item in graph.Children)
            {
                if(item as NationalInstruments.Controls.Cursor !=null)
                {
                    var cursor =item as NationalInstruments.Controls.Cursor;
                    if(cursor.Equals(_SelectedCursor))
                    {
                        _SelectedCursor.TargetShape = PointShape.InwardTriangle;
                        _SelectedCursor.ToolTip = ">";
                    }
                    else
                    {
                        cursor.TargetShape = PointShape.OutwardTriangle;
                        cursor.ToolTip = "";
                    }
                }
            }
        }
        /// 清除trace里面的数据，但是横纵坐标不动
        /// </summary>
        public void ClearTraceInfo()
        {
            Traces.Clear();
            Clear();
        }
        /// <summary>
        /// 全部清除
        /// </summary>
        public void clearData()
        {
            Clear();
            graph.Axes.Clear();
        }
        private void Clear()
        {
            SelectedIndex = 0;
            graph.DataSource = null;
            listMarker.ItemsSource = null;
            lstYxis.ItemsSource = null;
            lstAxis.SelectedIndex = -1;
            for (int i = graph.Children.Count(); i > 0; i--)
            {
                graph.Children.RemoveAt(i - 1);
            }
            pointlist.Clear();
            graph.Data.Clear();
            graph.Plots.Clear();
        }

        /// <summary>
        /// 只更新数据源
        /// </summary>
        public void UpdatePointData(int traceIndex)
        {
            if (Traces.Count() == 0) return;
            isCouple.IsChecked = false;
            graph.RenderMode = RenderMode.Hardware;
            this.lstYxis.ItemsSource = Traces;
            List<Point[]> dataSource = new List<Point[]>();
            List<AxisDouble> axisDouble = new List<AxisDouble>();
            listMarker.ItemsSource = null;
            int index = 0;
            foreach (var tr in Traces)
            {
                if (index > 7)
                {
                    index = 0;
                }
                dataSource.Add(ProcessData(tr.TraceData));
                Plot p = new Plot() { Renderer = new LinePlotRenderer() { Stroke = Brushes[index], StrokeThickness = 1.5 } };
                tr.Color = (SolidColorBrush)(p.Renderer as LinePlotRenderer).Stroke;
                graph.Plots.Add(p);
                foreach (Marker marker in tr.Markers)
                {
                    NationalInstruments.Controls.Cursor cursor = GetCursor();
                    cursor.Plot = p;

                    graph.Children.Add(cursor);
                    double x1 = tr.TraceData.Select(x => x.X).Min();
                    double x2 = tr.TraceData.Select(x => x.X).Max();
                    cursor.SetDataPosition(new List<double>() { (marker.X - x1) / (x2 - x1), marker.Y });
                    cursor.Tag = marker.DisplayName;
                    cursor.Label = new Label() { Content = tr.Markers.IndexOf(marker), Foreground = (p.Renderer as LinePlotRenderer).Stroke };
                    cursor.CrosshairBrush = (p.Renderer as LinePlotRenderer).Stroke;
                }
                p.VerticalScale = graph.Axes[traceIndex];
                index++;
            }
            getlimitPoint(Traces[traceIndex]);
            listMarker.ItemsSource = graph.Children;
            _LastSelectedTrace = Traces[traceIndex];
            if (_limitPointlist.Count() > 0)
            {
                foreach (var points in _limitPointlist)
                {
                    dataSource.Add(ProcessData(points));
                    Plot p = new Plot() { Renderer = new LinePlotRenderer() { Stroke = new SolidColorBrush(Colors.Red) } };
                    graph.Plots.Add(p);
                }
            }
            graph.DataSource = dataSource;
            lstYxis.SelectedItem = Traces[traceIndex];
            isCouple.IsChecked = true;
        }

        /// <summary>
        /// 更新trace中的数据
        /// </summary>
        public void UpdateData()
        {
            if (Traces.Count == 0) return;
            isCouple.IsChecked = false;
            graph.RenderMode = RenderMode.Hardware;
            this.lstYxis.ItemsSource = Traces;
            List<Point[]> dataSource = new List<Point[]>();
            List<AxisDouble> axisDouble = new List<AxisDouble>();
            int i = 0;
            foreach (var tr in Traces)
            {
                if (i > 7)
                {
                    i = 0;
                }
                dataSource.Add(ProcessData(tr.TraceData));
                Plot p = new Plot() { Renderer = new LinePlotRenderer() { Stroke = Brushes[i], StrokeThickness = 1.5 } };
                tr.Color = (SolidColorBrush)(p.Renderer as LinePlotRenderer).Stroke;
                AxisDouble axis = getAxis(tr.Title);
                graph.Axes.Add(axis);
                p.VerticalScale = axis;
                graph.Plots.Add(p);
                foreach (Marker marker in tr.Markers)
                {
                    NationalInstruments.Controls.Cursor cursor = GetCursor();
                    cursor.Plot = p;

                    graph.Children.Add(cursor);
                    double x1 = tr.TraceData.Select(x => x.X).Min();
                    double x2 = tr.TraceData.Select(x => x.X).Max();
                    cursor.SetDataPosition(new List<double>() { (marker.X - x1) / (x2 - x1), marker.Y });
                    cursor.Tag = marker.DisplayName;
                    cursor.Label = new Label() { Content = tr.Markers.Count, Foreground = (p.Renderer as LinePlotRenderer).Stroke };
                    cursor.CrosshairBrush = (p.Renderer as LinePlotRenderer).Stroke;
                }
                i++;
            }

            listMarker.ItemsSource = graph.Children;
            getlimitPoint(Traces[SelectedIndex]);
            _LastSelectedTrace = Traces[SelectedIndex];
            if (_limitPointlist.Count() > 0)
            {
                foreach (var points in _limitPointlist)
                {
                    dataSource.Add(ProcessData(points));
                    Plot p = new Plot() { Renderer = new LinePlotRenderer() { Stroke = new SolidColorBrush(Colors.Red) } };
                    graph.Plots.Add(p);
                }
            }
            graph.DataSource = dataSource;
            //for (int traceIndex = 0; traceIndex < Traces.Count; traceIndex++)
            //{
            //    lstYxis.SelectedIndex = traceIndex;
            //}
            _LastSelectedTrace = Traces[SelectedIndex];          
            lstYxis.SelectedIndex = SelectedIndex;
            graph.AllScales.Last().Visibility = Visibility.Collapsed;
            this.lstAxis.ItemsSource = XAxisList;
            isCouple.IsChecked = true;
            scaleCouple();
        }

        /// <summary>
        /// 获取每个trace对应的纵坐标
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private AxisDouble getAxis(string traceTitle)
        {
            AxisDouble axis = new AxisDouble()
            {
                Orientation = Orientation.Vertical,
                MajorGridLines = new GridLines() { Stroke = new SolidColorBrush(Colors.White), Visibility = Visibility.Collapsed },
                MinorGridLines = new GridLines() { Visibility = Visibility.Collapsed },
                Name = traceTitle,
                Adjuster = RangeAdjuster.FitLoosely,
                Visibility = Visibility.Collapsed
            };
            axis.RangeChanged += Axis_RangeChanged;
            return axis;
        }


        //private void clearData()
        //{
        //    graph.DataSource = null;
        //    listMarker.ItemsSource = null;
        //    lstYxis.ItemsSource = null;
        //    lstAxis.SelectedIndex = -1;
        //    for (int i = graph.Children.Count(); i > 0; i--)
        //    {
        //        graph.Children.RemoveAt(i-1);
        //    }
        //    pointlist.Clear();
        //    graph.Data.Clear();
        //    graph.Plots.Clear();
        //    graph.Axes.Clear();
        //}

  
        /// <summary>
        /// 归一化X轴的数据到0和1之间
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Point[] ProcessData(Point[] v)
        {
            //const int BestPointCount = 1000;
            if (v.Count() == 0)
                return null;
            else if (v.Count() == 1)
            {
                Point[] v1 = new Point[2];
                v1[0].X = 0;
                v1[0].Y = v.First().Y;
                v1[1].X = 1;
                v1[1].Y = v.First().Y;
                return v1;
            }
            else
            {
                double[] xList = v.Select(x => x.X).ToArray();
                double[] yList = v.Select(x => x.Y).ToArray();
                double xMax = xList.Max();
                double xMin = xList.Min();
                double[] newxList = new double[xList.Count()];

                Point[] v1 = new Point[xList.Count()];
                for (int i = 0; i < xList.Count(); i++)
                {
                    v1[i].X = (xList[i] - xMin) / (xMax - xMin);
                    v1[i].Y = yList[i];
                }
                return v1;
            }
        }

        private void lstYxis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstYxis.SelectedItem !=null)
            {   
                SelectedIndex = lstYxis.SelectedIndex;
                Trace trace = lstYxis.SelectedItem as Trace;
                if(_LastSelectedTrace==null)return;
                if (_LastSelectedTrace != null)
                {
                    for (int i = 0; i < _LastSelectedTrace.LimitLineList.Count; i++)
                    {
                        graph.Plots.RemoveAt(graph.Plots.Count - 1);
                    }
                }
                _LastSelectedTrace = trace;
                AxisDouble yAxis = new AxisDouble();
                int index = Traces.IndexOf(trace);
                if(isCouple.IsChecked == false)
                {
                    for (int i = 0; i < graph.Plots.Count; i++)
                    {
                        if (i == index)
                        {
                            graph.Axes[i].Visibility = Visibility.Visible;
                            yAxis = graph.Axes[i] as AxisDouble;
                        }
                        else
                        {
                            graph.Axes[i].Visibility = Visibility.Collapsed;
                            AxisDouble axis = graph.Axes[i] as AxisDouble;

                            axis.MinorGridLines = new GridLines() { Visibility = Visibility.Hidden };
                            axis.MajorGridLines = new GridLines() { Visibility = Visibility.Hidden };
                        }
                    }
                }
                else
                {
                    if (coupleIndex >= Traces.Count)
                    {
                        for (int i = 0; i < graph.Plots.Count; i++)
                        {
                            if (i == index)
                            {
                                graph.Axes[i].Visibility = Visibility.Visible;
                                yAxis = graph.Axes[i] as AxisDouble;
                                foreach (Plot plot in graph.Plots)
                                {
                                    plot.VerticalScale = graph.Axes[i];
                                }
                                coupleIndex = index;
                            }
                            else
                            {
                                graph.Axes[i].Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
                int axisIndex = trace.AxisIndex;
                for (int i = 0; i < XAxisList.Count; i++)
                {
                    if (i == axisIndex)
                    {
                        XAxisList[i].ToolTip = ">";
                    }
                    else
                    {
                        XAxisList[i].ToolTip = "";
                    }
                }
                lstAxis.ItemsSource = null;
                lstAxis.ItemsSource = XAxisList;
                this.lstAxis.SelectedIndex = axisIndex;
                UpdateDataSource();
                yAxis.MajorDivisions = new RangeLabeledDivisions() { LabelVisibility = Visibility.Visible, Mode = RangeDivisionsMode.CreateCountMode(trace.DivisionCount + 1) };
                yAxis.MinorGridLines = new GridLines() { Stroke = new SolidColorBrush(Colors.White), Visibility = Visibility.Collapsed, StrokeThickness = 0.5 };
                yAxis.MajorGridLines = new GridLines() { Stroke = new SolidColorBrush(Colors.White), Visibility = Visibility.Visible, StrokeThickness = 0.5 };
                double X = trace.ReferenceLevel - trace.Scale * trace.ReferencePosition;
                double Y = trace.ReferenceLevel + trace.Scale * (trace.DivisionCount - trace.ReferencePosition);
                if (X != Y)
                {
                    yAxis.Range = new Range<double>(X, Y);
                    graph.Axes[index].Adjuster = RangeAdjuster.None;
                }
                _SelectedPlot = graph.Plots[SelectedIndex];
                foreach (Plot p in graph.Plots)
                {
                    ((LinePlotRenderer)p.Renderer).StrokeThickness = 1.0;
                }
                ((LinePlotRenderer)_SelectedPlot.Renderer).StrokeThickness = 2.0;
            }
        }
        private void getlimitPoint(Trace _selectedTrace)
        {
            _limitPointlist.Clear();
            foreach (LimitLine LL in _selectedTrace.LimitLineList)
            {
                Point[] Limitpoints = new Point[2];
                Limitpoints[0] = new Point() { X = LL.X1, Y = LL.Y1 };
                Limitpoints[1] = new Point() { X = LL.X2, Y = LL.Y2 };
                _limitPointlist.Add(Limitpoints);
            }      
        }

        private void listMarker_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove) 
            {
                FrameworkElement currEle = sender as FrameworkElement;
                double xPos = oldPoint.X - e.GetPosition(null).X + (double)currEle.GetValue(Canvas.RightProperty);
                double yPos = e.GetPosition(null).Y - oldPoint.Y + (double)currEle.GetValue(Canvas.TopProperty);
                currEle.SetValue(Canvas.RightProperty, xPos);
                currEle.SetValue(Canvas.TopProperty, yPos);

                oldPoint = e.GetPosition(null);
            }
        }

        private void listMarker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMove = false;
        }

        private void listMarker_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMove = true;
            oldPoint = e.GetPosition(null);
        }

        private void Axis_RangeChanged(object sender, ValueChangedEventArgs<Range<double>> e)
        {
            AxisDouble yAxis = sender as AxisDouble;
            if(lstYxis.SelectedItem !=null)
            {   
                Trace trace = lstYxis.SelectedItem as Trace;
                if (yAxis.Range.Minimum == trace.ReferenceLevel - trace.Scale * trace.ReferencePosition && yAxis.Range.Maximum == trace.ReferenceLevel + trace.Scale * (trace.DivisionCount - trace.ReferencePosition))
                {
                }
                else
                {
                    if(trace.DivisionCount !=0)
                    {
                        trace.Scale = (yAxis.Range.Maximum - yAxis.Range.Minimum) / trace.DivisionCount;
                        trace.ReferenceLevel = yAxis.Range.Minimum + (yAxis.Range.Maximum - yAxis.Range.Minimum) * trace.ReferencePosition / trace.DivisionCount;
                    }
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {    
             MenuItem item=sender as MenuItem;
             Trace tr = lstYxis.SelectedItem as Trace;
             int index = Traces.IndexOf(tr);
             if(item.Header.ToString()=="AutoScale")
             {
                 AutoScale(index);
             }
             else if (item.Header.ToString() == "Add Marker")
             {   
                 if(lstYxis.SelectedItem !=null)
                 {
                     AddMarker(index);
                 }
             }
            else if (item.Header.ToString() == "Delete Trace")
            {
                DeleteTrace(index);
            }
            else if (item.Header.ToString() == "Delete All Trace")
            {
                Traces.Clear();
                clearData();
            }
            else if (item.Header.ToString() == "Scale Couple")
            {
                scaleCouple();
            }
            else if (item.Header.ToString() == "Save Chart")
            {
                SaveChartImage("./1.jpg");
            }
            else if (item.Header.ToString() == "Set up")
            {
                WindowSetUp windowSetUp = new WindowSetUp();
                windowSetUp.Show();
            }
        }
        public void AddMarker(int traceIndex)
        {
            Trace tr=Traces[traceIndex];
            double i = Math.Floor(Convert.ToDouble((tr.TraceData.Select(x => x.X).Count()) / 2));
          //  Marker marker = new Marker() { DisplayName = tr.Title + "-" + "Marker" + tr.Markers.Count + 1, X = tr.TraceData[Convert.ToInt32(i)].X, Y = tr.TraceData[Convert.ToInt32(i)].Y };
            Marker marker = new Marker();
            NationalInstruments.Controls.Cursor cursor = GetCursor();
            tr.Cursors.Add(cursor);
            tr.Markers.Add(marker);
            Plot p=graph.Plots[traceIndex];
            cursor.Plot = p;
            graph.Children.Add(cursor);
            double x1 = tr.TraceData.Select(x => x.X).Min();
            double x2 = tr.TraceData.Select(x => x.X).Max();
            cursor.SetDataPosition(new List<double>() { (marker.X - x1) / (x2 - x1), marker.Y });
          //  string xStr = Convert.ToString((new FreqStringConverter()).Convert(tr.TraceData[Convert.ToInt32(i)].X, null, null, null)); 
            cursor.Tag = tr.TraceData[Convert.ToInt32(i)].X + "~" + tr.TraceData[Convert.ToInt32(i)].Y;
            cursor.LabelAlignment = PointAlignment.MiddleCenter;
            cursor.Label = new Label() { Content = tr.Markers.Count, Foreground = (p.Renderer as LinePlotRenderer).Stroke };
            cursor.CrosshairBrush = (p.Renderer as LinePlotRenderer).Stroke;
            if(isSingleTrace && _SelectedPlot != null)
            {
                List<NationalInstruments.Controls.Cursor> cursors = new List<NationalInstruments.Controls.Cursor>();
                foreach (NationalInstruments.Controls.Cursor c in graph.Children)
                {
                    if (c.Plot != _SelectedPlot)
                    {
                        c.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        cursors.Add(c);
                    }
                }
                listMarker.ItemsSource = cursors;
            }
          
            
        }
        public void RemoveMarker(int traceIndex, int markerIndex)
        {
            graph.Children.Remove(_SelectedCursor);
            Plot p = graph.Plots[traceIndex];
            Traces[traceIndex].Markers.RemoveAt(markerIndex);
            Traces[traceIndex].Cursors.RemoveAt(markerIndex);
            foreach(var cursor in Traces[traceIndex].Cursors)
            {   
                int i=Traces[traceIndex].Cursors.IndexOf(cursor) +1;
                cursor.Label = new Label() { Content = i, Foreground = (p.Renderer as LinePlotRenderer).Stroke };
            }
        }
        public void AutoScale(int traceIndex)
        {  
            graph.Axes[traceIndex].Adjuster = RangeAdjuster.FitLoosely;
            graph.Refresh();
            graph.Axes[traceIndex].Adjuster = RangeAdjuster.None;
        }
        public void SearchMarker(int traceIndex,MarkerSearchTypeEnum searchType)
        {
            double Y=0;
            if (searchType == MarkerSearchTypeEnum.Max)
            {
                Y = Traces[traceIndex].TraceData.Select(y => y.Y).Max();
            }
            else if(searchType==MarkerSearchTypeEnum.Min)
            {
                Y = Traces[traceIndex].TraceData.Select(y => y.Y).Min();
            }
            double x1 = Traces[traceIndex].TraceData.Select(x => x.X).Min();
            double x2 = Traces[traceIndex].TraceData.Select(x => x.X).Max();
            Point[] points = Traces[traceIndex].TraceData.Where(data => data.Y == Y).ToArray();
            if (x1 != x2)
            {
                _SelectedCursor.SetDataPosition(new List<double>() { (points[0].X - x1) / (x2 - x1), Y });
            }
        }
        public void DeleteTrace(int traceIndex)
        {
            graph.Plots.RemoveAt(traceIndex);
            foreach (var cursor in Traces[traceIndex].Cursors)
            {
               if(graph.Children.Contains(cursor))
               {
                   graph.Children.Remove(cursor);
               }
            }
            //for (int i = 0; i < Traces[traceIndex].LimitLineList.Count; i++)
            //{
            //    graph.Plots.RemoveAt(graph.Plots.Count - 1);
            //}
            Traces.RemoveAt(traceIndex);
            graph.Axes.RemoveAt(traceIndex);
            lstYxis.ItemsSource = null;
            lstYxis.ItemsSource = Traces;
            if (Traces.Count > 0)
            {
                lstYxis.SelectedIndex = 0;
            }
            else
            {
                clearData();
            }
          
        }
        private void UpdateDataSource()
        {
            if (Traces.Count == 0)
            {
                graph.DataSource = null;
                return;
            }
            List<Point[]> dataSource = new List<Point[]>();
            foreach(var tr in Traces)
            {
               dataSource.Add(ProcessData( tr.TraceData));
            }
            getlimitPoint(Traces[SelectedIndex]);
            if (_limitPointlist.Count() > 0)
            {
                foreach (var points in _limitPointlist)
                {
                    dataSource.Add(ProcessData(points));
                    Plot p = new Plot() { Renderer = new LinePlotRenderer() { Stroke = new SolidColorBrush(Colors.Red) } };
                    if (isCouple.IsChecked == false)
                    {
                        p.VerticalScale = graph.Axes[SelectedIndex];
                    }
                    else
                    {
                        p.VerticalScale = graph.Axes[coupleIndex];
                    }
                    graph.Plots.Add(p);
                }
            }
            foreach (AxisDouble axis in graph.Axes)
            {
                axis.RangeChanged -= Axis_RangeChanged;
            }
            graph.DataSource = dataSource;
            foreach (AxisDouble axis in graph.Axes)
            {
                axis.RangeChanged += Axis_RangeChanged;
            }
            graph.AllScales.Last().Visibility = Visibility.Collapsed; 
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
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                result = new RenderTargetBitmap((int)FE.Width, (int)FE.Height, 96, 96, PixelFormats.Pbgra32);
                result.Render(FE);
                (_window.Content as ScrollViewer).Content = null;
                Parent.Children.Add(FE);
                _window.Close();
                action.Invoke(result);
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle).Wait();
        }

        public void SaveChartImage(string FileName)
        {
            Size size = new Size(1300, 600);
            Measure(size);
            Arrange(new Rect(DesiredSize));
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));


            RenderTargetBitmap bmp = new RenderTargetBitmap((int)gridChart.ActualWidth, (int)gridChart.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
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
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.gridChart.ActualWidth, (int)this.gridChart.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            bmp.Render(this.gridChart);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            MemoryStream ms = new MemoryStream();
            encoder.Save(ms);
            byte[] bytes = ms.GetBuffer();
            ms.Close();

            return bytes;


        }

        private static readonly GraphQueryArgs query = new GraphQueryArgs(
                                PlotsToSearch.Any, SearchDimensions.HorizontalAndVertical,
                                SearchDirections.ForwardAndReverse, isInclusive: true); 
        private void graph_PlotAreaMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            if(graph.Plots.Count()==0)return;
            Point screenPosition = e.GetPosition(graph);
            Point relativePosition = graph.ScreenToRelative(screenPosition);
            PlotValue nearestValue = graph.FindNearestValue(graph.Plots[0], relativePosition, query);
            IPlot plot1 = nearestValue.PlotObserver;
            var plot = (Plot)graph.Plots[plot1.Index];
            if(plot == null || plot == _SelectedPlot)return;
            _SelectedPlot = plot;  
            foreach(Plot p in graph.Plots)
            {
                ((LinePlotRenderer)p.Renderer).StrokeThickness = 1.0;
            }
            ((LinePlotRenderer)_SelectedPlot.Renderer).StrokeThickness = 2.0;
            int index = graph.Plots.IndexOf(plot);
            lstYxis.SelectedIndex = index;           
            
        }
        private void graph_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {   
          
        }

        private void lstYxis_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (isSingleTrace)
            {
                foreach (Plot plot in graph.Plots)
                {
                    plot.Visibility = Visibility.Visible;
                }
                foreach (NationalInstruments.Controls.Cursor cursor in graph.Children)
                {
                    cursor.Visibility = Visibility.Visible;
                }
                listMarker.ItemsSource = graph.Children;
                isSingleTrace = false;
            }
            else
            {
                if (_SelectedPlot != null)
                {
                    List<NationalInstruments.Controls.Cursor> cursors = new List<NationalInstruments.Controls.Cursor>();
                    foreach (Plot plot in graph.Plots)
                    {
                        plot.Visibility = Visibility.Collapsed;
                    }
                    foreach (NationalInstruments.Controls.Cursor cursor in graph.Children)
                    {
                        if (cursor.Plot != _SelectedPlot)
                        {
                            cursor.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            cursors.Add(cursor);
                        }
                    }
                    listMarker.ItemsSource = cursors;
                    _SelectedPlot.Visibility = Visibility.Visible;
                    if (_LastSelectedTrace != null)
                    {
                        for (int i = 0; i < _LastSelectedTrace.LimitLineList.Count; i++)
                        {
                            graph.AllPlots[graph.Plots.Count - 1 - i].Visibility = Visibility.Visible;
                        }
                    }
                    isSingleTrace = true;
                }
            }
        }

        private void scaleCouple()
        {
            if (isCouple.IsChecked && lstYxis.SelectedItem != null)
            {   
                var axise = graph.Axes[SelectedIndex];
                foreach(Plot plot in graph.Plots)
                {
                    plot.VerticalScale = axise;
                }
                coupleIndex = SelectedIndex;
            }
            else if(isCouple.IsChecked == false && lstYxis.SelectedItem !=null)
            {
                for (int k = 0; k < graph.Axes.Count;k++ )
                {
                    int limitCount = Traces[SelectedIndex].LimitLineList.Count();
                    graph.Plots[k].VerticalScale = graph.Axes[k];
                    for (int i = 0; i < graph.Plots.Count - limitCount; i++)
                    {
                        if (i == SelectedIndex)
                        {
                            graph.Axes[i].Visibility = Visibility.Visible;
                            AxisDouble axis = graph.Axes[i] as AxisDouble;
                        }
                        else
                        {
                            graph.Axes[i].Visibility = Visibility.Collapsed;
                            AxisDouble axis = graph.Axes[i] as AxisDouble;
                        }
                    }
                }
            }
        }
    }
}
