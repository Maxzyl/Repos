using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace SymtChartLib
{
    public class Trace
    {
        public Trace()
        {
            Markers = new List<Marker>();
            Cursors = new List<NationalInstruments.Controls.Cursor>();
            LimitLineList = new List<LimitLine>();
            DivisionCount = 10;
            ReferenceLevel = 0;
            Scale = 10;
            ReferencePosition = 5;
            AxisIndex = 0;
        }
        //public string Name { get; set; }
        /// <summary>
        /// 迹线标题,dp
        /// </summary>
        public string Title { get; set; }
        public List<LimitLine> LimitLineList { get; set; }
        /// <summary>
        /// 门限迹线是否显示,dp
        /// </summary>
        public bool LimitLineDisplayEnable { get; set; }
        /// <summary>
        /// 是否打开门限测试功能,dp
        /// </summary>
        public bool LimitTestEnable { get; set; }
        /// <summary>
        /// 数据单位,dp
        /// </summary>
        public string Unit { get; set; }
        //public string Format { get; set; }

        /// <summary>
        /// 每个Trace下面的Cursor
        /// </summary>
        public List<NationalInstruments.Controls.Cursor> Cursors { get; set; }
        public List<Marker> Markers { get; set; }
        /// <summary>
        /// dp,是否显示Memory迹线
        /// </summary>
        public bool MemoryDisplayEnable { get; set; }
        public void DataToMemory() { }
        /// <summary>
        /// dp
        /// </summary>
        public double Scale { get; set; }
        public double ReferenceLevel { get; set; }
        public int ReferencePosition { get; set; }
        public int DivisionCount { get; set; }
        //public void AutoScale() { }
        public SolidColorBrush Color { get; set; }
        public Point[] TraceData { get; set; }
        /// <summary>
        /// Axis index of trace in window
        /// 
        /// </summary>
        public int AxisIndex { get; set; }
        //public int WindowIndex { get; set; }
        /// <summary>
        /// measurement index of this trace bindded when trace bind to measurement
        /// if MeasIndex == -1, trace is not binded to any measurment
        /// </summary>
        public int MeasIndex { get; set; }
        public bool PassFail
        {
            get
            {
                return true;
            }
        }
        public event Action<int> MarkerAdded;
        public event Action PassFailUpdate;

        public double? UpLimit { get; set; }
        public double? LowLimit { get; set; }
    }
    public struct LimitLine
    {
        public double X1;
        public double X2;
        public double Y1;
        public double Y2;
        public LimitLineTypeEnum Type { get; set; }
    }
    
    public enum LimitLineTypeEnum
    {
        None,Max, Min
    }
    public class Marker
    {
        public string DisplayName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public void Search(MarkerSearchTypeEnum searchType)
        {

        }
        public Range SearchRange { get; set; }
        public bool MarkerSearchRangeEnable { get; set; }
    }
    public enum MarkerSearchTypeEnum
    {
        Max,Min
    }
    public struct Range
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
    }
    /// <summary>
    /// 窗口的横坐标，一个窗口可以有几个横坐标。不同的Trace对应不同的横坐标。
    /// 一个或几个trace可以共用相同的XAxis
    /// </summary>
    public class XAxis
    {
        public string ToolTip { get; set; }
        public string Start { get; set; }
        public string Stop { get; set; }
        public string Unit { get; set; }
        public string Center { get; set; }
    }
}
