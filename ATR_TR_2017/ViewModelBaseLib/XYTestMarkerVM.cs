using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
namespace ViewModelBaseLib
{
    public class XYTestMarkerVM : TestMarkerVM
    {
        private XYTestMarker XYMarker
        {
            get
            {   
                return Marker as XYTestMarker;
            }
        }
        private const string TypeNamePropertyName = "TypeName";
        public XYTestMarkerTypeEnum TypeName
        {
            get
            {   
                if(!MarkerTypeList.Contains(XYMarker.Type))
                {
                    XYMarker.Type = MarkerTypeList[0];
                }
                return XYMarker.Type;
            }
            set
            {
                XYMarker.Type = value;
                NotifyPropertyChanged(TypeNamePropertyName);
            }
        }
        private const string IsTestPropertyName = "IsTest";
        public bool IsTest
        {
            get
            {
                return XYMarker.IsTest;
            }
            set
            {
                XYMarker.IsTest = value;
                NotifyPropertyChanged(IsTestPropertyName);
            }
        }
        private const string FunctionEnablePropertyName = "FunctionEnable";
        public bool FunctionEnable
        {
            get
            {
                return XYMarker.FunctionEnable;
            }
            set
            {
                XYMarker.FunctionEnable = value;
                NotifyPropertyChanged(FunctionEnablePropertyName);
            }
        }

        private const string StartPropertyName = "Start";
        [UIDisplay("开始")]
        public double Start
        {
            get
            {
                return XYMarker.Start;
            }
            set
            {
                XYMarker.Start = value;
                NotifyPropertyChanged(StartPropertyName);
                NotifyPropertyChanged(FreqRangePropertyName);
            }
        }

        private const string StopPropertyName = "Stop";
        [UIDisplay("结束")]
        public double Stop
        {
            get
            {
                return XYMarker.Stop;
            }
            set
            {
                XYMarker.Stop = value;
                NotifyPropertyChanged(FreqRangePropertyName);
                NotifyPropertyChanged(StopPropertyName);
            }
        }
        private string _FreqRange;
        private const string FreqRangePropertyName = "FreqRange";
        public string FreqRange
        {
            get
            {
                return XYMarker.Start.ToString() + "-" + XYMarker.Stop.ToString();
            }
        }

        private const string XMinPropertyName = "XMin";
        public double? XMin
        {
            get
            {
                return XYMarker.Limit.XMin;
            }
            set
            {
                XYMarker.Limit.XMin = value;
                NotifyPropertyChanged(XMinPropertyName);
                NotifyPropertyChanged(XRangePropertyName);
            }
        }

        private const string XMaxPropertyName = "XMax";
        public double? XMax
        {
            get
            {
                return XYMarker.Limit.XMax;
            }
            set
            {
                XYMarker.Limit.XMax = value;
                NotifyPropertyChanged(XMaxPropertyName);
                NotifyPropertyChanged(XRangePropertyName);
            }
        }
        private string _XRange;
        private const string XRangePropertyName = "XRange";
        public string XRange
        {
            get
            {
                return XMin.ToString() + "-" + XMax.ToString();
            }
        }

        private const string YMinPropertyName = "YMin";
        public double? YMin
        {
            get
            {
                return XYMarker.Limit.YMin;
            }
            set
            {
                XYMarker.Limit.YMin = value;
                NotifyPropertyChanged(YMinPropertyName);
                NotifyPropertyChanged(YRangePropertyName);
            }
        }

        private const string YMaxPropertyName = "YMax";
        public double? YMax
        {
            get
            {
                return XYMarker.Limit.YMax;
            }
            set
            {
                XYMarker.Limit.YMax = value;
                NotifyPropertyChanged(YMaxPropertyName);
                NotifyPropertyChanged(YRangePropertyName);
            }
        }
        private string _YRange;
        private const string YRangePropertyName = "YRange";
        public string YRange
        {
            get
            {
                return YMin.ToString() + "-" + YMax.ToString();
            }
        }

        private const string XValueJudgeEnablePropertyName = "XValueJudgeEnable";
        public bool XValueJudgeEnable
        {
            get
            {
                return XYMarker.Limit.XValueJudgeEnable;
            }
            set
            {
                XYMarker.Limit.XValueJudgeEnable = value;
                NotifyPropertyChanged(XValueJudgeEnablePropertyName);
            }
        }

        private const string YValueJudgeEnablePropertyName = "YValueJudgeEnable";
        public bool YValueJudgeEnable
        {
            get
            {
                return XYMarker.Limit.YValueJudgeEnable;
            }
            set
            {
                XYMarker.Limit.YValueJudgeEnable = value;
                NotifyPropertyChanged(YValueJudgeEnablePropertyName);
            }
        }

        private const string TestConfigDesciptionPropertyName = "TestConfigDesciption";
        public string TestConfigDesciption
        {
            get
            {
                return XYMarker.TestConfigDesciption;
            }
            set
            {
                XYMarker.TestConfigDesciption = value;
                NotifyPropertyChanged(TestConfigDesciptionPropertyName);
            }
        }

        private const string YDescriptionPropertyName = "YDescription";
        public string YDescription
        {
            get
            {
                return XYMarker.YDescription;
            }
            set
            {
                XYMarker.YDescription = value;
                NotifyPropertyChanged(YDescriptionPropertyName);
            }
        }

        private const string XDescriptionPropertyName = "XDescription";
        public string XDescription
        {
            get
            {
                return XYMarker.XDescription;
            }
            set
            {
                XYMarker.XDescription = value;
                NotifyPropertyChanged(XDescriptionPropertyName);
            }
        }

        private const string LimitDescriptionPropertyName = "LimitDescription";
        public string LimitDescription
        {
            get
            {
                return XYMarker.LimitDescription;
            }
            set
            {
                XYMarker.LimitDescription = value;
                NotifyPropertyChanged(LimitDescriptionPropertyName);
            }
        }
        public bool IsTRTestItem
        {
            get
            {
                return XYMarker.IsTRTestItem;
            }
            set
            {
                XYMarker.IsTRTestItem = value;
            }
        }
        public XYTestMarkerTypeEnum[] MarkerTypeList
        {
            get
            {   
                if(IsTRTestItem)
                {
                   return new XYTestMarkerTypeEnum[] {XYTestMarkerTypeEnum.ABSMax,XYTestMarkerTypeEnum.ABSMin,XYTestMarkerTypeEnum.Max,XYTestMarkerTypeEnum.Min,XYTestMarkerTypeEnum.Peak};
                }
                else
                {
                    return new XYTestMarkerTypeEnum[] {XYTestMarkerTypeEnum.ABSMax,XYTestMarkerTypeEnum.ABSMin, XYTestMarkerTypeEnum.CenterFreq, XYTestMarkerTypeEnum.BandWidth, XYTestMarkerTypeEnum.InRange, XYTestMarkerTypeEnum.InsertionLoss, XYTestMarkerTypeEnum.LeftFreq, XYTestMarkerTypeEnum.Max, XYTestMarkerTypeEnum.Mean, XYTestMarkerTypeEnum.Min, XYTestMarkerTypeEnum.Normal, XYTestMarkerTypeEnum.PhaseRipple, XYTestMarkerTypeEnum.Pin_NdB, XYTestMarkerTypeEnum.Q_Value, XYTestMarkerTypeEnum.RightFreq, XYTestMarkerTypeEnum.Ripple, XYTestMarkerTypeEnum.RippleInRange, XYTestMarkerTypeEnum.Target,XYTestMarkerTypeEnum.Peak,XYTestMarkerTypeEnum.PowerMean,XYTestMarkerTypeEnum.RippleHalf }; 
                }
            }
        }

        private string _UserDefName;
        private const string UserDefNamePropertyName = "UserDefName";
        public string UserDefName
        {
            get
            {
                return Marker.UserDefName;
            }
            set
            {
                Marker.UserDefName = value;
                NotifyPropertyChanged(UserDefNamePropertyName);
            }
        }
        
    }
}
