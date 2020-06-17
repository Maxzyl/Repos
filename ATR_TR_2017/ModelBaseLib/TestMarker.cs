using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    [Serializable]
    public class TestMarker
    {
        public TestMarker()
        {
            IsTest = true;
        }
        public bool? PassFail { get; set; }
        /// <summary>
        /// 频率描述
        /// </summary>
        public string XDescription { get; set; }
        /// <summary>
        /// 参数描述
        /// </summary>
        public string YDescription { get; set; }
        //public string LimitXDesciption { get; set; }
        /// <summary>
        /// 指标描述
        /// </summary>
        public string LimitDescription { get; set; }
        /// <summary>
        /// 存放和被测件相关的说明，例如测试端口，端口描述
        /// </summary>
        public string TestConfigDesciption { get; set; }
        public bool IsSendToDB { get; set; }
        public string VarName { get; set; }
        //[System.Xml.Serialization.XmlIgnore]
        public bool IsTest { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string UserDefName { get; set; }                  
        public bool FunctionEnable { get; set; }
        public virtual void JudgePassFail()
        {

        }
    }
    /// <summary>
    /// XYMarker Typer
    /// </summary>
    public enum XYTestMarkerTypeEnum
    {
        Normal,
        Max,
        Min,
        Mean,
        ABSMax,
        ABSMin,
        PhaseRipple,
        Ripple,
        InRange,
        Target,
        Q_Value,
        BandWidth,
        LeftFreq,
        RightFreq,
        CenterFreq,
        InsertionLoss,
        Pin_NdB,
        Po_NdB,
        RippleInRange,
        Peak,
        RippleHalf,
        PowerMean
    }
    
    public class XYMakerLimit
    {
        public double? XMax { get; set; }
        public double? XMin { get; set; }
        public double? YMax { get; set; }
        public double? YMin { get; set; }
        public bool XValueJudgeEnable { get; set; }
        public bool YValueJudgeEnable { get; set; }
    }
    /// <summary>
    /// XYMarker类型定义，用两列在数据库中存放数据。根据Maker类型设置仪表，从Trace中获取数据。
    /// 根据Limit判断PassFail
    /// </summary>
    public class XYTestMarker:TestMarker
    {
        public XYTestMarker()
        {
            Limit = new XYMakerLimit();
            MarkerResult = new XYDataArr();
        }
        public XYTestMarkerTypeEnum Type { get; set; }
        public double Start { get; set; }
        public double Stop { get; set; }
        public XYMakerLimit Limit { get; set; }
        
        /// <summary>
        /// Marker的数据
        /// </summary>
        public XYDataArr MarkerResult { get; set; }

        //public string DisplayStr { get; set; }

        public override void JudgePassFail()
        {
            foreach(XYData xydata in MarkerResult)
            {
                if (!(Symtant.GeneFunLib.GeneFun.IsPassFail(xydata.X, Limit.XMax, Limit.XMin) &&
                    Symtant.GeneFunLib.GeneFun.IsPassFail(xydata.Y, Limit.YMax, Limit.YMin)))
                {
                    PassFail = false;
                    return;
                }
            }
            PassFail = true;
        }
        public bool IsTRTestItem { get; set; }
    }
}
