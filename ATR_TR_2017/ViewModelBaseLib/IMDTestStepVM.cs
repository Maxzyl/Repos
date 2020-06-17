using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelBaseLib;
using TestModelLib;


namespace ViewModelBaseLib
{
    public class IMDTestStepVM : TestStepVM
    {
        public IMDTestStep testStep
        {
            get
            {
                return TestStep as IMDTestStep;
            }
        }

        private double _F1;
        private const string F1PropertyName = "F1";
        public double F1
        {
            get
            {
                return testStep.F1;
            }
            set
            {
                testStep.F1 = value;
                NotifyPropertyChanged(F1PropertyName);
            }
        }

        private double _F2;
        private const string F2PropertyName = "F2";
        public double F2
        {
            get
            {
                return testStep.F2;
            }
            set
            {
                testStep.F2 = value;
                NotifyPropertyChanged(F2PropertyName);
            }
        }

        private double _F1Power;
        private const string F1PowerPropertyName = "F1Power";
        public double F1Power
        {
            get
            {
                return testStep.F1Power;
            }
            set
            {
                testStep.F1Power = value;
                NotifyPropertyChanged(F1PowerPropertyName);
            }
        }

        private double _F2Power;
        private const string F2PowerPropertyName = "F2Power";
        public double F2Power
        {
            get
            {
                return testStep.F2Power;
            }
            set
            {
                testStep.F2Power = value;
                NotifyPropertyChanged(F2PowerPropertyName);
            }
        }

        private IMDSweepModeEnum _SweepMode;
        private const string SweepModePropertyName = "SweepMode";
        public IMDSweepModeEnum SweepMode
        {
            get
            {
                return testStep.SweepMode;
            }
            set
            {
                testStep.SweepMode = value;
                NotifyPropertyChanged(SweepModePropertyName);
            }
        }

        private int _Order;
        private const string OrderPropertyName = "Order";
        public int Order
        {
            get
            {
                return testStep.Order;
            }
            set
            {
                testStep.Order = value;
                NotifyPropertyChanged(OrderPropertyName);
            }
        }

        private double _RecvAtten;
        private const string RecvAttenPropertyName = "RecvAtten";
        public double RecvAtten
        {
            get
            {
                return testStep.RecvAtten;
            }
            set
            {
                testStep.RecvAtten = value;
                NotifyPropertyChanged(RecvAttenPropertyName);
            }
        }
        
        private double _MeasBandwidth;
        private const string MeasBandwidthPropertyName = "MeasBandwidth";
        public double MeasBandwidth
        {
            get
            {
                return testStep.MeasBandwidth;
            }
            set
            {
                testStep.MeasBandwidth = value;
                NotifyPropertyChanged(MeasBandwidthPropertyName);
            }
        }

        private IMDSidebandEnum _Sideband;
        private const string SidebandPropertyName = "Sideband";
        public IMDSidebandEnum Sideband
        {
            get
            {
                return testStep.Sideband;
            }
            set
            {
                testStep.Sideband = value;
                NotifyPropertyChanged(SidebandPropertyName);
            }
        }

        private bool _IsTest;
        private const string IsTestPropertyName = "IsTest";
        public bool IsTest
        {
            get
            {
                return testStep.IsTest;
            }
            set
            {
                testStep.IsTest = value;
                NotifyPropertyChanged(IsTestPropertyName);
            }
        }

        private bool _IsSavePic;
        private const string IsSavePicPropertyName = "IsSavePic";
        public bool IsSavePic
        {
            get
            {
                return testStep.IsSavePic;
            }
            set
            {
                testStep.IsSavePic = value;
                NotifyPropertyChanged(IsSavePicPropertyName);
            }
        }
        
        

        private double _PowerTolerance;
        private const string PowerTolerancePropertyName = "PowerTolerance";
        public double PowerTolerance
        {
            get
            {
                return testStep.PowerTolerance;
            }
            set
            {
                testStep.PowerTolerance = value;
                NotifyPropertyChanged(PowerTolerancePropertyName);
            }
        }

        private double _PowerMeterLoss;
        private const string PowerMeterLossPropertyName = "PowerMeterLoss";
        public double PowerMeterLoss
        {
            get
            {
                return testStep.PowerMeterLoss;
            }
            set
            {
                testStep.PowerMeterLoss = value;
                NotifyPropertyChanged(PowerMeterLossPropertyName);
            }
        }

        public IMDSweepModeEnum[] IMDSweepModeList
        {
            get
            {
                return new IMDSweepModeEnum[] { IMDSweepModeEnum.Point, IMDSweepModeEnum.Sweep };
            }
        }

        public int[] OrderList
        {
            get
            {
                return new int[] { 3, 5, 7, 9, 11, 13, 15 };
            }
        }

        public IMDSidebandEnum[] IMDSidebandList
        {
            get
            {
                return new IMDSidebandEnum[] { IMDSidebandEnum.Avg, IMDSidebandEnum.Worst, IMDSidebandEnum.High, IMDSidebandEnum.Low };
            }
        }

        private bool _CorrectionEnable;
        private const string CorrectionEnablePropertyName = "CorrectionEnable";
        public bool CorrectionEnable
        {
            get
            {
                return testStep.CorrectionEnable;
            }
            set
            {
                testStep.CorrectionEnable = value;
                NotifyPropertyChanged(CorrectionEnablePropertyName);
            }
        }

        private int _CalInterval;
        private const string CalIntervalPropertyName = "CalInterval";
        public int CalInterval
        {
            get
            {
                return testStep.CalInterval;
            }
            set
            {
                testStep.CalInterval = value;
                NotifyPropertyChanged(CalIntervalPropertyName);
            }
        }

        private CalWarningTypeEnum _CalWarning;
        private const string CalWarningPropertyName = "CalWarning";
        public CalWarningTypeEnum CalWarning
        {
            get
            {
                return testStep.CalWarning;
            }
            set
            {
                testStep.CalWarning = value;
                NotifyPropertyChanged(CalWarningPropertyName);
            }
        }

        private bool _IsCalEachTest;
        private const string IsCalEachTestPropertyName = "IsCalEachTest";
        public bool IsCalEachTest
        {
            get
            {
                return testStep.IsCalEachTest;
            }
            set
            {
                testStep.IsCalEachTest = value;
                NotifyPropertyChanged(IsCalEachTestPropertyName);
            }
        }

        public CalWarningTypeEnum[] CalWarningTypeEnumList
        {
            get
            {
                return new CalWarningTypeEnum[] { CalWarningTypeEnum.不提醒, CalWarningTypeEnum.强制校准, CalWarningTypeEnum.提醒校准 };
            }
        }

    }
}
