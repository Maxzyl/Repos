using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModelLib;
using ModelBaseLib;
namespace ViewModelBaseLib
{
    public class SParamTestStepVM:TestStepVM
    {
        private SParamTestStep testStep
        {
            get
            {
                return TestStep as SParamTestStep;
            }
        }
        private double _StartFreq;
        private const string StartFreqPropertyName = "StartFreq";
        
        [UIDisplay("起始频率",typeof(DataUtils.FreqStringConverter))]
        public double StartFreq
        {
            get
            {
                return testStep.StartFreq;
            }
            set
            {
                testStep.StartFreq = value;
                NotifyPropertyChanged(StartFreqPropertyName);
            }
        }
        private double _StopFreq;
        private const string StopFreqPropertyName = "StopFreq";
        [UIDisplay("截止频率", typeof(DataUtils.FreqStringConverter))]
        public double StopFreq
        {
            get
            {
                return testStep.StopFreq;
            }
            set
            {
                testStep.StopFreq = value;
                NotifyPropertyChanged(StopFreqPropertyName);
            }
        }
        private int _SweepPoints;
        private const string SweepPointsPropertyName = "SweepPoints";
        [UIDisplay("扫描点数")]
        public int SweepPoints
        {
            get
            {
                return testStep.SweepPoints;
            }
            set
            {
                testStep.SweepPoints = value;
                NotifyPropertyChanged(SweepPointsPropertyName);
            }
        }
        private int _PortNum;
        private const string PortNumPropertyName = "PortNum";
        [UIDisplay("仪表端口数")]
        public int PortNum
        {
            get
            {
                return testStep.PortNum;
            }
            set
            {
                testStep.PortNum = value;
                NotifyPropertyChanged(PortNumPropertyName);
            }
        }
        
        
    }
    public class SParamTestTraceVM : TestTraceVM
    {
        public SParamTestTraceVM()
        {
            FormatList = new string[] { SParamTestTraceFormatType.LogMag, SParamTestTraceFormatType.phase, SParamTestTraceFormatType.SWR };
        }
        private SParamTestTrace testTrace
        {
            get
            {
                return TestTrace as SParamTestTrace;
            }
        }
        private string _Format;
        private const string FormatPropertyName = "Format";
        [UIDisplay("Format",null,"FormatList")]
        public string Format
        {
            get
            {
                return testTrace.Format;
            }
            set
            {
                testTrace.Format = value;
                NotifyPropertyChanged(FormatPropertyName);
            }
        }
        public string[] FormatList { get; set; }
    }
}
