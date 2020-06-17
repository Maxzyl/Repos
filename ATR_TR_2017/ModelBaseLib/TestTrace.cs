using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symtant.GeneFunLib;
namespace ModelBaseLib
{
    [Serializable]
    public class TestTrace:TestItem
    {
        public TestTrace()
        {
            TestSpecList = new List<TestTraceSpec>();
            IsTest = true;
            DivCount = 10;
            XAxisInfo = new XAxisInfo();
            ResultData = new XYDataArr();
        }
        //public string TypeName { get; set; }
        //public double Compensation { get; set; }
        
        //public TraceResult TestResult { get; set; }
        
        /// <summary>
        /// 测试结果的名称重新定义，例如把S11定义成驻波
        /// </summary>
        public List<TestTraceSpec> TestSpecList { get; set; }
        /// <summary>
        /// 所有Trace上的点都添加为Marker，这些Marker不会显示在界面上,
        /// 这些Marker的Limit使用Trace的Limit
        /// </summary>
        public bool IsAllTracePointMarker { get; set; }
        //[System.Xml.Serialization.XmlIgnore]
        //public bool IsTest { get; set; }
        //[System.Xml.Serialization.XmlIgnore]
        //[Newtonsoft.Json.JsonIgnore]
        //public bool? PassFail { get; set; }
        //public string Unit { get; set; }
        public void CalcInfo()
        {

            if (ResultData == null || ResultData.Count == 0)
                return;
            for (int i = 0; i < ResultData.Count(); i++)
            {
                ResultData[i] = new XYData() { X = ResultData[i].X, Y = ResultData[i].Y + Compensation };
            }
            foreach (var spec in TestSpecList)
            {
                foreach (var marker in spec.TestMarkerList)
                {
                    #region
                    if (marker is XYTestMarker)
                    {
                        var xyMarker = marker as XYTestMarker;
                        xyMarker.MarkerResult.Clear();
                        switch (xyMarker.Type)
                        {
                            case XYTestMarkerTypeEnum.Normal:
                                double[] lstXData = ResultData.Select(x => x.X).ToArray();
                                double[] lstYData = ResultData.Select(x => x.Y).ToArray();

                                double xData = xyMarker.Start;
                                
                                double yData = GeneFun.LinearInterp(lstXData, lstYData, xData);
                                xyMarker.MarkerResult.Add(new XYData(xData, yData));
                                break;
                            case XYTestMarkerTypeEnum.Max:
                                double ymax = ResultData.Select(x => x.Y).Max();
                                double xmax = ResultData.Find(x => x.Y == ymax).X;
                                xyMarker.MarkerResult.Add(new XYData(xmax, ymax));
                                break;
                            case XYTestMarkerTypeEnum.Min:
                                double ymin = ResultData.Select(x => x.Y).Min();
                                double xmin = ResultData.Find(x => x.Y == ymin).X;
                                xyMarker.MarkerResult.Add(new XYData(xmin, ymin));
                                break;
                            case XYTestMarkerTypeEnum.ABSMax:
                                double yAbsmax = ResultData.Select(x => Math.Abs(x.Y)).Max();
                                double xAbsmax = ResultData.Find(x => Math.Abs(x.Y) == yAbsmax).X;
                                xyMarker.MarkerResult.Add(new XYData(xAbsmax, yAbsmax));
                                break;
                            case XYTestMarkerTypeEnum.ABSMin:
                                double yAbsmin = ResultData.Select(x => Math.Abs(x.Y)).Min();
                                double xAbsmin = ResultData.Find(x => Math.Abs(x.Y) == yAbsmin).X;
                                xyMarker.MarkerResult.Add(new XYData(xAbsmin, yAbsmin));
                                break;
                            case XYTestMarkerTypeEnum.Peak:
                                double y1Max = ResultData.Select(x => x.Y).Max();
                                double x1Max = ResultData.Find(x => x.Y == y1Max).X;
                                double y1Min = ResultData.Select(x => x.Y).Min();
                                double x1Min = ResultData.Find(x => x.Y == y1Min).X;
                                if (Math.Abs(y1Max) >= Math.Abs(y1Min))
                                {
                                    xyMarker.MarkerResult.Add(new XYData(x1Max,y1Max));
                                }
                                else
                                {
                                    xyMarker.MarkerResult.Add(new XYData(x1Min,y1Max));
                                }
                                break;
                            case XYTestMarkerTypeEnum.Mean:
                                double avg = ResultData.Select(x => x.Y).Average();
                                double xx = ResultData.Select(x => x.X).First();
                                xyMarker.MarkerResult.Add(new XYData(xx, avg));
                                break;
                            case XYTestMarkerTypeEnum.PhaseRipple:
                                double phaseNolin=GeneFun.CalcPhaseNonlin(ResultData.Select(x => x.Y).ToArray(), 
                                    10000, 0.01);
                                xyMarker.MarkerResult.Add(new XYData(ResultData.Select(x => x.X).First(), phaseNolin));
                                break;
                            case XYTestMarkerTypeEnum.Ripple:
                                double ymin1 = ResultData.Select(x => x.Y).Min();
                                double ymax1 = ResultData.Select(x => x.Y).Max();
                                xyMarker.MarkerResult.Add(new XYData(ResultData.Select(x => x.X).First(), 
                                    ymax1 - ymin1));
                                break;
                            case XYTestMarkerTypeEnum.InRange:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.Target:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.Q_Value:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.BandWidth:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.LeftFreq:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.RightFreq:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.CenterFreq:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.InsertionLoss:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.Pin_NdB:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.Po_NdB:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.RippleInRange:
                                xyMarker.MarkerResult.Add(new XYData(0, 0));
                                break;
                            case XYTestMarkerTypeEnum.RippleHalf:
                                double ymin2 = ResultData.Select(x => x.Y).Min();
                                double ymax2 = ResultData.Select(x => x.Y).Max();
                                xyMarker.MarkerResult.Add(new XYData(ResultData.Select(x => x.X).First(), 
                                    (ymax2 - ymin2)/2));
                                break;
                            case XYTestMarkerTypeEnum.PowerMean:
                                double avg1 = ResultData.Select(x => Math.Pow(10,((x.Y-30)/10))).Average();
                                double xx1 = ResultData.Select(x => x.Y).First();
                                xyMarker.MarkerResult.Add(new XYData(xx1, avg1));
                                break;
                            default:
                                break;
                        }
                    }
                    marker.JudgePassFail();
                    #endregion
                }
                if (!spec.TestLimit.Enable)
                {
                    bool? specPassFail = null;
                    if (spec.UpLimit != null && spec.LowLimit != null)
                    {
                        int upCount = ResultData.Where(x => x.Y > spec.UpLimit).Count();
                        int lowCount = ResultData.Where(x => x.Y < spec.LowLimit).Count();
                        if (upCount > 0 || lowCount > 0)
                        {
                            specPassFail = false;
                        }
                        else
                        {
                            specPassFail = true;
                        }
                    }
                    else if (spec.UpLimit != null && spec.LowLimit == null)
                    {
                        int upCount = ResultData.Where(x => x.Y > spec.UpLimit).Count();
                        if (upCount > 0)
                        {
                            specPassFail = false;
                        }
                        else
                        {
                            specPassFail = true;
                        }

                    }
                    else if (spec.UpLimit == null && spec.LowLimit != null)
                    {
                        int lowCount = ResultData.Where(x => x.Y < spec.LowLimit).Count();
                        if (lowCount > 0)
                        {
                            specPassFail = false;
                        }
                        else
                        {
                            specPassFail = true;
                        }
                    }
                    else
                    {
                        specPassFail = null;
                    }
                    spec.PassFail = GeneFun.NullBoolAnd(GeneFun.NullBoolAndList(spec.TestMarkerList.Select(x => x.PassFail).ToList()), specPassFail);
                }
            }
            PassFail = GeneFun.NullBoolAndList(TestSpecList.Select(x => x.PassFail).ToList());
        }
        //ZYL add this method
        public void UpdateSpecs(int specIndex)
        {
            while (TestSpecList.Count <= specIndex)
            {
                TestSpecList.Add(new TestTraceSpec());
            }
        }
        public void AddDefaultMarker(int specIndex)
        {
            while (TestSpecList.Count <= specIndex)
            {
                TestSpecList.Add(new TestTraceSpec());
            }
            TestSpecList[specIndex].TestMarkerList.Add(new XYTestMarker());
        }
        public virtual TestMarker GetDefaultMarker()
        {
            return new XYTestMarker();
        }
        /// <summary>
        /// Trace隶属的Marker是否固定，如果固定在界面上不能添加和删除Marker
        /// </summary>
        public bool IsFixedMarkers { get; set; }
        /// <summary>
        /// 画图时参考线对应的值
        /// </summary>
        public double RefValue { get; set; }
        public int RefPosition { get; set; }
        public double Scale { get; set; }
        /// <summary>
        /// 画图中分隔的个数
        /// </summary>
        public int DivCount { get; set; }
        public bool IsAutoScale { get; set; }
        public bool IsSaveImage { get; set; }



        public XAxisInfo XAxisInfo { get; set; }

        public XYDataArr ResultData { get; set; }  
    }
    //public class TraceResult
    //{
    //    //public string LimitDescription { get; set; }
    //    //public double YMax { get; set; }
    //    //public double YMin { get; set; }

    //}
    //public class XYTraceResult:TraceResult
    //{
    //    public XYDataArr ResultData { get; set; }
    //    public XYTraceResult()
    //    {
    //        ResultData = new XYDataArr();
    //    }
    //}
}
